using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRStartMenu : MonoBehaviour {

	public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Initialization");
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exit button pressed");
        Application.Quit();
    }
}
