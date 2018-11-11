using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour {
    public GameObject col;

    public UnityEvent OnCollided;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject == col)
        {
            OnCollided.Invoke();
        }
    }
}
