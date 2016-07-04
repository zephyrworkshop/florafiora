using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PregenPlanet : MonoBehaviour
{

	public GameObject seedizenPrefab;

	public List <PregenPlanet> connectedPlanets = new List<PregenPlanet> ();

	public Dictionary <PregenPlanet, PregenVine> vines = new Dictionary<PregenPlanet, PregenVine> ();

	public string planetType;

	public bool demands;
	public bool makesPollen;

	public int numBridges;
	public int maxNumBridges;

	public PregenDemands hasDemands;

	/// <summary>
	/// This is so you can wire up planets in the editor
	/// </summary>
	public List <PregenVine> startConnected = new List<PregenVine> ();

	public void AddConnectedPlanet (PregenPlanet pc, PregenVine v)
	{
		connectedPlanets.Add (pc);
		pc.connectedPlanets.Add (this);

		vines [pc] = v;
		pc.vines [this] = v;
	}

	public void DeleteConnectedPlanet (PregenPlanet pc)
	{
		pc.vines [this] = null;
		vines [pc] = null;

		pc.connectedPlanets.Remove (this);
		connectedPlanets.Remove (pc);
	}


	// Use this for initialization
	public virtual void Start ()
	{
		//gameObject.name = GetPlanetName ();

		foreach (var s in startConnected)
		{
			AddConnectedPlanet (s.ends [0], s);//so yeah, this is the behavior of startconnected
		}

		makesPollen = false;
		//planets have needs
		hasDemands = gameObject.GetComponent <PregenDemands> ();
		if (hasDemands == null)
			hasDemands = gameObject.AddComponent <PregenDemands> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public virtual void OnMouseDown ()
	{
		string neighbors = "";

		foreach (var p in connectedPlanets)
			neighbors = neighbors + p.gameObject.name + ", ";
		//Debug.Log ("Clicked on a planet! " + gameObject.name + " Neighbors: " + neighbors);

	}

	public virtual void LoadSeedizenHere ()
	{
	}

	public void OnMouseEnter ()
	{
		PregenFlowerDrag.RegisterMouseOnPlanet (this);
	}

	public void OnMouseExit ()
	{
		PregenFlowerDrag.RegisterNoMouseOnPlanet (this);
	}

	public void ProcessSeedizen (PregenSeedizen seedizen)
	{
		if (planetType != null && planetType == "pollen")
		{
			seedizen.TurnOnPollen ();
		}

		var hd = gameObject.GetComponent <PregenDemands> ();
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

	public virtual void IncrementNumBridges ()
	{
	}
}