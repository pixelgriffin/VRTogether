using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenDestroy : MonoBehaviour {

    public GameObject chickenDeathEffects;

    private GameObject levelManager;

    private Vector3 oldPos;
    private Vector3 posOffset;

    void Start()
    {
        oldPos = this.transform.position;
        posOffset = Vector3.up * 0.5f;

        levelManager = GameObject.Find("LevelManager");
    }

    private void Update()
    {
        /*
        if (this.transform.position != oldPos)
        {
            this.transform.GetChild(0).localPosition = (Vector3.up * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10f)) * 0.2f) - posOffset;
        }
        else
        {
            this.transform.GetChild(0).localPosition = Vector3.MoveTowards(this.transform.GetChild(0).localPosition, -posOffset, Time.deltaTime * 5f);
        }

        oldPos = this.transform.position;
        */       
    }

    public void OnDestroy()
    {
        // instantiate the death effects
        GameObject chickenDeathObject = Instantiate(
            chickenDeathEffects, 
            transform.position,
            Quaternion.identity);

        // get the death sound
        AudioSource chickenDeathSound = 
            GetComponent<ChickenSounds>().GetDeathSource();

        // add a audio source component to the death object
        chickenDeathObject.AddComponent<AudioSource>();
        AudioSource chickenDeathSoundCopy = 
            chickenDeathObject.GetComponent<AudioSource>();

        // copy each field in the components
        chickenDeathSoundCopy.clip = chickenDeathSound.clip;
        chickenDeathSoundCopy.outputAudioMixerGroup = chickenDeathSound.outputAudioMixerGroup;
        chickenDeathSoundCopy.spatialBlend = chickenDeathSound.spatialBlend;
        chickenDeathSoundCopy.rolloffMode = chickenDeathSound.rolloffMode;
        chickenDeathSoundCopy.minDistance = chickenDeathSound.minDistance;
        chickenDeathSoundCopy.maxDistance = chickenDeathSound.maxDistance;

        // play the sound
        chickenDeathSoundCopy.Play();

        // destroy the object after 5 seconds
        Destroy(chickenDeathObject, 5.0f);

        // decrement the players alive count
        levelManager.GetComponent<LevelManager>().DecrPlayersAliveCount();
    }
}
