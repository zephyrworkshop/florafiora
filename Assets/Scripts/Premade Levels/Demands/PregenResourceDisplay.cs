using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PregenResourceDisplay : MonoBehaviour {

	public Dictionary <string, int> counts = new Dictionary<string, int> ();

	public Dictionary <string, ResourceCount> displays = new Dictionary<string, ResourceCount> ();

	public GameObject resourceCountPrefab;

	public static PregenResourceDisplay instance;

	public static string [] demandTypes = new string[] {"marsh", "marigold", "flora"};

	// Use this for initialization
	void Start () {
		instance = this;

		for (int i = 0; i < 30; i++) {
			Add (1, demandTypes [Random.Range (0, demandTypes.Length)]);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void Add (int num, string resource) {
		if (resource == "") {
			return;
		}

		if (!counts.ContainsKey (resource))
			counts [resource] = 0;

		counts [resource] += num;

		if (!displays.ContainsKey (resource)) {
			//add a display
			var disp = GameObject.Instantiate (resourceCountPrefab);

			disp.transform.SetParent (gameObject.transform, false);

			var rc = disp.GetComponent <ResourceCount> ();
			rc.position = new Vector3 (0f, -75f * displays.Count, 0f);

			displays [resource] = rc;
		} 

		displays [resource].Display (counts [resource], resource);

		/*if (counts [resource] == 0) {
			GameObject.Destroy (displays [resource]);
			displays.Remove (resource);
		}*/
	}

	public int GetAvailable (string resource) {
		if (resource == "" || resource == null)
			return 0;

		if (!counts.ContainsKey (resource))
			return 0;
		else 
			return counts [resource];
	}
}