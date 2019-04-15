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
    public float jumpTime = 0.5f;

    private GameObject chicken;
    //private Vector3 orgChickenMeshPos;
    private CharacterController chickenCtrl;
    private Vector3 velocity;
    private float jumpTimer;

    private Vector3 oldPos;
    private Vector3 posOffset;

    // ui controls
    private GameObject canvas;
    private ChickenUIControls controls;

    private bool grounded = false;
    private bool backCollisionWall = false;
    private bool frontCollisionWall = false;
    private bool leftCollisionWall = false;
    private bool rightCollisionWall = false;
    private bool collidingWithWall = false;
    private bool movingUp, movingDown, movingLeft, movingRight, jumping;

	// Use this for initialization
	void Start () {

        chicken = transform.GetChild(0).gameObject;
        //orgChickenMeshPos = chicken.transform.GetChild(0).localPosition;
        //chickenRB = chicken.GetComponent<Rigidbody>();
        chickenCtrl = chicken.GetComponent<CharacterController>();

        velocity = Vector3.zero;

        jumpTimer = jumpTime;

        oldPos = this.transform.position;
        posOffset = Vector3.up * 0.5f;

        canvas = GameObject.Find("Canvas");
        canvas.GetComponent<Canvas>().enabled = true;
        controls = canvas.transform.GetChild(0).gameObject.
            GetComponent<ChickenUIControls>();

        Gizmos.color = Color.green;
		
	}
	
	// Update is called once per frame
	void Update () {

        grounded = Physics.CheckBox(
            chicken.transform.position - chicken.transform.up * 0.35f, 
            new Vector3(0.5f, 0.125f, 0.5f),
            Quaternion.identity,
            1 << 12
            );

        //DrawBox(chicken.transform.position - chicken.transform.up * 0.3f, new Vector3(0.1f, 0.4f, 0.1f));

        frontCollisionWall = Physics.CheckBox(
            chicken.transform.position + chicken.transform.forward * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f),
            Quaternion.identity,
            1 << 13
            );

        backCollisionWall = Physics.CheckBox(
            chicken.transform.position - chicken.transform.forward * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f),
            Quaternion.identity,
            1 << 13
            );

        leftCollisionWall = Physics.CheckBox(
            chicken.transform.position - chicken.transform.right * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f),
            Quaternion.identity,
            1 << 13
            );

        rightCollisionWall = Physics.CheckBox(
            chicken.transform.position + chicken.transform.right * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f),
            Quaternion.identity,
            1 << 13
            );

        velocity.x = 0f;
        velocity.z = 0f;
        if (grounded && velocity.y <= 0f)
        {
            velocity.y = 0f;
        }

        jumpTimer += Time.deltaTime;

        movingUp = controls.IsUpPressed() || Input.GetKey(KeyCode.W);
        movingDown = controls.IsDownPressed() || Input.GetKey(KeyCode.S);
        movingLeft = controls.IsLeftPressed() || Input.GetKey(KeyCode.A);
        movingRight = controls.IsRightPressed() || Input.GetKey(KeyCode.D);
        jumping = (controls.IsScreenPressed() || Input.GetKey(KeyCode.Space)) 
            && grounded && jumpTimer >= jumpTime;

        // hop animation
        if (grounded)
        {
            if (chicken.transform.position != oldPos)
            {
                chicken.transform.GetChild(0).localPosition = (Vector3.up * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10f)) * 0.2f) - posOffset;
            }
            else
            {
                chicken.transform.GetChild(0).localPosition = Vector3.MoveTowards(chicken.transform.GetChild(0).localPosition, -posOffset, Time.deltaTime * 5f);
            }

            oldPos = chicken.transform.position;
        }

        if (movingUp && !frontCollisionWall)
        {
            this.transform.Rotate(Vector3.up, 1.0f * Time.deltaTime * zMoveSpeed);
        }

        if (movingDown && !backCollisionWall)
        {
            this.transform.Rotate(Vector3.up, -1.0f * Time.deltaTime * zMoveSpeed);
        }

        if (movingLeft && !leftCollisionWall)
        {
            velocity += chicken.transform.TransformDirection(Vector3.left) * Time.deltaTime * xMoveSpeed;
        }

        if (movingRight && !rightCollisionWall)
        {
            velocity += chicken.transform.TransformDirection(Vector3.right) * Time.deltaTime * xMoveSpeed;
        }

        if (jumping)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity * mass);
            jumpTimer = 0.0f;
        }

        velocity.y += gravity * mass * Time.deltaTime;

        chickenCtrl.Move(velocity);
    }

    private void FixedUpdate()
    {

    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(chicken.transform.position - chicken.transform.up * 0.35f, new Vector3(0.1f, 0.25f, 0.1f));
        Gizmos.DrawWireCube(chicken.transform.position + chicken.transform.forward * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawWireCube(chicken.transform.position - chicken.transform.forward * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawWireCube(chicken.transform.position - chicken.transform.right * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawWireCube(chicken.transform.position + chicken.transform.right * 0.35f,
            new Vector3(0.1f, 0.1f, 0.1f));
    }

    private void DrawBox(Vector3 center, Vector3 extents)
    {
        //Bounds bounds = GetComponent<MeshFilter>().mesh.bounds;

        //Bounds bounds;
        //BoxCollider bc = GetComponent<BoxCollider>();
        //if (bc != null)
        //    bounds = bc.bounds;
        //else
        //return;

        //Vector3 v3Center = bounds.center;
        //Vector3 v3Extents = bounds.extents;

        /*
        Vector3 v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        Vector3 v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        Vector3 v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        Vector3 v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        Vector3 v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        Vector3 v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        Vector3 v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        Vector3 v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);

        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, Color.green);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, Color.green);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, Color.green);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, Color.green);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, Color.green);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, Color.green);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, Color.green);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, Color.green);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, Color.green);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, Color.green);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, Color.green);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, Color.green);
        */

        //Gizmos.DrawWireCube(center, extents);       
    }
}
