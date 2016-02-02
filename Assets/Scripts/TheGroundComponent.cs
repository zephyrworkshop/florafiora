using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheGroundComponent : MonoBehaviour {

	public List <SeedizenComponent> seedizens = new List<SeedizenComponent> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddSeedizen (SeedizenComponent seed) {
		seedizens.Add (seed);
	}
}
