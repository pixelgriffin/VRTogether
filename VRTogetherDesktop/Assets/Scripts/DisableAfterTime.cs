using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour {

    public List<GameObject> disabled;

    public float time = 30f;

	void Start () {
        StartCoroutine(Disable());
	}

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(time);

        foreach(GameObject obj in disabled)
        {
            obj.SetActive(false);
        }
    }
}
