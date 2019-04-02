using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour {
    public Transform[] transforms;
    public GameObject cup;

	// Use this for initialization
	public void Spawn () {
		foreach (Transform t in transforms)
        {
            Instantiate(cup, t.position, t.rotation);

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
