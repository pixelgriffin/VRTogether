using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    public GameObject col;
    public string checkTag = "";

    public UnityEvent OnCollided;

    public void OnCollisionEnter(Collision collision)
    {
        if (checkTag != "")
        {
            if (collision.collider.tag == checkTag)
            {
                OnCollided.Invoke();
            }
        }
        else if (collision.collider.gameObject == col)
        {
            OnCollided.Invoke();
        }
    }
}
