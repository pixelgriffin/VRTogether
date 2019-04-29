using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class StartMusicOnPlayersReady : MonoBehaviour
{
    private AudioSource src;

    private bool fired = false;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (MinigameServer.Instance.AllPlayersReady() && !fired)
        {
            fired = true;
            src.Play();
        }
    }
}
