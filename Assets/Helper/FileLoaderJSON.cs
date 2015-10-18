using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class FileLoaderJSON {

	public void loadScenarioFile(string filename) {

		// set scence
		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());

	
		string data = System.IO.File.ReadAllText (filename); //TODO better use file-reading as in FileLoader.cs, performance/safety?
		
		JSONNode rootnode = JSON.Parse(data);
		JSONArray obstacles = rootnode["vadere"]["topography"]["obstacles"].AsArray;


		for (int i = 0; i < obstacles.Count; i++) {
			JSONNode obstacle = obstacles[i];
			Debug.Log (obstacle);
			float height = 4;
			ObstacleExtrudeGeometry.create("wall", parsePoints(obstacle), height);
		}

		//TODO create other objects, distinguish them by their ID: agree with Vadere group on IDs for object types


		loadTrajectoriesFile (filename.Split('.') [0] + ".trajectories"); //we expect it to have to the same filename, should always be like that, right?
	
	}

	private void loadTrajectoriesFile(string filename){
		
		/*while (!System.IO.File.Exists(file)) {
			Debug.LogError (filename + "was not found. Choose your trajectories file!");
			OpenFileDialog myOpenFileDialog = new OpenFileDialog();		
			myOpenFileDialog.Filter = "Trajectories Files|*.trajectories";
			if (myOpenFileDialog.ShowDialog() == DialogResult.OK) 
				file = myOpenFileDialog.FileName;
			else 
				return;
		}*/
		
		if (!System.IO.File.Exists (filename)) {
			Debug.LogError (filename + "was not found!");
			return;
		} else {
			string data = System.IO.File.ReadAllText(filename); 

			string[] lines = data.Split ("\n" [0]);

			for (int i = 1; i < lines.Length; i++) { // skip first line, which is "step time id x y targetId sourceId"
				string line = lines [i];
				string[] parts = line.Split (' ');
				if (line.Length > 0) {
					int step, id;
					decimal time;
					float x, y;
					int.TryParse (parts [0], out step);
					decimal.TryParse (parts [1], out time);
					int.TryParse (parts [2], out id);
					float.TryParse (parts [3], out x);
					float.TryParse (parts [4], out y);
					//Debug.Log(step + " / " + time + " / " + id + " / " + x + " / " + y);

					//TODO create the pedestrian object as in FileLoadXML.cs or FileLoader.cs
				}
			}
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
