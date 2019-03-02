using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenSpawnCamera : MonoBehaviour {

    public Vector3 cameraPos = Vector3.zero;

    private NetworkID id;

    private Camera camera;

    // Use this for initialization
    void Start()
    {

        id = GetComponent<NetworkID>();

        if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {

            //create a camera with the same transform as the chicken and parent it
            GameObject cameraObject = Instantiate(
                GameObject.Find("EmptyObject"),
                this.transform);
            cameraObject.transform.localPosition = cameraPos;
            cameraObject.transform.LookAt(this.transform);
            camera = cameraObject.AddComponent<Camera>();
            camera.name = "camera_" + id.netID;

            GameObject.Find("ChickenOverviewCamera").GetComponent<Camera>().enabled = false;
            camera.enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
