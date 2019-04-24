using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSounds : MonoBehaviour {

    private GameObject soundManager;
    private AudioClip runSound, jumpSound, deathSound;
    private AudioSource runSource, jumpSource, deathSource;

    private void Start()
    {
        soundManager = GameObject.Find("SoundManager");

        // generate who's voice this chicken will be using
        int sourceIndex = Random.Range(0, 5);
        Debug.Log(sourceIndex + " will be this chicken");
        runSound = soundManager.transform.GetChild(0).GetChild(sourceIndex).GetComponent<AudioSource>().clip;

        AudioSource[] sources = GetComponents<AudioSource>();
        runSource = sources[0];
        jumpSource = sources[1];
        deathSource = sources[2];

        runSource.clip = runSound;
    }

    public void PlayRunSound()
    {
        // slightly randomize the pitch
        runSource.pitch = 1;
        runSource.pitch += Random.Range(0.0f, 0.25f);

        // play the sound
        runSource.Play();
        Debug.Log("Played run sound with pitch " + runSource.pitch);
    }
}
