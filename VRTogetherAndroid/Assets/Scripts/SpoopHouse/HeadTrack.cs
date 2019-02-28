using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrack : MonoBehaviour {
    private GameObject body;

    // Use this for initialization
    void Start()
    {
        body = GameObject.Find("VRHead");
        //attach to this object's transform
        body.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        body.transform.SetParent(this.transform);
        
    }
}
