using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class LevelManager : MonoBehaviour {

    private NetworkID id;

    private NetworkBool chickenFinished = new NetworkBool("chickenFinished", false);

    private int playersAliveCount;

	// Use this for initialization
	void Start () {

        id = GetComponent<NetworkID>();

        MinigameServer.Instance.RegisterVariable(id.netID, chickenFinished);

        playersAliveCount = MacrogameServer.Instance.GetMacroPlayers().Count;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (chickenFinished.value)
        {
            Debug.Log("GAME OVER - CHICKEN WINS");
            MinigameServer.Instance.EndGame("Scenes/MainMenu", false, 1);
        }

        if (playersAliveCount <= 0)
        {
            Debug.Log("GAME OVER - CHICKEN FEEDER WINS");
            MinigameServer.Instance.EndGame("Scenes/MainMenu", true, 1);
        }

    }

    public void DecrPlayersAliveCount()
    {
        playersAliveCount--;
    }
}
