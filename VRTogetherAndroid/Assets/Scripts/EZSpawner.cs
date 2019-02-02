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
            if (MinigameClient.Instance.AllPlayersReady())
            {
                GameObject inst = MinigameClient.Instance.NetworkInstantiate(obj);
                inst.transform.position = this.transform.position;
                doSpawn = false;
                Destroy(this.gameObject);
            }
        }
	}
}
