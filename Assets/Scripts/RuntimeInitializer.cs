using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	public GeometryLoader geometryLoader;
	public string fileLoaderIdentifier;
	public List<string> trajectoryLines; 

	public List<Dings> dingsList;

	void Start () {

		foreach (Dings d in dingsList) {
			d.action();
		}

		FileLoader runtimeFileLoader = null;
		switch (fileLoaderIdentifier) {
			case "vadere":
				runtimeFileLoader = new FileLoaderJSON();
				break;	
			case "accurate":
				runtimeFileLoader = new FileLoaderXML();
				break;
		}

		runtimeFileLoader.loadTrajectories (trajectoryLines);
	}

}
