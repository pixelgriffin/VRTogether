using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class Ball : MonoBehaviour {

    public float force = 3;

    public GameObject particles;

    private Rigidbody body;
    public BallThrow thrower;

    private GameObject sounds;

	void Start ()
    {
        body = GetComponent<Rigidbody>();
        sounds = GameObject.Find("CupHuntSounds");
        if (sounds == null)
            Debug.Log("SOUNDS IS NULL");
	}
	
	void FixedUpdate ()
    {
        body.AddForce(force * Vector3.down, ForceMode.Acceleration);
	}

    public void BeginDestroyTimer()
    {
        StartCoroutine(DestroyTimer());

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Table"))
        {
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playBallBounce();
            Destroy(soundObject, 5);

            MinigameServer.Instance.NetworkDestroy(gameObject);

        }
        else if (collision.collider.CompareTag("Cup"))
        {
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playCupHit();
            Destroy(soundObject, 5);
        }
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
