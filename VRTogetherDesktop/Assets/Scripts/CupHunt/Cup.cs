using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class Cup : MonoBehaviour {

    private GameObject cupObject;
    private GameObject sounds;

    private Vector3 lastPosition;

    private float slideTimer;
    private float slideInterval;

    private void Start()
    {
        cupObject = transform.parent.gameObject; 
        sounds = GameObject.Find("CupHuntSounds");
        if (sounds == null)
            Debug.Log("SOUNDS IS NULL");
        else Debug.Log("SOUNDS IS OK");

        lastPosition = cupObject.transform.position;

        slideInterval = 1.0f;
        slideTimer = 0.0f;
    }

    private void Update()
    {
        if (cupObject.transform.position != lastPosition && slideTimer >= slideInterval)
        {
            // reset timer
            slideTimer = 0.0f;

            // play slide sound
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playCupSlide();
            Destroy(soundObject, 5);
        }

        lastPosition = cupObject.transform.position;
        slideTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playBallSink();
            Destroy(soundObject, 5);

            MinigameServer.Instance.NetworkDestroy(cupObject);

        }

    }

}
