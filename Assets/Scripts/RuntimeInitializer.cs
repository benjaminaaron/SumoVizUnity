using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	public GeometryLoader geometryLoader;
	public string fileLoaderIdentifier;
	public List<string> trajectoryLines; 

	void Start () {

		FileLoader fileLoader = null;
		switch (fileLoaderIdentifier) {
			case "vadere":
				fileLoader = new FileLoaderJSON();
				break;	
			case "accurate":
				fileLoader = new FileLoaderXML();
				break;
		}

		fileLoader.loadTrajectories (trajectoryLines);
	}

}
