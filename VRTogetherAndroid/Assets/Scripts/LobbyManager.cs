using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTogether.Net;

public class LobbyManager : MonoBehaviour {

	public string code = string.Empty; //The code or IP to connect with
	public string username;

	public GameObject codePanel;
	public GameObject ipPanel;
	public GameObject lobbyPanel;
	public GameObject errorText;

	public Text roomCodeText;

	private MacrogameClient macrogameClient;
	private string ip = string.Empty;

	void Start ()
	{
		macrogameClient = GetComponent<MacrogameClient>();

	}

	public void JoinLobby ()
	{
		if (code != string.Empty)
		{
			roomCodeText.text = "Room Code: " + code;

			//Actual code to connect here...
			if (code.Length != 4 && code.Length != 15) //If it is not an IP or a Code
			{
				//Fail the request immediately

			}

			if (code.Length == 4)
			{
				//Get the IP corresponding to the room code here
				transform.GetComponent<CodeToIP>().Submit(code);

			} else {
				ip = code;
				errorText.SetActive(false);

				macrogameClient.AttemptConnection(ip);

			}

			//Debug.Log("Wow shit bork, we got the ip:" + ip);



		}

	}

	public void SetCode (string newCode)
	{
		code = newCode.ToUpper();

	}

	public void SetUsername (string newUsername)
	{
		username = newUsername;

		macrogameClient.playerName = username;

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

	public void SetIP ()
	{
		ip = transform.GetComponent<CodeToIP>().GetIP();

		Debug.Log("Got IP: " + ip);

		code = ip;

		JoinLobby();

	}

	public void EnableError ()
	{
		Debug.Log("Invalid IP");
		errorText.SetActive(true);

	}
}
