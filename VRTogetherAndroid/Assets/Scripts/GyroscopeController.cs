using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Input.gyro.enabled = true;
        transform.localRotation = Quaternion.Euler(90f, 90f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

        Quaternion orientation = Input.gyro.attitude;
        transform.localRotation = new Quaternion(
            orientation.x,
            orientation.y,
            -orientation.z,
            -orientation.w);

	}
}
