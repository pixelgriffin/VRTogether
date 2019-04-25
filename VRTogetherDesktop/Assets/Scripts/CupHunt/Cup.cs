using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class Cup : MonoBehaviour {

    private GameObject sounds;

    private void Start()
    {
        sounds = GameObject.Find("CupHuntSounds");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playBallSink();
            Destroy(soundObject, 5);

        }

    }

}
