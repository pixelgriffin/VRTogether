using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyXZPosition : MonoBehaviour {

    public Transform src;

	void Update () {
        this.transform.position = new Vector3(src.position.x, this.transform.position.y, src.position.z);
	}
}
