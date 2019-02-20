﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkFlyLeftHand : MonoBehaviour
{

    private GameObject body;
    private bool addComponent = true;

    // Use this for initialization
    void Start()
    {

        body = GameObject.Find("PlayerBody");

        GameObject body_ik = body.transform.GetChild(0).gameObject;
        body_ik.GetComponent<IK>().leftHandObj = this.transform;
        addComponent = false;

    }

    // Update is called once per frame
    void Update()
    {


    }
}
