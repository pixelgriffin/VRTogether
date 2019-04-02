using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class Ball : MonoBehaviour {

    public float force = 3;

    public GameObject particles;

    private Rigidbody body;
    public BallThrow thrower;

	void Start ()
    {
        body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        body.AddForce(force * Vector3.down, ForceMode.Acceleration);
	}

    public void BeginDestroyTimer()
    {
        StartCoroutine(DestroyTimer());

    }

    IEnumerator DestroyTimer ()
    {
        yield return new WaitForSeconds(5f);

        thrower.balls.Remove(gameObject);

        GameObject parts = MinigameServer.Instance.NetworkInstantiate(particles);
        parts.transform.position = transform.position;

        MinigameServer.Instance.NetworkDestroy(gameObject);

        //GameObject.Destroy(parts, 3f);
        //GameObject.Destroy(gameObject);

    }
}
