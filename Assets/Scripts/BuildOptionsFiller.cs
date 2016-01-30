using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildOptionsFiller : MonoBehaviour {
	
	private delegate void ButtonPressDelegate ();

	private List <UnityEngine.UI.Button> buttons = new List<UnityEngine.UI.Button> ();

	public GameObject buttonPrefab;

	public static string [] buildTypes = new string[] {"Water", "Fire", "Life", "Snow", "Pinecone"};

	// Use this for initialization
	void Start () {
		FillPlanets ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Clear () {
		Transform cont = GetComponent <UnityEngine.UI.ScrollRect> ().content;
		for (int i = 0; i < cont.childCount; i++) {
			GameObject.Destroy (cont.GetChild (i).gameObject);
		}
		foreach (var b in buttons) {
			GameObject.Destroy (b.gameObject);
		}
		buttons.Clear ();
	}

	private void AddButton (Sprite image, ButtonPressDelegate onClick) {
		GameObject butt = GameObject.Instantiate (buttonPrefab);

		butt.GetComponent <UnityEngine.UI.Image> ().sprite = image;

		butt.GetComponent <UnityEngine.UI.Button> ().onClick.AddListener (() => {onClick();});

		butt.transform.SetParent (GetComponent <UnityEngine.UI.ScrollRect> ().content);
	}

	private Sprite GetSprite (string loc) {
		return Resources.Load <Sprite> (loc);
	}
	
	public void FillEmpty () {
		Clear ();
	}

	public void FillPlanets () {
		Clear ();
		AddButton (GetSprite ("PlanetImages/EmptyPlanet"), () => {EditorPlacing.StartPlacingPlanet ("Empty");});

		foreach (var bt in buildTypes) {
			var btInScope = bt.ToLower ();
			AddButton (GetSprite ("PlanetImages/Flora/" + bt + "Planet"), () => 
			           {EditorPlacing.StartPlacingPlanet (btInScope);});
		}
		
		AddButton (GetSprite ("PlanetImages/VictoryPlanet"), () => 
		           {EditorPlacing.StartPlacingPlanet ("victory");});

		//TODO home planet
	}

	public void FillSeedizens () {
		Clear ();

		foreach (var bt in buildTypes) {
			var btInScope = bt + "";
			AddButton (GetSprite ("PlanetImages/" + bt + "Seedizen"), () => 
			           {EditorPlacing.StartPlacingSeedizen (btInScope);});
		}
	}
	
	public void FillFlowers () {
		Clear ();
		
		AddButton (GetSprite ("PlanetImages/vine"), () => 
		           {EditorPlacing.StartPlacingVine ("vine");});
		AddButton (GetSprite ("PlanetImages/Flower"), () => 
		           {EditorPlacing.StartPlacingFlower ("flower");});
	}
	
	public void FillDemands () {
		Clear ();

		foreach (var bt in buildTypes) {
			var btInScope = bt + "";
			AddButton (GetSprite ("PlanetImages/" + bt), () => 
			           {EditorPlacing.StartPlacingDemand (btInScope);});
		}
	}
}
