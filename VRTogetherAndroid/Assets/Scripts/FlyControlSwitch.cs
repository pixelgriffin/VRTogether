using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControlSwitch : MonoBehaviour {

    public bool useGyroControls;

    private GameObject joystickImage;
    private RightJoystickTouchContoller joystickControl;

	void Awake () {

        joystickImage = GameObject.Find("FlyCanvas").transform.GetChild(0).gameObject;
        joystickControl = GameObject.Find("RightJoystickTouchController").GetComponent<RightJoystickTouchContoller>();
       
    }
	
	void Update () {

        if (useGyroControls)
        {
            // enable gyro and diable joystick
            GetComponent<GyroscopeController>().enabled = true;
            GetComponent<JoystickController>().enabled = false;

            // disable the joystick canvas and associated scripts
            joystickImage.SetActive(false);
            joystickControl.enabled = false;

        }
        else
        {
            // diable gyro and enable joystick
            GetComponent<GyroscopeController>().enabled = false;
            GetComponent<JoystickController>().enabled = true;

            // disable the joystick canvas and associated scripts
            joystickImage.SetActive(true);
            joystickControl.enabled = true;
        }

    }
}
