using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuKeyboardInput : MonoBehaviour {

    public GameObject mainCanvas, optionsCanvas;

    private VRStartMenu startMenu;
    private VRCanvasSwitcher canvasSwitcher;
    
    private void Start()
    {
        startMenu = GetComponent<VRStartMenu>();
        canvasSwitcher = GetComponent<VRCanvasSwitcher>();
    }

    private void Update()
    {
        if (!canvasSwitcher.switchingCanvas)
        {
            if (mainCanvas.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    startMenu.OnExitButtonPressed();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    startMenu.OnStartButtonPressed();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    startMenu.OnOptionsButtonPressed();
                }
            }
            else if (optionsCanvas.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    startMenu.OnBackButtonPressed();
                }
            }
        }
    }
}
