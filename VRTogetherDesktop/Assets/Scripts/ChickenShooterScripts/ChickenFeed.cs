using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenFeed : MonoBehaviour {

    public float secondsToDestroy = 1.0f;
    private float destroyTimer;

	// Use this for initialization
	void Start () {

        destroyTimer = 0.0f;
		
	}
	
	// Update is called once per frame
	void Update () {

        destroyTimer += Time.deltaTime;

        if (destroyTimer >= secondsToDestroy)
        {
            MinigameServer.Instance.NetworkDestroy(gameObject);
        }

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
