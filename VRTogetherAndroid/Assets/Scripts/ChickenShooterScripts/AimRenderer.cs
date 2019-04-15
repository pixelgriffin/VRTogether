using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class AimRenderer : MonoBehaviour {

    private LineRenderer lineRenderer;

    private Vector3 aimStartPos;
    private Vector3 aimEndPos;

    private NetworkID id;
    private NetworkFloat[] networkedAimAssist = new NetworkFloat[6];

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
