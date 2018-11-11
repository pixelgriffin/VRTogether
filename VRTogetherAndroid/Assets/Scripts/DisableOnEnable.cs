using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnEnable : MonoBehaviour {

    public bool enable = false;

    public List<GameObject> toChange;

    private void OnEnable()
    {
        foreach(GameObject obj in toChange)
        {
            obj.SetActive(enable);
        }
    }
}
