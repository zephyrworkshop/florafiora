using UnityEngine;
using System.Collections;

public class AbyssComponent : MonoBehaviour {

	public static AbyssComponent instance;

	public PlanetComponent homePlanet;

	public Vector3 rescueSpot;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CaptureSeedizen (SeedizenComponent seed) {
		seed.gameObject.transform.position = rescueSpot;

		seed.AttachToPlanet (gameObject.GetComponent <PlanetComponent> ());

		seed.destinationPlanet = homePlanet;
	}
}
