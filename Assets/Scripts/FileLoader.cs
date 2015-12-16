using UnityEngine;
using System.Collections.Generic;

public abstract class FileLoader {

	public abstract void loadFileByPath (string filename);
	public abstract void buildGeometry();
	public abstract void loadTrajectories (List<string> trajectoryLines);
	public abstract void loadTrajectoryLines (string filename);
	public abstract string getInputfileExtension();

	PedestrianLoader pl = new PedestrianLoader ();//GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
	//TrajectoryBakingCentre tbc = GameObject.Find("TrajectoryBakingCentre").GetComponent<TrajectoryBakingCentre>();


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



	protected void addPedestrianPosition(int id, decimal time, float x, float y){

		Debug.Log ("- - - " + id + ": " + time + ", " + x + "/" + y);

		PedestrianPosition pos = new PedestrianPosition (id, time, x, y);
		pl.addPedestrianPosition (pos);
		//tbc.addPedestrianPosition (pos);
	}

	protected void createPedestrians(){
		pl.createPedestrians ();
		//tbc.createPedestrians ();
	}

	public void bakeTrajectories(){
		//tbc.bakeTrajectories ();
	}

	public List<PedPosGranularPackage> getPosPackages(){
		return pl.getPosPackages ();
	}

}
