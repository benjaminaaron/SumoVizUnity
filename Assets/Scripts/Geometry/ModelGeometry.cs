using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelGeometry : Geometry {


	public static  void create (string prefabName, List<Vector2> edges, float height) {

		GameObject obj = (GameObject) Instantiate (Resources.Load (prefabName));
		obj.isStatic = true;
		if (obj == null)
			throw new UnityException (prefabName + " not found");
		GeometryLoader gl = GameObject.Find ("GeometryLoader").GetComponent<GeometryLoader> ();
		gl.setWorldAsParent (obj);

		switch (prefabName) {
			case "Roof_FBX":
				float minX = float.MaxValue;
				float maxX = float.MinValue;
				float minY = float.MaxValue;
				float maxY = float.MinValue;
				
				foreach (Vector2 point in edges){ //edges = roofpoints
					if(point.x < minX)
						minX = point.x;
					if(point.x > maxX)
						maxX = point.x;
					if(point.y < minY)
						minY = point.y;
					if(point.y > maxY)
						maxY = point.y;
				}
				
				float xDim = maxX - minX;
				float yDim = maxY - minY;
				float xCenter = minX + xDim / 2;
				float yCenter = minY + yDim / 2;
				
				obj.transform.position = new Vector3(xCenter, height, yCenter);
				
				var renderer = obj.GetComponent<Renderer>();
				//renderer.sharedMaterial = gl.theme.getRoofMaterial(); //TODO

				float currentXdim = renderer.bounds.extents.x * 2;
				float currentYdim = renderer.bounds.extents.z * 2;
				float currentXscale = obj.transform.localScale.x;
				float currentYscale = obj.transform.localScale.z;
				
				float newXscale = (xDim / currentXdim) * currentXscale;
				float newYscale = (yDim / currentYdim) * currentYscale;
				obj.transform.localScale = new Vector3(newXscale, newXscale / 4, newYscale); //TODO a smarter value for the height of the triangular prism?

				break;
			case "Table_FBX":
			case "Bench_FBX":
			default:
				Vector2 xy = edges [0];
				
				//TODO IF the sim gets more Models than Bench and Table
				//-> this Transform setting to static methods
				
				//for getting transformed from the middle
				
				Vector2 widthHeight = (xy - edges [2]) / 2;
				widthHeight = new Vector2 (Mathf.Abs (widthHeight.x), Mathf.Abs (widthHeight.y));
				xy = xy + widthHeight;
				
				obj.transform.position = new  Vector3 (xy.x, height, xy.y);
				
				//for objects needs to be rotated check the height and width to 
				//know how to rotate the object the value dependants on output file from vadere
				
				if (widthHeight.x > widthHeight.y)
					obj.transform.Rotate(0, 90, 0);

				break;			
		}

	}
	
}
