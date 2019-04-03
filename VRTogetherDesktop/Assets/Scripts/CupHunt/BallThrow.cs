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

    public List<GameObject> balls;

	// Use this for initialization
	void Start () 
    {
        balls = new List<GameObject>();

        thisHand = GetComponent<Hand>();
        source = thisHand.handType;
        point = thisHand.transform;

        
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Held
        if (heldBall == null && SteamVR_Input._default.inActions.GrabPinch.GetStateDown(source) && balls.Count < 10)
        {
            heldBall = MinigameServer.Instance.NetworkInstantiate(ball);
            //heldBall = Instantiate(ball);
            heldBall.GetComponent<Ball>().thrower = this;
            heldBall.transform.position = thisHand.transform.position + thisHand.transform.up * 3 + thisHand.transform.forward * 3;
            heldBall.transform.rotation = thisHand.transform.rotation;
            heldBall.GetComponent<Rigidbody>().isKinematic = true;
            balls.Add(heldBall);

        }

        if (heldBall)
        {
            heldBall.transform.position = point.position;
            heldBall.transform.rotation = thisHand.transform.rotation;

        }

        // Released
        if (heldBall && SteamVR_Input._default.inActions.GrabPinch.GetStateUp(source))
        {
            heldBall.GetComponent<Rigidbody>().isKinematic = false;
            heldBall.GetComponent<Rigidbody>().velocity = thisHand.GetTrackedObjectVelocity();
            heldBall.GetComponent<Rigidbody>().angularVelocity = thisHand.GetTrackedObjectAngularVelocity();
            heldBall.GetComponent<Ball>().enabled = true;
            heldBall.GetComponent<Ball>().BeginDestroyTimer();

            heldBall = null;

        }
    }
}
