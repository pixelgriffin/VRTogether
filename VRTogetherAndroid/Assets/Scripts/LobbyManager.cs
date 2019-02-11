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

	public Text roomCodeText;

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

			roomCodeText.text = "Room Code: " + code;

			//Actual code to connect here...
			if (code.Length != 4 && code.Length != 15) //If it is not an IP or a Code
			{
				//Fail the request immediately

			}

			if (code.Length == 4)
			{
				//Get the IP corresponding to the room code here

			}



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

	public bool IsValidIP (string ip)
	{
		string[] splitStrings = ip.Split('.');

		// Check if the IP can be split into four parts based of of '.'
		if (splitStrings.Length != 4)
		{
			return false;

		}

		// Check that each byte is within [0, 255]
		foreach (string byteString in splitStrings)
		{
			if (int.Parse(byteString) > 255 || int.Parse(byteString) < 0)
			{
				return false;

			}
			
		}

		// It is a string formatted as a valid IP, will not know if it will connect based off of this though
		return true;


	}
}
