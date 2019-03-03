using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenOnDestroy : MonoBehaviour {

	public void OnDestroy()
    {
        Debug.Log("Chicken being destroyed");
        Destroy(transform.parent.gameObject);
    }
}
