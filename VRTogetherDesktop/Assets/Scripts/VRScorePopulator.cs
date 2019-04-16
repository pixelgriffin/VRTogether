using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTogether.Net;

public class VRScorePopulator : MonoBehaviour {

    public Text text;

	void Update () {
        text.text = "VR Player Score: " + MacrogameServer.Instance.GetVRScore();
    }
}
