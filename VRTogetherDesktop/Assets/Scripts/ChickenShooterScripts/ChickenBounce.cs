using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBounce : MonoBehaviour {

    public float jumpingThresh;

    private Vector3 oldPos;
    private Vector3 posOffset;

    private bool grounded;

    // Use this for initialization
    void Start () {

        oldPos = this.transform.position;
        posOffset = Vector3.up * 0.5f;

        grounded = true;

    }
	
	// Update is called once per frame
	void Update () {

        if (grounded && this.transform.position.y >= jumpingThresh)
        {
            grounded = false;
        }
        else if (!grounded && this.transform.position.y < jumpingThresh)
        {
            grounded = true;
        }

        if (grounded)
        {
            if (this.transform.position != oldPos)
            {
                Debug.Log("Bouncing 1");
                this.transform.GetChild(0).localPosition = (Vector3.up * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10f)) * 0.2f) - posOffset;
            }
            else
            {
                Debug.Log("Bouncing 2");
                this.transform.GetChild(0).localPosition = Vector3.MoveTowards(this.transform.GetChild(0).localPosition, -posOffset, Time.deltaTime * 5f);
            }

            oldPos = this.transform.position;
        }

    }
}
