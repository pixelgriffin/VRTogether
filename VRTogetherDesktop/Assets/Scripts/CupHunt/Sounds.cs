using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

    private AudioSource ballBounce;
    private AudioSource ballSink;
    private AudioSource cupHit;
    private AudioSource cupSlide;

    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        ballBounce = sources[0];
        ballSink = sources[1];
        cupHit = sources[2];
        cupSlide = sources[3];

        if (cupSlide == null)
            Debug.Log("start - sound is null");
        else
            Debug.Log("start - sound is ok");
    }

    public void playBallBounce()
    {
        ballBounce.Play();
    }

    public void playBallSink()
    {
        ballSink.Play();
    }

    public void playCupHit()
    {
        cupHit.Play();
    }

    public void playCupSlide()
    {
        if (cupSlide == null)
            Debug.Log("play - sound is null");
        else
            Debug.Log("play - sound is ok");
        cupSlide.Play();
    }

}
