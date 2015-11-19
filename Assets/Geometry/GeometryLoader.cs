using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeometryLoader : MonoBehaviour {

	public List<Geometry> geometry;
	public ThemingMode theme;


	public void setTheme(ThemingMode mode) {
		theme = mode;
		theme.getTerrain();//Getter is also the init
	}

	public void addObject(Geometry obj) {
		geometry.Add(obj);
	}
}
