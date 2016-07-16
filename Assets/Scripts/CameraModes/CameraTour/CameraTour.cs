﻿using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class CameraTour : MonoBehaviour {

	private PlaybackControlNonGUI pc;
	private float currentTime = 0;

	private List<Waypoint> waypoints = new List<Waypoint> ();

	float s_ges = 0;

	private bool firstUpdateDone = false;

	private List<Section> sections = new List<Section>();
	private int currentSectionIndex;

	private float t_ges;
	private float t_waitSum; // sum of all waiting times in waypoints

	private float accelEndMarkerPerc = 0.2f; // 1/5
	private float decelStartMarkerPerc = 0.8f; // 4/5

	private Transform focusPoint;
	private Transform cam;
	//private GameObject cameraObj;


	void Start () {
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControlNonGUI> ();
		focusPoint = GameObject.Find ("FocusPoint").transform;
		cam = Camera.main.transform;

		importWaypoints ();

		if (waypoints[0].doWait()) // extra check this, because i starts at 1 in following for-loop
			addWaitSection (waypoints[0]);

		for (int i = 1; i < waypoints.Count; i ++) {
			Waypoint startWaypoint = waypoints[i - 1];
			Vector3 startWaypointCoords = startWaypoint.getPoint();
			float velocReducerStart = startWaypoint.getVelocReducer();

			Waypoint endWaypoint = waypoints[i];
			Vector3 endWaypointCoords = endWaypoint.getPoint();
			float velocReducerEnd = endWaypoint.getVelocReducer();

			float dist = Vector3.Distance (startWaypointCoords, endWaypointCoords);

			float s_accel = dist * accelEndMarkerPerc;
			float s_decel = dist - (dist * decelStartMarkerPerc);
			float s_const = dist - s_accel - s_decel;

			Vector3 accelEndPoint = Vector3.Lerp (startWaypointCoords, endWaypointCoords, accelEndMarkerPerc);
			Vector3 decelStartPoint = Vector3.Lerp (startWaypointCoords, endWaypointCoords, decelStartMarkerPerc);

			Section accelSect = new Section(Section.Type.ACCELERATION, startWaypoint, endWaypoint, startWaypointCoords, accelEndPoint, velocReducerStart, s_accel);
			sections.Add(accelSect);
			Section constSect = new Section(Section.Type.CONSTANT, startWaypoint, endWaypoint, accelEndPoint, decelStartPoint, 1, s_const);
			sections.Add(constSect);
			Section decelSect = new Section(Section.Type.DECELERATION, startWaypoint, endWaypoint, decelStartPoint, endWaypointCoords, velocReducerEnd, s_decel);
			sections.Add(decelSect);

			if (endWaypoint.doWait())
				addWaitSection(endWaypoint);

			s_ges += dist;
		}
	}

	public string waypointsFile = "/Scripts/CameraModes/CameraTour/waypointsFile-final2000_werbefilm.csv";

	private void importWaypoints() {	
		FileInfo fi = new FileInfo(Application.dataPath + waypointsFile);
		StreamReader reader = fi.OpenText ();
		using (reader) {
			string line;
			while((line = reader.ReadLine ()) != null) {
				if (line.Length > 0) {
					if (line.Substring (0, 1) != "#") {
						string[] values = line.Split(',');
						float x, y, z, velocReducer, waitingTime;

						float.TryParse(values[0].Trim(), out x);
						float.TryParse(values[1].Trim(), out y);
						float.TryParse(values[2].Trim(), out z);
						float.TryParse(values[3].Trim(), out velocReducer);
						float.TryParse(values[4].Trim(), out waitingTime);

						Vector3 point = new Vector3 (x, y, z);

						float.TryParse (values[5].Trim(), out x);
						float.TryParse (values[6].Trim(), out y);
						float.TryParse (values[7].Trim(), out z);

						waypoints.Add (new Waypoint (waypoints.Count, point , velocReducer, waitingTime, new Vector3 (x, y, z)));
					}
				}
			}
			reader.Close ();
		}
	}

	private void addWaitSection(Waypoint wp) {
		Section waitSect = new Section(Section.Type.WAIT, null, null, wp.getPoint(), wp.getPoint(), 0, 0);
		waitSect.setTinSection(wp.getWaitingTime());
		sections.Add (waitSect);
		t_waitSum += wp.getWaitingTime();
	}

	private void onFirstUpdate(){ // because pc.total_time is not know when Start() is executed
		firstUpdateDone = true;

		t_ges = (float) pc.total_time - t_waitSum; // Debug.Log (pc.total_time);

		if (t_ges <= 0)
			throw new UnityException ("the sum of waiting times in camera tour waypoints is bigger than the total simulation time");

		float v_max = 0;
		foreach (var section in sections)
			v_max += section.getFormulaContrib();		
		v_max /= t_ges; // Debug.Log ("v_max: " + v_max);

		float t_sum = 0;
		foreach (var section in sections) {
			section.calcTinSection(v_max); //TODO consolidate these three method calls into one?
			section.setTupToHere(t_sum);
			section.calcAccel();
			t_sum += section.getTinSection();
		}

		newLoop ();
	}

	private void newLoop() {
		currentSectionIndex = 0;
	}

	void Update () {
		if (!firstUpdateDone)
			onFirstUpdate ();

		if (currentTime > (float) pc.current_time) // next loop starts
			newLoop();

		currentTime = (float) pc.current_time;

		Section sec = sections [currentSectionIndex];
		while (!sec.thatsMe (currentTime))
			sec = sections [++ currentSectionIndex];

		Vector3 newPos = sec.getCoordAtT (currentTime);
		cam.position = newPos; //Debug.Log (currentTime + ": " + newPos);

		Waypoint secStartWp = sec.sectionStartWaypoint;
		Waypoint secEndWp = sec.sectionEndWaypoint;

		if (!sec.isWaitSection ()) {
			float distBtwnWps = Vector3.Distance (secStartWp.getPoint (), secEndWp.getPoint ());
			float distToSecStartWp = Vector3.Distance (secStartWp.getPoint (), newPos);
			float perc = distToSecStartWp / distBtwnWps;
			//Debug.Log (dist + "  /  " + distToStart + "  /  " + perc);
			focusPoint.position = Vector3.Lerp (secStartWp.getFocusPoint (), secEndWp.getFocusPoint (), perc);
		}

		cam.LookAt (focusPoint);
	}
}