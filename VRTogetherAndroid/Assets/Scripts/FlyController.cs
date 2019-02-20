using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class FlyController : MonoBehaviour {

    public float moveStep = 3f;

    private bool isHoldingGrape = false;
    private bool isMoving = false;

    private bool shouldMove = false;

	void Update () {

        // if there are screen touches
        if (Input.touchCount > 0)
        {
            // get all touches
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                if (!IsTouchingUIObject(touches[i]))
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

        // if mouse is down and was clicked not over a ui object
        if (Input.GetKey(KeyCode.Mouse0) && shouldMove)
        {
            Move();
        }
        else
        {
            shouldMove = false;
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
