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

	void Start () {
        server = GetComponent<ServerRoomCode>();
	}

    public void Submit()
    {
        Submit(input.text);
    }

    private void Submit(string code)
    {
        StartCoroutine(SubmitCode(code));
    }

    private IEnumerator SubmitCode(string code)
    {

        ServerData<IPData> ips = new ServerData<IPData>();
        yield return server.GetIP(code.ToUpper(), value => ips = value);
        if (ips.isError)
        {
            Debug.Log(ips.errorMessage);
        }
        else
        {
            Debug.Log("Trying to start client from code (" + code + ") with IP (" + ips.data.localIP + ")");
            mgr.TryStartClient(ips.data.localIP);

            OnCodeSubmitted.Invoke();

            //Debug.Log("Local IP recieved: " + ips.data.localIP);
        }
    }
}
