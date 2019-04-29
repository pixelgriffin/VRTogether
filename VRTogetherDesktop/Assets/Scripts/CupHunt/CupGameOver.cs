using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTogether.Net;

public class CupGameOver : MonoBehaviour {
    public Cup[] cups;

    public float timePerCup = 20f;
    private float timeAllowed = 15f;
    private float startTime = -1f;

    private NetworkFloat timeRemaining = new NetworkFloat("timeRemaining", -1f);
    private bool justStarted = true;

    public bool atLeastOne = false;

    public TextMesh timerText;

	// Use this for initialization
	void Start ()
    {
        NetworkID id = GetComponent<NetworkID>();

        MinigameServer.Instance.RegisterVariable(
            id.netID,
            timeRemaining);

        startTime = 0.0f;
        timeAllowed = MacrogameServer.Instance.GetMacroPlayers().Count * timePerCup;  // The game will last as long as timePerCup * number of mobile players seconds
	}
	
	void FixedUpdate () {

        if (MinigameServer.Instance.AllPlayersReady())
        {
            if (justStarted)
            {
                startTime = Time.time;
                justStarted = false;
            }

            cups = FindObjectsOfType<Cup>();

            if (!atLeastOne && cups.Length > 0)
            {
                atLeastOne = true;

            }

            if (cups.Length == 0 && atLeastOne)
            {
                MinigameServer.Instance.EndGame("Scenes/MainMenu", true, 1);

            }

            if (Time.time >= startTime + timeAllowed)
            {
                MinigameServer.Instance.EndGame("Scenes/MainMenu", false, 1);

            }
            else
            {
                timeRemaining.value = timeAllowed - (Time.time - startTime);

                timerText.text = "Time Remaining:\n" + GetTimeFormattedAsTimer(timeRemaining.value);

                MinigameServer.Instance.SendFloatToAll(timeRemaining);

            }
        }
        else
        {
            timeRemaining.value = timeAllowed;
            timerText.text = "Time Remaining:\n" + GetTimeFormattedAsTimer(timeRemaining.value);
            MinigameServer.Instance.SendFloatToAll(timeRemaining);
        }
    }

    string GetTimeFormattedAsTimer(float sentTime)
    {
        int hourCount = 0;
        int minuteCount = 0;
        float secondCount = 0f;

        string finalString = "";

        if (sentTime >= 3600f)
        {
            float hourCountFl = sentTime / 3600; //Get the number of hours, 1 hr = 3600 s
            hourCount = (int)hourCountFl;

            sentTime = (hourCountFl - hourCount) * 3600;

            if (hourCount > 99)
            {
                hourCount = 99;

            }
            else if (hourCount < 10)
            {
                finalString += "0";

            }

            finalString += hourCount.ToString() + ":";

        }
        else
        {
            //If no hours, do not add it to string

        }

        if (sentTime >= 60)
        {
            float minuteCountFl = sentTime / 60; //Get the number of minutes, 1 min = 60 s
            minuteCount = (int)minuteCountFl;

            sentTime = (minuteCountFl - minuteCount) * 60;

            if (minuteCount < 10)
            {
                finalString += "0";

            }

            finalString += minuteCount.ToString() + ":";

        }
        else
        {
            finalString += "00:";

        }

        if (sentTime > 0f)
        {
            secondCount = sentTime;

            if (secondCount < 10)
            {
                finalString += "0";

            }

            finalString += secondCount.ToString("F3");

        }
        else
        {
            finalString += "00.0";

        }

        return finalString;

    }
}
