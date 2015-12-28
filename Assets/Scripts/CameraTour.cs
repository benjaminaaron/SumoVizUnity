using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTour : MonoBehaviour {
	
	private GameObject cameraObj;
	private PlaybackControlNonGUI pc;
	private float currentTime = 0;

	private List<Vector3> waypoints = new List<Vector3>();
	private SortedList<float, Vector3> waypointsDist = new SortedList<float, Vector3> (); //waypoints and in addition the key is the running distance from start to end
	float totalTourLength = 0;
	
	IEnumerator<KeyValuePair<float, Vector3>> enumeratorPassed;
	IEnumerator<KeyValuePair<float, Vector3>> enumeratorNext;
	KeyValuePair<float, Vector3> entryPassed;
	KeyValuePair<float, Vector3> entryNext;
	

	void Start () {
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControlNonGUI> ();
		cameraObj = GameObject.Find ("Sphere"); //TODO

		//TODO read this in from .scenario file
		waypoints.Add (new Vector3 (-10, 1, -10));
		waypoints.Add (new Vector3 (-10, 8, 10));
		waypoints.Add (new Vector3 (10, 3, 10));
		waypoints.Add (new Vector3 (10, 9, -10));
		waypoints.Add (waypoints[0]); //close the loop

		waypointsDist.Add(0, waypoints [0]);
		for (int i = 1; i < waypoints.Count; i ++) {
			totalTourLength += Vector3.Distance (waypoints [i - 1], waypoints [i]);
			waypointsDist.Add(totalTourLength, waypoints [i]);
		}

		resetEnumerators ();
	}

	private void resetEnumerators() {
		enumeratorPassed = waypointsDist.GetEnumerator ();
		enumeratorNext = waypointsDist.GetEnumerator ();
		entryPassed = enumeratorPassed.Current;
		enumeratorNext.MoveNext (); // move this iterator one step further
		entryNext = enumeratorNext.Current;
	}
	
	void Update () {
		if (currentTime > pc.current_time) // reset when next loop starts
			resetEnumerators ();

		currentTime = pc.current_time;
		float lapsedPerc = currentTime / pc.total_time; // from 0 to 1
		float currentTourDist = totalTourLength * lapsedPerc; // from 0 to wayLength

		if (currentTourDist > entryNext.Key) {
			enumeratorPassed.MoveNext();
			enumeratorNext.MoveNext ();
			entryPassed = enumeratorPassed.Current;
			entryNext = enumeratorNext.Current;
		}
		//Debug.Log (currentTourDist + " >> passed: " + entryPassed.Key + " / " + entryPassed.Value + ", next: " + entryNext.Key + " / " + entryNext.Value);

		float percBtwnWaypoints = (currentTourDist - entryPassed.Key) / (entryNext.Key - entryPassed.Key);
		Vector3 newPos = Vector3.Lerp (entryPassed.Value, entryNext.Value, percBtwnWaypoints);
		if(!float.IsNaN(newPos.x))
			cameraObj.transform.position = newPos;
	}

}
