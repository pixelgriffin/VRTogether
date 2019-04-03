using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class CupSpawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public GameObject cup;

    int spawnIndex = 0;

    bool playersSpawned = false;

    private void Start()
    {
        //MinigameServer.Instance.OnAllClientsReady.AddListener(OnClientJoined);
    }

    private void Update()
    {
        if (!playersSpawned && MinigameServer.Instance.AllPlayersReady())
        {
            OnClientJoined();

            playersSpawned = true;

        } else if (playersSpawned)
        {
            GameObject.Destroy(this);

        }
    }

    private void OnDestroy()
    {
        //MinigameServer.Instance.OnAllClientsReady.RemoveListener(OnClientJoined);
    }

    private void OnClientJoined()
    {
        //Debug.Log("Client joined!");

        foreach(MacrogamePlayer p in MacrogameServer.Instance.GetMacroPlayers())
        {
            //Network instantiate
            MinigameServer.Instance.NetworkRequestInstantiate(cup, spawnPoints[spawnIndex].position, Quaternion.identity, p.connectionID);

            spawnIndex++;

            spawnIndex = spawnIndex % 10;
        }
    }
}
