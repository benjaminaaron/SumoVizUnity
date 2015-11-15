using UnityEngine;
using System.Collections;

public class RuntimeInitializer : MonoBehaviour {

	public GeometryLoader geometryLoader;
	public string fileLoaderIdentifier;
	public string trajectoriesFilename = "";

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

		fileLoader.loadTrajectories (trajectoriesFilename);

	}

}
