using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRStartMenu : MonoBehaviour {

    private GameObject canvasManager;

    private void Awake()
    {
        //canvasManager = transform.parent.parent.parent.gameObject;
    }

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Initialization");
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exit button pressed");
        Application.Quit();
    }

    public void OnOptionsButtonPressed()
    {
        StartCoroutine(
            gameObject.GetComponent<VRCanvasSwitcher>().SwitchCanvas(0.5f));
    }

    public void OnBackButtonPressed()
    {
        StartCoroutine(
            gameObject.GetComponent<VRCanvasSwitcher>().SwitchCanvas(0.5f));
    }
}
