using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoopController : MonoBehaviour {
    //spoop controller controls the mobile player's character
    //implements tank controls for main movement using on screen trackpad,
    //and uses pressing and holding for jumping

    //will be used for auto camera placement
    public Camera playerCam;

	void Start () {
        playerCam.gameObject.transform.parent = gameObject.transform;
	}
	
	void Update () {
		
	}
}
