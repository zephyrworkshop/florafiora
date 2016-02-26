using UnityEngine;
using System.Collections;

public class FlowerComponent : MonoBehaviour {

	public PlanetComponent planet;

	public float maxDragDist = 300f;
	
	public int numBridges = 2;
	public int maxNumBridges = 2;

	public Sprite sadFlower;

	public Sprite happyFlower;

	public GameObject textPrefab;

	//FlowerTextComponent ftc;

	// Use this for initialization
	void Start () {
		if (textPrefab == null)
			textPrefab = Resources.Load <GameObject> ("FlowerText");
		var t = GameObject.Instantiate (textPrefab);
		t.transform.SetParent (GameObject.Find ("Canvas").transform, false);
		//ftc = t.GetComponent <FlowerTextComponent> ();
		//ftc.flower = this;
		//ftc.planet = planet;
		//ftc.Refresh ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DecrementNumBridges () {
		numBridges --;

		if (numBridges <= 0) {
			gameObject.GetComponent <SpriteRenderer> ().sprite = sadFlower;

			planet.hasDemands.GainPollenAfterWait (maxNumBridges * maxNumBridges);
		}

		/*if (ftc != null)
			ftc.Refresh ();*/
	}

	public void IncrementNumBridges () {
		numBridges ++;
		maxNumBridges ++;
		
		if (numBridges > 0) {
			gameObject.GetComponent <SpriteRenderer> ().sprite = happyFlower;
		}

		/*if (ftc != null)
			ftc.Refresh ();*/
	}

	public void VineDelete()
	{
		numBridges++;



		if (numBridges > 0) 
		{
			gameObject.GetComponent <SpriteRenderer> ().sprite = happyFlower;
		}
		/*if (ftc != null)
			ftc.Refresh ();*/
	}

	public void OnMouseDown () {
		FlowerDragManager.StartDrag (this);
	}
}