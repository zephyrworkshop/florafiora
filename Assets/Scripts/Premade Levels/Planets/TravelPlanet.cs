using UnityEngine;
using System.Collections;

public class TravelPlanet : SingleVinePlanet {

	// Use this for initialization
	void Start () {

		numBridges = 1;
		maxNumBridges = 1;

		planetType = "travelVine";
	
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
			travelVine.vineDelete ();
		}
		PregenFlowerDrag.StartDrag (this , false);


		if (vines.Count <= 0)
			return;


		if (ResourcesDisplay.instance == null || planetType == null)
		{
			return;
		}			
	}
}
