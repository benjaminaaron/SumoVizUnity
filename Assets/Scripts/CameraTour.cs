using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTour : MonoBehaviour {

	private PlaybackControlNonGUI pc;

	private List<Vector3> waypoints = new List<Vector3>();
	private SortedList<float, Vector3> way = new SortedList<float, Vector3> ();
	float wayLength = 0;

	void Start () {
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControlNonGUI> ();

		//TODO read this in from .scenario file
		waypoints.Add (new Vector3 (-10, 6, -10));
		waypoints.Add (new Vector3 (-10, 6, 10));
		waypoints.Add (new Vector3 (10, 6, 10));
		waypoints.Add (new Vector3 (10, 6, -10));
		waypoints.Add (new Vector3 (-10, 6, -10));

		way.Add(0, waypoints [0]);

		for (int i = 1; i < waypoints.Count; i++) {
			wayLength += Vector3.Distance (waypoints [i - 1], waypoints [i]);
			way.Add(wayLength, waypoints [i]);
		}

		/*
		Debug.Log (wayLength);
		foreach (KeyValuePair<float, Vector3> entry in way){
			Debug.Log(entry.Value);
			Debug.Log(entry.Key);
		}*/
	}
	
	void Update () {
		float lapsedPerc = pc.current_time / pc.total_time; //from 0 to 1
		float targetWayLength = wayLength * lapsedPerc; // from 0 to wayLength

		IEnumerator<KeyValuePair<float, Vector3>> enumeratorPassed = way.GetEnumerator ();
		IEnumerator<KeyValuePair<float, Vector3>> enumeratorNext = way.GetEnumerator ();

		KeyValuePair<float, Vector3> entryPassed = enumeratorPassed.Current;
		enumeratorNext.MoveNext ();
		KeyValuePair<float, Vector3> entryNext = enumeratorNext.Current;

		bool cont = true;
		while (cont){
			cont = enumeratorPassed.MoveNext();
			enumeratorNext.MoveNext ();
			entryPassed = enumeratorPassed.Current;
			entryNext = enumeratorNext.Current;
			if (entryPassed.Key > targetWayLength)
				cont = false;
		}

		Debug.Log (targetWayLength + " >> passed: " + entryPassed.Key + " / " + entryPassed.Value + ", next: " + entryNext.Key + " / " + entryNext.Value);

	}

}
