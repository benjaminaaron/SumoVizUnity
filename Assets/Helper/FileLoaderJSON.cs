using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class FileLoaderJSON {

	/**
	 * Loads the vadere output file .scenario and creates 3D objects from the 2D objects based on the IDmappings.config file.
	 * Concludes with calling the method to load the .trajectories file.
	 * @param FileInfo object wrapped around the .scenario file
	 */
	public void loadScenarioFile(FileInfo file) {

		// set scence
		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());

		//load config file for IDmappings 2D -> 3D //TODO check if exists, if not use default behaviour
		var IDmappings = getIDmappings (Application.dataPath + "/data/IDmappings.config");

		//load topography objects from .scenario file
		string data = System.IO.File.ReadAllText (file.FullName);
		JSONNode topography = JSON.Parse (data) ["vadere"] ["topography"];

		List<Vector2> roofpoints = new List<Vector2>();

		JSONArray obstacles = topography["obstacles"].AsArray;

		for (int i = 0; i < obstacles.Count; i++) {
			string obstacleID = obstacles[i]["id"];
			JSONNode shape = obstacles[i]["shape"];

			float height = 2; // just a hack until we have all DesiredobjectExtrudeGeometry objects that have height assigned internally

			if(IDmappings.ContainsKey (obstacleID)){
				switch (IDmappings[obstacleID]) {
					case "bench":
						height = 0.75f;
					ModelCreator.create("bench",parsePoints(shape));
						
						break;
					case "table":
						height = 0.8f;
					ObstacleExtrudeGeometry.create("wall", parsePoints(shape), height);
						break;
						
					case "roofpoints":
						float x = shape["x"].AsFloat + shape["width"].AsFloat / 2;
						float y = shape["y"].AsFloat + shape["height"].AsFloat / 2;
						roofpoints.Add(new Vector2(x, y));
						break;		
					default:

					//Now only all not model objects gets created as cubic
					ObstacleExtrudeGeometry.create("wall", parsePoints(shape), height);
						break;
				}
			}


		}

		if (roofpoints.Count > 0) {
			float floatingHeight = 8; // distance from ground to ceiling
			float roofThickness = 1;
			RoofFloatingGeometry.create("roof", roofpoints, floatingHeight, roofThickness);
			// what if more than one roof? exclude that case or make clear through ID-namespacing?
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

		loadTrajectoriesFile(new FileInfo(file.DirectoryName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension (file.FullName) + ".trajectories")); //we expect it to have to the exact same filename
	}

	/**
	 * Loads the vadere output file .trajectories and create pedestrians with their respective trajectories.
	 * @param FileInfo object wrapped around the .trajectories file
	 */
	private void loadTrajectoriesFile(FileInfo file){

		if (!file.Exists) {
			Debug.LogError ("file " + file.Name + " was not found!"); //TODO open FileOpenDialog to search for it?
		} else {
			PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();

			System.IO.StreamReader sr = new System.IO.StreamReader(file.FullName);
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

	/**
	 * Parses points from obstacles in the .scenario file to a Vector2 list that can be passed to classes extending the Geometry class.
	 * @param one 2D object encapsulated in a JSONNode
	 * @return list of Vector2 objects
	 */
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

	/**
	 * Parses an ID mapping config file to a dictionary that is used to decide what type of 3D object to create based on a 2D object.
	 * @param filename of the config file
	 * @return Dictionary<string, string> containing the mapping from 2D-ID to 3D-typename
	 */
	private Dictionary<string, string> getIDmappings(string filename){ //TODO use FileInfo here as well and check for exists
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
			IDmappings.Add(content.Split('=')[0].Trim(), content.Split('=')[1].Trim ());
		}
		return IDmappings;
	}
	
}
