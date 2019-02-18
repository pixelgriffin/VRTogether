using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour {

    public float zMoveSpeed = 1.0f;
    public float xMoveSpeed = 1.0f;
    public float maxVelocityX = 1.0f;

    private GameObject chicken;
    private Rigidbody chickenRB;

	// Use this for initialization
	void Start () {

        chicken = transform.GetChild(0).gameObject;
        chickenRB = chicken.GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Rotate(Vector3.up, 1.0f * Time.deltaTime * zMoveSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Rotate(Vector3.up, -1.0f * Time.deltaTime * zMoveSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 movement = Vector3.left * Time.deltaTime * xMoveSpeed;
            chicken.transform.Translate(movement, Space.Self);
            /*
            chickenRB.AddRelativeForce(-xMoveSpeed, 0, 0, ForceMode.VelocityChange);
            Debug.Log(chickenRB.GetRelativePointVelocity(transform.position));
            if (chickenRB.GetRelativePointVelocity(transform.position).x > maxVelocityX)
            {
                chickenRB.AddRelativeForce(xMoveSpeed, 0, 0, ForceMode.VelocityChange);
            }
            */
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 movement = Vector3.right * Time.deltaTime * xMoveSpeed;
            chicken.transform.Translate(movement, Space.Self);
            /*
            chickenRB.AddRelativeForce(xMoveSpeed, 0, 0, ForceMode.VelocityChange);
            if (chickenRB.GetRelativePointVelocity(transform.position).x < -maxVelocityX)
            {
                chickenRB.AddRelativeForce(-xMoveSpeed, 0, 0, ForceMode.VelocityChange);
            }
            */
        }

    }
}
