using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PedPosGranularPackage {

	public int pedId;
	private List<PedestrianPosition> positions = new List<PedestrianPosition>();
	public List<PedPosGranular> positionsGranular = new List<PedPosGranular>();

	public PedPosGranularPackage (int pedId, List<PedestrianPosition> positions) {
		this.pedId = pedId;
		this.positions = positions;
	}

		
}
