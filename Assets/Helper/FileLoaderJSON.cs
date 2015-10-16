using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class FileLoaderJSON {

	public void loadJSONFile(string filename) {

		// set scence
		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());

	
		string jsonStr = System.IO.File.ReadAllText (filename);

		JSONNode rootnode = JSON.Parse(jsonStr);
		JSONArray obstacles = rootnode["vadere"]["topography"]["obstacles"].AsArray;


		for (int i = 0; i < obstacles.Count; i++) {
			JSONNode obstacle = obstacles[i];
			Debug.Log (obstacle);
			float height = 4;
			ObstacleExtrudeGeometry.create("wall", parsePoints(obstacle), height);
		}

	
	}


	static List<Vector2> parsePoints(JSONNode obstacle) {
		List<Vector2> list = new List<Vector2>();

		var x = obstacle["shape"]["x"].AsFloat;
		var y = obstacle["shape"]["y"].AsFloat;
		var width = obstacle["shape"]["width"].AsFloat;
		var height = obstacle["shape"]["height"].AsFloat;

		list.Add(new Vector2(x, y));
		list.Add(new Vector2(x + width, y));
		list.Add(new Vector2(x + width, y + height));
		list.Add(new Vector2(x, y + height));

		return list;
	}

}
