using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Networking;
using System.Text;

public class LeaderBoardScript : MonoBehaviour
{
    public GameObject cellPrefab;

    public Text UserCell_Position;
    public Text UserCell_Name;
    public Text UserCell_Score;

    // Start is called before the first frame update
    void Start()
    {
        //for(int i = 0; i < 50; i++)
        //{
        //    GameObject obj = Instantiate(cellPrefab);
        //    obj.transform.SetParent(this.gameObject.transform,false);
        //    obj.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
        //    obj.transform.GetChild(1).GetComponent<Text>().text = "Mahad";
        //    obj.transform.GetChild(2).GetComponent<Text>().text = i.ToString();
        //}
        StartCoroutine(GetItems());
    }

    private IEnumerator GetItems()
    {
        //ScoreBoard.text = null;
        string uri = $"https://boss-fall.herokuapp.com/api/leaderboard/{Logins.UserName}?pageNumber=1";//"https://boss-fall.herokuapp.com/api/leaderboard?pageNumber=1";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Error: " + webRequest.error);
                    //loadingScreen.SetActive(false);
                    //ServerErrorMsg.SetActive(true);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);

                    string responseJson = webRequest.downloadHandler.text;
                   API_response_storage tz = JsonUtility.FromJson<API_response_storage>(responseJson);
                   
                    UserCell_Name.text = tz.user.user.username;
                    UserCell_Position.text = tz.user.position + ".";
                    UserCell_Score.text = tz.user.score.ToString();
                    
                    List<Datum> list = tz.data.ToList();
                    if (list.Count > 0)
                    {
                        int i = 0;
                        foreach (Datum item in list)
                        {
                            GameObject obj = Instantiate(cellPrefab);
                            obj.transform.SetParent(this.gameObject.transform, false);
                            obj.transform.GetChild(0).GetComponent<Text>().text = tz.data[i].position + ".";
                            obj.transform.GetChild(1).GetComponent<Text>().text = tz.data[i].user.username;
                            obj.transform.GetChild(2).GetComponent<Text>().text = tz.data[i].score.ToString();
                            i++;
                            
                        }
                        //Debug.Log(tz.data.LeaderBoard[0].username + "   " + tz.data.LeaderBoard[0].score);
                        
                    }
                    else
                    {
                        //NoActiveEventMsg.SetActive(true);
                    }

                    //loadingScreen.SetActive(false);
                    break;
            }
        }
    }
}

[Serializable]
public class User
{
    public string _id;
    public string username;
}

[Serializable]
public class Datum
{
    public string _id;
    public User user;
    public int score;
    public int position;
}

[Serializable]
public class API_response_storage
{
    public List<Datum> data;
    public Datum user;
    public string currentPage;
}
