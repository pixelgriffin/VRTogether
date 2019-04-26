using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupHitDetector : MonoBehaviour {

    private GameObject sounds;

    private void Awake()
    {
        sounds = GameObject.Find("CupHuntSounds");
        if (sounds == null)
            Debug.Log("SOUNDS IS NULL");
        else Debug.Log("SOUNDS IS OK");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Ball"))
        {
            // play sound effect
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playCupHit();
            Destroy(soundObject, 5);
        }
    }
}
