using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleExtrudeGeometry : ExtrudeGeometry  {

	public static void create  (string name, List<Vector2> verticesList, float height) {
		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		Material sideMaterial = gl.theme.getBoxMaterial();
		Material topMaterial =  gl.theme.getBoxMaterial();
		ExtrudeGeometry.create (name, verticesList, height, topMaterial, sideMaterial);
	}
}

