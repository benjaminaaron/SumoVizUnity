using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ModelGeometry : Geometry {

	
	/**
	 * This Geometrie is made for initializing the model prefabs.
	 * It's get used in the Fileloading process.
	 * 
	 * 
	 * */
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
		case "RoofV2":
				obj.transform.position = moveCenter(minmax[0],dim, height);
			Debug.Log (dim);
				scaleObject(obj, dim);
				break;
			/*
			// Preparations for comming Tower and neu full walls
			case "wall":
				//TODO find way to findout how to rotate the walls with UV
			break;
			case "Tower":
			obj.transform.position = moveCenter(minmax[0],dim, height);
			break;
			*/
			case "Table_FBX":



			//there are five kinds of beer glasses on the table

			System.Random rnd = new System.Random();

			int nbOfTable = rnd.Next(1,5);
			
			string beerPrefix = "BeerGlassesType";
			
			GameObject beers = (GameObject) Instantiate (Resources.Load (beerPrefix + nbOfTable));

			beers.transform.parent = obj.transform;

			//transform the hole Table

			obj.transform.position = moveCenter(minmax[0],dim, height);

			//for benches and table needs to be rotated check the height and width to 
			//know how to rotate the object the value dependants on output file from vadere


			if (Mathf.Abs(dim.x) > Mathf.Abs(dim.y)){
				obj.transform.Rotate(0, 90, 0);
				
			}

				break;

			case "Bench_FBX":

			obj.transform.position = moveCenter(minmax[0],dim, height);
			
			//for benches and table needs to be rotated check the height and width to 
			//know how to rotate the object the value dependants on output file from vadere
			
			
			if (Mathf.Abs(dim.x) > Mathf.Abs(dim.y)){
				obj.transform.Rotate(0, 90, 0);
				
			}
			break;

			default:

			obj.transform.position = moveCenter(minmax[0],dim, height);
				break;			
		}


	}



	/**
	 * This method calculates the max and min from an quatratic plane
	 * 
	 * @param points the 4 edges of the groundplane
	 * 
	 * @return minMax is an array in with min is the first value and max the second
	 * 
	 * */
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
	 * This method calculates the dimension by giving it the max and min
	 * 
	 * 
	 * param min is the edge with smalles coordinates
	 * param max is the edge with biggest coordinates
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






	/**
	 * This method calculates the to Transform Center Position
	 * 
	 * @param min is the upperleft corner of the quatratic plain
	 * @param dim is the dimention of the plain in a Vector in x width and in y height
	 * @param height is the height on which level the object lies
	 * 
	 * @return the Transform Position as Center
	 * 
	 * */
	public static Vector3 moveCenter(Vector2 min,Vector2 dim,float height){

	
		Vector2 center = min + dim/2;

		return new Vector3(center.x, height,center.y);

	}

	/**
	 * This Method rescales an object by using the proposition of the dimentions 
	 * dim and the dimentions of the model to the scale of the model to get
	 * new scale
	 * 
	 *@param toScale with is the object gets scaled
	 *@param dim the dimention of the quatratic plane
	 * 
	 * 
	 * */
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
