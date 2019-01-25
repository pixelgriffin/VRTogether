using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour {

	public string code = ""; //The code or IP to connect with

	public void SetCode (string newCode)
	{
		code = newCode;

	}
}
