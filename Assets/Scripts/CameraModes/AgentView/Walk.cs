using UnityEngine;
using System.Collections;

/**
 * This class is used to implement the moving around (or walking) in a scene.
 * This is done by moving the camera. */

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
	
	void Start () {
		head = Camera.main.GetComponent<StereoController>().Head;
	}
	
	/** Every frame it is first checked if the user activated the trigger and if yes the camera either starts
	 * moving or stops. If the camera is moving this function interpretes the head movement
	 * and translates it into coordinates to move to. */ 
	
	void Update () {
		//turn the movement on or off with a trigger/rap
		if (Cardboard.SDK.Triggered)
			isMoving = !isMoving;
		
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
