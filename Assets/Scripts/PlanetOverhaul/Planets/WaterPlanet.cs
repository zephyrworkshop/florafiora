using UnityEngine;
using System.Collections;

public class WaterPlanet : ResourcePlanet {

	public override void Start()
	{
		planetType = "water";
		makesPollen = true;
	}

	public override void OnMouseDown ()
	{

	}

}