using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour {

	CardboardHead head = null;

	//determine if the camera moves or not
	private bool isMoving = false;

	//This is the variable for the movement speed
	[Tooltip("Speed of the camera movement.")]
	public float speed = 2f;

	[Tooltip("Activate if the height of the camera should be fixed.")]
	public bool fixedHeight = false; 

	[Tooltip("The height (y-coordinate) of the camera.")]
	public float yOffset = 1.55f;

	// Use this for initialization
	void Start () {
	
		head = Camera.main.GetComponent<StereoController>().Head;
	}
	
	// Update is called once per frame
	void Update () {
	
		//turn the movement on or off with a trigger/rap
		if (Cardboard.SDK.Triggered) {
			
			isMoving = !isMoving;
		}

		if (isMoving) {
			Vector3 direction = new Vector3 (head.transform.forward.x, 0, head.transform.forward.z).normalized * speed * Time.deltaTime;
			Quaternion rotation = Quaternion.Euler (new Vector3 (0, -transform.rotation.eulerAngles.y, 0));
			transform.Translate (rotation * direction);
		}
			if(fixedHeight)
			{
				transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
			}
		}


	}
	
	