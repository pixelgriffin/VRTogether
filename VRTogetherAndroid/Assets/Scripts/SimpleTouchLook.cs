using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTogether.Net;

public class SimpleTouchLook : MonoBehaviour {

    public Transform target;  // The target to fix this gameOjbect's position to
    public float targetFollowSpeed = 3f;

    public Camera myCamera;

	Vector2 rotation = new Vector2 (0, 0);
    public float speed = 3;
	public bool rotate = false;

    public string[] layersToHit;  // If the ray hits an object in this layer, do not rotate

    public UnityEvent onLayerHit; // Run these actions when an object in the layers above is hit
    
	void Start ()
	{
		myCamera.transform.LookAt (transform.position);

	}

    void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * targetFollowSpeed);



		if (Input.GetMouseButtonDown(0))
		{
            Vector2 touchPosition = Input.mousePosition;

            if (Input.touchCount > 0)
            {
                touchPosition = Input.touches[0].position;

            }

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hit;

            // False if can't rotate (touching cup)
            rotate = !Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(layersToHit));

            Debug.DrawLine(ray.origin, ray.GetPoint(1000), rotate ? Color.red : Color.green);

            target.GetComponent<CupController>().SetCanCharge(!rotate);


            if (!rotate)
            {

            } else
            {
                //target.GetComponent<CupController>().SetCanCharge(false);

            }

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
