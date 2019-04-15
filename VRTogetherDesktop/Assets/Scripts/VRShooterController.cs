using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;
using Valve.VR;

public class VRShooterController : MonoBehaviour {

    public GameObject projectile;
    public float projectileForce;
    public float shootTimer = 1.0f;

    public float aimAssistDistance = 10.0f;
    private LineRenderer aimAssist;

    private Vector3 aimStartPos;
    private Vector3 aimEndPos;

    private float timeSinceLastShot;

    private bool networkReady;

    private NetworkID id;
    private NetworkFloat[] networkedAimAssist = new NetworkFloat[6];

	// Use this for initialization
	void Start () {

        aimAssist = GetComponent<LineRenderer>();
        aimAssist.enabled = true;
        timeSinceLastShot = shootTimer;

        networkReady = false;

        id = GetComponent<NetworkID>();
        for (int i = 0; i < 6; i++)
        {
            networkedAimAssist[i] = new NetworkFloat(
                "networkedAimAssist" + i,
                0.0f);

            MinigameServer.Instance.RegisterVariable(
                id.netID,
                networkedAimAssist[i]);
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (!networkReady && MinigameServer.Instance.AllPlayersReady())
        {
            networkReady = true;
        }

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, aimAssistDistance);
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
            aimEndPos = transform.position + transform.forward * aimAssistDistance;
        }

        // draw the line
        Vector3[] positions = { aimStartPos, aimEndPos };
        aimAssist.SetPositions(positions);

        // set the networked aim assist values
        networkedAimAssist[0].value = aimStartPos.x;
        networkedAimAssist[1].value = aimStartPos.y;
        networkedAimAssist[2].value = aimStartPos.z;
        networkedAimAssist[3].value = aimEndPos.x;
        networkedAimAssist[4].value = aimEndPos.y;
        networkedAimAssist[5].value = aimEndPos.z;

        // send the line over the network
        if (networkReady)
        {
            for (int i = 0; i < 6; i++)
            {
                MinigameServer.Instance.SendFloatToAll(networkedAimAssist[i]);
            }
        }

        // accumulate timer
        timeSinceLastShot += Time.deltaTime;

        // check for input
        if ((SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand) ||
            Input.GetKey(KeyCode.Space)) &&
            timeSinceLastShot >= shootTimer)
        {
            // spawn bullet over network (make sure bullet is a networked prefab)
            if (networkReady)
            {
                GameObject feedInstance = MinigameServer.Instance.NetworkInstantiate(projectile);
                feedInstance.transform.position = this.transform.position;
                feedInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce);
            }

            // reset timer
            timeSinceLastShot = 0.0f;
        }

    }
}
