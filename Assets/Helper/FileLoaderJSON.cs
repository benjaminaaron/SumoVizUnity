using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class FileLoaderJSON {

	public void loadScenarioFile(string filename) {

		// set scence

		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());


		//load topography objects from .scenario file

		string data = System.IO.File.ReadAllText (filename); //TODO better use file-reading as in FileLoader.cs, performance/safety?
		JSONNode topography = JSON.Parse (data) ["vadere"] ["topography"];

		JSONArray obstacles = topography["obstacles"].AsArray;
		for (int i = 0; i < obstacles.Count; i++) {
			JSONNode shape = obstacles[i]["shape"];
			float height = 4;
			ObstacleExtrudeGeometry.create("wall", parsePoints(shape), height);
		}

		JSONArray sources = topography["sources"].AsArray;
		for (int i = 0; i < sources.Count; i++) {
			JSONNode shape = sources[i]["shape"];
			AreaGeometry.create("source", parsePoints(shape));
		}

		JSONArray targets = topography["targets"].AsArray;
		for (int i = 0; i < targets.Count; i++) {
			JSONNode shape = targets[i]["shape"];
			AreaGeometry.create("target", parsePoints(shape));
		}

		//TODO distinguish objects by their ID: agree with Vadere group on IDs for object types

		loadTrajectoriesFile (filename.Split('.') [0] + ".trajectories"); //we expect it to have to the same filename, should always be like that, right?
	}

	private void loadTrajectoriesFile(string filename){

		if (!System.IO.File.Exists (filename)) {
			Debug.LogError ("file " + filename + " was not found!"); //TODO open FileOpenDialog to search for it
		} else {
			PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();

			System.IO.StreamReader sr = new System.IO.StreamReader(filename);
			string line = sr.ReadLine(); // skip the first line containing column labels

			while ((line = sr.ReadLine()) != null) {
				string[] parts = line.Split (' ');
				if (line.Length > 0) {
					int id;
					decimal time;
					float x, y;
					//int.TryParse (parts [0], out step);
					decimal.TryParse (parts [1], out time);
					int.TryParse (parts [2], out id);
					float.TryParse (parts [3], out x);
					float.TryParse (parts [4], out y);
					//Debug.Log(time + " / " + id + " / " + x + " / " + y);

					pl.addPedestrianPosition(new PedestrianPosition(id, time, x, y));
				}
			}

			pl.createPedestrians ();
		}
	}


	static List<Vector2> parsePoints(JSONNode shape) {
		List<Vector2> list = new List<Vector2>();

		var x = shape["x"].AsFloat;
		var y = shape["y"].AsFloat;
		var width = shape["width"].AsFloat;
		var height = shape["height"].AsFloat;

		list.Add(new Vector2(x, y));
		list.Add(new Vector2(x + width, y));
		list.Add(new Vector2(x + width, y + height));
		list.Add(new Vector2(x, y + height));

		return list;
	}
	
}
