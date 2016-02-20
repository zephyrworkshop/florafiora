using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetsGenerator : MonoBehaviour
{
	
	public GameObject stuffHolder;

	public GameObject prefab;

	public List <GameObject> planets = new List<GameObject> ();

	string level = "";
	//"Level1";

	// Use this for initialization
	void Start ()
	{
		if (level == "")
		{
			for (int i = 0; i < 80; i++)
			{
				SpawnPlanet (GetLocation (), GetType ());
			}
		} else
		{
			Debug.Log ("Loading level, surprisingly");
			var planets = LevelLoader.LoadLevel (level);
			foreach (var p in planets)
			{
				SpawnPlanet (p.position, new PlanetType (p.type));
			}
		}
	}

	public Vector3 GetLocation ()
	{
		Vector3 pos;
		bool isolated = true;
		int tries = 200;
		do
		{
			isolated = true;
			tries--;
			pos = new Vector3 (Random.Range (CameraPanningScript.lowBoundaries.x, CameraPanningScript.highBoundaries.x)
			                   , Random.Range (CameraPanningScript.lowBoundaries.y, CameraPanningScript.highBoundaries.y)
			                   , 0f);

			foreach (var p in planets)
			{
				if (Vector3.Distance (p.transform.position, pos) < 16f)
				{
					isolated = false;
					break;
				}
			}
		} while (!isolated && tries > 0);
		return pos;
	}

	public PlanetType GetType ()
	{
		if (Random.value < .4f)
			return new PlanetType ("flower");

		if (Random.value < .25f)
			return new PlanetType ("pollen");

		string[] options = HasDemands.demandTypes;
		var pt = options [Random.Range (0, options.Length)];
		return new PlanetType (pt + "");
	}

	public void SpawnPlanet (Vector3 pos, PlanetType pt)
	{

		GameObject plan = pt.GetInstance ();

		plan.transform.parent = stuffHolder.transform;

		plan.transform.position = pos;

		//Debug.Log ("Loading " + pt.assetLoc + " " + Resources.Load ("PlanetImages/" + pt.assetLoc, typeof(Sprite)) as Sprite);
		//plan.GetComponent <SpriteRenderer> ().sprite = Resources.Load ("PlanetImages/" + pt.assetLoc, typeof(Sprite)) as Sprite;

		planets.Add (plan);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


}
