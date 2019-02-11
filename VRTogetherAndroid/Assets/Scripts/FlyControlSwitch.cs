using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControlSwitch : MonoBehaviour {

    public bool useGyroControls;

	void Awake () {

		if (useGyroControls)
        {
            // enable gyro and diable joystick
            GetComponent<GyroscopeController>().enabled = true;
            GetComponent<JoystickController>().enabled = false;

            // disable the joystick canvas and associated scripts
            GameObject.Find("RightJoystickCanvas").GetComponent<Canvas>().enabled = false;
            GameObject.Find("RightJoystickTouchController").GetComponent<RightJoystickTouchContoller>().enabled = false;

        }
        else
        {
            // diable gyro and enable joystick
            GetComponent<GyroscopeController>().enabled = false;
            GetComponent<JoystickController>().enabled = true;

            // disable the joystick canvas and associated scripts
            GameObject.Find("RightJoystickCanvas").GetComponent<Canvas>().enabled = true;
            GameObject.Find("RightJoystickTouchController").GetComponent<RightJoystickTouchContoller>().enabled = true;
        }

    }
	
	void Update () {
		
	}
}
