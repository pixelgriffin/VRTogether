using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CodeToIP : MonoBehaviour {

    public ClientManager mgr;

    public InputField input;

    public UnityEvent OnCodeSubmitted;

    private ServerRoomCode server;

    private string ip = string.Empty;

	void Start () {
        server = GetComponent<ServerRoomCode>();
	}

    public void Submit()
    {
        Submit(input.text);
    }

    public void Submit(string code)
    {
        StartCoroutine(SubmitCode(code));
    }

    /*
        Modified to not start the game, instead it stores the returned IP and invokes the event.
        Add functions to the event in the editor to modify the behavior for what should happen once
        an IP is returned.
     */
    private IEnumerator SubmitCode(string code)
    {

        ServerData<IPData> ips = new ServerData<IPData>();
        yield return server.GetIP(code.ToUpper(), value => ips = value);
        if (ips.isError)
        {
            Debug.Log(ips.errorMessage);
            ip = "ERROR";

            if (transform.GetComponent<LobbyManager>() != null)
            {
                transform.GetComponent<LobbyManager>().EnableError();

            }
        }
        else
        {
            //Debug.Log("Trying to start client from code (" + code + ") with IP (" + ips.data.localIP + ")");
            //mgr.TryStartClient(ips.data.localIP);
            ip = ips.data.localIP;

            OnCodeSubmitted.Invoke();

            //Debug.Log("Local IP recieved: " + ips.data.localIP);
        }
    }

    public string GetIP ()
    {
        return ip;

    }

}
