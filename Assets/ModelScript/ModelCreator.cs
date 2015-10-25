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

	public static void create (string modelType, List<Vector2> edges) {

		switch (modelType) {
		case "bench":
			 initObject ("Bierbank_merged", edges);
			break;
		case "table":
			//initTest = initObject ("Biertisch_merged", edges);
			break;
		}


	}

	private static  void initObject (string prefabName, List<Vector2> edges) {
		GameObject p = (GameObject)Instantiate (Resources.Load (prefabName));


		if (p == null) {
			throw new UnityException ();
		}
		Vector2 xy = edges [0];

		//for getting transformed from the middle

		//widthHeight = (xy - widthHeight) / 2;
		//Vector2 location = xy + widthHeight;


		p.transform.position =new  Vector3(xy.x,0.5f,xy.y);


		//TODO rotating the bench or table 

		//Vector2 widthHeight = xy- edges [2];


	}
	
	
}
