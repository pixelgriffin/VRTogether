using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenUIControls : MonoBehaviour {

    private bool upPressed, downPressed, leftPressed, rightPressed, screenPressed;

    private bool touchingObject;

    private int touchCount = 0;

	void Start () {

        upPressed = false;
        downPressed = false;
        leftPressed = false;
        rightPressed = false;
        screenPressed = false;
		
	}

    void Update()
    {
        //touchingObject = false;
        //touchCount = Input.touchCount;

        // check if each touch is over ui element
        foreach (Touch touch in Input.touches)
        {
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId) 
                && touch.phase != TouchPhase.Ended)
            {
                screenPressed = true;
                break;
            }

            screenPressed = false;
        }

        if (Input.touchCount <= 0)
            screenPressed = false;

        // check if mouse is over ui element
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            touchingObject = true;
        }

        // if no ui element is being touched, and there is indeed input
        if (!touchingObject && touchCount > 0)
        {
            screenPressed = true;
        }
        else
        {
            screenPressed = false;
        }
        */
    }

    public void OnUpPressed()
    {
        upPressed = true;
    }

    public void OnUpReleased()
    {
        upPressed = false;
    }

    public void OnDownPressed()
    {
        downPressed = true;
    }

    public void OnDownReleased()
    {
        downPressed = false;
    }

    public void OnLeftPressed()
    {
        leftPressed = true;
    }

    public void OnLeftReleased()
    {
        leftPressed = false;
    }

    public void OnRightPressed()
    {
        rightPressed = true;
    }

    public void OnRightReleased()
    {
        rightPressed = false;
    }

    public bool IsUpPressed()
    {
        return upPressed;
    }

    public bool IsDownPressed()
    {
        return downPressed;
    }

    public bool IsLeftPressed()
    {
        return leftPressed;
    }

    public bool IsRightPressed()
    {
        return rightPressed;
    }

    public bool IsScreenPressed()
    {
        return screenPressed;
    }
}
