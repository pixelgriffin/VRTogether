using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTogether.Net;

public class CubeSpawner : MonoBehaviour {

    public GameObject cubePrefab;

    public Text beatText;

    public float bpm = 127;

    public int maxBeats = 2;

    private int beatsLeft = 0;

    private List<Vector3> spawnPoints = new List<Vector3>();

    private void Start()
    {
        beatsLeft = maxBeats;
        StartCoroutine(SpawnQueued());
        StartCoroutine(IncreaseBeatCount());
    }

    private IEnumerator IncreaseBeatCount()
    {
        yield return new WaitForSeconds(2f / (127 / 60f));

        beatsLeft++;
        if (beatsLeft > maxBeats)
            beatsLeft = maxBeats;

        StartCoroutine(IncreaseBeatCount());
    }

    private IEnumerator SpawnQueued()
    {
        yield return new WaitForSeconds(1f / (127 / 60f));

        foreach(Vector3 point in spawnPoints)
        {
            MinigameClient.Instance.NetworkRequestServerInstantiate(cubePrefab, point, Quaternion.identity);
        }

        spawnPoints.Clear();

        StartCoroutine(SpawnQueued());
    }

    void Update () {
        beatText.text = "Beats Left: " + beatsLeft;

        if (MinigameClient.Instance.AllPlayersReady() && beatsLeft > 0)
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
                        spawnPoints.Add(spawnPoint);
                        beatsLeft--;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(r, out hit))
                    {
                        Vector3 spawnPoint = hit.point;
                        spawnPoint.y = Random.Range(0.25f, 1.25f);
                        spawnPoints.Add(spawnPoint);
                        beatsLeft--;
                    }
                }
            }
        }
	}
}
