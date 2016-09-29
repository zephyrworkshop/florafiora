using UnityEngine;
using System.Collections;

public class WiltedPlanet : PregenPlanet {

	public GameObject transformationPrefab;

	void start()
	{
		planetType = "wilted";
	}

	public void transformWilted()
	{
		var newPlanet = GameObject.Instantiate (transformationPrefab);
		newPlanet.transform.position = gameObject.transform.position;
		Destroy (this.gameObject);
	}


}
