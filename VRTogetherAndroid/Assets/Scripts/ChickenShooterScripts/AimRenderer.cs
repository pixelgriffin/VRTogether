using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class AimRenderer : MonoBehaviour {

    private LineRenderer lineRenderer;

    private Vector3 aimStartPos;
    private Vector3 aimEndPos;

    private bool networkReady;

    private NetworkID id;
    private NetworkFloat[] networkedAimAssist = new NetworkFloat[6];

    // Use this for initialization
    void Start () {

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;

        networkReady = false;

        id = GetComponent<NetworkID>();
        for (int i = 0; i < 6; i++)
        {
            networkedAimAssist[i] = new NetworkFloat(
                "networkedAimAssist" + i,
                0.0f);

            MinigameClient.Instance.RegisterVariable(
                id.netID,
                networkedAimAssist[i]);
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (!networkReady && MinigameClient.Instance.AllPlayersReady())
        {
            networkReady = true;
            Debug.Log("network ready");
        }

        if (networkReady)
        {
            // set aim positions
            aimStartPos.x = networkedAimAssist[0].value;
            aimStartPos.y = networkedAimAssist[1].value;
            aimStartPos.z = networkedAimAssist[2].value;
            aimEndPos.x = networkedAimAssist[3].value;
            aimEndPos.y = networkedAimAssist[4].value;
            aimEndPos.z = networkedAimAssist[5].value;

            //Debug.Log("Aim start: " + aimStartPos);
            //Debug.Log("Aim end: " + aimEndPos);

            // draw the line
            Vector3[] positions = { aimStartPos, aimEndPos };
            lineRenderer.SetPositions(positions);
        }
		
	}
}
