using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using VRTogether.Net;

public class FlyController : MonoBehaviour {

    public float moveStep = 3f;

    private bool isHoldingGrape = false;
    private bool isMoving = false;

    private bool shouldMove = false;

    private JoystickController joystick;

    private NetworkID id;

    void Start()
    {
        joystick = GetComponent<JoystickController>();
        id = GetComponent<NetworkID>();
    }

    void Update () {

        if (!MinigameClient.Instance.networkedPrefabs.IsSlave(id.netID))
        {
            // if there are screen touches
            /*
            if (Input.touchCount > 0)
            {
                // get all touches
                Touch[] touches = Input.touches;
                for (int i = 0; i < touches.Length; i++)
                {
                    if (!IsTouchingUIObject(touches[i]) && !joystick.IsBeingMoved())
                    {
                        // move fly if touch is not over ui object
                        Move();
                        break;
                    }

                    // if on last touch then fly is not moving
                    if (i == touches.Length - 1)
                    {
                        isMoving = false;
                    }
                }
            }
            // if the left mouse was clicked and it was not over a ui object
            else if (Input.GetKeyDown(KeyCode.Mouse0) && !IsPointerOverUIObject())
            {
                shouldMove = true;
            }
            else
            {
                isMoving = false;
            }
            */

            bool screenTouch = false;

            // if there are 2 or more touches
            if (Input.touchCount >= 2)
            {
                // assume one touch is a screen touch
                screenTouch = true;
            }
            // else if there is one touch
            else if (Input.touchCount == 1)
            {
                // check if joystick is not being touched
                if (!IsTouchingUIObject(Input.touches[0]) && !joystick.IsBeingMoved())
                {
                    screenTouch = true;
                }
                else
                {
                    isMoving = false;
                }
            }
            // if mouse is down and was clicked not over a ui object
            else if (Input.GetKey(KeyCode.Mouse0) && !IsPointerOverUIObject() && !joystick.IsBeingMoved())
            {
                Move();
            }
            else
            {
                isMoving = false;
            }

            if (screenTouch)
            {
                Move();
            }


        }
    }

    private void Move()
    {
        transform.localPosition += transform.forward * Time.deltaTime * moveStep;
        isMoving = true;
    }

    private bool IsTouchingUIObject(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void HoldGrape()
    {
        isHoldingGrape = true;
    }

    public void DropGrape()
    {
        isHoldingGrape = false;
    }

    public bool IsHoldingGrape()
    {
        return isHoldingGrape;
    }
}
