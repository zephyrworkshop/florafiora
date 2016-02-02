using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//gameObject.transform.LookAt (ClickToDrag.GetCursorWorldLocation ());
		var diff = ClickToDrag.GetCursorWorldLocation () - gameObject.transform.position;
		gameObject.transform.rotation = Quaternion.LookRotation (Vector3.forward, diff);
	}
}
