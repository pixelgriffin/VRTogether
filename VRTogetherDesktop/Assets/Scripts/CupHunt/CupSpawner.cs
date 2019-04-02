using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class CupSpawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public GameObject cup;

    int spawnIndex = 0;

    private void Start()
    {
        MinigameServer.Instance.OnAllClientsReady.AddListener(OnClientJoined);
    }

    private void OnDestroy()
    {
        MinigameServer.Instance.OnAllClientsReady.RemoveListener(OnClientJoined);
    }

    private void OnClientJoined()
    {
        Debug.Log("Client joined!");

        foreach(MacrogamePlayer p in MacrogameServer.Instance.GetMacroPlayers())
        {
            //Network instantiate
            MinigameServer.Instance.NetworkRequestInstantiate(cup, spawnPoints[spawnIndex].position, Quaternion.identity, p.connectionID);

            spawnIndex++;
        }
    }
}
