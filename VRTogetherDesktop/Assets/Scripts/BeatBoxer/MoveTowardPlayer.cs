using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour {

    public float speed = 5f;

    public Transform playerHead;

	void Start () {
        if(playerHead == null)
        {
            playerHead = Camera.main.transform;
        }

        Vector3 look = new Vector3(playerHead.position.x, this.transform.position.y, playerHead.position.z);
        this.transform.LookAt(look);
    }


	void Update () {
        this.transform.position += this.transform.forward * Time.deltaTime * speed;
	}
}
