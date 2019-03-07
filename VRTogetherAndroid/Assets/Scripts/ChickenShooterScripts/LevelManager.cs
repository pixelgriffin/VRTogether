using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class LevelManager : MonoBehaviour {

    public NetworkBool chickenFinished = new NetworkBool("chickenFinished", false);

    private NetworkID id;

	// Use this for initialization
	void Start () {

        id = GetComponent<NetworkID>();

        MinigameClient.Instance.RegisterVariable(id.netID, chickenFinished);
		
	}

    private void Update()
    {
        if (chickenFinished.value)
        {
            MinigameClient.Instance.SendBooleanToAll(chickenFinished);
        }
    }
}
