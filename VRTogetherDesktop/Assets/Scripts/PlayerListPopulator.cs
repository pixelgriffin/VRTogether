using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTogether.Net;

public class PlayerListPopulator : MonoBehaviour {

    public Text playerListText;

	void Update () {
        playerListText.text = "Players:\n";
        foreach(MacrogamePlayer p in MacrogameServer.Instance.GetMacroPlayers())
        {
            playerListText.text += p.name + " - " + MacrogameServer.Instance.GetPlayerScore(p.name) + "\n";
        }
	}
}
