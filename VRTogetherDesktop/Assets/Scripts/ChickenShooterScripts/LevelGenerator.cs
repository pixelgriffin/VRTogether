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

public enum FloorSpace
{
    NO_FLOOR, // this space has no floor
    OPEN,     // this space has no obstructions
    FENCES,   // this space has a fence obstruction
    WALLS     // this space has a wall obstruction
};

public class LevelGenerator : MonoBehaviour {

    public GameObject floorOpen, floorFence, floorWall;

    public int spaceCount = 36;
    public int minNoFloor = 10, maxNoFloor = 15;

    private int[] spaceStats;

	// Use this for initialization
	void Start () {

        // list to hold all floors (this excludes noFloors)
        List<int> floorList = new List<int>(spaceCount);

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
        int noFloorCount = Random.Range(minNoFloor, maxNoFloor + 1);
        int noFloorUsed = 0;

        // generate all floors as not noFloors initially
        spaceStats[0] = (int)FloorSpace.WALLS;
        floorList.Add(0);
        for (int i = 1; i < spaceCount - 1; i++)
        {
            spaceStats[i] = Random.Range(1, 4);

            // initialize floor list with index
            floorList.Add(i);
        }
        spaceStats[spaceCount - 1] = (int)FloorSpace.WALLS;
        floorList.Add(spaceCount - 1);

        // for all noFloors remaining, pick a random spot on the list to place
        // it, then add the noFloor to the array of all spaces, then delete 
        // that floor and the floors adjacent to it on the list each iteration
        for (int i = 0; i < noFloorCount; i++)
        {
            // generate random spot on list of floors
            int floorIndex = Random.Range(1, floorList.Count - 1);

            // set space to noFloor
            spaceStats[floorList[floorIndex]] = (int)FloorSpace.NO_FLOOR;

            // remove adjacent floors
            floorList.Remove(floorIndex + 1);
            floorList.Remove(floorIndex - 1);
            floorList.Remove(floorIndex);
        }

        for (int i = 0; i < spaceCount; i++)
        {
            Debug.Log("Floor " + i + ": " + spaceStats[i]);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int[] GetSpaceStats()
    {
        return spaceStats;
    }
}
