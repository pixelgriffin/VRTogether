using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChickenSounds : MonoBehaviour {

    private GameObject soundManager;

    private AudioClip runSound, jumpSound, deathSound;
    private AudioMixerGroup runGroup, jumpGroup, deathGroup;
    private AudioSource runSource, jumpSource, deathSource;

    private void Start()
    {
        soundManager = GameObject.Find("SoundManager");

        // generate who's voice this chicken will be using
        int sourceIndex = Random.Range(0, 5);
        Debug.Log(sourceIndex + " will be this chicken");

        // get this sound's parameters
        runSound = soundManager.transform.GetChild(0).GetChild(sourceIndex).GetComponent<AudioSource>().clip;
        runGroup = soundManager.transform.GetChild(0).GetChild(sourceIndex).GetComponent<AudioSource>().outputAudioMixerGroup;
        jumpSound = soundManager.transform.GetChild(1).GetChild(sourceIndex).GetComponent<AudioSource>().clip;
        jumpGroup = soundManager.transform.GetChild(1).GetChild(sourceIndex).GetComponent<AudioSource>().outputAudioMixerGroup;

        // get this chicken's audio sources
        AudioSource[] sources = GetComponents<AudioSource>();
        runSource = sources[0];
        jumpSource = sources[1];
        deathSource = sources[2];

        // apply this sound's parameters to this chicken
        runSource.clip = runSound;
        runSource.outputAudioMixerGroup = runGroup;
        jumpSource.clip = jumpSound;
        jumpSource.outputAudioMixerGroup = jumpGroup;
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

    public void PlayJumpSound()
    {
        jumpSource.Play();
    }
}
