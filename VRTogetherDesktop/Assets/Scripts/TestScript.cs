using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class TestScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //An example usage of getting the data
        StartCoroutine(test());
    }

    IEnumerator test()
    {
        //Get a reference to the server room code
        ServerRoomCode server = GetComponent<ServerRoomCode>();

        //Generate and get the code
        string code = "";
        yield return server.GetCode(GetLocalIP(), value => code = value);
        Debug.Log("Code Generated: " + code);

        //Get an IP based on a code
        IPData data = new IPData();
        yield return server.GetIP(code, value => data = value);

        Debug.Log("Public IP recieved: " + data.publicIP);
        Debug.Log("Local IP recieved: " + data.localIP);
    }

    private string GetLocalIP()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
