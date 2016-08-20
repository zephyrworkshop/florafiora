using UnityEngine;
using System.Collections;

public class ResourcePlanet : PregenPlanet {

	public bool infiniteResource;
	public int ResourceCount;

	public override void Start()
	{
		if (infiniteResource == null)
			infiniteResource = true;
	}

	public override bool HasResources()
	{

		if (ResourceCount > 0 && infiniteResource == false)
		{
			ResourceCount--;
			return true;
		} 
		else if (infiniteResource == true)
			return true;
		else
			return false;
	}

}