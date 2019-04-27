using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct IPData {
    public string localIP;
    public string publicIP;
}

public struct ServerData<T> {
    public T data;
    public bool isError;
    public string errorMessage;
}

public class ServerRoomCode : MonoBehaviour {
    public string serverDeployment = "vrt-dev";
    readonly string url = "api.ripostory.com";

    public IEnumerator GetCode(string localIP, System.Action<ServerData<string>> outData) {
        UnityWebRequest responseData = null;
        yield return ServerRequest(value => responseData = value, "POST", "connect-code", "local=" + localIP);

        //output data to serverdata struct
        ServerData<string> parsedData = new ServerData<string>();
        if (responseData != null && AssertData(responseData, out parsedData.errorMessage))
        {
            parsedData.data = responseData.downloadHandler.text;
            parsedData.isError = false;
        } else if (responseData.isNetworkError)
        {
            parsedData.isError = true;

        }
        else 
        {
            parsedData.isError = true;
        }
        outData(parsedData);
    }

    public IEnumerator ReleaseCode(string code)
    {
        UnityWebRequest responseData = null;
        yield return ServerRequest(value => responseData = value, "POST", "release-code", "code=" + code);

        //we don't care about if this request succeeds or not
    }

    public IEnumerator GetIP(string code, System.Action<ServerData<IPData>> outData)
    {
        UnityWebRequest responseData = null;
        yield return ServerRequest(value => responseData = value, "GET", "connect-code", "code=" + code);

        //output data to serverdata struct
        ServerData<IPData> parsedData = new ServerData<IPData>();
        if (responseData != null && AssertData(responseData, out parsedData.errorMessage)) {
            parsedData.data = JsonUtility.FromJson<IPData>(responseData.downloadHandler.text);
            parsedData.isError = false;

            //check if IPs given are valid
            if (!AssertIP(parsedData.data.localIP, out parsedData.errorMessage)) {
                parsedData.isError = true;
            }

            if (!AssertIP(parsedData.data.publicIP, out parsedData.errorMessage))
            {
                parsedData.isError = true;
            }
        }
        else
        {
            parsedData.isError = true;
        }
        outData(parsedData);
    }

    bool AssertIP(string ip, out string reason) {
        string[] nums = ip.Split('.');
        if (nums.Length != 4) {
            reason = "INVALID IP: parse error";
            return false;
        }
        foreach (string number in nums) {
            int data = 999;
            if (int.TryParse(number, out data)) {
                if ( data < 0 || data > 255) {
                    reason = "INVALID IP: numbers out of range";
                    return false;
                }
            }
            else
            {
                reason = "INVALID IP: parse error";
                return false;
            }
        }
        reason = "";
        return true;
    }

    bool AssertData(UnityWebRequest data, out string reason) {
        //check for errors
        if (data.isHttpError) {
            reason = "The HTTP response returned an error: " + data.error;
            return false;
        }

        if (data.isNetworkError)
        {
            reason = "The network returned an error: " + data.error;
            return false;
        }

        //succeed
        reason = "";
        return true;
    }

    IEnumerator ServerRequest(System.Action<UnityWebRequest> result, string verb, string method, string query = "", string body = "")
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

        rest.timeout = 5;

        yield return rest.SendWebRequest();

        if (rest.isNetworkError)
        {
            Debug.Log("ERROR");

        }

        //return the full response
        result(rest);
    }
}
