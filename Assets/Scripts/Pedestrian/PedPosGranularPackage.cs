using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PedPosGranularPackage : ScriptableObject {

	public int pedId;
	public List<PedestrianPosition> positions = new List<PedestrianPosition>();
	//public List<PedPosGranular> positionsGranular = new List<PedPosGranular>();

	public PedPosGranularPackage (int pedId, SortedList p) {
		this.pedId = pedId;
		foreach (PedestrianPosition ped in p.Values) {
			positions.Add(ped);
		}

		Debug.LogError ("PedPosGranularPackage positions count at creation: " + positions.Count);
	}

	public SortedList getPositions(){
		Debug.LogError ("at getter: " + positions.Count);

		SortedList spos = new SortedList ();

		foreach (PedestrianPosition ped in positions) {
			spos.Add (ped.getTime (), ped);
		}
			
		return spos;
	}

	public void action(){
		Debug.Log ("ACtION, i have positions: " + positions.Count);

		Debug.Log (pedId);

	}

		
}
