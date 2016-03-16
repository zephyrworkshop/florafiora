using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeedizenComponent : MonoBehaviour {

	public static bool flingWithMouse = false;

	public PlanetComponent currentPlanet;
	public PlanetComponent destinationPlanet;
	public PlanetComponent startPlanet;


	public VineComponent currentVine;

	public ParticleSystem pollenParticles;

	public bool hasPollen = false;

	public bool inTransit = true;

	public float speed = 3.2f;
	//	float flightSpeed = 8f;

	bool inFling = false;

	float flingTime = float.MaxValue;

	float flingDur = .3f;

	bool flying = false;

	Vector3 mousePosAtStartOfFling;

	//Vector3 flightDir;

	public float idealAngle = 0f;

	public string type = "";

	public BoxCollider2D col;


	// Use this for initialization
	void Start () {
		/*if (!flingWithMouse) {
			gameObject.GetComponent <Collider2D> ().enabled = false;
		}*/
		var pp = transform.FindChild ("Particle System");
		if (pollenParticles == null && pp != null)
			pollenParticles = pp.GetComponent <ParticleSystem> ();
		TurnOffPollen ();
		col = gameObject.GetComponent<BoxCollider2D> ();
	}

	// Update is called once per frame
	void Update () {
		if (flingWithMouse) {
			//flinging
			if (Time.time > flingTime + flingDur) {
				//CameraPanningScript.EnableControls ();
				flingTime = float.MaxValue;
				StartFlight (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePosAtStartOfFling);
			}
			//ending fling
			if (inFling && Input.GetKeyUp (KeyCode.Mouse0)) {
				inFling = false;
				CameraPanningScript.Enable ();
				speed = 2f;
			}
		}

		//walking
		if (inTransit && destinationPlanet != null) {
			Vector3 dir = destinationPlanet.gameObject.transform.position - gameObject.transform.position;
			dir.Normalize ();
			dir = dir * Time.deltaTime * speed;
			gameObject.transform.position += dir;

			//facing
			/*var rot = gameObject.transform.rotation;
			if (dir.x < 0)
				rot.y = 0f;
			else
				rot.y = 180f;
			gameObject.transform.rotation = rot;*/

			//var angle = Mathf.Atan2(currentVine.dir.y, currentVine.dir.x) * Mathf.Rad2Deg;
			//gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		if (destinationPlanet != null) {
			if (Vector3.Distance (destinationPlanet.gameObject.transform.position, gameObject.transform.position) < .5f) {
				currentPlanet = destinationPlanet;
				destinationPlanet = null;
			}
		}
		if (currentPlanet != null) {
			if (gameObject.transform.position.x > currentPlanet.gameObject.transform.position.x)
			{
				gameObject.transform.rotation = Quaternion.LookRotation (Vector3.forward, (gameObject.transform.position - currentPlanet.gameObject.transform.position).normalized) * Quaternion.Euler(0,0,90);

				var rot = gameObject.transform.rotation;
				rot.y = 180f;
				gameObject.transform.rotation = rot;
			} else
			{
				gameObject.transform.rotation = Quaternion.LookRotation (Vector3.forward, (currentPlanet.gameObject.transform.position - gameObject.transform.position).normalized) * Quaternion.Euler(0,0,90);

				var rot = gameObject.transform.rotation;
				rot.y = 0f;
				gameObject.transform.rotation = rot;
			}
		}

		//randomly choosing a new destination when it gets to its planet
		if (currentPlanet != null && destinationPlanet == null) {
			GoToRandomNeighbor (currentPlanet);
		}

		//walk around the currentplanet
		if (!flying && currentPlanet != null && destinationPlanet == null) {
			//TODO calculate ideal angle
			gameObject.transform.up = (gameObject.transform.position - currentPlanet.gameObject.transform.position).normalized;
		}

		//try to remain upright
		/*float angleDif = gameObject.transform.rotation.z - idealAngle;
		if (angleDif != 0) {
			angleDif = Mathf.Max (angleDif, angleDif * Time.deltaTime * 3f);
			var rot = gameObject.transform.rotation;
			rot.z -= angleDif;
			//Debug.Log (gameObject.transform.rotation + " " + angleDif + " " + rot);
			gameObject.transform.rotation = rot;
		}*/

		//catch the fallen ones
		/*if (gameObject.transform.position.y < CameraPanningScript.minDepth) {
			AbyssComponent.instance.CaptureSeedizen (this);
		}*/
	}


	public IEnumerator ColliderDisableCoroutine(float t){
		col.enabled = false;
		yield return new WaitForSeconds (t);
		Debug.Log ("Turning on collision");
		col.enabled = true;
		yield return new WaitForSeconds (10);
		col.enabled = false;
	}

	public void temporarilyDisableCollider(float t){
		//col.enabled = false;
		StartCoroutine (ColliderDisableCoroutine (t));
	}

	public void TurnOffPollen () {
		hasPollen = false;
		ParticleSystem.EmissionModule em = pollenParticles.emission;
		em.enabled = true;
	}

	public void TurnOnPollen () {
		hasPollen = true;
		ParticleSystem.EmissionModule em = pollenParticles.emission;
		em.enabled = false;
	}

	public void StartFlight (Vector3 dir) {
		Debug.Log (dir);
		Vector2 dir2d = dir;

		flying = true;
		inTransit = false;
		if (currentVine != null)
			currentVine.seedizens.Remove (this);
		currentPlanet = null;
		destinationPlanet = null;
		currentVine = null;
		//flightDir = dir;

		//Debug.Log (this);
		//Debug.Log (gameObject);

		var rigid = gameObject.GetComponent <Rigidbody2D> ();
		rigid.gravityScale = 1f;


		rigid.AddForce (dir2d.normalized * Mathf.Sqrt (dir2d.magnitude) * 500f);
	}

	public void EndFlight () {
		flying = false;
		inTransit = true;

		var rigid = gameObject.GetComponent <Rigidbody2D> ();
		rigid.gravityScale = 0f;
		rigid.velocity = Vector2.zero;
		rigid.angularVelocity = 0;
		var col = gameObject.GetComponent <Collider2D> ();
		col.enabled = false;
	}

	/// <summary>
	/// It's assumed this will only be called when they're at the planet
	/// </summary>
	public void GoToRandomNeighbor (PlanetComponent planet) {
		if (planet == null)
			return;
		planet.ProcessSeedizen (this);
		//		var oldPlanet = planet;
		/*var options = new List <PlanetComponent> (planet.connectedPlanets);
		foreach (var p in planet.connectedPlanets)
			if (planet.vines [p].dispreferred)
				options.Remove (p);
		if (options.Count == 0) {
			destinationPlanet = null;
			return;
		}
		destinationPlanet = options [Random.Range (0, options.Count)];*/
		destinationPlanet = PathfindingManager.GetDirection (planet, this);
		if (destinationPlanet == null)
			return;
		var oldVine = currentVine;
		if (destinationPlanet == null)
			return;
		currentVine = destinationPlanet.vines [planet];
		if (oldVine != null)
			oldVine.seedizens.Remove (this);
		if (!currentVine.seedizens.Contains (this))
			currentVine.seedizens.Add (this);
		idealAngle = 0f;
	}

	public void GoToRandomNeighbor (VineComponent vine) {
		if (vine == null)
			return;
		if (vine.ends.Count < 2) {
			Debug.Log ("Weird vine collision");
			return;
		}
		PlanetComponent dest;
		if (Random.Range (0, 2) == 0)
			dest = vine.ends [0];
		else
			dest = vine.ends [1];
		destinationPlanet = dest;
	}

	void OnMouseDown () {
		if (flingWithMouse) {
			Debug.Log ("Clicked on a seedizen");
			flingTime = Time.time;
			inFling = true;
			CameraPanningScript.Disable ();
			speed = 0f;
			mousePosAtStartOfFling = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		//Debug.Log ("Collided with: " + col.gameObject.name + " at speed: " + col.relativeVelocity);
		var g = col.gameObject.GetComponent<TheGroundComponent>();
		if (g != null)
		{
			ResourcesDisplay.instance.Add (1, startPlanet.planetType.produces);
			Destroy (this.gameObject);
		}

		var v = col.gameObject.GetComponent <VineComponent> ();
		if (v != null) {
			//Debug.Log ("Velocity colliding with " + col.gameObject + " is " + col.relativeVelocity + " with magnitude: " + col.relativeVelocity.magnitude);
			if (col.relativeVelocity.magnitude < 6f)
				AttachToVine (v);
			else {
				//BOUNCE!
				//gameObject.GetComponent <Rigidbody2D> ().AddRelativeForce (col.relativeVelocity * -100f);
				//TODO fix bouncing
				gameObject.GetComponent <Rigidbody2D> ().velocity = col.relativeVelocity * -.8f;
			}
		}

		var p = col.gameObject.GetComponent <PlanetComponent> ();
		if (p != null) {
			p.ProcessSeedizen (this);
			//var ccol = col.gameObject.GetComponent<CircleCollider2D>();
			//var displ = ccol.radius*(gameObject.transform.position-col.transform.position).normalized;
			//gameObject.transform.rotation=Quaternion.LookRotation(Vector3.forward,displ.normalized);

		}
	}

	void AttachToVine (VineComponent vc) {
		EndFlight ();
		currentVine = vc;
		if (!vc.seedizens.Contains (this))
			vc.seedizens.Add (this);
		GoToRandomNeighbor (vc);
	}

	public void AttachToPlanet (PlanetComponent pc) {
		EndFlight ();
		currentPlanet = pc;
		destinationPlanet = null;
		GoToRandomNeighbor (pc);
	}
}
