using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoSideWallGeometry :Geometry{

	public static void create  (string name, List<Vector2> verticesList, float height) {

		const float MiddleX = 45f;
		const float MiddleY = 65f;

		Vector2[] minmax = ModelGeometry.minMax(verticesList);


		Vector2 dim = ModelGeometry.getDim(minmax[0],minmax[1]);

		Vector3 center = ModelGeometry.moveCenter(minmax[0],dim,height/2);

		Vector2 center2D = new Vector2(center.x , center.z);

		//Debug.Log ("Center" + center);

		Material mat1 = Resources.Load("TentWall") as Material;
		Material mat2 = Resources.Load("TentWall 1") as Material;



		List<Vector2> wallList = new List<Vector2>();
		wallList.Clear();

		string innerWall = "InnerWall";
		string outerWall = "OuterWall";





		if(dim.x >= dim.y){
			Vector2 dimY = new Vector2();
			dimY.y = dim.y/2;

			if((center2D.x - MiddleX) < 0f &&(center2D.y -MiddleY) < 0f || (center2D.y -MiddleY)< 0f){
				
				//Debug.Log ("negiert");
				
				string copyString = innerWall;
				innerWall = outerWall;
				outerWall = copyString;
				
				Material copy = mat1;
				mat1 = mat2;
				mat2 = copy;
			}else{
				//Debug.Log("Nicht negiert");
			}
		

			wallList.Add(new Vector2(verticesList[0].x,verticesList[0].y));
			wallList.Add(new Vector2(verticesList[1].x,verticesList[1].y));
			wallList.Add (new Vector2((verticesList[1] + dimY).x,(verticesList[1] + dimY).y));
			wallList.Add (new Vector2((verticesList[0] + dimY).x,(verticesList[0] + dimY).y));



			
			ExtrudeGeometry.create(name + outerWall,wallList,height,mat1,mat1);
			
			wallList.Clear();
			
			
			wallList.Add (new Vector2((verticesList[0] + dimY).x,(verticesList[0] + dimY).y));	
			wallList.Add (new Vector2((verticesList[1] + dimY).x,(verticesList[1] + dimY).y));
			wallList.Add(new Vector2(verticesList[2].x,verticesList[2].y));
			wallList.Add(new Vector2(verticesList[3].x,verticesList[3].y));
		

			ExtrudeGeometry.create(name + innerWall,wallList,height,mat2,mat2);


	}else if (dim.x <dim.y){
		Vector2 dimX = new Vector2();
		dimX.x= dim.x/2;

			if((center2D.x - MiddleX) < 0f &&(center2D.y -MiddleY) < 0f || (center2D.x -MiddleX)< 0f){
				
				//Debug.Log ("negiert");
				
				string copyString = innerWall;
				innerWall = outerWall;
				outerWall = copyString;
				
				Material copy = mat1;
				mat1 = mat2;
				mat2 = copy;
			}else{
				//Debug.Log("Nicht negiert");
			}

			//if((dim - center2D).x >= 0f ||(dim - center2D).y >= 0f){

				
			wallList.Add(new Vector2(verticesList[0].x,verticesList[0].y));
			wallList.Add (new Vector2((verticesList[0] + dimX).x,(verticesList[0] + dimX).y));
			wallList.Add (new Vector2((verticesList[3] + dimX).x,(verticesList[3] + dimX).y));
			wallList.Add(new Vector2(verticesList[3].x,verticesList[3].y));

			ExtrudeGeometry.create(name + outerWall,wallList,height,mat1,mat1);
			
			wallList.Clear();
			
			
			wallList.Add (new Vector2((verticesList[0] + dimX).x,(verticesList[0] + dimX).y));
			wallList.Add(new Vector2(verticesList[1].x,verticesList[1].y));
			wallList.Add(new Vector2(verticesList[2].x,verticesList[2].y));
			wallList.Add (new Vector2((verticesList[3] + dimX).x,(verticesList[3] + dimX).y));


			
			ExtrudeGeometry.create(name + innerWall ,wallList,height,mat2,mat2);

					

		}

	}

}
	
