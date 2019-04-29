using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTogether.Net;

public class MoveTowardPlayer : MonoBehaviour {

    public float speed = 5f;

    public Transform playerHead;

    private float initialDistance = 0f;

    private Vector3 initialPos;

	void Start () {
        if(playerHead == null)
        {
            playerHead = Camera.main.transform;
        }

        initialPos = playerHead.position;
        Vector3 look = new Vector3(playerHead.position.x, this.transform.position.y, playerHead.position.z);
        this.transform.LookAt(look);

        initialDistance = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(initialPos.x, initialPos.z));
    }


	void Update () {
        float deadDist = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(initialPos.x, initialPos.z));

        if (deadDist < 0.75f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<BBPlayerHealth>().Damage(1);
            MinigameServer.Instance.NetworkDestroy(this.gameObject);
        }

        this.transform.position += (this.transform.forward * Time.deltaTime * initialDistance) / ( speed * 2);
	}

    public void GetPunched()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        initialDistance = 0;
        MinigameServer.Instance.NetworkRequestUnslave(this.gameObject);

        yield return new WaitForSeconds(1f);

        MinigameServer.Instance.networkedPrefabs.NetworkDestroy(this.gameObject);
    }
}
