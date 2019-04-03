using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownForceZone : MonoBehaviour {

    public float force = 10;

    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce(force * Vector3.down, ForceMode.Acceleration);
    }
}
