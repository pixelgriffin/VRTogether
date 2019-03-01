using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;
using Valve.VR;

public class PlayerController : MonoBehaviour {

    public GameObject chickenFeed;
    public float feedForce;
    public float shootTimer = 2.0f;

    private LineRenderer aimAssist;

    private Vector3 aimStartPos;
    private Vector3 aimEndPos;

    private float timeSinceLastShot;

	// Use this for initialization
	void Start () {

        aimAssist = GetComponent<LineRenderer>();
        aimAssist.enabled = true;
        timeSinceLastShot = shootTimer;

    }
	
	// Update is called once per frame
	void Update () {

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, 10f);
        //Debug.DrawRay(transform.position, transform.forward * 10, Color.yellow);

        // set start position for aim assist line
        aimStartPos = transform.position;

        // set end position for aim assist line
        if (hit)
        {
            // if raycast hit something, set end position to that thing              
            aimEndPos = hitInfo.point;
        }
        else
        {
            // default end position is the forward vector * radius of level
            aimEndPos = transform.position + transform.forward * 10f;
        }

        // draw the line
        Vector3[] positions = { aimStartPos, aimEndPos };
        aimAssist.SetPositions(positions);

        // accumulate timer
        timeSinceLastShot += Time.deltaTime;

        // check for input
        if ((SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand) ||
            Input.GetKey(KeyCode.Space)) &&
            timeSinceLastShot >= shootTimer)
        {
            // spawn bullet over network (make sure bullet is a networked prefab)
            if (MinigameServer.Instance.AllPlayersReady())
            {
                GameObject feedInstance = MinigameServer.Instance.NetworkInstantiate(chickenFeed);
                feedInstance.transform.position = this.transform.position;
                feedInstance.GetComponent<Rigidbody>().AddForce(transform.forward * feedForce);
            }

            // reset timer
            timeSinceLastShot = 0.0f;
        }

    }
}
