using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class NetworkFly : MonoBehaviour {

    public GameObject body;
    public GameObject grape;

    private NetworkID id;

    private NetworkBool holdingGrape = new NetworkBool("holdingGrape", false);

    private Camera camera;

	void Start () {
        id = GetComponent<NetworkID>();
        MinigameClient.Instance.RegisterVariable(id.netID, holdingGrape);

        //create a camera with the same transform as the fly and parent it
        GameObject cameraObject = Instantiate(GameObject.Find("EmptyObject"), this.transform);
        camera = cameraObject.AddComponent<Camera>();
        camera.name = "camera_" + id.netID;

        //if this is not a slave, set camera to active and enable joystick canvas
        if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            GameObject.Find("FlyOverviewCamera").GetComponent<Camera>().enabled = false;
            camera.enabled = true;
            GameObject.Find("RightJoystickCanvas").GetComponent<Canvas>().enabled = true;
        }
    }
	
	void Update () {
        if (MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID)) //if this is a slave
        {
            body.SetActive(true); //show body
        }
        else //if this is us
        {
            body.SetActive(false); //don't show body
        }

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
