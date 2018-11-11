using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveFly : MonoBehaviour {

    public bool moving = false;

	void Update () {
        if(moving)
        {
            this.transform.position += this.transform.forward * Time.deltaTime * 2f;
        }
	}
}
