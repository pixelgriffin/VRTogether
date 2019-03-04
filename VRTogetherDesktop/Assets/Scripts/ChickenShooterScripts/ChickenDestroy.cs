using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenDestroy : MonoBehaviour {

    private GameObject levelManager;

    void Start()
    {
        levelManager = GameObject.Find("LevelManager");
    }

    public void OnDestroy()
    {
        levelManager.GetComponent<LevelManager>().DecrPlayersAliveCount();
    }
}
