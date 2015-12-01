using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

public class FileLoaderXML : FileLoader {
	
	// Listen anlegen für verschiedene Objekt-Typen aus dem accu:rate-output
	List<XmlElement> openWalls;
	List<XmlElement> walls;
	List<XmlElement> origins;
	List<XmlElement> destinations;
	List<XmlElement> scaledAreas;
	List<XmlElement> waitingZones;
	List<XmlElement> portals;
	List<XmlElement> beamExits;
	List<XmlElement> eofWalls;
	List<XmlElement> queuingAreas;
	List<XmlElement> obstacles;
	float height;

	// Neues Format: Dateien liegen im Speicher!!!!! nicht im xml!

	public override string getIdentifier(){
		return "accurate";
	}
	
	public override string getInputfileExtension(){
		return "xml";
	}

	public override void loadTrajectories (string filename){

		string filecontent = (Resources.Load ("accurate_output/" + filename + "_scenario") as TextAsset).text;

		/*
		// In dieser Methode die Fallunterscheidung machen zwischen alten und neuen Dateien!
		if (!System.IO.File.Exists (filename)) {
			Debug.LogError ("Error: File " + filename + " not found.");
			return;
		}*/
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(filecontent);
		XmlNode output = xmlDoc.SelectSingleNode ("//output");
		if (output == null) {
			Debug.Log("Debug: No output/ pedestrian position data found in file.");
			return;
		}

		PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();

		foreach (XmlElement floor in output.SelectNodes("floor")) {
			//if (floor.GetAttribute("csvAt") == null) {

				foreach (string line in floor.InnerText.ToString().Split("\n"[0])) { //TODO definitely streamreading again, but how...
				Debug.Log (line);	
				/*string[] v = line.Split (',');
					if(v.Length >= 3) {
						decimal time;
						int id;
						float x;
						float y;
						decimal.TryParse (v[0], out time);
						int.TryParse(v[1], out id);
						float.TryParse(v[2], out x);
						float.TryParse(v[3], out y);
						pl.addPedestrianPosition(new PedestrianPosition(id, time, x, y));
					}*/
				}


			//}
			/*else { //TODO use TextAsset instead of StreamReader
				// hier den Pfad zu den Dateien holen.... 
				// Wie erfahre ich welche floor-ID ich brauch? Es soll ja nach floors unterschieden werden
				// auslesen, wo die Datei liegt
				string pathToCsv = floor.GetAttribute("csvAt");
				//dem Pfad folgen und die Datei lesen.. BUT HOW?..:(

				//StreamReader reader = new StreamReader(new BufferedStream())

				// pfad extrahieren von der xml-datei ohne den namen der Datei
				string pathToFile = Path.GetFullPath(filename);
				string pathToDir = Path.GetDirectoryName(pathToFile);
				// Kann man die strings einfach mit + aneinanderhängen
				string pathToOutFile = pathToDir + pathToCsv;

				var csvReader = new StreamReader(File.OpenRead(pathToOutFile));
				string line = csvReader.ReadLine();
				var values = line.Split(';');
				if(values.Length >= 3) {
					decimal time;
					int id;
					float x;
					float y;
					decimal.TryParse(values[0], out time);
					int.TryParse (values[1], out id);
					float.TryParse (values[2], out x);
					float.TryParse (values[3], out y);
					pl.addPedestrianPosition(new PedestrianPosition(id, time, x, y));
				}
			}*/
		}
		pl.createPedestrians ();
	}

