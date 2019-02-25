using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMouseLook : MonoBehaviour {

	public GameObject cameraGO;
	public Camera camera;

	Vector2 rotation = new Vector2 (0, 0);
    public float speed = 3;
	bool rotate = false;

	void Start ()
	{
		camera.transform.LookAt (transform.position);

	}

    void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown(0))
		{
			rotate = !Physics.Raycast(ray, 100);

		} else if (Input.GetMouseButtonUp(0))
		{
			rotate = false;
		}


		if (Input.GetMouseButton(0) && rotate) 
		{
			rotation.y += Input.GetAxis ("Mouse X");
			rotation.x += -Input.GetAxis ("Mouse Y");
			transform.eulerAngles = (Vector2)rotation * speed;

		}

       
    }
}
