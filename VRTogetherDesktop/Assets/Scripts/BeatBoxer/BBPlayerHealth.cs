using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class BBPlayerHealth : MonoBehaviour {

    public int health = 10;

    public void Damage(int dmg)
    {
        health -= dmg;

        if (health < 0)
            health = 0;
    }

	void Update () {
		if(health <= 0)
        {
            MinigameServer.Instance.EndGame("Scenes/MainMenu", false, 1);
        }
	}
}
