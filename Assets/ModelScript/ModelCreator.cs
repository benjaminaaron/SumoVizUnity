using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelCreator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void createModel (string modelType, List<Vector2> edges) {
		bool initTest = false;
		switch (modelType) {
		case "bench":
			initTest = initObject ("Bierbank_merged", edges);
			break;
		case "table":
			initTest = initObject ("Biertisch_merged", edges);
			break;
		}
		if(!initTest) throw new UnityException();

	}

	bool initObject (string prefabName, List<Vector2> edges) {
		GameObject p = (GameObject)Instantiate(Resources.Load(prefabName));
		if (p == null)
			return false;
		Vector2 xy = edges [1];
		Vector2 widthHeight = edges [3];
		widthHeight = (xy - widthHeight) / 2;
		Vector2 location = xy + widthHeight;
		p.transform.position = location;
		return true;
	}
	
	
}
