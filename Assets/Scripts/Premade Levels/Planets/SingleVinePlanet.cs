using UnityEngine;
using System.Collections;

public class SingleVinePlanet : VinePlanet {

	//public float maxDragDist = 500f;

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

		numBridges = 1;
		maxNumBridges = 1;

		planetType = "singleVine";
	}

	// Update is called once per frame
	void Update () {

	}

	/*public void DecrementNumBridges () {
		numBridges --;

		if (numBridges <= 0) {
			this.hasDemands.GainResourceAfterWait (maxNumBridges * maxNumBridges);
		}

		if (ftc != null)
			ftc.Refresh ();
	}*/

	public override void IncrementNumBridges () {

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
