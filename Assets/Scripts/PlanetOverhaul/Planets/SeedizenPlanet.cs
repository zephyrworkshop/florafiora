using UnityEngine;
using System.Collections;

public class SeedizenPlanet : PregenPlanet {
	private float lastSpawned = 0f;

	public override void OnMouseDown ()
	{
		string neighbors = "";

		foreach (var p in connectedPlanets)
			neighbors = neighbors + p.gameObject.name + ", ";
		//Debug.Log ("Clicked on a planet! " + gameObject.name + " Neighbors: " + neighbors);

		if (vines.Count <= 0)
			return;

		if (Time.time < lastSpawned + .5f)
			return;

		if (ResourcesDisplay.instance == null || planetType == null)
		{
			return;
		}

		if (ResourcesDisplay.instance.GetAvailable (planetType) > 0)
		{
			ResourcesDisplay.instance.Add (-1, planetType);
			LoadSeedizenHere ();
			lastSpawned = Time.time;
		}

	}

	public override void LoadSeedizenHere ()
	{
		if (seedizenPrefab != null)
		{
			var seedizen = GameObject.Instantiate (seedizenPrefab);
			seedizen.GetComponent <PregenSeedizen> ().currentPlanet = this;
			seedizen.GetComponent <PregenSeedizen> ().startPlanet = this;
			seedizen.transform.position = gameObject.transform.position;
		}
	}
}
