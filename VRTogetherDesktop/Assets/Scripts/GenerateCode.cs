using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class GenerateCode : MonoBehaviour {

    public Text codeText;

    private ServerRoomCode server;

	void Start () {
        server = GetComponent<ServerRoomCode>();

        StartCoroutine(GetCode());
	}
	
	private IEnumerator GetCode()
    {
        ServerData<string> code = new ServerData<string>();
        yield return server.GetCode(GetLocalIPAddress(), value => code = value);
        if (code.isError)
        {
            Debug.Log(code.errorMessage);
        }
        else
        {
            codeText.text = ("ENTER CODE:\n" + code.data).Replace("\\n", "\n");
        }
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
