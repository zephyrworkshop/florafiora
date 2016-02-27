using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetComponent : MonoBehaviour {

	public FlowerComponent flower;

	public GameObject seedizenPrefab;

	public List <PlanetComponent> connectedPlanets = new List<PlanetComponent> ();

	public Dictionary <PlanetComponent, VineComponent> vines = new Dictionary<PlanetComponent, VineComponent> ();

	public PlanetType planetType;

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

	public void DeleteConnectedPlanet(PlanetComponent pc)
	{
		pc.vines [this] = null;
		vines [pc] = null;

		pc.connectedPlanets.Remove (this);
		connectedPlanets.Remove (pc);
	}

	/// <summary>
	/// Copied from Voidborne
	/// </summary>
	/// <returns>The planet name.</returns>
	public static string GetPlanetName() {
		string[] syllables = new string[] {"kar", "cho", "een", "od", "q'q", "jin", "X", "zhchk", "auo", "ord", "elt", "gin", "bleen", "gno", "qat", "-eee-", 
			"xhar", "nock", "blet", "shu", "nin", "foo", "bar", "que", "doon", "jar", "hal", "kor", "yen", "zeng", "shen", "pun"};
		string[] prefixes = new string[] {"Old ", "New ", "The ", "Neo", "New ", "New, new ", "Mega", "Giga", "Planet ", "planet ", "planet "};
		string[] bases = new string[] {"alex", "chris", "andrew", "alex", "chris", "andrew", "bleen", "QQQ", "mars", "earth", "york", 
			"boston", "cambridge", "frick", "X", "game", "collider", "raycast", "crime", "pluto", "fortress", "georgia", "england", 
			"massachusetts", "Hunt", "somer", "medford", "mongolia", "Detroit", "Atlanta", "Chicago", "Kinshasa", "Cairo"};
		string[] suffixes = new string[] {"opolis", " the 3rd", " City", " Planet", "field", "port", "opolis", "bleen", " in space", 
			" world", "istan", "land", " town", "cester", "ton", "berg", "dale", "ham", "ville", "rock"};
		string name = "";
		string name2 = "";
		for (int i = 0; i < Random.Range (1,4); i++) {
			name2 = name2 + syllables[Random.Range (0, syllables.Length)];
		}
		int presuffbalance = Random.Range (0, 10) + 3;
		if(Random.Range(0,10) < presuffbalance + 1)
			name = name + prefixes[Random.Range(0, prefixes.Length)];
		if(Random.Range(0,10) < 7)
			name = name + bases[Random.Range(0, bases.Length)];
		else
			name = name + name2;
		if(Random.Range(0,10) > presuffbalance - 1)
			name = name + suffixes[Random.Range(0, suffixes.Length)];
		return char.ToUpper(name[0]) + name.Substring(1);
	}

	// Use this for initialization
	void Start () {
		//gameObject.name = GetPlanetName ();

		foreach (var s in startConnected) {
			AddConnectedPlanet (s.ends [0], s);//so yeah, this is the behavior of startconnected
		}

		//planets have needs
		hasDemands = gameObject.GetComponent <HasDemands> ();
		if (hasDemands == null)
			hasDemands = gameObject.AddComponent <HasDemands> ();

		if (planetType == null) {
			planetType = new PlanetType ("starting planet");
			planetType.hasDemands = false;
			planetType.growsFlower = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	private float lastSpawned = 0f;
	
	public void OnMouseDown () {
		string neighbors = "";

		foreach (var p in connectedPlanets)
			neighbors = neighbors + p.gameObject.name + ", ";
		//Debug.Log ("Clicked on a planet! " + gameObject.name + " Neighbors: " + neighbors);

		if (planetType.myTypeString == "starting planet" || planetType.myTypeString == "flower")
		{
			FlowerDragManager.StartDrag (flower);
		}

		if (vines.Count <= 0)
			return;

		if (Time.time < lastSpawned + .5f)
			return;

		if (ResourcesDisplay.instance == null || planetType == null) {
			return;
		}
		if (ResourcesDisplay.instance.GetAvailable (planetType.produces) > 0) {
			ResourcesDisplay.instance.Add (-1, planetType.produces);
			LoadSeedizenHere ();
			lastSpawned = Time.time;
		}
	}

	public void LoadSeedizenHere () {
		if (seedizenPrefab != null) {
			var seedizen = GameObject.Instantiate (seedizenPrefab);
			seedizen.GetComponent <SeedizenComponent> ().currentPlanet = this;
			seedizen.transform.position = gameObject.transform.position;
		}
	}
	
	public void OnMouseEnter () {
		FlowerDragManager.RegisterMouseOnPlanet (this);
		EditorPlacing.RegisterMouseOnPlanet (this);
	}
	
	public void OnMouseExit () {
		FlowerDragManager.RegisterNoMouseOnPlanet (this);
		EditorPlacing.RegisterNoMouseOnPlanet (this);
	}

	public void ProcessSeedizen (SeedizenComponent seedizen) {
		if (planetType != null && planetType.makesPollen) {
			seedizen.TurnOnPollen ();
		}
		
		//Debug.Log ("I collided with: " + p.gameObject.name + " " + col.relativeVelocity.magnitude);
		
		//if (col.relativeVelocity.magnitude < 18f)
		//	AttachToPlanet (p);

		var hd = gameObject.GetComponent <HasDemands> ();
		if (hd.MeetDemandWithSeedizen (seedizen)) {
			GameObject.Destroy (seedizen.gameObject);
		}

		if (hd.MeetDemandWithPollen (seedizen)) {
			seedizen.TurnOffPollen ();
		}
	}

	public bool CanConnectVine () {
		foreach (var db in hasDemands.myDemandBubbles) {
			if (db.isSeedizenDemand)
				return false;
		}
		return true;

		/*if (hasDemands != null && hasDemands.myDemandBubbles.Count > 0)
			return false;
		else
			return true;*/
	}

	public Sprite colonizedSprite;
	bool isColonized = false;
	public void BeColonized () {
		if (isColonized)
			return;
		//change image
		if (colonizedSprite != null)
			transform.FindChild ("planet").GetComponent <SpriteRenderer> ().sprite = colonizedSprite;
		isColonized = true;
	}
}