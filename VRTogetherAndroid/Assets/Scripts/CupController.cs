﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour {

	public GameObject director;  // The arrow which spins below the cup
	public SpriteRenderer directorSprite;
	public float directorRotateSpeed = 5f;  // Degress/s
	public float directorChargeTime = 3f;  // How long it takes for the director to fully charge
	public float moveForceMax = 10f;
	private float directorHoldStartTime;  // The time (float) that the mouse was started held down
	private float timeHeldRatio;
	public Vector3 directorChargeScale;  // The maximum scale for the director to reach
	private Vector3 directorStartScale;

	public Gradient powerColors;

	// Use this for initialization
	void Start () {
		directorStartScale = director.transform.localScale;
		
	}
	
	// Update is called once per frame
	void Update () {

		/**
			When the mouse is first held down, store the time

			When the mouse is first released, apply the charged force 
		 */
		if (Input.GetMouseButtonDown(0))
		{
			directorHoldStartTime = Time.time;

		} else if (Input.GetMouseButtonUp(0))
		{
			GetComponent<Rigidbody>().AddForce (director.transform.up * timeHeldRatio * moveForceMax);

		}

		/**
			While the mouse is held, find how long it has been held in terms of a ratio of 0 to 1 where 0 is the start time
			and 1 is the end time, and use that ratio to scale the director and set its color

			Else reset the color and scale of the director (this doesn't need to be done every frame) and rotate it

		 */
		if (Input.GetMouseButton(0))
		{
			timeHeldRatio = 1 - (-(Time.time - (directorChargeTime + directorHoldStartTime)) / directorChargeTime);

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
}
