using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PedestrianLoader : ScriptableObject {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();
	public List<PedPosGranularPackage> posPackages = new List<PedPosGranularPackage>();
	

	public void addPedestrianPosition(PedestrianPosition p) {
		positions.Add (p);

		//if (p.getTime ()>pc.total_time) pc.total_time = p.getTime ();
	}

	public void createPedestrians() {

		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();

		SortedList currentList = new SortedList ();

		for (int i = 0; i < positions.Count; i ++) {
			if (positions.Count() > (i+1) && positions[i].getX() == positions[i+1].getX() && positions[i].getY() == positions[i+1].getY()) {
				// Only take into account time steps with changed coordinates. We want smooth animation.
				continue;
			}
			currentList.Add(positions[i].getTime(), positions[i]);

			if ((i == (positions.Count - 1) || positions[i].getID() != positions[i + 1].getID()) && currentList.Count > 0) {
				PedPosGranularPackage posPackage = new PedPosGranularPackage(positions[i].getID(), currentList);
				posPackages.Add (posPackage);
				currentList.Clear();
			}
		}
	}

	public List<PedPosGranularPackage> getPosPackages(){
		return posPackages;
	}



	// Update is called once per frame
	void Update () {
	
	}
}
