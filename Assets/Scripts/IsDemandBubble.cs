using UnityEngine;
using System.Collections;

public class IsDemandBubble : MonoBehaviour {

	public GameObject demandedThing;

	public GameObject host;

	public float distance = 4f;

	public float angle = 0f;

	public string demanded = "";

	public bool isSeedizenDemand = false;

	public bool isGenericDemand = false;

	// Use this for initialization
	void Start () {
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
			if (isGenericDemand) {
				if (isSeedizenDemand) {
					sprite = Resources.Load <Sprite> ("PlanetImages/GenericSeedizen");
				} else {
					sprite = Resources.Load <Sprite> ("PlanetImages/Generic");
				}
			} else {
				if (isSeedizenDemand) {
					sprite = Resources.Load <Sprite> ("PlanetImages/" + 
						demanded.Substring (0, 1).ToUpper () + demanded.Substring (1) + "Seedizen");
				} else {
					sprite = Resources.Load <Sprite> ("PlanetImages/" + 
                      	demanded.Substring (0, 1).ToUpper () + demanded.Substring (1));
				}
			}

			if (sprite != null) {
				sr.sprite = sprite;
			} else 
				Debug.Log ("I could not find the sprite: " + demanded);
		} else 
			Debug.Log ("I have no host planet!");
	}
	
	// Update is called once per frame
	void Update () {

	}
}
