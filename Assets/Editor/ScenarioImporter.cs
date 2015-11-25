﻿using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ScenarioImporter : MonoBehaviour {


	[MenuItem("Assets/Import Vadere output")]

	static void importVadereOutput() {	
		importOutput (new FileLoaderJSON());
	}

	[MenuItem("Assets/Import accu:rate output")]
	
	static void importAccurateOutput() {
		importOutput (new FileLoaderXML());
	}
	

	private static void importOutput(FileLoader fileLoader){
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		
		string currentSceneName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
		var continueOk = true;
		if (currentSceneName == "SumoViz") {
			continueOk = !EditorUtility.DisplayDialog("duplicate scene", "It is recommend that you first duplicate the SumoViz Scene (select it in the Scenes folder and use Edit > Duplicate), rename it optionally and doubleclick the duplicated scene.", "let me duplicate first", "continue");
		}
		
		if (continueOk) {
			var path = EditorUtility.OpenFilePanel ("", Application.dataPath + "/Resources/" + fileLoader.getIdentifier() + "_output", fileLoader.getInputfileExtension()); //(string title, string directory, string extension)
			var fileName = Path.GetFileNameWithoutExtension (path); //var dir = Path.GetDirectoryName (path);
			
			RuntimeInitializer runtimeInitializer = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();
			runtimeInitializer.trajectoriesFilename = fileName;
			runtimeInitializer.geometryLoader = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
			runtimeInitializer.geometryLoader.setTheme (new BeerTentThemingMode ());
			
			fileLoader.loadFileByPath(path);
			fileLoader.buildGeometry();
			runtimeInitializer.fileLoaderIdentifier = fileLoader.getIdentifier();
			
			//EditorApplication.SaveScene("Assets/Scenes/" + sceneName);
		}
	}
	
}