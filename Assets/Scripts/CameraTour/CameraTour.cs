using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraTour : MonoBehaviour {
	
	private GameObject cameraObj;
	private PlaybackControlNonGUI pc;
	private float currentTime = 0;

	private List<Vector3> waypoints = new List<Vector3>();
	float s_ges = 0;

	private bool firstUpdateDone = false;

	private List<Section> sections = new List<Section>();
	private List<float> times = new List<float> (); //TODO find better name: stores either velocity percentage or in case of 0 the waiting time

	private float t_ges;
	private float t_waitSum; // sum of all waiting times in waypoints

	private float accelEndMarkerPerc = 0.2f;//   1/5
	private float decelStartMarkerPerc = 0.8f;// 4/5
	
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

		addWaypoint (0, 7, 5, 0f);
		addWaypoint (5, 7, 5, -1.5f);
		addWaypoint (5, 7, -5, 0.4f);
		addWaypoint (5, 12, -5, 0f);

		if (times [0] < 0) // because i starts at 1 in for loop
			addWaitSection (waypoints [0], Math.Abs(times [0]));

		for (int i = 1; i < waypoints.Count; i ++) {
			Vector3 startWaypoint = waypoints [i - 1];
			float velocReducerStart = times[i - 1] < 0 ? 0 : times[i - 1]; // below 0 means waiting time
			Vector3 endWaypoint = waypoints [i];
			float velocReducerEnd = times[i] < 0 ? 0 : times[i];

			float dist = Vector3.Distance (startWaypoint, endWaypoint);
			//Debug.Log ("dist: " + dist);

			float s_accel = dist * accelEndMarkerPerc;
			float s_decel = dist - (dist * decelStartMarkerPerc);
			float s_const = dist - s_accel - s_decel;
			//Debug.Log (string.Concat("s_accel: ", s_accel, " s_const: ", s_const, " s_decel: ", s_decel));

			Vector3 accelEndPoint = Vector3.Lerp (startWaypoint, endWaypoint, accelEndMarkerPerc);
			Vector3 decelStartPoint = Vector3.Lerp (startWaypoint, endWaypoint, decelStartMarkerPerc);

			Section accelSect = new Section(sections.Count, Section.Type.ACCELERATION, startWaypoint, accelEndPoint, velocReducerStart, s_ges, s_accel);
			//Debug.Log ("ACCEL-SECT: " + accelSect);
			sections.Add(accelSect);
			Section constSect = new Section(sections.Count, Section.Type.CONSTANT, accelEndPoint, decelStartPoint, 1, s_ges + s_accel, s_const);
			//Debug.Log ("CONST-SECT: " + constSect);
			sections.Add(constSect);
			Section decelSect = new Section(sections.Count, Section.Type.DECELERATION, decelStartPoint, endWaypoint, velocReducerEnd, s_ges + s_accel + s_const, s_decel);
			//Debug.Log ("DECEL-SECT: " + decelSect);
			sections.Add(decelSect);

			if (times [i] < 0)
				addWaitSection (waypoints [i], Math.Abs(times [i]));

			s_ges += dist;
		}
	}

	private void addWaitSection(Vector3 waypoint, float waitingTime){
		Section waitSect = new Section(sections.Count, Section.Type.WAIT, waypoint, waypoint, 0, 0, 0);
		waitSect.setTinSection(waitingTime);
		sections.Add (waitSect);
		t_waitSum += waitingTime;
	}

	private void addWaypoint(float x, float y, float z, float timeVal){
		waypoints.Add (new Vector3 (x, y, z));
		times.Add (timeVal); // below 0 means waiting time
	}
	
	private void onFirstUpdate(){// because pc.total_time is not know when Start() is executed
		firstUpdateDone = true;

		t_ges = pc.total_time - t_waitSum;

		if (t_ges <= 0)
			throw new Exception ("The sum of waiting times in camera tour waypoints is bigger than the total simulation time.");

		float v_max = 0;
		foreach (var section in sections)
			v_max += section.getFormulaContrib();		
		v_max /= t_ges;
		//Debug.Log ("v_max: " + v_max);

		foreach (var section in sections)
			section.calcTinSection(v_max);

		float t_sum = 0;
		foreach (var section in sections) {
			section.setTupToHere(t_sum);
			t_sum += section.getTinSection();
			section.calcAccel();
		}
		/*Debug.Log ("sum of t_inSection's: " + t_sum + " <- must be " + t_ges);
		foreach (var section in sections) {
			Debug.Log(section.check());
		}*/
	}

	void Update () {
		if (!firstUpdateDone)
			onFirstUpdate ();

		currentTime = pc.current_time;
		int i = 0;
		Section sec = sections [i];
		while (!sec.thatsMe(currentTime)){
			sec = sections [++ i];
		}
		
		Vector3 pos = sec.getCoordAtT (currentTime);
		//Debug.Log (currentTime + ": " + pos);

		cameraObj.transform.position = pos;
		//transform.position = newPos;
	}

}
