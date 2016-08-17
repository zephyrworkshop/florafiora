using UnityEngine;
using System.Collections;

public class PregenDemandBubble : MonoBehaviour
{

	public GameObject demandedThing;

	public GameObject host;

	public float distance = 4f;

	public float angle = 0f;

	public bool isPollenDemand = false;

	public bool isSeedizenDemand = false;

	public bool isWaterDemand = false;

	public bool isVineDemand = false;

	// Use this for initialization
	void Start ()
	{
		if (host != null) {
			Vector3 pos = host.transform.position;
			pos.x += Mathf.Sin (angle) * distance;
			pos.y += Mathf.Cos (angle) * distance;
			gameObject.transform.position = pos;

			gameObject.transform.up = gameObject.transform.position - host.transform.position;

			demandedThing.transform.up = Vector3.up;

			//set the image
			var sr = demandedThing.GetComponent <SpriteRenderer> ();

			Sprite sprite;
			if (isSeedizenDemand) {
				sprite = Resources.Load <Sprite> ("PlanetImages/GenericSeedizen");
			} else if (isWaterDemand) {
				sprite = Resources.Load <Sprite> ("PlanetImages/water");
			} else if (isVineDemand) {
				sprite = Resources.Load <Sprite> ("PlanetImages/SadFlower");
			} else {
				sprite = Resources.Load <Sprite> ("PlanetImages/Generic");
			}

			if (sprite != null) {
				sr.sprite = sprite;
			} else
				Debug.Log ("I could not find the sprite: ");
		} else
			Debug.Log ("I have no host planet!");
	}

	// Update is called once per frame
	void Update ()
	{

	}
}