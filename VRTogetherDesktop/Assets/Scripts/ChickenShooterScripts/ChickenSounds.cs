using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSounds : MonoBehaviour {

    private GameObject soundManager;
    private AudioClip runSound, jumpSound, deathSound;

    private void Start()
    {
        soundManager = GameObject.Find("SoundManager");

        runSound = soundManager.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>().clip;
    }

    public void PlayRunSound()
    {
        AudioSource.PlayClipAtPoint(runSound, this.transform.position);
    }
}
