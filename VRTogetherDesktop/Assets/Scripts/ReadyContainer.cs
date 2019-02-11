using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ReadyContainer : MonoBehaviour {

    public GameObject cube;

    private void Start()
    {
        cube.SetActive(false);
    }

    void Update () {
		if(MinigameServer.Instance.AllPlayersReady())
        {
            cube.SetActive(true);
        }
	}
}
