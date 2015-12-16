using UnityEngine;
using System.Collections;

[System.Serializable]
public class PedPosGranular {
	
	private decimal time;
	private float x;
	private float y;
	public PedPosGranular (decimal time, float x, float y) {
		this.time = time;
		this.x = x;
		this.y = y;
	}
	
	public decimal getTime() {return this.time;}
	public float getX() {return this.x;}
	public float getY() {return this.y;}

}
