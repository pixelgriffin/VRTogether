using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class CupSpawner : MonoBehaviour
{
    public GameObject obj;

    public Transform[] spawnPoints;

    private bool doSpawn = true;

    void Start()
    {


    }

    void Update()
    {
        if (doSpawn)
        {
            if (MinigameClient.Instance.AllPlayersReady())
            {
                GameObject inst = MinigameClient.Instance.NetworkInstantiate(obj);
                inst.transform.position = spawnPoints[Random.Range(0, 9)].position;
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
