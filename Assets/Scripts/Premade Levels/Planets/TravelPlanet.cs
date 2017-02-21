using UnityEngine;
using System.Collections;

public class TravelPlanet : SingleVinePlanet {

	// Use this for initialization
	void Start () {

		numBridges = 2;
		maxNumBridges = 2;

		planetType = "travelPlanet";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnMouseDown()
	{
		string neighbors = "";

		foreach (var p in connectedPlanets)
			neighbors = neighbors + p.gameObject.name + ", ";
		//Debug.Log ("Clicked on a planet! " + gameObject.name + " Neighbors: " + neighbors);

		if (travelVine != null) 
		{
			travelVineOld=travelVine;
		}
		PregenFlowerDrag.StartDrag (this , false);
		if (travelVineOld != null && travelVine == null) {
			travelVine = travelVineOld;
			travelVineOld = null;
		}
		if (vines.Count <= 0)
			return;


		if (ResourcesDisplay.instance == null || planetType == null)
		{
			return;
		}			
	}
}
