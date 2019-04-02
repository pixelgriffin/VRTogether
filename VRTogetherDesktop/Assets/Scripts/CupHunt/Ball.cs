using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float force = 3;

    public GameObject particles;

    private Rigidbody body;

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
        yield return new WaitForSeconds(10f);

        GameObject parts = Instantiate(particles, transform.position, Quaternion.identity);
        GameObject.Destroy(parts, 3f);
        GameObject.Destroy(gameObject);

    }
}
