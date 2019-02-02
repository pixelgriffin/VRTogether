using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class ScoreCounter : MonoBehaviour
{

    private NetworkID id;

    public int forwardScore;
    public NetworkInt flyScore = new NetworkInt("flyScore", 0);

    void Start()
    {
        id = GetComponent<NetworkID>();
        MinigameClient.Instance.RegisterVariable(id.netID, flyScore);
    }

    private void Update()
    {
        forwardScore = flyScore.value;
    }
}
