using UnityEngine;
using System.Collections.Generic;

public abstract class FileLoader {

	public abstract void loadFileByPath (string path);
	public abstract void buildGeometry();
	public abstract void loadTrajectories (string filename);
	public abstract string getIdentifier();
	public abstract string getInputfileExtension();

	protected void createWall(string name, List<Vector2> verticesList, float height){
		ObstacleExtrudeGeometry.create(name, verticesList, height);
	}

	protected void createTable(string name, List<Vector2> verticesList, float height){
		ModelGeometry.create(name, verticesList, height); 
	}

	protected void createBench(string name, List<Vector2> verticesList, float height){
		ModelGeometry.create (name, verticesList, height); 
	}

	protected void createAreaGeometry(string name, List<Vector2> verticesList){
		AreaGeometry.create(name, verticesList);
	}

}
