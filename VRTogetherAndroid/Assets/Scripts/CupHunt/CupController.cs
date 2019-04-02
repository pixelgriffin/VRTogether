using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class CupController : MonoBehaviour {

	public bool inControl = true;

	public GameObject director;  // The arrow which spins below the cup
    public GameObject cameraOject;  // The camera object, spawns only for the cup which is not a slave
	public SpriteRenderer directorSprite;
	public float directorRotateSpeed = 5f;  // Degress/s
	public float directorChargeTime = 3f;  // How long it takes for the director to fully charge
	public float moveForceMax = 10f;
    public float correctedForceDistanceOffset = 0.5f;  // [0, 10]
	private float directorHoldStartTime;  // The time (float) that the mouse was started held down
	private float timeHeldRatio;
	public Vector3 directorChargeScale;  // The maximum scale for the director to reach
	private Vector3 directorStartScale;

	public Gradient powerColors;

    private bool isCharging = false;
    private bool canCharge = false;  // Is this cup allowed to charge? (Player touching cup)
    private GameObject myCam;

	void Start ()
    {
        inControl = !MinigameClient.Instance.networkedPrefabs.IsSlave(GetComponent<NetworkID>().netID);

		directorStartScale = director.transform.localScale;

		SetInControl (inControl);
		
	}
	
	void Update () 
    {
        /*
			When the mouse is first held down, store the time

			When the mouse is first released, apply the charged force 
		 */
        if (Input.GetMouseButtonDown(0) && !isCharging && canCharge)
		{
			

		} else if (Input.GetMouseButtonUp(0) && isCharging)
		{
            RaycastHit hit;
            float correctedForce = timeHeldRatio * moveForceMax;

            // If that path would make us collide with another cup, damp the force.
            if (Physics.SphereCast(transform.position, 5f, director.transform.up, out hit, 10 * timeHeldRatio, LayerMask.GetMask("Cup")))
            {
                correctedForce = Mathf.Clamp(hit.distance - correctedForceDistanceOffset, 0, 10) / 10 * moveForceMax;

            }

			GetComponent<Rigidbody>().AddForce (director.transform.up * correctedForce);

            isCharging = false;

            canCharge = false;

		}

		/*
			While the mouse is held, find how long it has been held in terms of a ratio of 0 to 1 where 0 is the start time
			and 1 is the end time, and use that ratio to scale the director and set its color

			Else reset the color and scale of the director (this doesn't need to be done every frame) and rotate it

		 */
		if (Input.GetMouseButton(0) && isCharging)
		{
			timeHeldRatio = 1 - (-(Time.time - (directorChargeTime + directorHoldStartTime)) / directorChargeTime);
			timeHeldRatio = Mathf.Clamp(timeHeldRatio, 0f, 1f);
			//Grow the arrow
			directorSprite.color = powerColors.Evaluate(timeHeldRatio);
			director.transform.localScale = Vector3.Lerp(directorStartScale, directorChargeScale, timeHeldRatio);


		} else
		{
			directorSprite.color = Color.white;
			director.transform.localScale = directorStartScale;

			director.transform.Rotate(Vector3.forward * directorRotateSpeed * Time.deltaTime);

		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "Ball")
		{
			Debug.Log("Score!");

            col.GetComponent<Rigidbody>().isKinematic = true;
            col.transform.SetParent(transform);

            GameObject.Destroy(myCam);
            MinigameClient.Instance.NetworkDestroy(col.gameObject);
            MinigameClient.Instance.NetworkDestroy(gameObject);

		}

	}

	public void SetInControl (bool state)
	{
		director.SetActive (state);
		this.enabled = state;

        if (state)
        {
            myCam = Instantiate(cameraOject);
            myCam.GetComponent<SimpleTouchLook>().target = transform;

        }

    }

    public void SetCanCharge (bool state)
    {
        canCharge = state;

        if (canCharge)
        {
            directorHoldStartTime = Time.time;

            isCharging = true;

        }



    }
}
