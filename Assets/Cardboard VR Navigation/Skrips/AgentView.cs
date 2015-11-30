﻿using UnityEngine;
using System.Collections;

public class AgentView : MonoBehaviour
{

	private CardboardHead head = null;

	//pedestrian that is currently followed
	private GameObject currentPedestrian = null;
	// Use this for initialization
	void Start ()
	{
	
		head = Camera.main.GetComponent<StereoController> ().Head;

		//		foreach(GameObject ped in GameObject.FindGameObjectsWithTag("pedestrian"))
		//		{
		//			Debug.Log("found");  ""
		//		}
		
		// Debug.Log(GameObject.FindGameObjectsWithTag("pedestrian").size();




	}
	
	// Update is called once per frame
	void Update ()
	{

		if (currentPedestrian != null && currentPedestrian.activeInHierarchy && currentPedestrian.activeSelf) {

			followPedestrian (currentPedestrian);
		} else {

			Debug.Log("No valid pedestrian found");
			findActivePedestrian ();
		}

		if(Cardboard.SDK.Triggered){
			findRandomPedestrian();

		}
		
	}

	private void followPedestrian (GameObject pedestrian)
	{

		Vector3 pedestrianPosition = pedestrian.transform.position;
		Vector3 newPosition = new Vector3 (pedestrianPosition.x, transform.position.y, pedestrianPosition.z); // not changing Y value
		transform.position = newPosition;

	}

	private void findRandomPedestrian ()
	{

		GameObject[] pedestrians = GameObject.FindGameObjectsWithTag ("pedestrian"); 

		if (pedestrians.Length == 0) {
			Debug.LogWarning ("No game objects are tagged with pedestrian");
		} else {

			System.Random random = new System.Random ();
			int position = random.Next (1, pedestrians.Length); 
			currentPedestrian = pedestrians [position];
			Debug.Log ("Set new pedestrian to follow : " + currentPedestrian);

		}
	}

	private void findActivePedestrian ()
	{

		GameObject[] pedestrians;
		pedestrians = GameObject.FindGameObjectsWithTag ("pedestrian"); 
		
		
		if (pedestrians.Length == 0) {
			Debug.LogWarning ("No game objects are tagged with pedestrian");
		} else {

			foreach (GameObject pedestrian in pedestrians) {
				if (pedestrian.activeSelf) {
					currentPedestrian = pedestrian;
					Debug.Log ("Set new pedestrian to follow : " + currentPedestrian
					);
					return;

				}
			}
		}
	}
}
