using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {

    public float moveStep = 2f;

    private bool isHoldingGrape = false;

	void Update () {
		if (IsMoving())
        {
            transform.localPosition += transform.forward * Time.deltaTime * moveStep;
        }
	}

    public bool IsMoving()
    {
        return Input.touchCount > 0;
    }

    public void HoldGrape()
    {
        isHoldingGrape = true;
    }

    public void DropGrape()
    {
        isHoldingGrape = false;
    }

    public bool IsHoldingGrape()
    {
        return isHoldingGrape;
    }
}
