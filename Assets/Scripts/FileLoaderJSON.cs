using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class FileLoaderJSON : FileLoader {

	public const float tentheight = 4f;
	JSONNode obstacles;
	JSONArray sources;
	JSONArray targets;
	Dictionary<string, string> IDmappings;
	string outputDir = Application.dataPath + "/data/vadere_output/";

	public override string getInputfileExtension(){
		return "scenario";
	}

	public override void loadFileByPath (string filename){
		string filecontent = System.IO.File.ReadAllText (outputDir + filename);
		JSONNode topography = JSON.Parse (filecontent) ["vadere"] ["topography"];
		obstacles = topography["obstacles"].AsArray;
		sources = topography["sources"].AsArray;
		targets = topography["targets"].AsArray;
		IDmappings = getIDmappings (System.IO.File.ReadAllText (outputDir + "IDmappings.txt"));
	}

	/**
	 * Loads the vadere output file name_scenario.json and creates 3D objects from the 2D objects based on the IDmappings.config file
	 */
	public override void buildGeometry(){
		buildObstacles ();
		buildSources ();
		buildTargets ();
	}

	private void buildObstacles(){
		List<Vector2> roofpoints = new List<Vector2>();

		for (int i = 0; i < obstacles.Count; i++) {
			string obstacleID = obstacles[i]["id"];
			JSONNode shape = obstacles[i]["shape"];

			if(IDmappings.ContainsKey (obstacleID)){
				switch (IDmappings[obstacleID]) {
				case "table":
					createTable("Table_FBX", parsePoints(shape), 0.78f); //Measurements Table: 220x70x77 (l,w,h)
					break;
				case "roofpoint":
					float x = shape["x"].AsFloat + shape["width"].AsFloat / 2;
					float y = shape["y"].AsFloat + shape["height"].AsFloat / 2;
					//createWall("roofpoint", parsePoints(shape), 6f); //to check if it works
					roofpoints.Add(new Vector2(x, y));
					break;
				case "objects":
					createWall("object", parsePoints(shape), 1.5f);//only an assumption
					break;
				case "fence":
					createWall("fence", parsePoints(shape), 1.0f);//only an assumption
					//ModelGeometry.create("Fence", parsePoints(shape), 1.0f);

					break;


				default:
			

					createWall("wall", parsePoints(shape), tentheight);

						
					//ModelGeometry.create("TwoSideWall",parsePoints(shape), 6.0f);
					break;			
				}
			} else {
				createWall("wall", parsePoints(shape), 2f);
			}
		}

		if (roofpoints.Count > 0)
			createRoof ("Roof_FBX", roofpoints, tentheight);
	}

	private void buildSources(){
		for (int i = 0; i < sources.Count; i++) {
			string sourceID = sources[i]["id"];
			JSONNode shape = sources[i]["shape"];

			if(IDmappings.ContainsKey (sourceID)){
				switch (IDmappings[sourceID]) {
				case "benchsource":
					createBench("Bench_FBX", parsePoints(shape), 0.48f); //Measurements Bench: 220x50x47.5 (l,w,h)
					break;	
				default:
					break;
				}
			} else {
				createAreaGeometry("source", parsePoints(shape));
			}
		}
	}
	
	private void buildTargets(){
		for (int i = 0; i < targets.Count; i++) {
			JSONNode shape = targets[i]["shape"];
			createAreaGeometry("target", parsePoints(shape));
		}
	}


	public override List<string> loadTrajectoryLines (string filename){
		if (!System.IO.File.Exists(outputDir + filename)) {
			Debug.LogError("Error: File " + filename + " not found.");
		}
		List<string> trajectoryLines = new List<string> ();

		using (StreamReader sr = new StreamReader(outputDir + filename)) {
			string line = sr.ReadLine(); // skip first line
			while ((line = sr.ReadLine()) != null) {
				trajectoryLines.Add(line);
			}
		}
		return trajectoryLines;
	}


	/**
	 * Loads the vadere output file name_trajectories.txt and creates pedestrians with their respective trajectories
	 * @param filename of the trajectories file without _trajectories.txt
	 */
	public override void loadTrajectories(List<string> trajectoryLines){
		PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();

		foreach (string line in trajectoryLines) {
			string[] parts = line.Split (' ');
			int id;
			float time;
			float x, y;
			//int.TryParse (parts [0], out step);
			float.TryParse (parts [1], out time);
			int.TryParse (parts [2], out id);
			float.TryParse (parts [3], out x);
			float.TryParse (parts [4], out y);
			//Debug.Log(time + " / " + id + " / " + x + " / " + y);
			
			pl.addPedestrianPosition(new PedestrianPosition(id, time, x, y));
		}
		pl.createPedestrians ();
	}

	/**
	 * Parses points from obstacles in the scenario file to a Vector2 list that can be passed to classes extending the Geometry class
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
	 * Parses an ID mapping config file to a dictionary that is used to decide what type of 3D object to create based on a 2D object
	 * @param filename of the config file
	 * @return Dictionary<string, string> containing the mapping from 2D-ID to 3D-typename
	 */
	private static Dictionary<string, string> getIDmappings(string filecontent){ //TODO check if exists
		var IDmappings = new Dictionary<string, string>();

		foreach (string line in filecontent.Split("\n"[0])) {
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
