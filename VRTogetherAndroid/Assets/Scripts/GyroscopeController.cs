using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeController : MonoBehaviour {

    public float beta = 0.1f;

    Quaternion orientation;

	// Use this for initialization
	void Start () {
        Input.gyro.enabled = true;
        orientation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 accelInput = Input.acceleration;
        Vector3 gyroInput = Mathf.Deg2Rad * (Input.gyro.attitude.eulerAngles);
        FuseSensorData(gyroInput, accelInput);
        /*
        Quaternion orientation = Input.gyro.attitude;
        transform.localRotation = Quaternion.Euler(90, 0, 0);
        transform.localRotation *= new Quaternion(
            orientation.x,
            orientation.y,
            -orientation.z,
            -orientation.w);
        */
        //transform.localRotation *= new Quaternion(0,0,1,0);
        transform.localRotation = orientation;
	}

    private void FuseSensorData(Vector3 gyro, Vector3 acc)
    {
        Quaternion q = orientation;
        Quaternion qDot;
        Vector4 stepMagnitude;

        //normalize accelerometer measurement
        acc.Normalize();

        //get step magnitude then normalize
        stepMagnitude.x = 4f * q.x * q.z * q.z + 2f * q.z + acc.x - 2f * q.y * acc.y;
        stepMagnitude.y = 4f * q.y * q.w * q.w - 2f * q.w * acc.x + 4f * q.x * q.x * q.y - 2f * q.x * acc.y - 4f * q.y + 8f * q.y * q.y * q.y + 8f * q.y * q.z * q.z + 4f * q.y + acc.z;
        stepMagnitude.z = 4f * q.x * q.x * q.z + 2f * q.x * acc.x + 4f * q.z * q.w * q.w - 2f * q.w * acc.y - 4f * q.z + 8f * q.z * q.y * q.y + 8f * q.z * q.z * q.z + 4f * q.z * acc.z;
        stepMagnitude.w = 4f * q.y * q.y * q.w - 2f * q.y * acc.x + 4f * q.z * q.z * q.w - 2f * q.z * acc.y;
        stepMagnitude.Normalize();

        //compute rate of change of quaternion
        qDot.x = 0.5f * (-q.y * gyro.x - q.z * gyro.y - q.w * gyro.z) - beta * stepMagnitude.x;
        qDot.y = 0.5f * (q.x * gyro.x + q.z * gyro.z - q.w * gyro.y) - beta * stepMagnitude.y;
        qDot.z = 0.5f * (q.x * gyro.y - q.y * gyro.z + q.w * gyro.x) - beta * stepMagnitude.z;
        qDot.w = 0.5f * (q.x * gyro.z + q.y * gyro.y - q.z * gyro.x) - beta * stepMagnitude.w;

        //apply time
        q.x += qDot.x * Time.deltaTime;
        q.y += qDot.y * Time.deltaTime;
        q.z += qDot.z * Time.deltaTime;
        q.w += qDot.w * Time.deltaTime;
        q.Normalize();

        //final quaternion of fused data
        orientation = q;
    }
}
