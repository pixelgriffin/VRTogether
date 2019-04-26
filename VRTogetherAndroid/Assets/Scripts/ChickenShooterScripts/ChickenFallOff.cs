using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenFallOff : MonoBehaviour {

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision");

        if (collider.gameObject.layer == 14)
        {
            MinigameClient.Instance.NetworkDestroy(collider.gameObject);
        }
    }
}
