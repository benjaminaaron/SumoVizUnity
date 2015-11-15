using UnityEngine;
using System.Collections;

public class RuntimeInitializer : MonoBehaviour {

	public GeometryLoader geometryLoader;
	public string fileLoaderIdentifier;
	public string trajectoriesFilename = "";

	void Start () {

		FileLoader fileLoader = null;
		switch (fileLoaderIdentifier) {
			case "json":
				fileLoader = new FileLoaderJSON();
				break;	
			case "xml":
				fileLoader = new FileLoaderXML();
				break;
		}

		fileLoader.loadTrajectories (trajectoriesFilename);

	}

}
