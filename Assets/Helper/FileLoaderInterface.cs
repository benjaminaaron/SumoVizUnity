using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface FileLoaderInterface {

	void loadFileByPath (string path);
	void buildGeometry();
	void loadTrajectories (string filename);
	string getIdentifier();
	string getInputfileExtension();

	/*
	void createWall (string name, List<Vector2> verticesList, float height);
	void createTable (string name, List<Vector2> verticesList, float height);
	void createBench (string name, List<Vector2> verticesList, float height);
	void createAreaGeometry(string name, List<Vector2> verticesList);
	*/

}
