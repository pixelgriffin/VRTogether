using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct IPData {
    public string localIP;
    public string publicIP;
}

public class ServerRoomCode : MonoBehaviour {
    public string serverDeployment = "vrt-dev";
    private string url = "api.ripostory.com";

    public IEnumerator GetCode(string localIP, System.Action<string> outData) {
        string responseData = "";
        yield return ServerRequest(value => responseData = value, "POST", "connect-code", "local=" + localIP);
        outData(responseData);
    }

    public IEnumerator GetIP(string code, System.Action<IPData> outData)
    {
        string responseData = "";
        yield return ServerRequest(value => responseData = value, "GET", "connect-code", "code=" + code);
        IPData finalData = JsonUtility.FromJson<IPData>(responseData);
        outData(finalData);
    }

    private IEnumerator ServerRequest(System.Action<string> result, string verb, string method, string query = "", string body = "")
    {
        //build URL
        string fullURL = "https://" + url + "/" + serverDeployment + "/" + method;
        if (query != "") 
            fullURL += "?" + query;

        //Get Data
        UnityWebRequest rest;
        if (verb == "POST") {
            rest = UnityWebRequest.Post(fullURL, body);
        } else if (verb == "GET") {
            rest = UnityWebRequest.Get(fullURL);
        } else {
            //default verb
            rest = UnityWebRequest.Get(fullURL);
        }

        yield return rest.SendWebRequest();

        if (rest.isNetworkError || rest.isHttpError)
        {
            Debug.Log(rest.error);
            result(null);
        }
        else
        {
            // Show results as text
            result(rest.downloadHandler.text);
        }
    }
}
