using UnityEngine;
using System.Collections.Generic;

public abstract class FileLoader {

	public abstract void loadFileByPath (string filename);
	public abstract void buildGeometry();
	public abstract void loadTrajectories (string trajectoryFilePath);
	//public abstract List<string> loadTrajectoryLines (string filename);
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

	protected void createRoof(string name, List<Vector2> roofpoints, float heightAboveGround){
		ModelGeometry.create (name, roofpoints, heightAboveGround); 
	}

}
