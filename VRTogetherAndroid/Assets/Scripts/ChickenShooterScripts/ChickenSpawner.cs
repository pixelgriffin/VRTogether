using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenSpawner : MonoBehaviour {

    public GameObject chicken;
    public GameObject parent;

    private bool doSpawn = true;

    void Update()
    {
        if (doSpawn)
        {
            if (MinigameClient.Instance.AllPlayersReady())
            {
                // instantiate non-networked parent
                GameObject parentInstance = Instantiate(
                    parent, 
                    Vector3.zero,
                    Quaternion.identity);

                // instantiate networked chicken
                GameObject chickenInstance = MinigameClient.Instance.NetworkInstantiate(chicken);
                chickenInstance.transform.SetParent(parentInstance.transform, true);
                chickenInstance.transform.position =  new Vector3(0f, 1f, 10.5f);
                chickenInstance.transform.Rotate(0f, 90f, 0f);
                parentInstance.transform.Rotate(Vector3.up, 10);


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
