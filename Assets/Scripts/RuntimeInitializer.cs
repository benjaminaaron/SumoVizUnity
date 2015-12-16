using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	public GeometryLoader geometryLoader;
	public string fileLoaderIdentifier;
	public List<string> trajectoryLines; 
	//public TrajectoryBakingCentre tbc;

	public List<PedPosGranularPackage> posPackages;

	private Object ped;
	GameObject Pedestrians;
	public List<GameObject> pedestrians = new List<GameObject>();


	void Awake(){
		ped = Resources.Load ("Hans");
		Pedestrians = new GameObject("Pedestrians");
	}


	//public List<Dings> dingsList;

	void Start () {

		/*foreach (Dings d in dingsList) {
			d.action();
		}*/

		FileLoader runtimeFileLoader = null;
		switch (fileLoaderIdentifier) {
			case "vadere":
				runtimeFileLoader = new FileLoaderJSON();
				break;	
			case "accurate":
				runtimeFileLoader = new FileLoaderXML();
				break;
		}

		//runtimeFileLoader.loadTrajectories (trajectoryLines);
		buildPedestrians ();
	}

	private void buildPedestrians(){
		Debug.Log (">> " + posPackages.Count);

		foreach (PedPosGranularPackage posPackage in posPackages) {
			posPackage.action();

			GameObject p = (GameObject) Instantiate(ped);
			p.transform.parent = null;


			p.GetComponent<Pedestrian>().setPositionsNew(posPackage.getPositions());
			p.GetComponent<Pedestrian>().setID(posPackage.pedId);
			pedestrians.Add(p);
			p.transform.SetParent(Pedestrians.transform);
		}
	}

}
