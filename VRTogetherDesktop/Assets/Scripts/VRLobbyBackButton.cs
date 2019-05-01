using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTogether.Net;

public class VRLobbyBackButton : MonoBehaviour {

	public void OnBackButtonPressed()
    {
        Destroy(MacrogameServer.Instance.gameObject);

        SceneManager.LoadScene("StartMenu");
    }
}
