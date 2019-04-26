using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using VRTogether.Net;

public class CupController : MonoBehaviour {

	public bool inControl = true;

	public GameObject director;  // The arrow which spins below the cup
    public GameObject cameraOject;  // The camera object, spawns only for the cup which is not a slave
	public SpriteRenderer directorSprite;
	public float directorRotateSpeed = 5f;  // Degress/s
	public float directorChargeTime = 3f;  // How long it takes for the director to fully charge
	public float moveForceMax = 10f;
    public float correctedForceDistanceOffset = 0.5f;  // [0, 10]
	private float directorHoldStartTime;  // The time (float) that the mouse was started held down
	private float timeHeldRatio;
	public Vector3 directorChargeScale;  // The maximum scale for the director to reach
	private Vector3 directorStartScale;

	public Gradient powerColors;

    private bool isCharging = false;
    private bool canCharge = false;  // Is this cup allowed to charge? (Player touching cup)
    private GameObject myCam;

    private GameObject sounds;

    private Text helpText;

	void Start ()
    {
        Text[] texts = FindObjectsOfType<Text>();

        foreach (Text text in texts)
        {
            if (text.gameObject.name == "HelpText")
            {
                helpText = text;

                break;

            }

        }
        
        helpText.text = "Press and hold the cup to being charging your movement in the arrow's direction!";

        inControl = !MinigameClient.Instance.networkedPrefabs.IsSlave(GetComponent<NetworkID>().netID);

		directorStartScale = director.transform.localScale;

		SetInControl (inControl);

        sounds = GameObject.Find("CupHuntSounds");
        if (sounds == null)
            Debug.Log("SOUNDS IS NULL");
        else Debug.Log("SOUNDS IS OK");

    }
	
	void Update () 
    {
        

        /*
			When the mouse is first held down, store the time

			When the mouse is first released, apply the charged force 
		 */
        if (Input.GetMouseButtonDown(0) && !isCharging && canCharge)
		{
            

        }
        else if (Input.GetMouseButtonUp(0) && isCharging)
		{
            RaycastHit hit;
            float correctedForce = timeHeldRatio * moveForceMax;

            // If that path would make us collide with another cup, damp the force.
            if (Physics.SphereCast(transform.position, 1f, director.transform.up, out hit, 2f * timeHeldRatio, LayerMask.GetMask("Cup")))
            {
                correctedForce = Mathf.Clamp(hit.distance - correctedForceDistanceOffset, 0, 10f) / 10 * moveForceMax;

            }

			GetComponent<Rigidbody>().AddForce (director.transform.up * correctedForce);

            // play sound effect
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playCupSlide();
            Destroy(soundObject, 5);

            isCharging = false;

            canCharge = false;

            helpText.text = "Press and hold the cup to being charging your movement in the arrow's direction!";

        }

        /*
			While the mouse is held, find how long it has been held in terms of a ratio of 0 to 1 where 0 is the start time
			and 1 is the end time, and use that ratio to scale the director and set its color

			Else reset the color and scale of the director (this doesn't need to be done every frame) and rotate it

		 */
        if (Input.GetMouseButton(0) && isCharging)
		{
			timeHeldRatio = 1 - (-(Time.time - (directorChargeTime + directorHoldStartTime)) / directorChargeTime);
			timeHeldRatio = Mathf.Clamp(timeHeldRatio, 0f, 1f);
			//Grow the arrow
			directorSprite.color = powerColors.Evaluate(timeHeldRatio);
			director.transform.localScale = Vector3.Lerp(directorStartScale, directorChargeScale, timeHeldRatio);

            helpText.text = "Release to move, or hold longer to move farther!";


        } else
		{
			directorSprite.color = Color.white;
			director.transform.localScale = directorStartScale;

			director.transform.Rotate(Vector3.forward * directorRotateSpeed * Time.deltaTime);

		}
	}

	void OnTriggerEnter (Collider col)
	{
        // this should all really be commented out since you are destroying on
        // the server side already
		if (col.tag == "Ball")
		{
            /*
			Debug.Log("Score!");

            col.GetComponent<Rigidbody>().isKinematic = true;
            col.transform.SetParent(transform);*/

            // play sound effect
            GameObject soundObject = Instantiate(sounds, Vector3.zero, Quaternion.identity);
            soundObject.GetComponent<Sounds>().playBallSink();
            Destroy(soundObject, 5);

            /*
            GameObject.Destroy(myCam);
            MinigameClient.Instance.NetworkDestroy(col.gameObject);
            MinigameClient.Instance.NetworkDestroy(gameObject);*/

		}

	}

    public void SetInControl (bool state)
	{
		director.SetActive (state);
		this.enabled = state;

        if (state)
        {
            myCam = Instantiate(cameraOject);
            myCam.GetComponent<SimpleTouchLook>().target = transform;

        }

    }

    public void SetCanCharge (bool state)
    {
        canCharge = state;

        if (canCharge)
        {
            directorHoldStartTime = Time.time;

            isCharging = true;

        }



    }
}
