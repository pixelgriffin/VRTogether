using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControlSwitch : MonoBehaviour {

    public bool useGyroControls;

    private Canvas joystickCanvas;
    private RightJoystickTouchContoller joystickControl;

	void Awake () {

        joystickCanvas = GameObject.Find("JoystickCanvas").GetComponent<Canvas>();
        joystickControl = GameObject.Find("RightJoystickTouchController").GetComponent<RightJoystickTouchContoller>();
       
    }
	
	void Update () {

        if (useGyroControls)
        {
            // enable gyro and diable joystick
            GetComponent<GyroscopeController>().enabled = true;
            GetComponent<JoystickController>().enabled = false;

            // disable the joystick canvas and associated scripts
            joystickCanvas.enabled = false;
            joystickControl.enabled = false;

        }
        else
        {
            // diable gyro and enable joystick
            GetComponent<GyroscopeController>().enabled = false;
            GetComponent<JoystickController>().enabled = true;

            // disable the joystick canvas and associated scripts
            joystickCanvas.enabled = true;
            joystickControl.enabled = true;
        }

    }
}
