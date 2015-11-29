using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeometryLoader : MonoBehaviour {

	public List<Geometry> geometry;
	public ThemingMode theme;
	GameObject world;	


	public void setTheme(ThemingMode mode) {
		theme = mode;
		theme.getTerrain();//Getter is also the init
	}
	public void setWorldAsParent (GameObject go){
		if(world == null)
			world = new GameObject("World");
		go.transform.SetParent (world.transform);
	}

	public void addObject(Geometry obj) {
		geometry.Add(obj);
	}
}
