using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;

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
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.Rotate(0, 100, 0);
			cube.transform.localScale = new Vector3(1.0f, 1.0f, 4.0f);
			EditorApplication.SaveScene("Assets/Scenes/" + sceneName);
		}

	}

}