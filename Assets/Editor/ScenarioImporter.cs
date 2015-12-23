﻿using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ScenarioImporter : MonoBehaviour {


	[MenuItem("Assets/Import Vadere output")]

	static void importVadereOutput() {	
		importOutput (new FileLoaderJSON(), "vadere");
	}

	[MenuItem("Assets/Import accu:rate output")]
	
	static void importAccurateOutput() {
		importOutput (new FileLoaderXML(), "accurate");
	}

	private static void importOutput(FileLoader fileLoader, string identifier){
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		
		string currentSceneName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
		var continueOk = true;
		if (currentSceneName == "BaseScene") {
			continueOk = !EditorUtility.DisplayDialog("duplicate scene", "It is recommend that you first duplicate the SumoViz Scene (select it in the Scenes folder and use Edit > Duplicate), rename it optionally and doubleclick the duplicated scene.", "let me duplicate first", "continue");
		}
		
		if (continueOk) {
			var path = EditorUtility.OpenFilePanel ("", Application.dataPath + "/data/" + identifier + "_output", fileLoader.getInputfileExtension()); //(string title, string directory, string extension)

			RuntimeInitializer runtimeInitializer = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();

			runtimeInitializer.geometryLoader = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
			runtimeInitializer.geometryLoader.setTheme (new BeerTentThemingMode ()); //TODO read specifics from a config file instead?

			fileLoader.loadFileByPath(Path.GetFileName(path));
			fileLoader.buildGeometry();
			runtimeInitializer.fileLoaderIdentifier = identifier;


			fileLoader.loadTrajectoryLines(Path.GetFileNameWithoutExtension(path) + ".trajectories");


			List<PedPosGranularPackage> pack = fileLoader.getPosPackages();

			Debug.LogError(pack.Count);

			foreach (PedPosGranularPackage posPackage in pack) {
				posPackage.action();
			}

			runtimeInitializer.setPosPackages(pack);

			//EditorUtility.SetDirty(runtimeInitializer);

			/*
			List<Dings> dingsList = new List<Dings> ();
			dingsList.Add(new Dings());
			dingsList.Add(new Dings());
			runtimeInitializer.dingsList = dingsList;*/
		}
	}

	[MenuItem("Assets/delete imported objects")]
	
	static void deleteImportedObjects() {
		DestroyImmediate (GameObject.Find ("World"));
	}

	/*
	[MenuItem("Assets/test")]
	
	static void test() {
		PlaybackControlNonGUI pc  = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControlNonGUI> ();
		decimal sum = 0;
		foreach (decimal delta in pc.deltas) {
			sum += delta;
		}
		Debug.Log ("average: " + (sum / pc.deltas.Count));

		// - - -

		var pedContainer = GameObject.Find ("Pedestrians");
		var peds = pedContainer.GetComponentsInChildren<Pedestrian>();
		//Debug.Log (pedestrians.Length);

		foreach (Pedestrian ped in peds) {		
			Debug.Log (ped.getID() + ": " + ped.isActive());
		}
	}
	*/

}
