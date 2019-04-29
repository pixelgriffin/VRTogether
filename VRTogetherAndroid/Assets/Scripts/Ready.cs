using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class Ready : MonoBehaviour {

    private void Update()
    {
        if (MinigameClient.Instance.AllPlayersReady())
        {
            gameObject.SetActive(false);
        }
    }
}
