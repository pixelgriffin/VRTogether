using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuKeyboardInput : MonoBehaviour {
	
	private void Update () 
    {
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetComponent<LoadMinigame>().Load("FlySwatter");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetComponent<LoadMinigame>().Load("ChickenShooter");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetComponent<LoadMinigame>().Load("BeatBoxer");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GetComponent<LoadMinigame>().Load("CupHunt");
        }
    }
}
