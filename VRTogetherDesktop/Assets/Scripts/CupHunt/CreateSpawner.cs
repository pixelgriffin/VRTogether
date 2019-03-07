using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class CreateSpawner : MonoBehaviour {

    public GameObject spawner;

    public bool spawned = false;

    private void Update()
    {
        if (!spawned && MinigameServer.Instance.AllPlayersReady())
        {
            GameObject spawnedSpawner = MinigameServer.Instance.NetworkInstantiate(spawner);
            spawnedSpawner.transform.position = new Vector3(0, 20, 0);

            spawned = true;

        }
    }
}
