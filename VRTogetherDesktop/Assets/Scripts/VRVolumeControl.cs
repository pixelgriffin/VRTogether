using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VRVolumeControl : MonoBehaviour {

    public AudioMixerGroup masterMixer;

    public void OnMusicVolumeChanged(float newVolume)
    {
        masterMixer.audioMixer.SetFloat("Master", newVolume);
    }

    public void OnSoundEffectVolumeChanged(float newVolume)
    {
        masterMixer.audioMixer.SetFloat("MasterSound", newVolume);
    }

}
