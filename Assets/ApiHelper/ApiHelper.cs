using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiHelper : MonoBehaviour
{
    public static IEnumerator Get(string url, Action<string> success, Action<string> error, string payload , Dictionary<string,string> headers = null)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get( url );

        if (headers != null)
        {
            foreach (KeyValuePair<string,string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key,pair.Value);
            }
        }
        
        webRequest.SetRequestHeader( "Content-Type", "application/json" );
        
        if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes( payload );
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw( bodyRaw );
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }
        
        Debug.Log($"Accessing Endpoint {url} with GET Request");
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Error Occured for Endpoint : " + url);
                Debug.LogError("Error Body : " + webRequest.error );
                error(webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + url);
                Debug.Log( "Response Body : " + webRequest.downloadHandler.text );
                success(webRequest.downloadHandler.text);
                break;
        }
    }
    
    public static IEnumerator Post(string url, Action<string> success, Action<string> error, string payload)
    {
        using UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        webRequest.SetRequestHeader( "Content-Type", "application/json" );

        if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes( payload );
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw( bodyRaw );
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }
            
        Debug.Log($"Accessing Endpoint {url} with POST Request");
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 400)
        {
            Debug.Log(webRequest.downloadHandler.text);
            success(webRequest.downloadHandler.text);
            yield break;
        }
        
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Error Occured for Endpoint : " + url);
                Debug.LogError("Error Body : " + webRequest.error );
                error(webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + url);
                Debug.Log( "Response Body : " + webRequest.downloadHandler.text );
                success(webRequest.downloadHandler.text);
                break;
        }
    }
    
    public static IEnumerator Put(string url, Action<string> success, Action<string> error, string payload)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Put(url,payload);//new UnityWebRequest(url, "POST");
        webRequest.SetRequestHeader( "Content-Type", "application/json" );

        /*if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes( payload );
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw( bodyRaw );
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }*/
            
        Debug.Log($"Accessing Endpoint {url} with PUT Request");
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Error Occured for Endpoint : " + url);
                Debug.LogError("Error Body : " + webRequest.error );
                error(webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + url);
                Debug.Log( "Response Body : " + webRequest.downloadHandler.text );
                success(webRequest.downloadHandler.text);
                break;
        }
    }
    
    public static IEnumerator DownloadImage( string mediaUrl, Action<Texture2D> success, Action<string> error )
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture( mediaUrl );
        
        yield return request.SendWebRequest();
        
        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Error Occured for Endpoint : " + mediaUrl);
                Debug.LogError("Error Body : " + request.error );
                error(request.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + mediaUrl);
                Debug.Log( "Response Body : " + request.downloadHandler.text );
                var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                success(texture);
                break;
        }
    }
}
