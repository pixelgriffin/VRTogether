using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class FlyDestroyer : MonoBehaviour {

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Fly")
        {
            MinigameServer.Instance.NetworkDestroy(collision.gameObject);
        }
    }
}
