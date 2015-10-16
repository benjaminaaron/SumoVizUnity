using UnityEngine;
using System.Collections;
using SimpleJSON;

public class FileLoaderJSON {

	public void loadJSONFile(string filename) {
	
		string jsonStr = System.IO.File.ReadAllText (filename);

		JSONNode rootnode = JSON.Parse(jsonStr);
		JSONArray arr = rootnode["vadere"]["topography"]["obstacles"].AsArray;

		for (int i = 0; i < arr.Count; i++) {
			Debug.Log (arr[i]);
		}

	}

}
