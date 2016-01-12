using UnityEngine;
using System.Collections;

public class Section {

	public enum Type {
		ACCELERATION, CONSTANT, DECELERATION, PAUSE
	};

	private Type type;

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
	
	public Section (Type type, float s_upToHere, float s_inSection, float t_upToHere, float t_inSection) {
		this.type = type;
		this.s_upToHere = s_upToHere;
		this.s_inSection = s_inSection;
		this.t_upToHere = t_upToHere;
		this.t_inSection = t_inSection;
	}

}
