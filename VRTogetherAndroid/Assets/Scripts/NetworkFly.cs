using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class NetworkFly : MonoBehaviour {

    public GameObject body;
    public GameObject grape;

    private NetworkID id;

    private NetworkBool holdingGrape = new NetworkBool("holdingGrape", false);

	void Start () {
        id = GetComponent<NetworkID>();
        MinigameClient.Instance.RegisterVariable(id.netID, holdingGrape);
    }
	
	void Update () {
        body.SetActive(MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID));//If this is not a slave then it is us, and we don't want to see the body
        grape.SetActive(holdingGrape.value);//If we are holding a grape then show a grape
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Grape")
        {
            //If we control this fly we should tell everyone else we now are holding a grape
            if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
            {
                holdingGrape.value = true;//Change the local value since we are authoritative
                MinigameClient.Instance.SendBooleanToAll(holdingGrape);//Update the variable over the network
            }
        }
        else if(collision.collider.tag == "DropZone")
        {
            //If it is us that touched the drop zone
            if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
            {
                if (holdingGrape.value)
                {
                    holdingGrape.value = false;
                    MinigameClient.Instance.SendBooleanToAll(holdingGrape);//Update the variable over the network

                    ScoreCounter counter = FindObjectOfType<ScoreCounter>();
                    counter.flyScore.value++;
                    MinigameClient.Instance.SendIntegerToAll(counter.flyScore);
                }
            }
        }
    }
}
