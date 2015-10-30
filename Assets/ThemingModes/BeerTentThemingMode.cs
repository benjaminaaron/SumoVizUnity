	using UnityEngine;
	using System.Collections;

	public class BeerTentThemingMode : ThemingMode
	{

	#region ThemingMode implementation

	public override string getTerrainName()
		{
			return "TentFloor";
		}
	
	public override Material getWallsMaterial ()
		{
		return (Material) Resources.Load("Wooden floor/Wooden floor 01/Wooden floor 01", typeof(Material));
		}

	public override Material getBoxMaterial ()
		{
			return (Material) Resources.Load("Woodbox", typeof(Material));
		}
	public override Material getHouseMaterial ()
		{
		throw new System.NotImplementedException ();
		}

	public override Material getRoofMaterial ()
		{
		throw new System.NotImplementedException ();
		}

	public override Vector2 getTextureScaleForHeight (float height)
		{
		throw new System.NotImplementedException ();
		} 





	#endregion

	}
