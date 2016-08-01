using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PregenDemands : MonoBehaviour
{

	public List <PregenDemandBubble> myDemandBubbles = new List<PregenDemandBubble> ();

	public PregenPlanet planet;

	public bool startWithNoDemands = false;

	public int[] pollenCounts;
	public int[] waterCounts;

	public int currentLevel = 0;

	public int currentGoalPollen;
	public int remainingPollen;

	public int currentGoalWater;
	public int remainingWater;

	public float distance = 10f;

	// Use this for initialization
	void Start ()
	{
		var cc = gameObject.GetComponent <CircleCollider2D> ();
		if (cc != null)
		{
			distance = cc.radius + 2.5f;//this is obviously just a sort of made up formula that has no good thought behind it
		}

		//TESTING
		planet = gameObject.GetComponent <PregenPlanet> ();
		string pt = planet.planetType;
		//var vpc = gameObject.GetComponent <VictoryPregenPlanet> ();

		if (!startWithNoDemands && (pt != null && planet.demands))
		{
			//needs pollen
			if (currentLevel <= pollenCounts.Length && pollenCounts.Length > 0) {
				for (int i = 0; i < pollenCounts [currentLevel]; i++) {
					AddDemand (true, false, false);
					currentGoalPollen = pollenCounts [currentLevel];
					remainingPollen = pollenCounts [currentLevel];
				}
			}
			if (currentLevel <= waterCounts.Length && waterCounts.Length > 0) {
				for (int i = 0; i < waterCounts [currentLevel]; i++) {
					AddDemand (false, false, true);
					currentGoalWater = waterCounts [currentLevel];
					remainingWater = waterCounts [currentLevel];				}
			}
		} else if (pt == "vine" && planet.connectedPlanets.Count == 0)
			{

				//flowers need a person at first
				//AddDemand (false, true, false);
			}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void AddDemand (bool isPollen, bool isSeedizen, bool isWater)
	{
		GameObject db = GameObject.Instantiate (PregenGenericUtility.GetDemandBubblePrefab ()) as GameObject;

		var pdb = db.GetComponent <PregenDemandBubble> ();

		pdb.host = gameObject;

		pdb.distance = distance + .5f;

		//pdb.angle = Random.Range (0f, 6.3f);
		pdb.angle = 1f + myDemandBubbles.Count * .5f;//TODO this should be based on angle


		//ADD NEW DEMAND TYPES HERE
		pdb.isPollenDemand = isPollen;
		pdb.isSeedizenDemand = isSeedizen;
		pdb.isWaterDemand = isWater;

		myDemandBubbles.Add (pdb);

		db.transform.parent = gameObject.transform;
	}

	public bool MeetDemandWithSeedizen (PregenSeedizen sc)
	{
		string d = sc.type;
		PregenDemandBubble found = null;
		foreach (var db in myDemandBubbles)
		{
			if (db.isSeedizenDemand)
			{
				found = db;
				break;
			}
		}

		if (found)
		{
			myDemandBubbles.Remove (found);

			GameObject.Destroy (found.gameObject);

			if (myDemandBubbles.Count == 0)
				OnDemandsAllMet ();

			return true;
		} else
		{
			return false;
		}
	}

	public bool MeetDemandWithPollen (PregenSeedizen sc)
	{
		PregenDemandBubble found = null;
		foreach (var db in myDemandBubbles)
		{
			if (sc.hasPollen && !db.isSeedizenDemand && db.isPollenDemand && !db.isWaterDemand)
			{
				found = db;
				break;
			}
		}

		if (found)
		{
			myDemandBubbles.Remove (found);

			GameObject.Destroy (found.gameObject);

			if (myDemandBubbles.Count == 0)
				OnDemandsAllMet ();
			remainingPollen--;
			return true;
		} else
		{
			return false;
		}
	}

	public bool MeetDemandWithWater (PregenSeedizen sc)
	{
		PregenDemandBubble found = null;
		foreach (var db in myDemandBubbles)
		{
			if (sc.hasWater && !db.isSeedizenDemand && !db.isPollenDemand && db.isWaterDemand)
			{
				found = db;
				break;
			}
		}

		if (found)
		{
			myDemandBubbles.Remove (found);

			GameObject.Destroy (found.gameObject);

			if (myDemandBubbles.Count == 0)
				OnDemandsAllMet ();
			remainingWater--;
			return true;
		} else
		{
			return false;
		}
	}

	public bool NeedsPollen (PregenSeedizen sc)
	{
		foreach (var db in myDemandBubbles)
		{
			if (sc.hasPollen && !db.isSeedizenDemand && !db.isWaterDemand)
			{
				return true;
			}
		}
		return false;
	}

	public bool NeedsWater (PregenSeedizen sc)
	{
		foreach (var db in myDemandBubbles)
		{
			if (sc.hasWater && !db.isSeedizenDemand && db.isWaterDemand)
			{
				return true;
			}
		}
		return false;
	}
		

	private void GainPollenDemands ()
	{
		for (int i = 0; i < pollenCounts [currentLevel]; i++)
		{
			AddDemand (true, false, false);
			currentGoalPollen = pollenCounts [currentLevel];
			remainingPollen = pollenCounts [currentLevel];
		}
	}

	private void GainWaterDemands ()
	{
		for (int i = 0; i < waterCounts [currentLevel]; i++)
		{
			AddDemand (false, false, true);
			currentGoalWater = waterCounts [currentLevel];
			remainingWater = waterCounts [currentLevel];
		}
	}

	public void GainResourceAfterWait (float waitTime)
	{
		StartCoroutine (GainResourceCoroutine (waitTime));
	}

	public IEnumerator GainResourceCoroutine (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		if (pollenCounts != null) {
			if (currentLevel <= pollenCounts.Length && pollenCounts.Length > 0) {
				if (pollenCounts [currentLevel] != null) {
					GainPollenDemands ();
				}
			}
		}
		if (waterCounts != null) {
			if (currentLevel <= waterCounts.Length && waterCounts.Length > 0) {
				if (waterCounts [currentLevel] != null) {
					GainWaterDemands ();
				}
			}
		}
	}

	private void OnDemandsAllMet ()
	{
		Debug.Log ("CONGRATULATIONS! You met all the demands of: " + gameObject.name);
		if (planet == null)
			planet = gameObject.GetComponent <PregenPlanet> ();
		var pt = planet.planetType;

		/*if (pt.isVictory) {
			GenericUtilityScript.instance.victoryText.SetActive (true);
			return;
		}*/
		if (currentLevel < pollenCounts.Length)
		{
			if (pt == "vine")
			{
				planet.IncrementNumBridges ();
			} else if (pt != "")
				{
					ResourcesDisplay.instance.Add (1, pt);
				}

			if (planet != null && planet.planetType != null && planet.numBridges > 0 && planet.planetType != null && planet.planetType == "vine")
			{
				Debug.Log ("Doing nothing");
				//do nothing. Flowers wait until they are out of bridges
			} else
			{
				GainResourceAfterWait (Random.Range (5f, 10f));
			}
			currentLevel++;
		}
	}
}



