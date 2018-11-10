using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationInterpolator : MonoBehaviour {

    public float interpSpeed = 5f;

    public Vector3 desiredPos;
    public Quaternion desiredRot;

    private void Start()
    {
        desiredPos = this.transform.position;
        desiredRot = this.transform.rotation;
    }

    void Update () {
        this.transform.position = Vector3.Lerp(this.transform.position, desiredPos, Time.deltaTime * interpSpeed);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, desiredRot, Time.deltaTime * interpSpeed);
	}
}
