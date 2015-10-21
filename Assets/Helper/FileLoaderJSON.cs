using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class FileLoaderJSON {

	public void loadScenarioFile(string parentdir, string filename) {

		// set scence

		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());


		//load config file for IDmappings 2D -> 3D //TODO check if exists, if not use default behaviour
		
		var IDmappings = getIDmappings (Application.dataPath + "/data/IDmappings.config");

		//load topography objects from .scenario file

		string data = System.IO.File.ReadAllText (parentdir + "/" + filename); //TODO better use file-reading as in FileLoader.cs, performance/safety?
		JSONNode topography = JSON.Parse (data) ["vadere"] ["topography"];

		JSONArray obstacles = topography["obstacles"].AsArray;
		for (int i = 0; i < obstacles.Count; i++) {
			string obstacleID = obstacles[i]["id"];
			JSONNode shape = obstacles[i]["shape"];
			//Debug.Log(shape);

			float height = 0.5f;
			float x, y;

			if(IDmappings.ContainsKey (obstacleID)){
				switch (IDmappings[obstacleID]) {
					case "bench":
						height = 1;
						break;
					case "table":
						height = 1.5f;
						break;
					case "roofpoints":

						//TODO 

						break;		
					default:
						break;
				}
			}

			ObstacleExtrudeGeometry.create("wall", parsePoints(shape), height);
		}

		JSONArray sources = topography["sources"].AsArray;
		for (int i = 0; i < sources.Count; i++) {
			//TODO identify different source-types
			JSONNode shape = sources[i]["shape"];
			AreaGeometry.create("source", parsePoints(shape));
		}

		JSONArray targets = topography["targets"].AsArray;
		for (int i = 0; i < targets.Count; i++) {
			JSONNode shape = targets[i]["shape"];
			AreaGeometry.create("target", parsePoints(shape));
		}

		loadTrajectoriesFile (parentdir + "/" + filename.Split ('.')[0] + ".trajectories"); //we expect it to have to the same filename, should always be like that, right?
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


	private static List<Vector2> parsePoints(JSONNode shape) {
		List<Vector2> list = new List<Vector2>();

		float x, y;

		switch (shape ["type"]) {

			case "RECTANGLE":
				x = shape["x"].AsFloat;
				y = shape["y"].AsFloat;
				var width = shape["width"].AsFloat;
				var height = shape["height"].AsFloat;

				list.Add(new Vector2(x, y));
				list.Add(new Vector2(x + width, y));
				list.Add(new Vector2(x + width, y + height));
				list.Add(new Vector2(x, y + height));
				break;

			case "POLYGON":
				JSONArray points = shape["points"].AsArray;
				for (int i = 0; i < points.Count; i++) {
					JSONNode point = points[i];
					x = point["x"].AsFloat;
					y = point["y"].AsFloat;
					list.Add(new Vector2(x, y));
				}
				break;

			default:
				Debug.LogError("I can't handle the type " + shape ["type"] + " yet");
				break;
		}

		return list;
	}


	private Dictionary<string, string> getIDmappings(string filename){
		var IDmappings = new Dictionary<string, string>();
		foreach (string line in System.IO.File.ReadAllLines(filename)) {
			string content = line.Trim();
			
			if(content.Length == 0) // is empty line
				continue;
			
			if(content[0].Equals('#')) // is comment
				continue;
			
			if(content.Split('=').Length != 2) // must be exactly one =
				continue;
			
			// TODO catch more error possibilites
			
			//Debug.Log("added mapping: " + type + " <-> " + id);
			IDmappings.Add(content.Split('=')[0], content.Split('=')[1]);
		}
		return IDmappings;
	}
	
}
