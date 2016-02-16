using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HasDemands : MonoBehaviour {

	public List <IsDemandBubble> myDemandBubbles = new List<IsDemandBubble> ();

	public PlanetComponent planet;

	public bool startWithNoDemands = false;
	
	public static string [] demandTypes = new string[] {"water", "fire", "life" /*,"snow", "pinecone"*/};
	//public static string [] seedizenDemandTypes = new string[] 
	//		{"WaterSeedizen", "FireSeedizen", "LifeSeedizen", "SnowSeedizen", "SoilSeedizen"};

	public float distance = 10f;

	// Use this for initialization
	void Start () {
		var cc = gameObject.GetComponent <CircleCollider2D> ();
		if (cc != null) {
			distance = cc.radius + 2.5f;//this is obviously just a sort of made up formula that has no good thought behind it
		}

		//TESTING
		planet = gameObject.GetComponent <PlanetComponent> ();
		var pt = planet.planetType;
		var vpc = gameObject.GetComponent <VictoryPlanetComponent> ();

		if (!startWithNoDemands && (vpc != null || pt != null && pt.hasDemands)) {
			if (Random.value < .5f) {
			
				//needs seedizens

				string d = demandTypes [Random.Range (0, demandTypes.Length)];
				for (int i = 0; i < Random.Range (1, 8); i++) {
					AddDemand (d, true, Random.value < .8f);
					
					if (Random.Range (0f, 1f) < .2f)
						d = demandTypes [Random.Range (0, demandTypes.Length)];
				}
			} else {

				//needs pollen
				
				string d = demandTypes [Random.Range (0, demandTypes.Length)];
				for (int i = 0; i < Random.Range (1, 8); i++) {
					AddDemand (d, false, Random.value < .8f);
					
					if (Random.Range (0f, 1f) < .2f)
						d = demandTypes [Random.Range (0, demandTypes.Length)];
				}
			}
		} else if (pt.growsFlower && planet.connectedPlanets.Count == 0) {

			//flowers need a person at first
			AddDemand ("", true, true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddDemand (string d, bool isSeedizen, bool isGeneric) {
		GameObject db = GameObject.Instantiate (GenericUtilityScript.GetDemandBubblePrefab ()) as GameObject;

		var idb = db.GetComponent <IsDemandBubble> ();

		idb.host = gameObject;

		idb.distance = distance;

		//idb.angle = Random.Range (0f, 6.3f);
		idb.angle = 1f + myDemandBubbles.Count * .5f;//TODO this should be based on angle

		idb.demanded = d;

		idb.isSeedizenDemand = isSeedizen;

		idb.isGenericDemand = isGeneric;

		myDemandBubbles.Add (idb);

		db.transform.parent = gameObject.transform;
	}

	public bool MeetDemandWithSeedizen (SeedizenComponent sc) {
		string d = sc.type;
		IsDemandBubble found = null;
		foreach (var db in myDemandBubbles) {
			if ((db.isGenericDemand || db.demanded == d) && db.isSeedizenDemand) {
				found = db;
				break;
			}
		}
		
		if (found) {
			myDemandBubbles.Remove (found);
			
			GameObject.Destroy (found.gameObject);
			
			if (myDemandBubbles.Count == 0)
				OnDemandsAllMet ();
			
			return true;
		} else {
			return false;
		}
	}
	
	public bool MeetDemandWithPollen (SeedizenComponent sc) {
		string d = sc.type;
		IsDemandBubble found = null;
		foreach (var db in myDemandBubbles) {
			if (sc.hasPollen && (db.isGenericDemand || db.demanded == d) && !db.isSeedizenDemand) {
				found = db;
				break;
			}
		}
		
		if (found) {
			myDemandBubbles.Remove (found);
			
			GameObject.Destroy (found.gameObject);
			
			if (myDemandBubbles.Count == 0)
				OnDemandsAllMet ();
			
			return true;
		} else {
			return false;
		}
	}
	
	public bool NeedsPollenOf (SeedizenComponent sc) {
		string d = sc.type;
		foreach (var db in myDemandBubbles) {
			if (sc.hasPollen && (db.isGenericDemand || db.demanded == d) && !db.isSeedizenDemand) {
				return true;
			}
		}
		return false;
	}

	private void GainPollenDemands () {
		for (int i = 0; i < Random.Range (1, 7); i++) {
			string d = demandTypes [Random.Range (0, demandTypes.Length)];
			AddDemand (d, false, Random.value < .8f);
		}
	}

	public void GainPollenAfterWait (float waitTime) {
		StartCoroutine (GainPollenCoroutine(waitTime));
	}

	public IEnumerator GainPollenCoroutine (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		GainPollenDemands ();
	}

	private void OnDemandsAllMet () {
		Debug.Log ("CONGRATULATIONS! You met all the demands of: " + gameObject.name);

		if (planet == null)
			planet = gameObject.GetComponent <PlanetComponent> ();
		var pt = planet.planetType;

		if (pt.isVictory) {
			GenericUtilityScript.instance.victoryText.SetActive (true);
			return;
		}

		if (pt.growsFlower && planet.flower != null) {
			planet.flower.IncrementNumBridges ();
		} else if (pt.produces != "") {
			ResourcesDisplay.instance.Add (Random.Range (1,2), pt.produces);
		}

		if (planet != null && planet.flower != null && planet.flower.numBridges > 0 && planet.planetType != null && planet.planetType.growsFlower) {
			Debug.Log ("Doing nothing");
			//do nothing. Flowers wait until they are out of bridges
		} else 
			GainPollenAfterWait (Random.Range (5f, 10f));
	}

	public string PrintDemands () {
		string s = "";

		foreach (var idb in myDemandBubbles) {
			s = s + idb.demanded;
		}

		return s;
	}
}
