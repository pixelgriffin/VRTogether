using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class NetworkFly : MonoBehaviour {

    public GameObject grape;

    //We need a reference to our network id so we can do network stuff later
    private NetworkID id;

    private NetworkBool holdingGrape = new NetworkBool("holdingGrape", false);
    private bool wasHoldingGrape;

    private FlyLevelManager levelManager;

	void Start () {
        id = GetComponent<NetworkID>();

        MinigameServer.Instance.RegisterVariable(id.netID, holdingGrape);
        wasHoldingGrape = false;

        levelManager = GameObject.Find("LevelManager").GetComponent<FlyLevelManager>();
	}
	
	void Update () {
        //Since this script is for the server, we know that the fly will always be a slave, in which case we don't need to do much other than read values
        grape.SetActive(holdingGrape.value);

        // if just picked up a grape
        if (!wasHoldingGrape && holdingGrape.value)
        {
            // play pickup sound
            GetComponent<AudioSource>().Play();
        }

        wasHoldingGrape = holdingGrape.value;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Swatter")
        {
            //Network destroy allows us to request the authority on this object destroy it
            MinigameServer.Instance.NetworkDestroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        levelManager.DecrPlayersAliveCount();
    }
}
