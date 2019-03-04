using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour {

    public GameObject floorOpen, floorFence, floorWall, noFloor;
    public GameObject wall;

    public float radius = 10;

    //private int[] spaceStats;
    //private int spaceCount;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Spawn(int[] spaceStats, int spaceCount)
    {
        //Debug.Log(spaceCount);

        for (int i = 3; i < spaceCount - 1; i++)
        {
            Vector3 position = NextCirclePos(
            transform.position,
            radius,
            i,
            spaceCount);

            Quaternion rotation = Quaternion.FromToRotation(
                Vector3.forward,
                transform.position - position);

            /*
            Debug.Log(
                i + ": " +
                rotation + ", " +
                position + ", " +
                position.x + "," +
                position.z);
            */

            GameObject floorInstance = null;
            switch (spaceStats[i])
            {
                case (int)FloorSpace.NO_FLOOR:
                    floorInstance = Instantiate(
                        noFloor,
                        position,
                        rotation,
                        transform);
                    break;

                case (int)FloorSpace.OPEN:
                    floorInstance = Instantiate(
                        floorOpen,
                        position,
                        rotation,
                        transform);
                    break;

                case (int)FloorSpace.FENCES:
                    floorInstance = Instantiate(
                        floorFence,
                        position,
                        rotation,
                        transform);
                    break;

                case (int)FloorSpace.WALLS:
                    floorInstance = Instantiate(
                        floorWall,
                        position,
                        rotation,
                        transform);
                    break;

                default:
                    break;
            }

            floorInstance.name = "FloorPiece" + i;

            // ensure last piece is rotated correctly
            if (i == spaceCount - 1 && (360 % spaceCount) == 0)
            {
                floorInstance.transform.Rotate(new Vector3(180, 180, 0));
            }

            // spawn walls
            GameObject wallInstance = Instantiate(
                wall,
                floorInstance.transform);

            //this.GetComponent<turretController>().turretSet.Add(turretInstance);
        }
    }

    private Vector3 NextCirclePos(Vector3 center, float r, int it, int amount)
    {

        float angle = (it + 1) * (360 / amount);
        Vector3 pos;
        pos.x = center.x + r * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + r * Mathf.Cos(angle * Mathf.Deg2Rad);
        return pos;

    }
}
