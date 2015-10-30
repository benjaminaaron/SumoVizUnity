using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelCreator : Geometry {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	public static  void create (string prefabName, List<Vector2> edges, float height) {
		GameObject p = (GameObject)Instantiate (Resources.Load (prefabName));


		if (p == null) {
			throw new UnityException ();
		}
		Vector2 xy = edges [0];

		//TODO IF the sim gets more Models than Bench and Table
		//-> this Transform setting to static methods

		//for getting transformed from the middle

		Vector2 widthHeight = (xy - edges [2]) / 2;
		widthHeight = new Vector2 (Mathf.Abs (widthHeight.x), Mathf.Abs (widthHeight.y));
		xy = xy + widthHeight;


		p.transform.position = new  Vector3 (xy.x, height, xy.y);


		//for objects needs to be rotated check the height and width to 
		//know how to rotate the object the value dependants on output file from vadere


		if (widthHeight.x > widthHeight.y) {

			p.transform.Rotate(0,90,0);
			
		}
	




	}

	
}
