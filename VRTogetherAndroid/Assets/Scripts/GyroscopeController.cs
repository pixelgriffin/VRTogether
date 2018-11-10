using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

        Quaternion orientation = Input.gyro.attitude;
        transform.rotation = new Quaternion(
            orientation.x,
            orientation.y,
            -orientation.z,
            -orientation.w);

	}
}
