using UnityEngine;
using System.Collections;

public class GrandFlowerPlanet : PregenPlanet
{

    public WiltedPlanet[] linkedPlanets;
	public GameObject transformationPrefab;
    // Use this for initialization
    void Start()
    {
        planetType = "grandFlowerWilted";
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    public override void updateLinkedPlanets()
    {
        foreach (WiltedPlanet planet in linkedPlanets)
        {
            planet.transformWilted();
        }
    }
	public void bloom()
	{
		var newPlanet = GameObject.Instantiate (transformationPrefab);
		newPlanet.transform.position = gameObject.transform.position;
		newPlanet.GetComponent <GrandFlowerPlanetBloomed> ().linkedPlanets = linkedPlanets;
		Destroy (this.gameObject);
	}
}
