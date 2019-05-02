using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VRVolumeControl : MonoBehaviour {

    public AudioSource playSound;
    public AudioMixerGroup masterMixer, musicMixer, soundMixer;
    public Slider masterSlider, musicSlider, soundSlider;
    public float playSoundInterval = 0.5f;
    private float playSoundTimer = 0.0f;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(masterSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(musicSlider.value); });
        soundSlider.onValueChanged.AddListener(delegate { OnSoundEffectVolumeChanged(soundSlider.value); });
    }

    private void Update()
    {
        playSoundTimer += Time.deltaTime;
    }

    public void OnMasterVolumeChanged(float newVolume)
    {
        masterMixer.audioMixer.SetFloat("Master", newVolume);
    }

    public void OnMusicVolumeChanged(float newVolume)
    {
        musicMixer.audioMixer.SetFloat("MasterMusic", newVolume);
    }

    public void OnSoundEffectVolumeChanged(float newVolume)
    {
        soundMixer.audioMixer.SetFloat("MasterSound", newVolume);
        if (playSoundTimer >= playSoundInterval)
        {
            playSound.Play();
            playSoundTimer = 0.0f;
        }
    }

}
