using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class MainMenuKeyboardInput : MonoBehaviour {

    public LoadMinigame minigameLoader;
    public GenerateCode codeGenerator;
    public VRLobbyBackButton backButton;
	
	private void Update () 
    {
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            minigameLoader.Load("FlySwatter");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            minigameLoader.Load("ChickenShooter");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            minigameLoader.Load("BeatBoxer");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            minigameLoader.Load("CupHunt");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            codeGenerator.DestroyCode();
            backButton.OnBackButtonPressed();
            // does not play sound
        }
    }
}
