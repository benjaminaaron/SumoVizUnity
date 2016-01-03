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


		Vector2[] minmax = minMax(edges);
		Vector2 dim = getDim(minmax[0],minmax[1]);

		switch (prefabName) {
			case "Roof_FBX":
		
				
				

				obj.transform.position = moveCenter(minmax[0],dim, height);

				scaleObject(obj, dim);
				
				break;

			case "Table_FBX":
			case "Bench_FBX":
			default:

			obj.transform.position = moveCenter(minmax[0],dim, height);
				
				//for benches and table needs to be rotated check the height and width to 
				//know how to rotate the object the value dependants on output file from vadere
				
				if (Mathf.Abs(dim.x) > Mathf.Abs(dim.y)){
					obj.transform.Rotate(0, 90, 0);
			}
				
				break;			
		}


	}

	/* old code for producing the transformation
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

		*/


	public static Vector2[] minMax(List<Vector2> edges){
		Vector2 min = new Vector2();

		min.x =float.MaxValue;
		min.y = float.MaxValue;


		Vector2 max = new Vector2();

		max.x =float.MinValue;
		max.y =float.MinValue;


		foreach (Vector2 point in edges){ 
			if(point.x < min.x)
				min.x= point.x;
			if(point.x > max.x)
				max.x = point.x;
			if(point.y < min.y)
				min.y = point.y;
			if(point.y > max.y)
				max.y = point.y;
		}



		return new Vector2[]{min,max};
	}


	/**
	 * 
	 * 
	 * This method transforms an Quadratic Object 
	 * It calculates the center of the Quadratgroundplane
	 * gives back the position
	 * 
	 * @param points the 4 edges of the groundplane
	 * 
	 * @return dim of the object
	 * 
	 **/
	public static Vector2 getDim(Vector2 min,Vector2 max){



		/*
		Vector2 xy = edges [0];

		Vector2 dim = (xy - edges [2]) / 2;
		dim = new Vector2 (Mathf.Abs (dim.x), Mathf.Abs (dim.y));

		*/



		Vector2 dim = max-min;
		return dim;
	}







	public static Vector3 moveCenter(Vector2 min,Vector2 dim,float height){

	
		Vector2 center = min + dim/2;

		return new Vector3(center.x, height,center.y);

	}

	public static void scaleObject(GameObject toScale,Vector2 dim){

		var renderer = toScale.GetComponent<Renderer>();
		//renderer.sharedMaterial = gl.theme.getRoofMaterial(); //TODO
		
		
		float currentXdim = renderer.bounds.extents.x * 2;
		float currentYdim = renderer.bounds.extents.z * 2;
		float currentXscale = toScale.transform.localScale.x;
		float currentYscale = toScale.transform.localScale.z;
		
		float newXscale = (dim.x / currentXdim) * currentXscale;
		float newYscale = (dim.y/ currentYdim) * currentYscale;
		
	
		toScale.transform.localScale = new Vector3(newXscale, newXscale / 4, newYscale); //TODO a smarter value for the height of the triangular prism?



	}
	
}
