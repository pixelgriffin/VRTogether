using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkFlyHead : MonoBehaviour
{

    private GameObject body;
    private GameObject lookAt;
    private bool addComponent = true;

    // Use this for initialization
    void Start()
    {
        // create and parent object vr player's ik will look at
        lookAt = new GameObject("lookAt");
        lookAt.transform.position = this.transform.position;
        lookAt.transform.rotation = this.transform.rotation;
        lookAt.transform.SetParent(this.transform);
        // set object position to be 1 unit forward in local space
        Vector3 lookAtPosition = lookAt.transform.localPosition;
        lookAtPosition.z += 1;
        lookAt.transform.localPosition = lookAtPosition;

        body = GameObject.Find("PlayerBody");

        // set ik body to head
        GameObject body_ik = body.transform.GetChild(0).gameObject;
        body_ik.GetComponent<IK>().bodyObj = this.transform;

        // set ik look at to lookAt obj
        body_ik.GetComponent<IK>().lookAtObj = lookAt.transform;

        addComponent = false;
    }

    // Update is called once per frame
    void Update()
    {


    }
}
