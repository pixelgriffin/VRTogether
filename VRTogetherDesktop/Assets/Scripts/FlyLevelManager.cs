using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class FlyLevelManager : MonoBehaviour {

    private NetworkID id;

    private ScoreCounter scoreCounter;

    private int playersAliveCount;

	// Use this for initialization
	void Start () {

        id = GetComponent<NetworkID>();

        scoreCounter = GameObject.Find("ScoreCounter").GetComponent<ScoreCounter>();

        playersAliveCount = MacrogameServer.Instance.GetMacroPlayers().Count;
        Debug.Log("Player alive: " + playersAliveCount);

    }
	
	// Update is called once per frame
	void Update () {

        // check if score was reached
        if (scoreCounter.forwardScore >= scoreCounter.goalScore)
        {
            Debug.Log("GAME OVER - FLIES WIN");
            MinigameServer.Instance.EndGame("Scenes/MainMenu");
        }

        // check if all flies swatted
        if (playersAliveCount <= 0)
        {
            Debug.Log("GAME OVER - FLY SWATTER WINS");
            MinigameServer.Instance.EndGame("Scenes/MainMenu");
        }

    }

    public void DecrPlayersAliveCount()
    {
        playersAliveCount--;
        Debug.Log("Player alive: " + playersAliveCount);
    }
}