	public override void loadFileByPath(string path) {
		if (!System.IO.File.Exists (path)) {
			Debug.LogError("Error: file " + path + " not found.");
			return;
		}
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (System.IO.File.ReadAllText(path));

		GeometryLoader gl = GameObject.Find ("GeometryLoader").GetComponent<GeometryLoader> ();
		gl.setTheme (new BeerTentThemingMode ());

		XmlNode spatial = xmlDoc.SelectSingleNode ("//spatial");
		// Jetzt arrays erstellen von allen objekten
		// Diese werden danach in buildGeometry verwendet
		foreach (XmlElement floor in spatial.SelectNodes("floor")) {

			height = TryParseWithDefault.ToSingle(floor.GetAttribute("height"), 1.0f);

			openWalls = new List<XmlElement>();
			walls = new List<XmlElement>();
			origins = new List<XmlElement>();
			destinations = new List<XmlElement>();
			scaledAreas = new List<XmlElement>();
			waitingZones = new List<XmlElement>();
			portals = new List<XmlElement>();
			beamExits = new List<XmlElement>();
			eofWalls = new List<XmlElement>();
			queuingAreas = new List<XmlElement>();
			obstacles = new List<XmlElement>();

			foreach(XmlElement geomObj in floor.SelectNodes("object")) {
				// Wann welchen Typ von extrudeGeometry verwenden?
				switch(geomObj.GetAttribute ("type")) {
				case "wall":
					//ObstacleExtrudeGeometry.create (geomObj.GetAttribute("name"), parsePoints(geomObj), height);
					walls.Add (geomObj);
					break;
				case "origin":
					// aufrufen von createAreaGeometry
					origins.Add(geomObj);
					break;
				case "destination":
					// aufrufen von createAreaGeometry
					destinations.Add(geomObj);
					break;
				case "openWall":
					//WallExtrudeGeometry.create (geomObj.GetAttribute ("name"), parsePoints(geomObj), height, -0.2f);
					openWalls.Add (geomObj);
					break;
				case "obstacle":
					//ObstacleExtrudeGeometry.create(geomObj.GetAttribute("name"), parsePoints(geomObj), height);
					obstacles.Add(geomObj);
					break;
				case "scaledArea":
					scaledAreas.Add (geomObj);
					break;
				case "waitingZone":
					waitingZones.Add (geomObj);
					break;
				case "queuingArea":
					//AreaGeometry.create(geomObj.GetAttributes("name").parsePoints(geomObj));
					queuingAreas.Add(geomObj);
					break;
				case "portal":
					portals.Add (geomObj);
					break;
				case "beamExit":
					beamExits.Add (geomObj);
					break;
				case "eofWall":
					eofWalls.Add (geomObj);
					break;
				default: 
					Debug.Log ("Warning: XML Geometr< parser: Don't know how to parse Object of type '" + geomObj.GetAttribute("type") + "'.");
					break;
				}
			}
		}
	}

	public override void buildGeometry(){
		buildSources ();
		buildTargets ();
		buildObstacles ();
		buildOtherObjects ();
	}

	private void buildTargets() {
		if (destinations.Count != 0) {
			foreach (var dest in destinations) {
				AreaGeometry.create (dest.GetAttribute ("name"), parsePoints (dest));
			}
		}
	}
	
	private void buildSources() {
		if (origins.Count != 0) {
			foreach (var origin in origins) {
				AreaGeometry.create (origin.GetAttribute ("name"), parsePoints (origin));
			}
		}
	}
	
	private void buildObstacles() {
		if (openWalls.Count != 0) {
			foreach (var wall in openWalls) {
				WallExtrudeGeometry.create (wall.GetAttribute ("name"), parsePoints (wall), height,-0.2f);
			}
		}
		if (obstacles.Count != 0) {
			foreach (var obstacle in obstacles) {
				ObstacleExtrudeGeometry.create (obstacle.GetAttribute ("name"), parsePoints (obstacle), height);
			}
		}
		if (walls.Count != 0) {
			foreach (var wall in walls) {
				ObstacleExtrudeGeometry.create (wall.GetAttribute ("name"), parsePoints (wall), height);
			}
		}
	}

	private void buildOtherObjects() {
		if (waitingZones.Count != 0) {
			foreach (var zone in waitingZones) {
				AreaGeometry.create (zone.GetAttribute ("name"), parsePoints (zone));
			}
		}
		if (scaledAreas.Count != 0) {
			foreach (var area in scaledAreas) {
				AreaGeometry.create (area.GetAttribute ("name"), parsePoints (area));
			}
		}
		if (queuingAreas.Count != 0) {
			foreach (var queue in queuingAreas) {
				AreaGeometry.create (queue.GetAttribute ("name"), parsePoints (queue));
			}
		}
	}
	
	// Parse an XmlElement full of <point> XmlElements into a coordinate list 
	private static List<Vector2> parsePoints(XmlElement polyPoints) {
		List<Vector2> list = new List<Vector2>();
		
		foreach(XmlElement point in polyPoints.SelectNodes("point")) {
			float x;
			float y;
			if (float.TryParse(point.GetAttribute("x"), out x) && float.TryParse(point.GetAttribute("y"), out y)) {
				list.Add(new Vector2(x, y));
			}
		}
		
		return list;
	}
}
