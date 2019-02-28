using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenFeed : MonoBehaviour {

    public float secondsToDestroy = 1.0f;

	// Use this for initialization
	void Start () {

        Destroy(this.gameObject, secondsToDestroy);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            // destroy game object
            MinigameServer.Instance.NetworkDestroy(collision.gameObject);
        }
    }
}
