using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGeneric : MonoBehaviour {
    public string targetName;
    private GameObject body;

    // Use this for initialization
    void Start () {
        body = GameObject.Find(targetName);
        if (body != null)
        {
            //attach to this object's transform
            body.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            body.transform.SetParent(this.transform);
        } else
        {
            Debug.LogError("Object with name '" + targetName + "' could not be found!");
        }
    }
}
