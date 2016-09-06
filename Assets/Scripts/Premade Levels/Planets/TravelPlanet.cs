using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TravelPlanet : PregenPlanet {

	public float maxDragDist = 800f;
	public TravelVine travelPath;


	//public GameObject textPrefab;

	//FlowerTextComponent ftc;

	// Use this for initialization
	void Start () {
		planetType = "travel";
	}

	// Update is called once per frame
	void Update () {

	}

	public void DecrementNumBridges () {
		numBridges --;

		if (numBridges <= 0) {
			this.hasDemands.GainResourceAfterWait (maxNumBridges * maxNumBridges);
		}

		/*if (ftc != null)
			ftc.Refresh ();*/
	}

	public void AddConnectedPlanet (PregenPlanet pc, TravelVine v)
	{
		connectedPlanets.Add (pc);
		pc.connectedPlanets.Add (this);
	}

	public void VineDelete()
	{
		travelPath = null;

	}

	public override void OnMouseDown()
	{
		string neighbors = "";

		foreach (var p in connectedPlanets)
			neighbors = neighbors + p.gameObject.name + ", ";
		//Debug.Log ("Clicked on a planet! " + gameObject.name + " Neighbors: " + neighbors);

		TravelPlanetDrag.StartDrag (this);


		if (vines.Count <= 0)
			return;


		if (ResourcesDisplay.instance == null || planetType == null)
		{
			return;
		}			
	}
}
