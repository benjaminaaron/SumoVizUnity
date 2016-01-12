using UnityEngine;
using System;
using System.Collections;


public class Section {

	public enum Type {
		ACCELERATION, CONSTANT, DECELERATION, PAUSE
	};

	private int id;
	private Type type;

	private Vector3 startWaypoint;
	private Vector3 endWaypoint;

	private float velocReducerStart;
	private float velocReducerEnd;

	// s = distance
	private float s_upToHere;
	private float s_inSection;
	private float s_end;

	// t = time
	private float t_upToHere;
	private float t_inSection;
	private float t_end;

	private float v_max;
	private float accel = 0; // acceleration
	
	public Section (int id, Type type, Vector3 startWaypoint, float velocReducerStart, Vector3 endWaypoint, float velocReducerEnd, float s_upToHere, float s_inSection) {
		this.id = id;
		this.type = type;
		this.startWaypoint = startWaypoint;
		this.velocReducerStart = velocReducerStart;
		this.endWaypoint = endWaypoint;
		this.velocReducerEnd = velocReducerEnd;
		this.s_upToHere = s_upToHere;
		this.s_inSection = s_inSection;
	}

	public override string ToString(){
		return string.Concat("id: ", id, "  s_upToHere: ", s_upToHere, "  s_inSection: ", s_inSection, "  velocReducerStart: ", velocReducerStart, "  velocReducerEnd: " + velocReducerEnd);
	}

	//for testing
	public string check(){
		return t_upToHere + " + " + t_inSection + " = " + t_end + "   /   accel: " + accel;
	}

	public float getFormulaContrib(){
		switch (type) {
			case Type.ACCELERATION:
				return s_inSection / (0.5f * velocReducerStart + 0.5f);
			case Type.CONSTANT:
				return s_inSection;
			case Type.DECELERATION:
				return s_inSection / (0.5f * velocReducerEnd + 0.5f);
			case Type.PAUSE:
				return 0f;
			default:
				return 0f;		
		}
	}

	public void calcTinSection(float v_max){
		this.v_max = v_max;

		switch (type) {
		case Type.ACCELERATION:
			t_inSection = s_inSection / (v_max * (0.5f + 0.5f * velocReducerStart));
			break;
		case Type.CONSTANT:
			t_inSection = s_inSection / v_max;
			break;
		case Type.DECELERATION:
			t_inSection = s_inSection / (v_max * (0.5f + 0.5f * velocReducerEnd));
			break;
		case Type.PAUSE:
			break;
		}
	}

	public float getTinSection(){
		return t_inSection;
	}
	
	public void setTupToHere(float t_upToHere){
		this.t_upToHere = t_upToHere;
		t_end = t_upToHere + t_inSection;
	}

	public void calcAccel(){
		switch (type) {
		case Type.ACCELERATION:
			accel = (2f * (s_inSection - velocReducerStart * v_max * t_inSection)) / (float) Math.Pow(t_inSection, 2);
			break;
		case Type.DECELERATION:
			accel = (2f * (s_inSection - v_max * t_inSection)) / (float) Math.Pow(t_inSection, 2);
			break;
		}

	}

	public bool thatsMe(float time){
		if (time >= t_upToHere && time < t_end)
			return true;
		return false;		
	}

	/*
	public Vector3 getCoordAtT(float time){
		float timepointWithinSection = time - t_upToHere;
		return null;
	}*/










}
