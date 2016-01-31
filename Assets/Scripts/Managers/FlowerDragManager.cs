using UnityEngine;
using System.Collections;

public class FlowerDragManager : MonoBehaviour {

	static FlowerComponent flower;

	static PlanetComponent currentPlanetByMouse;

	static VineComponent vine;

	public GameObject vinePrefab;

	/// <summary>
	/// The planet position
	/// </summary>
	static Vector3 startPos;

	public static FlowerDragManager instance;

	// Use this for initialization
	void Start () {
		instance = this;
	}

	void OnFlowerDragSuccessfulEnd () {
		flower.planet.AddConnectedPlanet (currentPlanetByMouse, vine);
		vine.ends.Add (currentPlanetByMouse);
		vine.gameObject.name = "Vine from " + vine.ends [0].gameObject.name + " to " + vine.ends [1].gameObject.name;
		vine.StopStretching ();
		flower.DecrementNumBridges ();

		vine.ends [0].BeColonized ();
		vine.ends [1].BeColonized ();
	}
	
	// Update is called once per frame
	void Update () {
		//get mouse position
		Vector3 mousePos = MathTools.ScreenToWorldPosition(Input.mousePosition);

		bool end = false;
		bool destroy = false;

		if (Input.GetMouseButtonUp (0)) {
			if (flower != null) {
				if (currentPlanetByMouse == null
				    || currentPlanetByMouse.connectedPlanets.Contains (flower.planet)
				    || currentPlanetByMouse == flower.planet
				    || !currentPlanetByMouse.CanConnectVine ()
				    ) {//|| !vine.IsNotColliding ()) {
					destroy = true;
				} else {
					mousePos = currentPlanetByMouse.transform.position;
					OnFlowerDragSuccessfulEnd ();
				}

				end = true;
			}
		}

		//limit their distance
		var dif = mousePos - startPos;
		if (flower != null && dif.magnitude > flower.maxDragDist)
			mousePos = startPos + dif.normalized * flower.maxDragDist;

		if (flower != null) {
			PlaceVine (startPos, mousePos, vine.gameObject);
			/*
			//position the vine between the two points
			Vector3 pos = (startPos + mousePos) / 2f;
			pos.z = .1f;
			vine.gameObject.transform.position = pos;

			//vine facing
			Vector3 dir = mousePos - startPos;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			vine.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			//vine stretching
			var scale = vine.gameObject.transform.localScale;
			scale.x = dir.magnitude / 8f;
			scale.y = 2f;
			//Debug.Log (mousePos + " " + startPos + " " + dir.magnitude);
			vine.gameObject.transform.localScale = scale;*/
		}

		if (destroy) {
			GameObject.Destroy (vine.gameObject);
		}
		if (end) {
			flower = null;
			vine = null;
			CameraPanningScript.Enable ();
		}
	}

	public static void PlaceVine (Vector3 p1, Vector3 p2, GameObject v) {
		//position the vine between the two points
		Vector3 pos = (p1 + p2) / 2f;
		pos.z = .1f;
		v.transform.position = pos;
		
		//vine facing
		Vector3 dir = p2 - p1;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		v.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		
		//vine stretching
		var scale = v.transform.localScale;
		scale.x = dir.magnitude / 8f;
		scale.y = 2f;
		//Debug.Log (mousePos + " " + startPos + " " + d
		v.transform.localScale = scale;
	}

	public static void StartDrag (FlowerComponent flo) {
		if (flo == null || flo.gameObject == null) {
			Debug.Log ("ERRORS");
			return;
		}

		if (flo.numBridges <= 0) {
			Debug.Log ("Insufficient bridges");
			return;
		}

		CameraPanningScript.Disable ();
		flower = flo;

		var v = GameObject.Instantiate (instance.vinePrefab);
		vine = v.GetComponent <VineComponent> ();
		vine.ends.Add (flower.planet);
		vine.flowerPlanet = flower.planet;
		vine.gameObject.transform.SetAsFirstSibling ();
		startPos = flo.gameObject.transform.parent.position;
	}
	
	public static void RegisterMouseOnPlanet (PlanetComponent planet) {
		currentPlanetByMouse = planet;
	}
	
	public static void RegisterNoMouseOnPlanet (PlanetComponent planet) {
		if (currentPlanetByMouse == planet)
			currentPlanetByMouse = null;
	}
}
