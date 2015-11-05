using UnityEditor;
using UnityEngine;
using System.Collections;

public class ScenarioImporter : MonoBehaviour {

	[MenuItem("Assets/Import Vadere Scenario")]

	static void ImportVadereScenario() {		
		EditorApplication.NewScene();

		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.Rotate(0, 100, 0);
		cube.transform.localScale = new Vector3(1.0f, 1.0f, 4.0f);
		
		EditorApplication.SaveScene("Assets/Scenes/NewScene.unity");
	}

}

/* potentially useful stuff:

EditorApplication.SaveCurrentSceneIfUserWantsTo(); // give user a chance to save the current scene if modified
var triggerOverrideWarning = EditorApplication.currentScene.Equals("Assets/Scenes/MyNewScene.unity");
var path = EditorUtility.OpenFilePanel ("", "", "scenario"); //(string title, string directory, string extension)
Debug.Log (path);
EditorApplication.ExecuteMenuItem("GameObject/3D Object/Cube");
GameObject cube = GameObject.Find("Cube");

https://channel9.msdn.com/Blogs/2p-start/Using-Unity-Editor-scripting-to-create-a-scene-quick-access
 */