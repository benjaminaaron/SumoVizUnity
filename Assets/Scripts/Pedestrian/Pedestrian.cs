using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class Pedestrian : MonoBehaviour {
	
	Vector3 start;
	Vector3 target;
	float movement_time_total;
	float movement_time_elapsed;
	private float speed;
	//to optimize the getTrait loop
	//private int currentTrait;

	int id;
	SortedList positions = new SortedList ();
	Color myColor;
	bool trajectoryVisible;
	VectorLine trajectory;

	//private InfoText it;

	private PlaybackControlNonGUI pc;
	private Renderer r;

	private bool active;

	// Use this for initialization
	void Start () {

		gameObject.AddComponent<BoxCollider>();
		transform.Rotate (0,90,0);
		//sets the color for the pedestrians
		/*myColor = Color.red;
		GetComponentInChildren<Renderer>().materials[1].color = myColor;*/

		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControlNonGUI> ();
		r = GetComponentInChildren<Renderer>() as Renderer;
	
		//set Tag of the game object in order to find gameobjects with the same tag
		gameObject.tag = "pedestrian";	
	}

	// Update is called once per frame
	void Update () {

		//if (pc.playing) {
			GetComponent<Animation>().Play ();
		/*} else {
			GetComponent<Animation>().Stop ();
		}*/

		int index = _getTrait(positions, pc.current_time);
		
		if (index < positions.Count - 1 && index > -1){
			active = true;
			r.enabled = true;
			PedestrianPosition pos = (PedestrianPosition) positions.GetByIndex (index);
			PedestrianPosition pos2 = (PedestrianPosition) positions.GetByIndex (index+1);
			start = new Vector3 (pos.getX(), 0, pos.getY());
			target = new Vector3 (pos2.getX(), 0, pos2.getY());
			float time = (float) pc.current_time;
			float timeStepLength = Mathf.Clamp((float)pos2.getTime() - (float)pos.getTime(), 0.1f, 50f); // We don't want to divide by zero. OTOH, this results in pedestrians never standing still.
			float movement_percentage = ((float)time - (float)pos.getTime()) / timeStepLength;
			Vector3 newPosition = Vector3.Lerp(start, target, movement_percentage);

			Vector3 relativePos = target - start;
			speed = relativePos.magnitude;

			GetComponent<Animation>()["MaleArm|Walking"].speed = getSpeed () / timeStepLength;
			if (start != target) transform.rotation = Quaternion.LookRotation(relativePos);

			transform.position = newPosition;
			gameObject.hideFlags = HideFlags.None;

		} else {
			//currentTrait = 0;
			active = false;
			r.enabled = false;
			gameObject.hideFlags = HideFlags.HideInHierarchy;
		}

	}

	public float getSpeed() {
		return speed;
	}

	private int _getTrait(SortedList thisList, decimal thisValue) {
		/*while(currentTrait < thisList.Count){
			if ((decimal)thisList.GetKey(currentTrait)>=thisValue) return (currentTrait-1);
			++currentTrait;

		}
		return -1;
		*/


		for (int i = 0; i < thisList.Count; i ++) {
			if ((decimal) thisList.GetKey(i) > thisValue) 
				return (i - 1);
		}
		return -1;
	}
	
	public void setID(int id) {
		this.id = id;
		this.name = "Pedestrian " + id;
	}

	public void setPositions(SortedList p) {
		positions.Clear();
		foreach (PedestrianPosition ped in p.Values) {
			positions.Add(ped.getTime(), ped);
		}
		PedestrianPosition pos = (PedestrianPosition)p.GetByIndex (0);
		transform.position = new Vector3 (pos.getX(), 0, pos.getY());
	}

	public bool isActive(){
		return active;
	}

}
