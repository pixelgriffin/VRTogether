using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class CupSpawner : MonoBehaviour
{
    private NetworkString spawnString = new NetworkString("spawnString", string.Empty);

    public GameObject obj;

    public Transform[] spawnPoints;

    public bool processedSpawn = false;
    private NetworkID id;

    void Start()
    {
        id = GetComponent<NetworkID>();

        MinigameClient.Instance.RegisterVariable(id.netID, spawnString);

    }

    void Update()
    {
        if (!processedSpawn && spawnString.value != string.Empty && MinigameClient.Instance.AllPlayersReady())
        {
            Debug.Log("Recieved String: " + spawnString.value);

            string[] posTuples = spawnString.value.Split('%');  // Splits into "<name>$<index>"
            int spawnIndex = -1;

            foreach (string tup in posTuples)
            {
                if (tup.Split('$')[0] == MinigameClient.Instance.name)
                {
                    spawnIndex = int.Parse(tup.Split('$')[1]);

                    break;

                }
            }

            if (spawnIndex == -1)
            {
                Debug.LogError("Could not find index in spawn string: " + spawnString.value);

            }


            GameObject inst = MinigameClient.Instance.NetworkInstantiate(obj);
            inst.transform.position = spawnPoints[spawnIndex].position;
            processedSpawn = true;
            Destroy(this.gameObject);
        } else
        {
            Debug.Log(processedSpawn.ToString() + "---" + spawnString.value + "---" + MinigameClient.Instance.AllPlayersReady().ToString());

        }
    }
}
