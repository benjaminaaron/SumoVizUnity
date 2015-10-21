using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingGeometry : Geometry {

	// floatingHeight = distance from ground to ceiling / height = thickness of the roof
	public static void create (string name, List<Vector2> verticesList, float floatingHeight, float height, Material topMaterial, Material sideMaterial, Material bottomMaterial) {

		//TODO copy ExtrudeGeometry and make two adjustments: 
		//- objects not all on groundlevel but at floatingHeight
		//- add bottomMaterial

	}
}
