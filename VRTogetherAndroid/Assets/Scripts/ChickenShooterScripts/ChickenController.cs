using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour {

    public float zMoveSpeed = 1.0f;
    public float xMoveSpeed = 1.0f;
    public float maxVelocityX = 1.0f;
    public float jumpHeight = 2.0f;
    public float mass = 0.1f;
    public float gravity = -5.0f;

    private GameObject chicken;
    private Rigidbody chickenRB;
    private CharacterController chickenCtrl;
    private Vector3 velocity;
    private bool grounded = false;

	// Use this for initialization
	void Start () {

        chicken = transform.GetChild(0).gameObject;
        //chickenRB = chicken.GetComponent<Rigidbody>();
        chickenCtrl = chicken.GetComponent<CharacterController>();

        velocity = Vector3.zero;
		
	}
	
	// Update is called once per frame
	void Update () {

        grounded = Physics.CheckBox(
            chicken.transform.position,
            new Vector3(0.2f, 0.5f, 0.2f),
            Quaternion.identity,
            1 << 9
            );

        velocity.x = 0f;
        velocity.z = 0f;
        if (grounded && velocity.y <= 0f)
        {
            velocity.y = 0f;
        }

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
            //Vector3 movement = chicken.transform.TransformDirection(Vector3.left) * Time.deltaTime * xMoveSpeed;
            velocity += chicken.transform.TransformDirection(Vector3.left) * Time.deltaTime * xMoveSpeed;
            //chicken.transform.Translate(movement, Space.Self);
            //chickenCtrl.Move(movement);
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
            //Vector3 movement = chicken.transform.TransformDirection(Vector3.right) * Time.deltaTime * xMoveSpeed;
            velocity += chicken.transform.TransformDirection(Vector3.right) * Time.deltaTime * xMoveSpeed;
            //chicken.transform.Translate(movement, Space.Self);
            //chickenCtrl.Move(movement);
            /*
            chickenRB.AddRelativeForce(xMoveSpeed, 0, 0, ForceMode.VelocityChange);
            if (chickenRB.GetRelativePointVelocity(transform.position).x < -maxVelocityX)
            {
                chickenRB.AddRelativeForce(-xMoveSpeed, 0, 0, ForceMode.VelocityChange);
            }
            */
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity * mass);

        if (grounded)
        {
            Debug.Log("Grounded at " + Time.time);
        }

        velocity.y += gravity * mass * Time.deltaTime;

        chickenCtrl.Move(velocity);
    }

    private void FixedUpdate()
    {

    }
}
