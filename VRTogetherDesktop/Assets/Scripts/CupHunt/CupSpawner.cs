using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class CupSpawner : MonoBehaviour {

    public NetworkString spawnString = new NetworkString("spawnString", string.Empty);
    private NetworkID id;
    private bool processedSpawn = false;

	// Use this for initialization
	void Start () 
    {
        id = GetComponent<NetworkID>();

        MinigameServer.Instance.RegisterVariable (id.netID, spawnString);
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (!processedSpawn && spawnString.value == string.Empty && MinigameServer.Instance.AllPlayersReady())
        {
            int spawnIndex = 0;

            foreach (MacrogamePlayer player in MacrogameServer.Instance.GetMacroPlayers())
            {
                spawnString.value += player.name + "$" + spawnIndex + "%";

                spawnIndex++;

            }

            Debug.Log("Sent String: " + spawnString.value);

            MinigameServer.Instance.SendStringToAll (spawnString);

            processedSpawn = true;

        }

    }
}
