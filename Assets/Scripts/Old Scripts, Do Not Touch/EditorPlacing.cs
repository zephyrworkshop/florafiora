/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorPlacing : MonoBehaviour {
	//static EditorPlacing instance;

	//the record
	public static List <PlacementDetails> record = new List<PlacementDetails> ();

	//planets
	static GameObject currentlyPlacing;
	//static PlanetType currentlyTyped;
	static bool useGrid = true;

	//vines
	PlanetComponent vineEnd1;
	PlanetComponent vineEnd2;
	static GameObject vine;

	//flower
	static GameObject flower;

	//demands
	static string currentDemand = "";

	//the state
	enum editingState {none, planets, seedizens, vine, flower, demands};
	static editingState state = editingState.none;

	static GameObject underCursor = null;

	// Use this for initialization
	void Awake () {
		//instance = this;
	}

	// Update is called once per frame
	void Update () {
		//mouse
		Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pz.z = 0;
		if (useGrid && state == editingState.planets) {
			pz.x = (int)pz.x;
			pz.y = (int)pz.y;
		}
		if (underCursor != null) {
			underCursor.transform.position = pz;
		}

		//right click escapes
		if (Input.GetMouseButton (1)) {
			GameObject.Destroy (currentlyPlacing);
			GameObject.Destroy (underCursor);
			End ();
		}

		switch (state) {
		case editingState.planets:
			if (Input.GetMouseButton (0)) {
				if (currentlyTyped != null) {
					var pd = new PlacementDetails (currentlyTyped, currentlyPlacing.transform.position);
					pd.thePlanetGob = currentlyPlacing;
					record.Add (pd);
				}
				currentlyPlacing = null;
				currentlyTyped = null;
				underCursor = null;
				state = editingState.none;
			}
			break;
		case editingState.vine:
			//TODO stretch
			if (vineEnd1 != null) {
				Vector3 end2 = Vector3.zero;
				if (vineEnd2 != null)
					end2 = vineEnd2.transform.position;
				else 
					end2 = pz;
				FlowerDragManager.PlaceVine (vineEnd1.transform.position, end2, vine);
			}

			if (Input.GetMouseButtonDown (0) && currentPlanetByMouse != null) {
				if (vineEnd1 == null) {
					//Debug.Log ("Vine1");
					vineEnd1 = currentPlanetByMouse;

					vine.gameObject.transform.SetAsFirstSibling ();
					//startPos = flo.gameObject.transform.parent.position;
				} else if (currentPlanetByMouse != vineEnd1) {
					//Debug.Log ("Vine2");
					vineEnd2 = currentPlanetByMouse;

					//record vine
					PlacementDetails p1 = null;
					PlacementDetails p2 = null;
					foreach (var p in record) {
						if (p.thePlanetGob == currentPlanetByMouse.gameObject)
							p1 = p;
						if (p.thePlanetGob == vineEnd1.gameObject)
							p2 = p;
					}
					if (p1 != null && p2 != null)
						p1.AddVineNeighbor (p2);
					else
						Debug.Log ("Vine recording problem: " + p1 + "-" + p2);

					//straighten out the end
					Vector3 end2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					FlowerDragManager.PlaceVine (vineEnd1.transform.position, end2, vine);

					//end
					vineEnd1 = null;
					vineEnd2 = null;
					vine = null;
					state = editingState.none;
					currentlyPlacing = null;
					underCursor = null;
				}
			}
			break;
		case editingState.flower:
			if (Input.GetMouseButtonDown (0) && currentPlanetByMouse != null) {
				//give the planet a flower
				//record flower
				foreach (var p in record) {
					if (p.thePlanetGob == currentPlanetByMouse.gameObject)
						p.hasFlower = true;
				}

				End ();
			}
			break;
		case editingState.demands:
			if (Input.GetMouseButtonDown (0) && currentPlanetByMouse != null && currentDemand != "") {
				//TODO record demand
				//Debug.Log ("Adding demand: "+ currentDemand);
				currentPlanetByMouse.hasDemands.AddDemand (currentDemand, false, false);
			}
			break;
		}
	}

	void End () {
		state = editingState.none;
		currentlyPlacing = null;
		currentlyTyped = null;
		underCursor = null;
		vineEnd1 = null;
		vineEnd2 = null;
		vine = null;
		currentDemand = "";
	}

	/*public static void StartPlacingPlanet (string name) {
		PlanetType pt = new PlanetType (name);

		currentlyPlacing = pt.GetInstance ();
		currentlyTyped = pt;
		underCursor = currentlyPlacing;
		currentlyPlacing.AddComponent <HasDemands> ().startWithNoDemands = true;

		state = editingState.planets;
	}

	public static void StartPlacingSeedizen (string name) {
		//TODO
		Debug.Log ("Seedizen: " + name);

		state = editingState.seedizens;
	}

	public static void StartPlacingFlower (string name) {
		//Debug.Log ("Flower: " + name);

		flower = GameObject.Instantiate (Resources.Load <GameObject> ("Flower2D")) as GameObject;
		underCursor = flower;

		Debug.Log (flower);

		state = editingState.flower;
	}

	public static void StartPlacingVine (string name) {
		//Debug.Log ("Vine: " + name);

		vine = GameObject.Instantiate (Resources.Load <GameObject> ("Vine"));
		underCursor = vine;

		state = editingState.vine;
	}

	public static void StartPlacingDemand (string name) {
		//TODO
		//Debug.Log ("Demand: " + name);

		currentDemand = name;

		state = editingState.demands;
	}

	//click click
	static PlanetComponent currentPlanetByMouse;

	public static void RegisterMouseOnPlanet (PlanetComponent planet) {
		currentPlanetByMouse = planet;
	}

	public static void RegisterNoMouseOnPlanet (PlanetComponent planet) {
		if (currentPlanetByMouse == planet)
			currentPlanetByMouse = null;
	}

	//print print
	public static string PrintRecord () {
		string s = "";

		foreach (var r in record) {
			s = s + r.PrintString () + "\n";
		}

		return s;
	}

	public void PrintRecordTest () {
		Debug.Log (PrintRecord ());
	}
}

public class PlacementDetails {
	static int idCount = 0;

	//public PlanetType planetType;

	public Vector3 pos;

	public bool hasFlower = false;

	public GameObject thePlanetGob;

	public int id;

	List <int> neighbors = new List<int> ();

	public PlacementDetails (/*PlanetType pt, Vector3 p) {
		//planetType = pt;
		pos = p;

		id = idCount;
		idCount ++;
	}

	public string PrintString ()
	{
		string s = "" + id + "|" + pos.x + "|" + pos.y + "|" /*+ planetType.myTypeString + "|" + hasFlower + "|" + thePlanetGob.GetComponent <HasDemands> ().PrintDemands () + "|";
		foreach (var n in neighbors)
			s = s + n + ",";

		return s;
	}

	public void AddVineNeighbor (PlacementDetails pd) {
		neighbors.Add (pd.id);
		pd.neighbors.Add (id);
	}
}
*/