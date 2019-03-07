using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTogether.Net;

public class ScoreCounter : MonoBehaviour
{
    private NetworkID id;

    public int forwardScore;
    public int goalScore = 8;
    public NetworkInt flyScore = new NetworkInt("flyScore", 0);

    public string scoreText = "Grapes Stolen: ";
    public Text scoreTextObject;

    void Start()
    {
        id = GetComponent<NetworkID>();
        MinigameClient.Instance.RegisterVariable(id.netID, flyScore);

        scoreTextObject.text = scoreText + "0/" + goalScore;
    }

    private void Update()
    {
        forwardScore = flyScore.value;
        scoreTextObject.text = scoreText + forwardScore + "/" + goalScore;
    }
}
