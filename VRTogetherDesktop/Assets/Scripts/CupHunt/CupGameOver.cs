using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class CupGameOver : MonoBehaviour {
    public Cup[] cups;

    public bool atLeastOne = false;

	// Use this for initialization
	void Start () {
	}
	
	void FixedUpdate () {
        cups = FindObjectsOfType<Cup>();

        if (!atLeastOne && cups.Length > 0)
        {
            atLeastOne = true;

        }

        if (cups.Length == 0 && atLeastOne)
        {
            //TODO update this to proper values, just added this so the script doesn't break
            MinigameServer.Instance.EndGame("Scenes/MainMenu", true, 100);

        }
		
	}
}
