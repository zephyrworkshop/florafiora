using UnityEngine;
using System.Collections;

public class ClickToDrag : MonoBehaviour {
	private bool dragging=false;
	private static GameObject dragAnchor;
	private static Rigidbody2D anchorRB;
	public SpringJoint2D joint;

	// Use this for initialization
	void Start () {
		if (joint == null) {
			joint = gameObject.AddComponent<SpringJoint2D>();
			joint.enabled=false;
		}
		if (dragAnchor == null) {
			dragAnchor = new GameObject ();
			anchorRB = dragAnchor.AddComponent<Rigidbody2D>();
			anchorRB.isKinematic=true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (dragging) {
			Drag ();
		}
	}

	public static Vector3 GetCursorWorldLocation(){
		float unitsPerPixel = (2f*Camera.main.orthographicSize)/Screen.height;
		Vector3 mouseOffsetFromCenter = new Vector3(Input.mousePosition.x - (Screen.width / 2f),Input.mousePosition.y - (Screen.height / 2f),0);
		return new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,0) + mouseOffsetFromCenter*unitsPerPixel;
	}

	private void Drag(){
		/*This is half of the vertical size of the viewing volume. Horizontal viewing size varies depending on viewport's aspect ratio. Orthographic size is ignored when camera is not orthographic (see orthographic). See Also: camera component.
			*/
		dragAnchor.transform.position = GetCursorWorldLocation ();

		//gameObject.transform.position = GetCursorWorldLocation ();
		//Debug.Log (new Vector3(Screen.width
		//Debug.Log (new Vector3(Input.mousePosition.x/Screen.width*Camera.main.orthographicSize,Input.mousePosition.y/Screen.height*Camera.main.orthographicSize,10);
	}


	public void OnMouseDown(){
		dragging = true;
		CameraPanningScript.Disable ();
		//gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		//*
		joint.enabled = true;
		joint.connectedBody = anchorRB;
		joint.connectedAnchor = Vector2.zero;
		var offset = GetCursorWorldLocation ()-gameObject.transform.position;
		joint.anchor = Vector2.zero;//new Vector2(offset.x, offset.y);
		joint.dampingRatio = 1f;
		joint.frequency = 8f;
		//*/

	}

	public void OnMouseUp(){
		dragging = false;
		CameraPanningScript.Enable ();
		//gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;

		joint.enabled = false;
	}
}

/*
public class ClickToDrag : MonoBehaviour {
	private static int frame=0;
	private static ClickToDrag pickedObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.frameCount > frame) {
			frame = Time.frameCount;
			MousePick ();
		}
		if (pickedObject == this) {
			Drag ();
		}
	}

	private static void MousePick(){
	}
	private void Drag(){
	}


	public void OnMouseDown(){
		Debug.Log ("dragging");
	}
 */