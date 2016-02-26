using UnityEngine;
using System.Collections;

public class ResourceCount : MonoBehaviour {

	public UnityEngine.UI.Text count;

	public UnityEngine.UI.Image image;

	public Vector3 position;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.localPosition = position;//TODO tweening
	}

	public void Display (int num, string asset) {
		count.text = "" + num;
		image.sprite = Resources.Load <Sprite> ("UI/" + asset);
	}
}
