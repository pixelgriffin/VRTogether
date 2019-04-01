using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenOnDestroy : MonoBehaviour {

    private NetworkID id;

    private Vector3 oldPos;

    private void Start()
    {
        oldPos = this.transform.position;

        // check this id
        id = GetComponent<NetworkID>();
        if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            Debug.Log("Me: " + id.netID);
        }
        else
        {
            Debug.Log(id.netID);
        }
    }

    private void Update()
    {
        if (MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            if (this.transform.position != oldPos)
            {
                this.transform.GetChild(0).localPosition = Vector3.up * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10f)) * 0.2f;
            }
            else
            {
                this.transform.GetChild(0).localPosition = Vector3.MoveTowards(this.transform.GetChild(0).localPosition, Vector3.zero, Time.deltaTime * 5f);
            }

            oldPos = this.transform.position;
        }
    }

    public void OnDestroy()
    {
        Debug.Log(id.netID + " being destroyed");

        if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            GameObject canvas;

            //GameObject currentCamera = gameObject.transform.GetChild(0).gameObject;

            // destroy the controller
            Debug.Log("Chicken being destroyed");
            Destroy(transform.parent.gameObject);

            // disable control canvas
            canvas = GameObject.Find("Canvas");
            canvas.GetComponent<Canvas>().enabled = false;
        }

        // if this slave/auth has more than one child (these children are always cameras)
        if (transform.childCount > 0)
        {
            GameObject nextCamera;

            // get all chickens
            GameObject[] chickens = GameObject.FindGameObjectsWithTag("Chicken");
            Debug.Log(chickens.Length);

            // disable current camera
            transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;

            if (chickens.Length == 0)
            {
                // if all chickens fed, set camera to overview until macro scene loaded
                nextCamera = GameObject.Find("ChickenOverviewCamera");
                nextCamera.GetComponent<Camera>().enabled = true;
            }
            else
            {
                // create a camera with the same transform as the next chicken and parent it
                nextCamera = Instantiate(
                    GameObject.Find("EmptyObject"),
                    chickens[0].transform);
                nextCamera.transform.localPosition = GetComponent<ChickenSpawnCamera>().cameraPos;
                nextCamera.transform.LookAt(chickens[0].transform);
                Camera camera = nextCamera.AddComponent<Camera>();
                camera.name = "camera_" + id.netID;

                // spectate surviving chicken
                nextCamera.GetComponent<Camera>().enabled = true;
            }
        }
    }
}
