using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class EZSpawner : MonoBehaviour {

    public GameObject obj;

    private bool doSpawn = true;

	void Update () {
        if (doSpawn)
        {
            if (MinigameServer.Instance.AllPlayersReady())
            {
                Debug.Log("Net create cube");
                GameObject inst = MinigameServer.Instance.NetworkInstantiate(obj);
                inst.transform.position = this.transform.position;
                doSpawn = false;
                Destroy(this.gameObject);
            }
        }
	}

    public bool HasObjectSpawned()
    {
        return !doSpawn;
    }
}
