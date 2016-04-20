using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetComponent : MonoBehaviour
{

	public FlowerComponent flower;

	public GameObject seedizenPrefab;

	public List <PlanetComponent> connectedPlanets = new List<PlanetComponent> ();

	public Dictionary <PlanetComponent, VineComponent> vines = new Dictionary<PlanetComponent, VineComponent> ();

	public string planetType;

	public bool demands;

	public HasDemands hasDemands;

	/// <summary>
	/// This is so you can wire up planets in the editor
	/// </summary>
	public List <VineComponent> startConnected = new List<VineComponent> ();

	public void AddConnectedPlanet (PlanetComponent pc, VineComponent v)
	{
		connectedPlanets.Add (pc);
		pc.connectedPlanets.Add (this);

		vines [pc] = v;
		pc.vines [this] = v;
	}

	public void DeleteConnectedPlanet (PlanetComponent pc)
	{
		pc.vines [this] = null;
		vines [pc] = null;

		pc.connectedPlanets.Remove (this);
		connectedPlanets.Remove (pc);
	}


	// Use this for initialization
	void Start ()
	{
		//gameObject.name = GetPlanetName ();

		foreach (var s in startConnected)
		{
			AddConnectedPlanet (s.ends [0], s);//so yeah, this is the behavior of startconnected
		}

		//planets have needs
		hasDemands = gameObject.GetComponent <HasDemands> ();
		if (hasDemands == null)
			hasDemands = gameObject.AddComponent <HasDemands> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private float lastSpawned = 0f;

	public virtual void OnMouseDown ()
	{
		string neighbors = "";

		foreach (var p in connectedPlanets)
			neighbors = neighbors + p.gameObject.name + ", ";
		//Debug.Log ("Clicked on a planet! " + gameObject.name + " Neighbors: " + neighbors);

		FlowerDragManager.StartDrag (flower);


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

	public void LoadSeedizenHere ()
	{
		if (seedizenPrefab != null)
		{
			var seedizen = GameObject.Instantiate (seedizenPrefab);
			seedizen.GetComponent <SeedizenComponent> ().currentPlanet = this;
			seedizen.GetComponent <SeedizenComponent> ().startPlanet = this;
			seedizen.transform.position = gameObject.transform.position;
		}
	}

	public void OnMouseEnter ()
	{
		FlowerDragManager.RegisterMouseOnPlanet (this);
	}

	public void OnMouseExit ()
	{
		FlowerDragManager.RegisterNoMouseOnPlanet (this);
	}

	public void ProcessSeedizen (SeedizenComponent seedizen)
	{
		if (planetType != null && planetType == "pollen")
		{
			seedizen.TurnOnPollen ();
		}

		var hd = gameObject.GetComponent <HasDemands> ();
		if (hd.MeetDemandWithSeedizen (seedizen))
		{
			GameObject.Destroy (seedizen.gameObject);
		}

		if (hd.MeetDemandWithPollen (seedizen))
		{
			seedizen.TurnOffPollen ();
		}
	}

	public bool CanConnectVine ()
	{
		foreach (var db in hasDemands.myDemandBubbles)
		{
			if (db.isSeedizenDemand)
				return false;
		}
		return true;

		if (hasDemands != null && hasDemands.myDemandBubbles.Count > 0)
			return false;
		else
			return true;
	}
		

	public Sprite colonizedSprite;
	bool isColonized = false;

	public void BeColonized ()
	{
		if (isColonized)
			return;
		//change image
		if (colonizedSprite != null)
			transform.FindChild ("planet").GetComponent <SpriteRenderer> ().sprite = colonizedSprite;
		isColonized = true;
	}
}