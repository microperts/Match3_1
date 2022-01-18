using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Networking;
using System.Text;
using UnityEditor;


public class Logins : MonoBehaviour
{
    public static bool IsLoggedIn = false;
    public static string UserName;
    
    public GameObject preConnection;
    public GameObject InputUserName;
    public GameObject postConnection;
    public InputField username;
    public GameObject LoadingScreen;
    public Text usernameText;
    public WebLogin webLogin;

    private string specialCharacter = "!@#$%^&*()_+{}[]:;'|?<>,.+-*/=- \"";
    
    void Start()
    {
        usernameText.text = UserName ?? "Username";

        if (IsLoggedIn) { return; }

        preConnection.SetActive(true);
        //StartCoroutine(GetItems());
    }

    public void connectButtonPressed()
    {
        LoadingScreen.SetActive(true);
        webLogin.onConnected.AddListener(() =>
        {
            LoadingScreen.SetActive(false);
            Debug.Log("Wallet Connected");
            InputUserName.SetActive(true);
            preConnection.SetActive(false);
        });
        webLogin.OnLogin();
    }
    
    public void submitButtonPressed()
    {
        if(username.text != null && username.text.Length >= 3)
        {
            foreach(char ch in specialCharacter)
            {
                if (username.text.Contains(ch))
                {
                    return;
                }
            }
            
            InputUserName.SetActive(false);
            LoadingScreen.SetActive(true);
            StartCoroutine(GetItems());
        }
        
    }
    public void doneButtonPressed()
    {
        postConnection.SetActive(false);
    }
    private IEnumerator GetItems()
    {
        //ScoreBoard.text = null;
        string uri = "https://boss-fall.herokuapp.com/api/user/"+username.text;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    if (webRequest.responseCode != 400)
                    {
                        Debug.LogError("Error: " + webRequest.error);
                    }
                    StartCoroutine(SetData_Coroutine());
                    //loadingScreen.SetActive(false);
                    //ServerErrorMsg.SetActive(true);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);

                    string responseJson = webRequest.downloadHandler.text;
                    GetData tz = JsonUtility.FromJson<GetData>(responseJson);
                    //Data data = JsonUtility.FromJson<Data>(responseJson);
                    //List<Datum> list = tz.data.ToList();
                    if (tz.code != "INVALID_USERNAME")
                    {
                       postConnection.SetActive(true);
                       //LoadingScreen.SetActive(false);
                        //Debug.Log(tz.data.LeaderBoard[0].username + "   " + tz.data.LeaderBoard[0].score);

                    }
                    else
                    {
                        //LoadingScreen.SetActive(false);
                        StartCoroutine(SetData_Coroutine());
                    }

                    usernameText.text = username.text;
                    UserName = username.text;
                    LoadingScreen.SetActive(false);
                    postConnection.SetActive(true);
                    IsLoggedIn = true;
                    break;
            }
        }
    }
    IEnumerator SetData_Coroutine()
    {
        // loadingScreen.SetActive(true);
        string uri = "https://boss-fall.herokuapp.com/api/user/register";
        var sendData = new SendData();
        sendData.username = username.text;
        var guid = System.Guid.NewGuid();
        sendData.walletId = guid.ToString();   //PlayerPrefs.GetString("PlayerName");

        Debug.Log(sendData.walletId);
        
        //sendData.score = playerscore.ToString();
        string jsonDataString = JsonUtility.ToJson(sendData);
        var request = UnityWebRequest.Post(uri, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonDataString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError ||
            request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError(request.error);
            Debug.Log(request.downloadHandler.text);
            //LoadingScreen.SetActive(false);
            //ServerErrorMsg.SetActive(true);
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);

            StartCoroutine(GetItems());
        }
    }

    

}


public class GetData
{
    public string code;
    public string _id;
    public string username;
    public string walletId;
    public int __v;
}

[Serializable]
public class SendData
{ 
    public string username;
    public string walletId;
}
