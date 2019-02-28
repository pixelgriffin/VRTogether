using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;
using Valve.VR;

public class PlayerController : MonoBehaviour {

    public GameObject chickenFeed;
    public float feedForce;

    private LineRenderer aimAssist;

    private Vector3 aimStartPos;
    private Vector3 aimEndPos;

	// Use this for initialization
	void Start () {

        aimAssist = GetComponent<LineRenderer>();
        aimAssist.enabled = true;

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

        Vector3[] positions = { aimStartPos, aimEndPos };
        aimAssist.SetPositions(positions);

        float distance = Vector3.Distance(aimStartPos, aimEndPos);
        aimAssist.materials[0].mainTextureScale = new Vector3(distance, 1, 1);

        // check for input
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            GameObject feedInstance = Instantiate(chickenFeed, transform.position, Quaternion.identity);
            feedInstance.GetComponent<Rigidbody>().AddForce(transform.forward * feedForce);
        }

    }
}
