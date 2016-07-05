using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PregenDemands : MonoBehaviour
{

	public List <PregenDemandBubble> myDemandBubbles = new List<PregenDemandBubble> ();

	public PregenPlanet planet;

	public bool startWithNoDemands = false;

	public int[] pollenCounts = new int[5];

	public int currentLevel = 0;

	public int currentGoal;
	public int remaining;

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
			for (int i = 0; i < pollenCounts [currentLevel]; i++)
			{
				AddDemand (false);
				currentGoal = pollenCounts [currentLevel];
				remaining = pollenCounts [currentLevel];
			}
		} else if (pt == "vine" && planet.connectedPlanets.Count == 0)
			{

				//flowers need a person at first
				AddDemand (true);
			}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void AddDemand (bool isSeedizen)
	{
		GameObject db = GameObject.Instantiate (PregenGenericUtility.GetDemandBubblePrefab ()) as GameObject;

		var idb = db.GetComponent <PregenDemandBubble> ();

		idb.host = gameObject;

		idb.distance = distance + .5f;

		//idb.angle = Random.Range (0f, 6.3f);
		idb.angle = 1f + myDemandBubbles.Count * .5f;//TODO this should be based on angle


		idb.isSeedizenDemand = isSeedizen;

		myDemandBubbles.Add (idb);

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
			if (sc.hasPollen && !db.isSeedizenDemand)
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
			remaining--;
			return true;
		} else
		{
			return false;
		}
	}

	public bool NeedsPollenOf (PregenSeedizen sc)
	{
		foreach (var db in myDemandBubbles)
		{
			if (sc.hasPollen && !db.isSeedizenDemand)
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
			AddDemand (false);
			currentGoal = pollenCounts [currentLevel];
			remaining = pollenCounts [currentLevel];
		}
	}

	public void GainPollenAfterWait (float waitTime)
	{
		StartCoroutine (GainPollenCoroutine (waitTime));
	}

	public IEnumerator GainPollenCoroutine (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		GainPollenDemands ();
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
				GainPollenAfterWait (Random.Range (5f, 10f));
			}
			currentLevel++;
		}
	}
}



