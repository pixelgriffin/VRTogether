using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class HackStartGame : MonoBehaviour {

	void Start () {
        MacrogameClient.Instance.AttemptConnection("127.0.0.1");
	}
}
