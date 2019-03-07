using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ChickenWin : MonoBehaviour {

    LevelManager levelManager;

    private void Start()
    {
        levelManager = transform.parent.GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Chicken")
        {
            levelManager.chickenFinished.value = true;
        }
    }
}
