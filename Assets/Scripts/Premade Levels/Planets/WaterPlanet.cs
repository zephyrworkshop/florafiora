using UnityEngine;
using System.Collections;

public class WaterPlanet : ResourcePlanet {

	public override void Start()
	{
		planetType = "water";
		makesWater = true;
	}

	public override void OnMouseDown ()
	{

	}

}