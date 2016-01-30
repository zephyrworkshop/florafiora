using UnityEngine;
using System.Collections;

public class GenericUtilityScript : MonoBehaviour {

	public GameObject demandBubblePrefab;

	public GameObject victoryText;

	public static GenericUtilityScript instance;

	// Use this for initialization
	void Awake () {
		instance = this;

		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static GameObject GetDemandBubblePrefab () {
		if (instance != null && instance.demandBubblePrefab != null) {
			return instance.demandBubblePrefab;
		}
		return Resources.Load <GameObject> ("Prefabs/DemandBubble");
	}
}
