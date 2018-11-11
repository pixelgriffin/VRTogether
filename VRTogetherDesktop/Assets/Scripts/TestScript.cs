using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

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
        ServerData<string> code = new ServerData<string>();
        yield return server.GetCode(GetLocalIPAddress(), value => code = value);
        if (code.isError) {
            Debug.Log(code.errorMessage);
        } else {
            Debug.Log("Code Generated: " + code.data);
        }

        //Get an IP based on a code
        ServerData<IPData> ips = new ServerData<IPData>();
        yield return server.GetIP(code.data, value => ips = value);
        if (ips.isError) {
            Debug.Log(ips.errorMessage);
        } else {
            Debug.Log("Public IP recieved: " + ips.data.publicIP);
            Debug.Log("Local IP recieved: " + ips.data.localIP);
        }

        yield return new WaitForSeconds(5);

        //Release code
        yield return server.ReleaseCode(code.data);
        Debug.Log("Releasing code: " + code.data);
    }

    public string GetLocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
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
