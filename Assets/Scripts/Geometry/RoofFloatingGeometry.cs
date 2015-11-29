using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoofFloatingGeometry : FloatingGeometry {

	public static void create (string name, List<Vector2> verticesList, float floatingHeight, float height) {

		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();

		//TODO for @zippo7: nicer materials
		Material topMaterial = gl.theme.getRoofMaterial();
		Material bottomMaterial = gl.theme.getRoofMaterial();
		Material sideMaterial = gl.theme.getBoxMaterial();

		FloatingGeometry.create (name, verticesList, floatingHeight, height, topMaterial, sideMaterial, bottomMaterial);
	
	}
}
