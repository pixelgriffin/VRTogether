using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTouchLook : MonoBehaviour {

    public Transform target;  // The target to fix this gameOjbect's position to
    public float targetFollowSpeed = 3f;

    public Camera myCamera;

	Vector2 rotation = new Vector2 (0, 0);
    public float speed = 3;
	bool rotate = false;

    public string[] tagsToHit;  // If the ray hits an object with this tag, do not rotate

	void Start ()
	{
		myCamera.transform.LookAt (transform.position);

	}

    void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * targetFollowSpeed);

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
			

			if (Input.touchCount > 0)
			{
				rotation.x += Input.touches[0].deltaPosition.y;
				rotation.y -= Input.touches[0].deltaPosition.x;

			} else
			{
				rotation.y += Input.GetAxis ("Mouse X");
				rotation.x += -Input.GetAxis ("Mouse Y");
					
			}

			rotation.x = Mathf.Clamp(rotation.x, 0, 60);
			transform.eulerAngles = (Vector2)rotation * speed;

		}

       
    }
}
