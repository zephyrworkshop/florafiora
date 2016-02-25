using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetType {
	public string assetLoc;

	public GameObject prefab;

	public bool growsFlower = false;

	public bool hasDemands = true;

	public bool isVictory = false;

	public string produces;

	public bool makesPollen = false;

	public string myTypeString;

	public PlanetType (string location) {
		myTypeString = location;
		if (location == "victory") {
			assetLoc = "";
			hasDemands = false;

			isVictory = true;

			prefab = Resources.Load <GameObject> ("Planets/Victory");
		} else if (location == "flower") {
			prefab = Resources.Load <GameObject> ("Planets/flowering");

			growsFlower = true;
			assetLoc = "";
			hasDemands = false;
			produces = "";
		} else if (location == "pollen") {
			prefab = Resources.Load <GameObject> ("Planets/pollen");

			makesPollen = true;
			assetLoc = "";
			hasDemands = false;
		} else {
			assetLoc = location;

			produces = location;

			//prefab = Resources.Load <GameObject> ("Planets/" + location + "Planet");
			prefab = Resources.Load <GameObject> ("Planets/generic");
		}
	}

	public GameObject GetInstance () {
		var inst = GameObject.Instantiate (prefab) as GameObject;

		if (assetLoc != "") {
			inst.transform.FindChild ("planet").GetComponent <SpriteRenderer> ().sprite 
				= Resources.Load <Sprite> ("PlanetImages/Flora/" + assetLoc + "Planet");

			inst.GetComponent <PlanetComponent> ().seedizenPrefab 
				= Resources.Load <GameObject> ("Prefabs/" + assetLoc + "Seedizen");

            inst.GetComponent<PlanetComponent>().fgSprite
                = Resources.Load <Sprite> ("PlanetImages/Flora/" + assetLoc + "PlanetControlled");
		}

		inst.GetComponent <PlanetComponent> ().planetType = this;

		return inst;
	}
}