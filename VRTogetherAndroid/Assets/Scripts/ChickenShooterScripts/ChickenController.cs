using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChickenController : MonoBehaviour {

    public float zMoveSpeed = 1.0f;
    public float xMoveSpeed = 1.0f;
    public float maxVelocityX = 1.0f;
    public float jumpHeight = 2.0f;
    public float mass = 0.1f;
    public float gravity = -5.0f;

    private GameObject chicken;
    private CharacterController chickenCtrl;
    private Vector3 velocity;

    // ui controls
    private GameObject canvas;
    private ChickenUIControls controls;

    private bool grounded = false;
    private bool movingUp, movingDown, movingLeft, movingRight, jumping;

	// Use this for initialization
	void Start () {

        chicken = transform.GetChild(0).gameObject;
        //chickenRB = chicken.GetComponent<Rigidbody>();
        chickenCtrl = chicken.GetComponent<CharacterController>();

        velocity = Vector3.zero;

        canvas = GameObject.Find("Canvas");
        canvas.GetComponent<Canvas>().enabled = true;
        controls = canvas.transform.GetChild(0).gameObject.
            GetComponent<ChickenUIControls>();
		
	}
	
	// Update is called once per frame
	void Update () {

        grounded = Physics.CheckBox(
            chicken.transform.position,
            new Vector3(0.2f, 0.5f, 0.2f),
            Quaternion.identity,
            1 << 12
            );

        velocity.x = 0f;
        velocity.z = 0f;
        if (grounded && velocity.y <= 0f)
        {
            velocity.y = 0f;
        }

        movingUp = controls.IsUpPressed() || Input.GetKey(KeyCode.W);
        movingDown = controls.IsDownPressed() || Input.GetKey(KeyCode.S);
        movingLeft = controls.IsLeftPressed() || Input.GetKey(KeyCode.A);
        movingRight = controls.IsRightPressed() || Input.GetKey(KeyCode.D);
        jumping = (controls.IsScreenPressed() || Input.GetKey(KeyCode.Space)) 
            && grounded;

        if (movingUp)
        {
            this.transform.Rotate(Vector3.up, 1.0f * Time.deltaTime * zMoveSpeed);
        }

        if (movingDown)
        {
            this.transform.Rotate(Vector3.up, -1.0f * Time.deltaTime * zMoveSpeed);
        }

        if (movingLeft)
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

        if (movingRight)
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

        if (jumping)
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
