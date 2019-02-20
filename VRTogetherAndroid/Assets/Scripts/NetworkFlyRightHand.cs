using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkFlyRightHand : MonoBehaviour {

    public GameObject swatterPrefab;

    private GameObject body;
    private bool addComponent = true;

	// Use this for initialization
	void Start () {

        body = GameObject.Find("PlayerBody");

        GameObject body_ik = body.transform.GetChild(0).gameObject;
        body_ik.GetComponent<IK>().rightHandObj = this.transform;
        addComponent = false;

        // spawn swatter
        Instantiate(swatterPrefab, this.transform, false);

    }
	
	// Update is called once per frame
	void Update () {
    

    }
}
