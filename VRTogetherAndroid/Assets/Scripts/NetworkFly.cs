using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class NetworkFly : MonoBehaviour {

    public GameObject body;
    public GameObject grape;
    public GameObject flyDeathObject;

    private NetworkID id;
    private bool isSlave = false;
    private NetworkBool holdingGrape = new NetworkBool("holdingGrape", false);
    private bool wasHoldingGrape;

    //private RightJoystickTouchContoller joystick;
    private Camera camera;
    private Camera overviewCamera;
    private Canvas flyCanvas, joystickCanvas;

    private AudioSource pickupSound;

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
            holdingGrape.value = false;

            //joystick = GameObject.Find("RightJoystickTouchController").GetComponent<RightJoystickTouchContoller>();

            // create a camera with the same transform as the fly and parent it
            GameObject cameraObject = Instantiate(GameObject.Find("EmptyObject"), this.transform);
            camera = cameraObject.AddComponent<Camera>();
            camera.name = "camera_" + id.netID;
            camera.nearClipPlane = 0.1f;

            // enable the camera
            overviewCamera = GameObject.Find("FlyOverviewCamera").GetComponent<Camera>();
            overviewCamera.enabled = false;
            camera.enabled = true;

            // enable the fly canvas
            flyCanvas = GameObject.Find("FlyCanvas").GetComponent<Canvas>();
            flyCanvas.enabled = true;

            // enable the joystick canvas if not using gyro controls
            joystickCanvas = GameObject.Find("JoystickCanvas").GetComponent<Canvas>();
            if (!GetComponent<FlyControlSwitch>().useGyroControls)
            {
                joystickCanvas.enabled = true;
            }
        }

        // get the pick up sound
        AudioSource[] sources = GetComponents<AudioSource>();
        pickupSound = sources[1];

        wasHoldingGrape = false;
    }
	
	void Update () {
        if (isSlave) //if this is a slave
        {
            body.SetActive(true); //show body

            if (!wasHoldingGrape && holdingGrape.value)
            {
                // play pickup sound effect for the slave fly
                pickupSound.Play();
            }
        }
        else //if this is us
        {
            body.SetActive(false); //don't show body
        }

        grape.SetActive(holdingGrape.value);//If we are holding a grape then show a grape
        wasHoldingGrape = holdingGrape.value;
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

                // play pickup sound effect for authority fly
                pickupSound.Play();
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
            // disable the joystick if not using gyro
            if (!GetComponent<FlyControlSwitch>().useGyroControls)
            {
                joystickCanvas.enabled = false;
            }

            // enable the overview camera
            //camera.enabled = false;
            overviewCamera.enabled = true;
        }

        Instantiate(flyDeathObject, transform.position, Quaternion.identity);
    }
}
