using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TrajectoryFileExpander {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();

	private float delta = 0.4f;
	private int stepsBetween = 10;

	public void addPedPos(PedestrianPosition p) {
		positions.Add (p);
	}

	public void sortPositions() {
		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();
	}

	public void writeExpandedTrajectoryFile(string path) {
		List<PedestrianPosition> expandedPositions = new List<PedestrianPosition>();

		int currentId;
		int nextId;
		for (int i = 0; i < positions.Count - 1; i ++){
			PedestrianPosition currentPos = positions [i];
			currentId = currentPos.getID ();
			PedestrianPosition nextPos = positions [i + 1];
			nextId = nextPos.getID ();

			expandedPositions.Add (currentPos);

			if (currentId == nextId) {
				decimal thisTimeDelta = (nextPos.getTime () - currentPos.getTime ()) / stepsBetween;
				float thisXdelta = (nextPos.getX () - currentPos.getX ()) / stepsBetween;
				float thisYdelta = (nextPos.getY () - currentPos.getY ()) / stepsBetween;

				for (int j = 1; j < stepsBetween; j++) {
					decimal time = currentPos.getTime() + j * thisTimeDelta;
					float x = currentPos.getX () + j * thisXdelta;
					float y = currentPos.getY () + j * thisYdelta;
					expandedPositions.Add (new PedestrianPosition(currentId, time, x, y));
				}
			}
		}

		StreamWriter sw = new StreamWriter(path, false);
		sw.Write ("id time id x y\n"); //leave out targetId and sourceId, keep step (as id again) to keep the file format consistent to vadere output

		foreach (PedestrianPosition pedPos in expandedPositions) {
			sw.Write (pedPos.toString () + "\n"); //TODO use System.Environment.NewLine, but doesn't seem to work
		}

		sw.Close ();
	}

}
