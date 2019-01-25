using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public RightJoystick rightJoystick; // the game object containing the RightJoystick script
    public Transform rotationTarget; // the game object that will rotate to face the input direction
    public float moveSpeed = 6.0f; // movement speed of the player character
    public int rotationSpeed = 8; // rotation speed of the player character
    public Animator animator; // the animator controller of the player character
    private Vector3 rightJoystickInput; // hold the input of the Right Joystick
    private Rigidbody rigidBody; // rigid body component of the player character

    void Start()
    {
        if (transform.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("A RigidBody component is required on this game object.");
        }
        else
        {
            rigidBody = transform.GetComponent<Rigidbody>();
        }

        if (rightJoystick == null)
        {
            Debug.LogError("The right joystick is not attached.");
        }

        if (rotationTarget == null)
        {
            Debug.LogError("The target rotation game object is not attached.");
        }
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        // get input from joystick
        rightJoystickInput = rightJoystick.GetInputDirection();

        float xMovementRightJoystick = rightJoystickInput.x; // The horizontal movement from joystick 02
        float zMovementRightJoystick = rightJoystickInput.y; // The vertical movement from joystick 02

        {
            // calculate the player's direction based on angle
            float tempAngle = Mathf.Atan2(zMovementRightJoystick, xMovementRightJoystick);
            //xMovementRightJoystick *= Mathf.Abs(Mathf.Cos(tempAngle));
            //zMovementRightJoystick *= Mathf.Abs(Mathf.Sin(tempAngle));

            // rotate the player to face the direction of input
            /*
            Vector3 temp = transform.rotation.eulerAngles;
            temp.x += xMovementRightJoystick;
            temp.y += zMovementRightJoystick;
            Vector3 lookDirection = temp;*/
            Vector3 temp = rotationTarget.localRotation.eulerAngles;
            Debug.Log("rotx: " + temp.x);
            Debug.Log("roty: " + temp.y);
            if (Mathf.Abs(temp.x) <= 360 && Mathf.Abs(temp.x) >= 270)
            {
                temp.x -= zMovementRightJoystick * rotationSpeed;
                if (temp.x < 275)
                {
                    temp.x = 275;
                }
            }
            else if (Mathf.Abs(temp.x) >= 0 && Mathf.Abs(temp.x) <= 90)
            {
                temp.x -= zMovementRightJoystick * rotationSpeed;
                if (temp.x > 85)
                {
                    temp.x = 85;
                }
            }

            temp.y += xMovementRightJoystick * rotationSpeed;
            Vector3 newRotation = temp;
            Vector3 lookDirection = rotationTarget.localRotation.eulerAngles - temp;
            if (lookDirection != Vector3.zero)
            {
                rotationTarget.localRotation = Quaternion.Slerp(rotationTarget.localRotation, Quaternion.Euler(newRotation), rotationSpeed * Time.deltaTime);
                //rotationTarget.localRotation = Quaternion.Euler(newRotation);
            }
        }
    }
}