using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTour : MonoBehaviour {
	
	private GameObject cameraObj;
	private PlaybackControlNonGUI pc;
	private float currentTime = 0;

	private List<Vector3> waypoints = new List<Vector3>();
	private SortedList<float, Vector3> waypointsDist = new SortedList<float, Vector3> (); //waypoints and in addition the key is the running distance from start to end
	float s_ges = 0;
	
	IEnumerator<KeyValuePair<float, Vector3>> enumeratorPassed;
	IEnumerator<KeyValuePair<float, Vector3>> enumeratorNext;
	KeyValuePair<float, Vector3> entryPassed;
	KeyValuePair<float, Vector3> entryNext;
	


	private bool firstUpdateDone = false;

	private List<Section> sections = new List<Section>();
	private List<float> times = new List<float> (); //TODO find better name: stores either velocity percentage or in case of 0 the waiting time

	private float t_ges;

	private float accelEndMarkerPerc = 1/5f;
	private float decelStartMarkerPerc = 4/5f;


	void Start () {
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControlNonGUI> ();
		cameraObj = GameObject.Find ("Sphere");

		/*
		waypoints.Add (new Vector3 (39, 5, 50));
		waypoints.Add (new Vector3 (39, 2, 0));
		waypoints.Add (new Vector3 (39, 2, 17));
		waypoints.Add (new Vector3 (19, 2, 17));
		waypoints.Add (new Vector3 (19, 2, 39));
		waypoints.Add (new Vector3 (0, 5, 39));
		waypoints.Add (new Vector3 (19, 2, 39));
		waypoints.Add (new Vector3 (19, 2, 64));
		waypoints.Add (new Vector3 (39, 2, 64));
		waypoints.Add (new Vector3 (39, 4, 82));
		waypoints.Add (new Vector3 (0, 6, 82));
		waypoints.Add (waypoints[0]);*/

		waypoints.Add (new Vector3 (0, 7, 5));
		times.Add (0f);
		waypoints.Add (new Vector3 (5, 7, 5));
		times.Add (0.6f);
		waypoints.Add (new Vector3 (5, 7, -5));
		times.Add (0f);

		//waypointsDist.Add(0, waypoints [0]);

		for (int i = 1; i < waypoints.Count; i ++) {
			Vector3 startWaypoint = waypoints [i - 1];
			float velocReducerStart = times[i - 1] < 0 ? 0 : times[i - 1]; // below 0 means waiting time TODO
			Vector3 endWaypoint = waypoints [i];
			float velocReducerEnd = times[i] < 0 ? 0 : times[i];

			float dist = Vector3.Distance (startWaypoint, endWaypoint);
			Debug.Log ("dist: " + dist);

			float s_accel = dist * accelEndMarkerPerc;
			float s_decel = dist - (dist * decelStartMarkerPerc);
			float s_const = dist - s_accel - s_decel;

			//Debug.Log (string.Concat("s_accel: ", s_accel, " s_const: ", s_const, " s_decel: ", s_decel));

			//public Section (Type type, Vector3 startWaypoint, float velocReducerStart, Vector3 endWaypoint, float velocReducerEnd, float s_upToHere, float s_inSection) {


			Section accelSect = new Section(Section.Type.ACCELERATION, startWaypoint, velocReducerStart, endWaypoint, velocReducerEnd, s_ges, s_accel);
			Debug.Log ("ACCEL-SECT: " + accelSect);
			sections.Add(accelSect);
			Section constSect = new Section(Section.Type.CONSTANT, startWaypoint, velocReducerStart, endWaypoint, velocReducerEnd, s_ges + s_accel, s_const);
			Debug.Log ("CONST-SECT: " + constSect);
			sections.Add(constSect);
			Section decelSect = new Section(Section.Type.DECELERATION, startWaypoint, velocReducerStart, endWaypoint, velocReducerEnd, s_ges + s_accel + s_const, s_decel);
			Debug.Log ("DECEL-SECT: " + decelSect);
			sections.Add(decelSect);

			s_ges += dist;

			//waypointsDist.Add(s_ges, waypoints [i]);
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


	// because pc.total_time is not know when Start() is executed
	private void onFirstUpdate(){
		firstUpdateDone = true;

		t_ges = pc.total_time;
		float v_max = 0;
		foreach (var section in sections) {
			v_max += section.getFormulaContrib();		
		}
		v_max /= t_ges;
		Debug.Log ("v_max: " + v_max);

		foreach (var section in sections) {
			section.calcTinSection(v_max);
		}

		//TEST
		float t_sum = 0;
		foreach (var section in sections) {
			t_sum += section.getTinSection();
		}
		Debug.Log ("sum of t_inSection's: " + t_sum + " <- must be " + t_ges);
	}


	
	void Update () {
		if (!firstUpdateDone)
			onFirstUpdate ();

		if (currentTime > pc.current_time) // reset when next loop starts
			resetEnumerators ();

		currentTime = pc.current_time;
		float lapsedPerc = currentTime / pc.total_time; // from 0 to 1
		float currentTourDist = s_ges * lapsedPerc; // from 0 to wayLength

		if (currentTourDist > entryNext.Key) {
			enumeratorPassed.MoveNext();
			enumeratorNext.MoveNext ();
			entryPassed = enumeratorPassed.Current;
			entryNext = enumeratorNext.Current;
		}
		//Debug.Log (currentTourDist + " >> passed: " + entryPassed.Key + " / " + entryPassed.Value + ", next: " + entryNext.Key + " / " + entryNext.Value);

		float percBtwnWaypoints = (currentTourDist - entryPassed.Key) / (entryNext.Key - entryPassed.Key);
		Vector3 newPos = Vector3.Lerp (entryPassed.Value, entryNext.Value, percBtwnWaypoints);
		if (!float.IsNaN (newPos.x)) {
			cameraObj.transform.position = newPos;
			//transform.position = newPos;
		}
	}

}
