using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFeed : MonoBehaviour {

    private AudioSource shotFiredSound, wallCollisionSound;

    public float collisionSoundInterval = 0.5f;
    private float collisionSoundTimer;

	// Use this for initialization
	void Start () {

        AudioSource[] sources = GetComponents<AudioSource>();

        shotFiredSound = sources[0];
        wallCollisionSound = sources[0];

        shotFiredSound.Play();

        collisionSoundTimer = collisionSoundInterval;
		
	}

    private void Update()
    {
        collisionSoundTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ground") && collisionSoundTimer >= collisionSoundInterval)
        {
            wallCollisionSound.Play();
        }
    }
}
