using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UIRaycaster : MonoBehaviour {

    public GameObject pointerObject;

    private LineRenderer lr;
    private Hand hand;

    private void Start()
    {
        hand = GetComponent<Hand>();
        lr = pointerObject.GetComponent<LineRenderer>();
    }

    void Update () {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                pointerObject.SetActive(true);
                pointerObject.transform.position = hit.point;

                lr.enabled = true;
                lr.SetPosition(0, hit.point);
                lr.SetPosition(1, this.transform.position);

                hand.hoveringGUI = true;
            }
            else
            {
                lr.enabled = false;
                hand.hoveringGUI = false;
            }
        }
        else
        {
            lr.enabled = false;
            hand.hoveringGUI = false;
            pointerObject.SetActive(false);
        }
	}
}
