using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class GenerateCode : MonoBehaviour {

    private static bool GENERATED_CODE = false;
    private static string CODE = "";

    public Text codeText;

    private ServerRoomCode server;

	void Start () {
        server = GetComponent<ServerRoomCode>();

        if (!GENERATED_CODE)
        {
            StartCoroutine(GetCode());
        }
        else
        {
            codeText.text = CODE;
        }
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
            GENERATED_CODE = true;
            CODE = ("ENTER CODE:\n" + code.data).Replace("\\n", "\n");
            codeText.text = CODE;
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
