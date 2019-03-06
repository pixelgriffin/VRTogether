using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class LevelManager : MonoBehaviour {

    private int playersAliveCount;

	// Use this for initialization
	void Start () {

        playersAliveCount = MacrogameServer.Instance.GetMacroPlayers().Count;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (playersAliveCount <= 0)
        {
            Debug.Log("GAME OVER");
            MinigameServer.Instance.EndGame("Scenes/MainMenu");
        }

    }

    public void DecrPlayersAliveCount()
    {
        playersAliveCount--;
    }
}
