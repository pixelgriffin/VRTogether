using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePointer : MonoBehaviour {

    private Transform objective;

    private void Awake()
    {
        objective = GameObject.Find("FlyGoal").transform;
    }

    private void Update()
    {
        transform.LookAt(objective);
        transform.Rotate(Vector3.right, 90f, Space.Self);
    }
}
