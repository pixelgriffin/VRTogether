using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;
using UnityEngine.SceneManagement;

public class LoadMinigame : MonoBehaviour {

    public void Load(string minigameScene)
    {
        MacrogameServer.Instance.LoadMinigame(minigameScene);
    }
}
