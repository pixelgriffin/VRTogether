using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

using VRTogether.Net;

public class TablePositioner : MonoBehaviour {
    public SteamVR_PlayArea area;
    public float offset = 10;
    public bool spawned = false;
    public GameObject table;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        if (!spawned && MinigameServer.Instance.AllPlayersReady())
        {
            spawned = true;

            GameObject spawnedTable = MinigameServer.Instance.NetworkInstantiate(table);
            spawnedTable.transform.position = area.vertices[0] + Vector3.forward * offset;
            spawnedTable.transform.position = new Vector3(spawnedTable.transform.position.x, spawnedTable.transform.position.y, -0.8f);

        }

		
	}
}
