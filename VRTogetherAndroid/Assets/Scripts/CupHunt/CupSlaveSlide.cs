using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class CupSlaveSlide : MonoBehaviour {

    private GameObject sounds;

    private Vector3 lastPosition;

    private float slideTimer;
    private float slideInterval;

    private NetworkID id;

	void Start () {

        id = GetComponent<NetworkID>();

        if (MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            sounds = GameObject.Find("CupHuntSounds");
            if (sounds == null)
                Debug.Log("SOUNDS IS NULL");
            else Debug.Log("SOUNDS IS OK");

            lastPosition = transform.position;

            slideInterval = 1.0f;
            slideTimer = 0.0f;
        }
		
	}
	
	void Update () {

        if (MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            float deltaPos = Vector3.Distance(transform.position, lastPosition);

            if (deltaPos > 0.1f && slideTimer >= slideInterval)
            {
                // reset timer
                slideTimer = 0.0f;

                // play slide sound
                GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
                soundObject.GetComponent<Sounds>().playCupSlide();
                Destroy(soundObject, 5);
            }

            Debug.Log(transform.position);
            lastPosition = transform.position;
            slideTimer += Time.deltaTime;
        }
		
	}
}
