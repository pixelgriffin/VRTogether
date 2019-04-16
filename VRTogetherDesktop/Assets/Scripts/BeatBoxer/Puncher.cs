using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour
{
    private Vector3 vel, prevLoc;

    private AudioSource punchSound;

    void Start()
    {
        prevLoc = this.transform.position;

        punchSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        vel = this.transform.position - prevLoc;


        prevLoc = this.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("BeatChunk") && collision.collider.GetComponent<Rigidbody>() == null)
        {
            collision.collider.gameObject.transform.parent = null;

            Rigidbody body = collision.collider.gameObject.AddComponent<Rigidbody>();
            body.AddForce(vel * 100f, ForceMode.Impulse);
        }
    }
}
