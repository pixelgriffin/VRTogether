using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {

    public float moveStep = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (IsMoving())
        {
            transform.localPosition += transform.forward * Time.deltaTime * moveStep;
        }
	}

    public bool IsMoving()
    {
        return Input.touchCount > 0;
    }
}
