using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenDestroy : MonoBehaviour {

    public GameObject featherSpawner;

    private GameObject levelManager;

    private Vector3 oldPos;
    private Vector3 posOffset;

    void Start()
    {
        oldPos = this.transform.position;
        posOffset = Vector3.up * 0.5f;

        levelManager = GameObject.Find("LevelManager");
    }

    private void Update()
    {
        if (this.transform.position != oldPos)
        {
            this.transform.GetChild(0).localPosition = (Vector3.up * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10f)) * 0.2f) - posOffset;
        }
        else
        {
            this.transform.GetChild(0).localPosition = Vector3.MoveTowards(this.transform.GetChild(0).localPosition, -posOffset, Time.deltaTime * 5f);
        }

        oldPos = this.transform.position;
    }

    public void OnDestroy()
    {
        //GetComponent<ParticleSystem>().Play();
        Instantiate(featherSpawner);

        levelManager.GetComponent<LevelManager>().DecrPlayersAliveCount();
    }
}
