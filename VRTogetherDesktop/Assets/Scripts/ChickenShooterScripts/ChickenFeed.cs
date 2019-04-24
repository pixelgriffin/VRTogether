using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenFeed : MonoBehaviour {

    public float secondsToDestroy = 1.0f;
    private float destroyTimer;

    private AudioSource wallCollisionSound;

	// Use this for initialization
	void Start () {

        destroyTimer = 0.0f;

        wallCollisionSound = GetComponent<AudioSource>();
		
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
        else
        {
            // play the wall collision sound effect
            wallCollisionSound.Play();
        }
    }
}
