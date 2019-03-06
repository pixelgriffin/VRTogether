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

        //if this is not a slave, set camera to active and enable joystick canvas if using joystick controls
        if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            //create a camera with the same transform as the fly and parent it
            GameObject cameraObject = Instantiate(GameObject.Find("EmptyObject"), this.transform);
            camera = cameraObject.AddComponent<Camera>();
            camera.name = "camera_" + id.netID;
            camera.nearClipPlane = 0.1f;

            GameObject.Find("FlyOverviewCamera").GetComponent<Camera>().enabled = true;
            camera.enabled = true;
            if (!GetComponent<FlyControlSwitch>().useGyroControls)
            {
                GameObject.Find("RightJoystickCanvas").GetComponent<Canvas>().enabled = true;
            }
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

    private void OnTriggerEnter(Collider collider)
    {

        if(collider.tag == "Grape")
        {
            //If we control this fly we should tell everyone else we now are holding a grape
            if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID) && !holdingGrape.value)
            {
                holdingGrape.value = true;//Change the local value since we are authoritative
                MinigameClient.Instance.SendBooleanToAll(holdingGrape);//Update the variable over the network
                Debug.Log("Picked up a grape!");
            }
        }
        else if(collider.tag == "DropZone")
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

                    Debug.Log("Score!");
                }
            }
        }
    }
}
