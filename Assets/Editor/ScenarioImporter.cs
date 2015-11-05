using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ScenarioImporter : MonoBehaviour {


	[MenuItem("Assets/Import Vadere Scenario")]

	static void ImportVadereScenario() {	

		EditorApplication.SaveCurrentSceneIfUserWantsTo(); // give user a chance to save the current scene if modified

		var path = EditorUtility.OpenFilePanel ("", Application.dataPath + "/Resources/vadere_output", "scenario"); //(string title, string directory, string extension)
		var dir = Path.GetDirectoryName (path);
		var fileName = Path.GetFileNameWithoutExtension (path);
		var sceneName = fileName + "_scene.unity";

		var createScene = true;

		if (File.Exists (Application.dataPath + "/Scenes/" + sceneName))
			createScene = EditorUtility.DisplayDialog("override scene", "A scene with the name " + sceneName + " already exists, do you want to override it?", "Yes", "No");

		if (createScene) {
			EditorApplication.NewScene();
			//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//cube.transform.Rotate(0, 100, 0);
			//cube.transform.localScale = new Vector3(1.0f, 1.0f, 4.0f);

			GameObject geometryLoader = new GameObject("GeometryLoader");
			geometryLoader.AddComponent<GeometryLoader>();

			GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
			gl.setTheme (new BeerTentThemingMode ());


			string jsonData = System.IO.File.ReadAllText (path);
			JSONNode topography = JSON.Parse (jsonData) ["vadere"] ["topography"];
			JSONArray obstacles = topography["obstacles"].AsArray;
			for (int i = 0; i < obstacles.Count; i++) {
				JSONNode shape = obstacles[i]["shape"];
				ObstacleExtrudeGeometry.create("wall", parsePoints(shape), 2); //TODO create new cubes instead of using Daniel Büchele's classes?
			}

			EditorApplication.SaveScene("Assets/Scenes/" + sceneName);
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
				Debug.LogError("parsePoints can't handle the type " + shape ["type"] + " yet");
				break;
		}
		return list;
	}

}