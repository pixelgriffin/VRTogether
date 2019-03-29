using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallThrow : MonoBehaviour {
    public GameObject ball;

    private Hand thisHand;
    private Transform point;
    private SteamVR_Input_Sources source;
    private GameObject heldBall = null;

	// Use this for initialization
	void Start () 
    {
        thisHand = GetComponent<Hand>();
        source = thisHand.handType;
        point = thisHand.transform;
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Don't do anything if the players aren't ready
        if (!MinigameServer.Instance.AllPlayersReady())
        {
            return;

        }

        // Held
        if (heldBall == null && SteamVR_Input._default.inActions.GrabPinch.GetStateDown(source))
        {
            heldBall = MinigameServer.Instance.NetworkInstantiate(ball);
            heldBall.transform.position = thisHand.transform.position;
            heldBall.transform.rotation = thisHand.transform.rotation;
            heldBall.GetComponent<Rigidbody>().isKinematic = true;

        }

        if (heldBall)
        {
            heldBall.GetComponent<Rigidbody>().position = point.position;
            heldBall.transform.rotation = thisHand.transform.rotation;

        }

        // Released
        if (heldBall && SteamVR_Input._default.inActions.GrabPinch.GetStateUp(source))
        {
            heldBall.GetComponent<Rigidbody>().isKinematic = false;
            heldBall.GetComponent<Rigidbody>().velocity = thisHand.GetTrackedObjectVelocity();
            heldBall.GetComponent<Rigidbody>().angularVelocity = thisHand.GetTrackedObjectAngularVelocity();
            
            heldBall = null;

        }
    }
}
