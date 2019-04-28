using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class TestSpawn : MonoBehaviour {
    public Transform target;
    public GameObject ball;
    public float heightOffset = 5f;
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            transform.position = new Vector3(target.position.x, target.position.y + heightOffset, target.position.z);

        }

        
        if (Input.GetKeyDown(KeyCode.X) && target)
        {
            GameObject newBall = MinigameServer.Instance.NetworkInstantiate(ball);

            newBall.transform.position = transform.position;
            newBall.GetComponent<Ball>().enabled = true;

        }
		
	}
}
