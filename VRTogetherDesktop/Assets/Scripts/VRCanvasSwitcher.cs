using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCanvasSwitcher : MonoBehaviour {

    private GameObject mainMenuCanvas, optionsCanvas;
    private int currentCanvas = 0;

    private void Awake()
    {
        mainMenuCanvas = transform.GetChild(0).gameObject;
        optionsCanvas = transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        optionsCanvas.SetActive(false);
    }

    public IEnumerator SwitchCanvas(float time)
    {
        Debug.Log("Entered switch coroutine");

        float angle;
        float elapsedTime = 0f;

        // disable functionality of current canvas
        VRCanvas[] canvases;
        if (currentCanvas == 0)
        {
            canvases = mainMenuCanvas.GetComponentsInChildren<VRCanvas>();
            foreach (VRCanvas canvas in canvases)
            {
                canvas.enabled = false;
            }

            optionsCanvas.SetActive(true);
            canvases = optionsCanvas.GetComponentsInChildren<VRCanvas>();
            foreach (VRCanvas canvas in canvases)
            {
                canvas.enabled = true;
            }

            angle = 180f;
        }
        else
        {
            canvases = optionsCanvas.GetComponentsInChildren<VRCanvas>();
            foreach (VRCanvas canvas in canvases)
            {
                canvas.enabled = false;
            }

            mainMenuCanvas.SetActive(true);
            canvases = mainMenuCanvas.GetComponentsInChildren<VRCanvas>();
            foreach (VRCanvas canvas in canvases)
            {
                canvas.enabled = true;
            }

            angle = -180f;
        }

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.AngleAxis(angle, Vector3.up);

        Debug.Log("Starting switch");

        while (elapsedTime < time)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Finished switch");

        transform.rotation = endRot;

        if (currentCanvas == 0)
        {
            mainMenuCanvas.SetActive(false);
            currentCanvas = 1;
        }
        else
        {
            optionsCanvas.SetActive(false);
            currentCanvas = 0;
        }
    }
}
