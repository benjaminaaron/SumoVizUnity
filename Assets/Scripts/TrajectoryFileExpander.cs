using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TrajectoryFileExpander {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();

	public void addPedPos(PedestrianPosition p) {
		positions.Add (p);
	}

	public void sortPositions() {
		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();
	}

	public void writeExpandedTrajectoryFile(string path) {
		StreamWriter sw = new StreamWriter(path, false);

		sw.Write ("id time id x y\n"); //leave out targetId and sourceId, keep step (as id again) to keep the file format consistent to vadere output

		foreach (PedestrianPosition pedPos in positions) {
			sw.Write (pedPos.toString () + "\n"); //TODO use System.Environment.NewLine, but doesn't seem to work
		}

		sw.Close ();
		//System.IO.File.WriteAllText(path, "This is text that goes into the text file");
	}

}
