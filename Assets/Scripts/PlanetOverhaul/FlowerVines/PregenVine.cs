using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PregenVine : MonoBehaviour
{

	public bool beingStretched = true;

	public List <PregenPlanet> ends = new List <PregenPlanet> ();

	public VinePlanet flowerPlanet;

	public List <PregenSeedizen> seedizens = new List <PregenSeedizen> ();

	public float colliderOnAgainTime = float.MaxValue;

	private HashSet <GameObject> colliding = new HashSet<GameObject> ();

	public bool dispreferred = false;

	//Color vineBaseColor;
	//Color vineInvalidColor;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (Time.time > colliderOnAgainTime)
		{
			colliderOnAgainTime = float.MaxValue;

			//turn collisions on again
			gameObject.GetComponent <Collider2D> ().enabled = true;
		}

		if (beingStretched)
		{
			//Debug.Log ("I'm a vine and I have this many colliders: " + colliding.Count);
			if (IsNotColliding ())
			{
				gameObject.GetComponent <SpriteRenderer> ().color = Color.white;
			} else
			{
				gameObject.GetComponent <SpriteRenderer> ().color = Color.red;
			}
		}
	}

	public void StopStretching ()
	{
		beingStretched = false;
		gameObject.GetComponent <SpriteRenderer> ().color = Color.white;
	}

	void OnMouseDown ()
	{
		//freeze seedizens in place on the vine
		//as we drag the vine find the vector between current and previous position
		//add a falloff for the force on seedizens as a function of distance from the click location
		if (PregenVineDrag.vineCut == false)
		{
			//PregenVineDrag.StartVineDrag(this);
		} else
		{
			if (flowerPlanet != null)
			{
				flowerPlanet.DeleteConnectedPlanet (ends [1]);
				flowerPlanet.VineDelete ();
			}
			Destroy (this.gameObject);
		}
	}

	/*public void VineDragStart (PregenSeedizen dragSeedizen)
	{
		PregenVineDrag.StartVineDrag (this, dragSeedizen);
	}*/


	void OnCollisionEnter2D (Collision2D col)
	{
		//Debug.Log ("I collided with: " + col.gameObject);
		if (ends.Count == 0 || ends [0] == null || ends [0].planetType != "vine")
		{
			//Debug.Log ("No end! " + gameObject.name);
			return;
		}
		if (col.gameObject == ends [0].gameObject || col.gameObject == ends [0].gameObject)
		{
			//Debug.Log ("Collisions with: " + col.gameObject.name + " don't count!");
			return;
		}
		colliding.Add (col.gameObject);
	}

	void OnCollisionExit2D (Collision2D col)
	{
		colliding.Remove (col.gameObject);
	}

	public bool IsNotColliding ()
	{
		return colliding.Count == 0;
	}

	void WitherAndDie ()
	{

	}
}
