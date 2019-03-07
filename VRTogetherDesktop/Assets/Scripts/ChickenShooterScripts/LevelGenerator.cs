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

    public const int spaceCount = 36;
    public const int minNoFloor = 10, maxNoFloor = 13;

    private int[] spaceStats;

    private NetworkID id;
    private NetworkInt[] networkedSpaceStats = new NetworkInt[spaceCount];
    private NetworkBool networkSendReady = new NetworkBool(
        "networkSendReady", false);
    private NetworkBool spaceStatsSent = new NetworkBool(
        "spaceStatsSent", false);
    private bool networkDataSent = false;

    private void Awake()
    {
        // list to hold all floors (this excludes noFloors)
        List<int> floorList = new List<int>();

        // list to hold stats about all spaces
        spaceStats = new int[spaceCount];

        /*
        // generate what this floor pattern will be with the following rules:
        // 1) The first and last floors must not be NO_FLOOR
        // 2) If a NO_FLOOR was just created, the next floor must not also be a 
        //    NO_FLOOR
        // 3) There must be a minimum of minNoFloor NO_FLOORS
        // 4) There must be a maximum of maxNoFloor NO_FLOORS
        */

        // generate how many noFloors
        bool justSpawnedOpenFloor = false;
        int noFloorCount = Random.Range(minNoFloor, maxNoFloor + 1);
        int noFloorUsed = 0;

        // generate all floors as not noFloors initially
        spaceStats[0] = (int)FloorSpace.WALLS;
        floorList.Add(0);
        spaceStats[1] = (int)FloorSpace.WALLS;
        floorList.Add(1);
        spaceStats[2] = (int)FloorSpace.WALLS;
        floorList.Add(2);
        for (int i = 3; i < spaceCount - 2; i++)
        {
            if (justSpawnedOpenFloor)
            {
                spaceStats[i] = Random.Range(2, 4);
                justSpawnedOpenFloor = false;
            }
            else
            {
                spaceStats[i] = Random.Range(1, 4);
                if (spaceStats[i] == (int)FloorSpace.OPEN)
                {
                    justSpawnedOpenFloor = true;
                }
            }

            // initialize floor list with index
            floorList.Add(i);
        }
        spaceStats[spaceCount - 2] = (int)FloorSpace.WALLS;
        floorList.Add(spaceCount - 2);
        spaceStats[spaceCount - 1] = (int)FloorSpace.WALLS;
        floorList.Add(spaceCount - 1);

        // for all noFloors remaining, pick a random spot on the list to place
        // it, if this is an invalid place, find the next space that is valid
        for (int i = 0; i < noFloorCount; i++)
        {
            // generate random spot on list of floors
            int floorIndex = Random.Range(3, floorList.Count - 2);
            Debug.Log("no floor at " + floorIndex);

            // iterate until valid space is reached
            int index = floorIndex;
            while (spaceStats[index - 1] == (int)FloorSpace.NO_FLOOR
                || spaceStats[index] == (int)FloorSpace.NO_FLOOR
                || spaceStats[index + 1] == (int)FloorSpace.NO_FLOOR)
            {
                if (index == floorList.Count - 3)
                {
                    index = 3;
                }
                else
                {
                    index++;
                }
                
                if (index == floorIndex)
                {
                    throw new System.Exception();
                }
            }
            floorIndex = index;

            // set space to noFloor
            try
            {
                spaceStats[floorList[floorIndex]] = (int)FloorSpace.NO_FLOOR;

                if (spaceStats[floorList[floorIndex] - 1] == (int)FloorSpace.OPEN
                    && spaceStats[floorList[floorIndex] + 1] == (int)FloorSpace.OPEN)
                {
                    spaceStats[floorList[floorIndex] - 1] = (int)FloorSpace.FENCES;
                }
            }
            catch
            {
                Debug.Log("Exception thrown at size " + floorList.Count);
                Debug.Log("floor index = " + floorIndex);
            }
        }

        // set space count and stats in spawner
        GetComponent<FloorSpawner>().SetSpaceStats(spaceStats);
        GetComponent<FloorSpawner>().SetSpaceCount(spaceCount);

        for (int i = 0; i < spaceCount; i++)
        {
            Debug.Log("Floor " + i + ": " + spaceStats[i]);
        }
        Debug.Log("Generated " + noFloorCount + " noFloors");

    }

    // Use this for initialization
    void Start () {

        // send data over network
        id = GetComponent<NetworkID>();
        for (int i = 0; i < spaceCount; i++)
        {
            networkedSpaceStats[i] = new NetworkInt(
                "networkedSpaceStats" + i,
                0);

            MinigameServer.Instance.RegisterVariable(
                id.netID,
                networkedSpaceStats[i]);

            networkedSpaceStats[i].value = spaceStats[i];
        }

        MinigameServer.Instance.RegisterVariable(id.netID, networkSendReady);

        MinigameServer.Instance.RegisterVariable(id.netID, spaceStatsSent);
    }
	
	// Update is called once per frame
	void Update () {

        if (MinigameServer.Instance.AllPlayersReady() && !networkDataSent)
        {
            for (int i = 0; i < spaceCount; i++)
            {
                MinigameServer.Instance.SendIntegerToAll(networkedSpaceStats[i]);
                Debug.Log("Space Stats sent");
            }

            spaceStatsSent.value = true;
            MinigameServer.Instance.SendBooleanToAll(spaceStatsSent);

            networkDataSent = true;
        }
    }

    public int[] GetSpaceStats()
    {
        return spaceStats;
    }
}
