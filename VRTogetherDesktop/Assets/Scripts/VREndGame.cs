using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using VRTogether.Net;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VREndGame : MonoBehaviour {

    public float holdDownTime = 5f;  // The amount of time the button needs to be held to end the game

    public Image radialIndicator;

    private float heldTime = 0f;

    private Hand thisHand;
    private Transform point;
    private SteamVR_Input_Sources source;

    void Start()
    {
        thisHand = GetComponent<Hand>();
        source = thisHand.handType;

    }

    // Update is called once per frame
    void Update () 
    {
        if (true)
        {
            heldTime += Time.deltaTime;

        } else
        {
            heldTime = 0f;

        }

        radialIndicator.fillAmount = Mathf.Clamp(heldTime, 0f, holdDownTime) / holdDownTime;

        if (heldTime >= holdDownTime)
        {
            MinigameServer.Instance.EndGame("MainMenu", true, 0);

        }

		
	}
}
