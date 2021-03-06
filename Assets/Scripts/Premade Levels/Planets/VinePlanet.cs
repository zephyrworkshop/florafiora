﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VinePlanet : PregenPlanet {

	public List <PregenVine> spawnedVines = new List<PregenVine> ();

	public PregenVine travelVine;
	public PregenVine travelVineOld;

	public float maxDragDist = 300f;

	//public GameObject textPrefab;

	//FlowerTextComponent ftc;

	// Use this for initialization
	void Start () {
		/*if (textPrefab == null)
			textPrefab = Resources.Load <GameObject> ("FlowerText");
		var t = GameObject.Instantiate (textPrefab);
		t.transform.SetParent (GameObject.Find ("Canvas").transform, false);
		//ftc = t.GetComponent <FlowerTextComponent> ();
		//ftc.flower = this;
		//ftc.planet = planet;
		//ftc.Refresh ();*/

		numBridges = 2;
		maxNumBridges = 2;

		planetType = "vine";
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

	public override void IncrementNumBridges () {
		numBridges ++;
		maxNumBridges ++;

		if (numBridges > 0) {
		}

		/*if (ftc != null)
			ftc.Refresh ();*/
	}

	public void VineDelete()
	{
		numBridges++;



		if (numBridges > 0) 
		{
		}
		/*if (ftc != null)
			ftc.Refresh ();*/
	}

	public override void OnMouseDown()
	{
		string neighbors = "";

		foreach (var p in connectedPlanets)
			neighbors = neighbors + p.gameObject.name + ", ";
		//Debug.Log ("Clicked on a planet! " + gameObject.name + " Neighbors: " + neighbors);

		PregenFlowerDrag.StartDrag (this, true);


		if (vines.Count <= 0)
			return;


		if (ResourcesDisplay.instance == null || planetType == null)
		{
			return;
		}			
	}
}
