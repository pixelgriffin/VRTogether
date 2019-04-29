using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class InstaReady : MonoBehaviour {

	void Start () {
        StartCoroutine(Ready());
	}

    private IEnumerator Ready()
    {
        yield return new WaitForSeconds(0.1f);

        MinigameClient.Instance.ReadyUp();
    }
}
