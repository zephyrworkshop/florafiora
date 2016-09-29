using UnityEngine;
using System.Collections;

public class GrandFlowerPlanetBloomed : PollenPlanet {

	public WiltedPlanet[] linkedPlanets;
	// Use this for initialization
	public override void Start () {
        planetType = "grandPollenBloomed";
		makesPollen = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void updateLinkedPlanets()
	{
		foreach (WiltedPlanet planet in linkedPlanets)
		{
			planet.transformWilted();
		}
	}
}
