using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFeed : MonoBehaviour {

    private AudioSource shotFiredSound, wallCollisionSound;

	// Use this for initialization
	void Start () {

        AudioSource[] sources = GetComponents<AudioSource>();

        shotFiredSound = sources[0];
        wallCollisionSound = sources[0];

        shotFiredSound.Play();
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            wallCollisionSound.Play();
        }
    }
}
