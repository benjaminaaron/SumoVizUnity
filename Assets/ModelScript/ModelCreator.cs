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

		//for getting transformed from the middle

		Vector2 widthHeight = (xy - edges[2]) / 2;
		widthHeight = new Vector2(Mathf.Abs (widthHeight.x),Mathf.Abs(widthHeight.y));
		xy = xy + widthHeight;


		p.transform.position =new  Vector3(xy.x,height,xy.y);


		//TODO rotating the bench or table 

		//Vector2 widthHeight = xy- edges [2];


	}
	
	
}
