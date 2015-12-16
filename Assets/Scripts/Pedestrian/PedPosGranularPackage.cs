using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PedPosGranularPackage {

	public int pedId;
	public List<PedestrianPosition> positions = new List<PedestrianPosition>();
	//public List<PedPosGranular> positionsGranular = new List<PedPosGranular>();

	public PedPosGranularPackage (int pedId, List<PedestrianPosition> positions) {
		this.pedId = pedId;
		this.positions = positions;

		Debug.LogError ("PedPosGranularPackage positions count at creation: " + positions.Count);
	}

	public List<PedestrianPosition> getPositions(){
		Debug.LogError ("at getter: " + positions.Count);

		return positions;
	}

	public void action(){
		Debug.Log ("ACtION");
	}

		
}
