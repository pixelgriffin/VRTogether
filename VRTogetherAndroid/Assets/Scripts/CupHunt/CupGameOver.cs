using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using VRTogether.Net;

public class CupGameOver : MonoBehaviour {

    private NetworkFloat timeRemaining = new NetworkFloat("timeRemaining", -1f);
    private Text myText;

	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>();

        NetworkID id = GetComponent<NetworkID>();

        MinigameClient.Instance.RegisterVariable(
            id.netID,
            timeRemaining);


    }

    // Update is called once per frame
    void FixedUpdate () {
        myText.text = "Time Remaining: " + GetTimeFormattedAsTimer(timeRemaining.value);
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
