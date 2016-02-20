using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewVineComponent : MonoBehaviour
{
	public Vector3 point1 = Vector3.zero;
	public Vector3 point2 = Vector3.zero;
	private static GameObject chainLinkPrefab;
	private List<GameObject> links = new List<GameObject> ();
	private LineRenderer lr;
	// Use this for initialization
	void Start ()
	{
		if (chainLinkPrefab == null)
		{
			Debug.Log ("Loading chain link prefab");
			chainLinkPrefab = Resources.Load ("Prefabs/ChainLink") as GameObject;
		}
		lr = gameObject.GetComponent<LineRenderer> ();
	}

	public void CreateLinks ()
	{
		var dist = (point2 - point1).magnitude;
		var dir = (point2 - point1).normalized;
		var num = Mathf.FloorToInt (dist);

		for (int i = 0; i < num; ++i)
		{
			var link = GameObject.Instantiate (chainLinkPrefab) as GameObject;
			link.transform.position = point1 + dir * i * dist / num;
		}



	}

	// Update is called once per frame
	void Update ()
	{
	
	}

	void UpdateVisibility ()
	{
		for (int i = 0; i < links.Count; ++i)
		{
			lr.SetPosition (i, links [i].transform.position);
		}
	}
}
