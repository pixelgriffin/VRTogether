using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {

    public float moveStep = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0)
        {
            transform.localPosition += transform.forward * Time.deltaTime * moveStep;
        }
	}
}
