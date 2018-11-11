using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveFly : MonoBehaviour
{
    public int id = -1;

    public bool moving = false;

    private bool swatted = false;

    void Update()
    {
        if (moving)
        {
            this.transform.position += this.transform.forward * Time.deltaTime * 2f;
        }
    }

    public void PickupGrape()
    {
        this.transform.GetChild(0).Find("Grape").gameObject.SetActive(true);
    }

    public void DropGrape()
    {
        this.transform.GetChild(0).Find("Grape").gameObject.SetActive(false);
    }

    public bool IsSwatted()
    {
        return swatted;
    }

    public void Swat()
    {
        swatted = true;

        GameObject.FindGameObjectWithTag("Server").GetComponent<ServerManager>().SwatFlyOverNetwork(this.id);

        this.gameObject.SetActive(false);
    }
}
