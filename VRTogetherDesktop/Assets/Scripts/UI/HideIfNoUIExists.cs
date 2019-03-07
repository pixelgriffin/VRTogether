using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HideIfNoUIExists : MonoBehaviour {

    private MeshRenderer mr;

	void Start () {
        mr = GetComponent<MeshRenderer>();
	}

	void Update () {
        mr.enabled = (GameObject.FindGameObjectsWithTag("UI").Length > 0);
    }
}
