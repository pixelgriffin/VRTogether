using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class Cup : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            MinigameServer.Instance.NetworkDestroy(transform.parent.gameObject);

        }

    }

}
