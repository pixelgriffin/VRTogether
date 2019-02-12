using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class FlyVRPlayerSpawner : MonoBehaviour {

    public GameObject parentRightHand, parentLeftHand, parentHead;
    public GameObject rightHand, leftHand, head;

    private bool doSpawn = true;

    void Update()
    {
        if (doSpawn)
        {
            if (MinigameServer.Instance.AllPlayersReady())
            {
                Debug.Log("Net create cube");

                GameObject inst1 = MinigameServer.Instance.NetworkInstantiate(rightHand);
                inst1.transform.position = parentRightHand.transform.position;
                inst1.transform.rotation = parentRightHand.transform.rotation;
                inst1.transform.SetParent(parentRightHand.transform);

                GameObject inst2 = MinigameServer.Instance.NetworkInstantiate(leftHand);
                inst2.transform.position = parentLeftHand.transform.position;
                inst2.transform.rotation = parentLeftHand.transform.rotation;
                inst2.transform.SetParent(parentLeftHand.transform);

                GameObject inst3 = MinigameServer.Instance.NetworkInstantiate(head);
                inst3.transform.position = parentHead.transform.position;
                inst3.transform.rotation = parentHead.transform.rotation;
                inst3.transform.SetParent(parentHead.transform);

                doSpawn = false;
                Destroy(this.gameObject);
            }
        }
    }

    public bool HasObjectSpawned()
    {
        return !doSpawn;
    }
}
