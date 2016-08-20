using UnityEngine;
using System.Collections;

public class PregenFlowerDrag : MonoBehaviour
{

	static VinePlanet VineDragPlanet;

	static PregenPlanet currentPlanetByMouse;

	static PregenVine vine;

	public GameObject vinePrefab;

	Color vineBaseColor;
	Color vineInvalidColor;

	/// <summary>
	/// The planet position
	/// </summary>
	static Vector3 startPos;

	public static PregenFlowerDrag instance;

	// Use this for initialization
	void Start ()
	{
		instance = this;

		vineBaseColor = vinePrefab.GetComponent<SpriteRenderer> ().sharedMaterial.color;
		vineInvalidColor = vineBaseColor;
		vineInvalidColor.r += .5f;
		//vineInvalidColor.g -= 20;
		vineInvalidColor.a = .8f;
	}

	void OnFlowerDragSuccessfulEnd ()
	{
		VineDragPlanet.AddConnectedPlanet (currentPlanetByMouse, vine);
		vine.ends.Add (currentPlanetByMouse);
		currentPlanetByMouse.hasDemands.MeetDemandWithVine ();
		vine.gameObject.name = "Vine from " + vine.ends [0].gameObject.name + " to " + vine.ends [1].gameObject.name;
		vine.StopStretching ();
		VineDragPlanet.DecrementNumBridges ();

		vine.ends [0].BeColonized ();
		vine.ends [1].BeColonized ();
	}

	// Update is called once per frame
	void Update ()
	{
		//get mouse position
		Vector3 mousePos = MathTools.ScreenToWorldPosition (Input.mousePosition);
		if (VineDragPlanet != null) {
			if (Vector3.Distance (mousePos, startPos) > VineDragPlanet.maxDragDist
			    || (currentPlanetByMouse != null
			    && (!currentPlanetByMouse.CanConnectVine () || currentPlanetByMouse.connectedPlanets.Contains (VineDragPlanet)))) {
				vine.GetComponent<SpriteRenderer> ().material.color = vineInvalidColor;
			} else {
				vine.GetComponent<SpriteRenderer> ().material.color = vineBaseColor;
			}
		}


		bool end = false;
		bool destroy = false;

		if (Input.GetMouseButtonUp (0)) {
			if (VineDragPlanet != null) {
				if (currentPlanetByMouse == null
				    || currentPlanetByMouse.connectedPlanets.Contains (VineDragPlanet)
				    || currentPlanetByMouse == VineDragPlanet
				    || !currentPlanetByMouse.CanConnectVine ()
				    || Vector3.Distance (mousePos, startPos) > VineDragPlanet.maxDragDist) {//|| !vine.IsNotColliding ()) {
					destroy = true;
				} else {
					mousePos = currentPlanetByMouse.transform.position;
					OnFlowerDragSuccessfulEnd ();
				}

				end = true;
			}
		}

		//limit their distance
		//var dif = mousePos - startPos;
		//if (flower != null && dif.magnitude > flower.maxDragDist)
		//mousePos = startPos + dif.normalized * flower.maxDragDist;

		if (VineDragPlanet != null) {
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
			VineDragPlanet = null;
			vine = null;
			CameraPanningScript.Enable ();
		}
	}


	public static void PlaceVine (Vector3 p1, Vector3 p2, GameObject v)
	{
		if (!(Vector3.Distance (p2, p1) > VineDragPlanet.maxDragDist)) {
			//position the vine between the two points
			Vector3 pos = (p1 + p2) / 2f;
			pos.z = .1f;
			v.transform.position = pos;
		

			//vine rotation	
			Vector3 dir = p2 - p1;
			var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			v.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			//vine stretching
			var scale = v.transform.localScale;
			scale.x = dir.magnitude / 10f;
			scale.y = 2f;
			//Debug.Log (mousePos + " " + startPos + " " + d
			v.transform.localScale = scale;
		}
		
	}

	public static void StartDrag (VinePlanet flo)
	{
		if (flo == null || flo.gameObject == null) {
			Debug.Log ("ERRORS");
			return;
		}

		if (flo.numBridges <= 0) {
			Debug.Log ("Insufficient bridges");
			return;
		}

		CameraPanningScript.Disable ();
		VineDragPlanet = flo;

		var v = GameObject.Instantiate (instance.vinePrefab);
		vine = v.GetComponent <PregenVine> ();
		vine.ends.Add (VineDragPlanet);
		vine.flowerPlanet = VineDragPlanet;
		vine.gameObject.transform.SetAsFirstSibling ();
		startPos = flo.gameObject.transform.position;
	}

	public static void RegisterMouseOnPlanet (PregenPlanet planet)
	{
		currentPlanetByMouse = planet;
	}

	public static void RegisterNoMouseOnPlanet (PregenPlanet planet)
	{
		if (currentPlanetByMouse == planet)
			currentPlanetByMouse = null;
	}
}
