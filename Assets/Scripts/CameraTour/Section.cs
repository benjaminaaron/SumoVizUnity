using UnityEngine;
using System.Collections;


public class Section {

	public enum Type {
		ACCELERATION, CONSTANT, DECELERATION, PAUSE
	};

	private Type type;

	private Vector3 startWaypoint;
	private Vector3 endWaypoint;

	private float velocReducerStart;
	private float velocReducerEnd;

	// s = distance
	private float s_upToHere;
	private float s_inSection;
	//private float s_accelEndMarker; // 1/5
	//private float s_decelStartMarker; // 4/5
	private float s_end;

	// t = time
	private float t_upToHere;
	private float t_inSection;
	private float t_end;

	private float a; // acceleration
	
	public Section (Type type, Vector3 startWaypoint, float velocReducerStart, Vector3 endWaypoint, float velocReducerEnd, float s_upToHere, float s_inSection) {
		this.type = type;
		this.startWaypoint = startWaypoint;
		this.velocReducerStart = velocReducerStart;
		this.endWaypoint = endWaypoint;
		this.velocReducerEnd = velocReducerEnd;
		this.s_upToHere = s_upToHere;
		this.s_inSection = s_inSection;
	}

	public void setT(){
		//TODO
	}

	public override string ToString(){
		return string.Concat("s_upToHere: ", s_upToHere, " s_inSection: ", s_inSection, " velocReducerStart: ", velocReducerStart, " velocReducerEnd: " + velocReducerEnd);
	}

	public float getFormulaContrib(){
		switch (type) {
			case Type.ACCELERATION:
				return 0f;
			case Type.CONSTANT:
				return s_inSection;
			case Type.DECELERATION:
				return 0f;
			case Type.PAUSE:
				return 0f; //TODO ??
			default:
				return 0f;		
		}
	}

}
