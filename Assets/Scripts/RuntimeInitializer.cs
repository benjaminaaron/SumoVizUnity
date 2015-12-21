using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	public GeometryLoader geometryLoader;
	public string fileLoaderIdentifier;
	//public List<string> trajectoryLines; 
	public string trajectoryFilePath;

	void Start () {

		FileLoader runtimeFileLoader = null;
		switch (fileLoaderIdentifier) {
			case "vadere":
				runtimeFileLoader = new FileLoaderJSON();
				break;	
			case "accurate":
				runtimeFileLoader = new FileLoaderXML();
				break;
		}
		runtimeFileLoader.loadTrajectories (trajectoryFilePath);

	}

}
