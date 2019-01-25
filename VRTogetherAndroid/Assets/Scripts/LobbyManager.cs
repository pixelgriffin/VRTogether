using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {

	public string code = string.Empty; //The code or IP to connect with
	public string username;

	public GameObject codePanel;
	public GameObject ipPanel;
	public GameObject lobbyPanel;

	public void JoinLobby ()
	{
		if (code != string.Empty)
		{
			//A test code used to bring up the lobby screen for demo purposes
			if (code == "DEMO")
			{
				ipPanel.SetActive(false);
				codePanel.SetActive(false);
				lobbyPanel.SetActive(true);

			}

			//Actual code to connect here...

		}

	}

	public void SetCode (string newCode)
	{
		code = newCode.ToUpper();

	}

	public void SetUsername (string newUsername)
	{
		username = newUsername.ToUpper();

	}
}
