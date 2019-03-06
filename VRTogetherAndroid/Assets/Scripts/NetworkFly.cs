using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class NetworkFly : MonoBehaviour {

    public GameObject body;
    public GameObject grape;

    private NetworkID id;
    private bool isSlave = false;
    private NetworkBool holdingGrape = new NetworkBool("holdingGrape", false);

    private Camera camera;
    private Camera overviewCamera;
    private Canvas canvas;

	void Start () {

        id = GetComponent<NetworkID>();

        if (MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            isSlave = true;
        }

        MinigameClient.Instance.RegisterVariable(id.netID, holdingGrape);

        // if this is not a slave, set camera to active and enable joystick canvas if using joystick controls
        if (!isSlave)
        {
            // create a camera with the same transform as the fly and parent it
            GameObject cameraObject = Instantiate(GameObject.Find("EmptyObject"), this.transform);
            camera = cameraObject.AddComponent<Camera>();
            camera.name = "camera_" + id.netID;
            camera.nearClipPlane = 0.1f;

            // enable the camera
            overviewCamera = GameObject.Find("FlyOverviewCamera").GetComponent<Camera>();
            overviewCamera.enabled = false;
            camera.enabled = true;

            // enable the canvas if not using gyro
            if (!GetComponent<FlyControlSwitch>().useGyroControls)
            {
                canvas = GameObject.Find("RightJoystickCanvas").GetComponent<Canvas>();
                canvas.enabled = true;
            }
        }
    }
	
	void Update () {
        if (isSlave) //if this is a slave
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
            if (!isSlave && !holdingGrape.value)
            {
                holdingGrape.value = true;//Change the local value since we are authoritative
                MinigameClient.Instance.SendBooleanToAll(holdingGrape);//Update the variable over the network
                Debug.Log("Picked up a grape!");
            }
        }
        else if(collider.tag == "DropZone")
        {
            //If it is us that touched the drop zone
            if (!isSlave)
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

    public void OnDestroy()
    {
        if (!isSlave)
        {
            // disable the canvas if not using gyro
            if (!GetComponent<FlyControlSwitch>().useGyroControls)
            {
                canvas = GameObject.Find("RightJoystickCanvas").GetComponent<Canvas>();
                canvas.enabled = true;
            }

            // enable the overview camera
            camera.enabled = false;
            overviewCamera.enabled = true;
        }
    }
}
