using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class BBMusicChecker : MonoBehaviour {

    private AudioSource src;

	void Start () {
        src = GetComponent<AudioSource>();
        src.Play();
	}
	
	void Update () {
		if(!src.isPlaying)
        {
            MinigameServer.Instance.EndGame("Scenes/MainMenu", true, 1);
        }
	}
}
