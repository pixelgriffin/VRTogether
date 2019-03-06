using UnityEngine;
using VRTogether.Net;

public class JoystickController : MonoBehaviour
{
    public RightJoystick rightJoystick; // the game object containing the RightJoystick script
    public Transform rotationTarget; // the game object that will rotate to face the input direction
    public int rotationSpeed = 8; // rotation speed of the player character

    private Vector3 rightJoystickInput; // holds the input of the Right Joystick

    private bool beingMoved;

    private NetworkID id;

    void Start()
    {
        rightJoystick = GameObject.Find("RightJoystick").GetComponent<RightJoystick>();
        rotationTarget = transform;

        if (rightJoystick == null)
        {
            Debug.LogError("The right joystick is not attached.");
        }

        if (rotationTarget == null)
        {
            Debug.LogError("The target rotation game object is not attached.");
        }

        id = GetComponent<NetworkID>();
    }

    void Update()
    {
        if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            // get input from joystick
            rightJoystickInput = rightJoystick.GetInputDirection();

            float x = rightJoystickInput.x; // The horizontal movement from joystick 02
            float y = rightJoystickInput.y; // The vertical movement from joystick 02

            //Debug.Log("X: " + x + " Y: " + y);
            if (System.Math.Abs(x) < 0.001f && System.Math.Abs(y) < 0.001f)
            {
                beingMoved = false;
                //Debug.Log("not moving");
            }
            else
            {
                beingMoved = true;
                //Debug.Log("moving");
            }

            Vector3 rotation = rotationTarget.localRotation.eulerAngles;
            //Debug.Log("rotx: " + rotation.y);
            //Debug.Log("roty: " + rotation.x);
            //Debug.Log("x: " + x + ", y: " + y);
            if (Mathf.Abs(rotation.x) <= 360 && Mathf.Abs(rotation.x) >= 270)
            {
                rotation.x -= y * rotationSpeed;
                if (rotation.x < 275)
                {
                    rotation.x = 275;
                }
            }
            else if (Mathf.Abs(rotation.x) >= 0 && Mathf.Abs(rotation.x) <= 90)
            {
                rotation.x -= y * rotationSpeed;
                if (rotation.x > 85)
                {
                    rotation.x = 85;
                }
            }

            rotation.y += x * rotationSpeed;

            rotation.z = 0.0f;

            Vector3 deltaRotation = rotationTarget.localRotation.eulerAngles - rotation;
            if (deltaRotation != Vector3.zero)
            {
                rotationTarget.localRotation = Quaternion.Slerp(rotationTarget.localRotation, Quaternion.Euler(rotation), rotationSpeed * Time.deltaTime);
                //rotationTarget.localRotation = Quaternion.Euler(newRotation);
            }
        }
    }

    public bool IsBeingMoved()
    {
        return beingMoved;
    }
}