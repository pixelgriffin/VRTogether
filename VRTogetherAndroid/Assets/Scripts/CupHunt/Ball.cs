using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private GameObject sounds;

    private void Awake()
    {
        sounds = GameObject.Find("CupHuntSounds");
        if (sounds == null)
            Debug.Log("SOUNDS IS NULL");
        else Debug.Log("SOUNDS IS OK");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playBallBounce();
            Destroy(soundObject, 5);
        }
    }
}
