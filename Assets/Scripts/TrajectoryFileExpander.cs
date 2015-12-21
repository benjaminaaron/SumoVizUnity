using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class TrajectoryFileExpander {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();

	public void addPedestrianPosition(PedestrianPosition p) {
		positions.Add (p);
	}

	public void createPedestrians() {
		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();
		SortedList currentList = new SortedList ();

		for (int i = 0; i< positions.Count;i++) {
			if (positions.Count() > (i+1) && positions[i].getX() == positions[i+1].getX() && positions[i].getY() == positions[i+1].getY()) {
				continue;
			}
			currentList.Add(positions[i].getTime(), positions[i]);
			if ((i == (positions.Count-1) || positions[i].getID()!=positions[i+1].getID()) && currentList.Count>0) {

				int id = positions[i].getID();
				//currentList

				foreach (PedestrianPosition pedpos in currentList.Values) {
					Debug.Log (pedpos.toString ());
				}
				Debug.Log ("----------------------------------------------------------------------");

				currentList.Clear();
			}
		}
	}

}
