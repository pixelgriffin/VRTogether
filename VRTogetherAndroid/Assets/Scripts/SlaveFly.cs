using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveFly : MonoBehaviour {

    public bool moving = false;

    private bool holdingGrape = false;

	void Update () {
        if(moving)
        {
            this.transform.position += this.transform.forward * Time.deltaTime * 2f;
        }
	}

    public void PickupGrape()
    {
        this.transform.Find("Grape").gameObject.SetActive(true);
    }

    public void DropGrape()
    {
        this.transform.Find("Grape").gameObject.SetActive(false);
    }
}
