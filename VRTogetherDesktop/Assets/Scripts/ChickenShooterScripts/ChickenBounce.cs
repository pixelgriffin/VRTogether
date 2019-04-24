﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBounce : MonoBehaviour {

    public float jumpingThresh;
    public float soundInterval;

    private Vector3 oldPos;
    private Vector3 posOffset;

    private bool grounded;
    private bool justBounced, justJumped;

    private ChickenSounds sound;
    private float timeSinceLastSound;

    // Use this for initialization
    void Start () {

        oldPos = this.transform.position;
        posOffset = Vector3.up * 0.5f;

        grounded = true;
        justBounced = false;
        justJumped = false;

        sound = GetComponent<ChickenSounds>();
        timeSinceLastSound = soundInterval;
    }
	
	// Update is called once per frame
	void Update () {

        timeSinceLastSound += Time.deltaTime;

        // check if the chicken is jumping
        if (grounded && this.transform.position.y >= jumpingThresh)
        {
            grounded = false;

            if (justJumped)
            {
                sound.PlayJumpSound();
            }
            justJumped = false;
        }
        else if (!grounded && this.transform.position.y < jumpingThresh)
        {
            grounded = true;

            justJumped = true;
        }

        // if the chicken is not jumping
        if (grounded)
        {
            if (this.transform.position != oldPos)
            {
                // if the chicken has just bounced, enough time has elapsed,
                // and the chicken did not just jump
                if (justBounced && timeSinceLastSound >= soundInterval 
                    && System.Math.Abs(this.transform.position.y - oldPos.y) < 0.001f)
                {
                    // play the run sound
                    sound.PlayRunSound();
                    timeSinceLastSound = 0f;
                }
                justBounced = false;

                // make the chicken bounce (make it's local position follow a sine wave
                this.transform.GetChild(0).localPosition = (Vector3.up * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10f)) * 0.2f) - posOffset;
            }
            else
            {
                justBounced = true;

                // move the chicken back to the ground
                this.transform.GetChild(0).localPosition = Vector3.MoveTowards(this.transform.GetChild(0).localPosition, -posOffset, Time.deltaTime * 5f);
            }

            oldPos = this.transform.position;
        }

    }
}
