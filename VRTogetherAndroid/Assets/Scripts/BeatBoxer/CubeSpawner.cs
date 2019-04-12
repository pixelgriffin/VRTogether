using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class CubeSpawner : MonoBehaviour {

    public GameObject cubePrefab;

	void Update () {
        if (MinigameClient.Instance.AllPlayersReady())
        {
            if (Input.touchSupported)
            {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    Ray r = Camera.main.ScreenPointToRay(t.position);

                    RaycastHit hit;
                    if (Physics.Raycast(r, out hit))
                    {
                        Vector3 spawnPoint = hit.point;
                        spawnPoint.y = Random.Range(0.25f, 1.25f);
                        MinigameClient.Instance.NetworkRequestServerInstantiate(cubePrefab, spawnPoint, Quaternion.identity);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("MOUSE DOWN");
                    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(r, out hit))
                    {
                        Debug.Log("RAYCAST HIT, REQUESTING SPAWN");
                        Vector3 spawnPoint = hit.point;
                        spawnPoint.y = Random.Range(0.25f, 1.25f);
                        MinigameClient.Instance.NetworkRequestServerInstantiate(cubePrefab, spawnPoint, Quaternion.identity);
                    }
                }
            }
        }
	}
}
