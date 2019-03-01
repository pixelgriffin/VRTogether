/*
 * @File: LevelGenerator
 * @Name: AlexKastanek
 * @Brief: This script generates a set of integers that determines what each
 *  space in the level will be.
 * @Description: There will be spaceCount spaces in the level. Each space
 *  can either be a floor with no walls, a floor with a fence, a floor with
 *  walls, or a "noFloor" where chickens will have to jump over the space
 *  to move on with the level
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public enum FloorSpace
{
    NO_FLOOR, // this space has no floor
    OPEN,     // this space has no obstructions
    FENCES,   // this space has a fence obstruction
    WALLS     // this space has a wall obstruction
};

public class LevelGenerator : MonoBehaviour {

    public static int spaceCount = 36;

    private NetworkInt[] networkedSpaceStats = new NetworkInt[spaceCount];
    private NetworkBool networkSendReady = new NetworkBool(
        "networkSendReady", false);
    private NetworkBool spaceStatsSent = new NetworkBool(
        "spaceStatsSent", false);
    private bool levelSpawned = false;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start () {

        // register network data and send ready confirmation
        NetworkID id = GetComponent<NetworkID>();
        for (int i = 0; i < spaceCount; i++)
        {
            networkedSpaceStats[i] = new NetworkInt(
                "networkedSpaceStats" + i, 0);

            MinigameClient.Instance.RegisterVariable(
                id.netID,
                networkedSpaceStats[i]);
        }

        MinigameClient.Instance.RegisterVariable(
            id.netID,
            networkSendReady);

        MinigameClient.Instance.RegisterVariable(
            id.netID,
            spaceStatsSent);

        Debug.Log("Registered all variables");

    }
	
	// Update is called once per frame
	void Update () {

        if (!networkSendReady.value)
        {
            // send ready confirmation
            networkSendReady.value = true;
            MinigameClient.Instance.SendBooleanToAll(networkSendReady);
            Debug.Log("Ready confirmation sent");
        }

        if (spaceStatsSent.value && !levelSpawned)
        {
            int[] spaceStats = new int[spaceCount];
            for (int i = 0; i < spaceCount; i++)
            {
                spaceStats[i] = networkedSpaceStats[i].value;
            }

            GetComponent<FloorSpawner>().Spawn(
                spaceStats,
                spaceCount);

            levelSpawned = true;
        }

    }

    /*
    public int[] GetSpaceStats()
    {
        return spaceStats;
    }
    */
}
