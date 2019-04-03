using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenDestroy : MonoBehaviour {

    private GameObject levelManager;

    private Vector3 oldPos;

    void Start()
    {
        oldPos = this.transform.position;

        levelManager = GameObject.Find("LevelManager");
    }

    private void Update()
    {
        if (this.transform.position != oldPos)
        {
            this.transform.GetChild(0).localPosition = Vector3.up * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10f)) * 0.2f;
        }
        else
        {
            this.transform.GetChild(0).localPosition = Vector3.MoveTowards(this.transform.GetChild(0).localPosition, Vector3.zero, Time.deltaTime * 5f);
        }

        oldPos = this.transform.position;
    }

    public void OnDestroy()
    {
        GetComponent<ParticleSystem>().Play();

        levelManager.GetComponent<LevelManager>().DecrPlayersAliveCount();
    }
}
