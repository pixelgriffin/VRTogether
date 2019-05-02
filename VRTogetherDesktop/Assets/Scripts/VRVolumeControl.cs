using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VRVolumeControl : MonoBehaviour {

    public AudioMixerGroup masterMixer, musicMixer, soundMixer;
    public Slider masterSlider, musicSlider, soundSlider;

    private void Update()
    {
        masterSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(masterSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(musicSlider.value); });
        soundSlider.onValueChanged.AddListener(delegate { OnSoundEffectVolumeChanged(soundSlider.value); });
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
    }

}
