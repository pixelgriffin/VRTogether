using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;
using Valve.VR;

public class PlayerController : MonoBehaviour {

    private int feedLayerMask = 1 << 8;
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
        Debug.DrawRay(transform.position, transform.forward * 10, Color.yellow);

        // set start position for aim assist line
        aimStartPos = transform.position;


        // set end position for aim assist line
        if (hit)
        {
            // if raycast hit something, set end position to that thing
            Debug.Log("Aim assist hit something");
            /*
            float distance = Vector3.Distance(
                transform.position,
                hitInfo.transform.position);\
            */               
            aimEndPos = hitInfo.point;
        }
        else
        {
            // default end position is the forward vector * radius of level
            Debug.Log("Hitting nothing");
            aimEndPos = transform.position + transform.forward * 10f;
        }

        Vector3[] positions = { aimStartPos, aimEndPos };
        aimAssist.SetPositions(positions);

        float distance = Vector3.Distance(aimStartPos, aimEndPos);
        aimAssist.materials[0].mainTextureScale = new Vector3(distance, 1, 1);

        transform.Rotate(Vector3.up, 5.0f * Time.deltaTime);
		
	}
}
