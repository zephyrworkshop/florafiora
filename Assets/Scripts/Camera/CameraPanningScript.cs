using UnityEngine;
using System.Collections;
//z value of stuff is at 0 camera is at -10


/*TODO
 * 
 * Add google style zooming
 *
 *
 *
 *
 * Add flinging
 * 
 * 
 * Make the beanstalks elasticy and fun
 *
 */

public class CameraPanningScript : MonoBehaviour {
	private static bool enableControls = true;
	public static bool enablePan=true;
	public static bool enableZoom=true;

	public Vector3 velocity = new Vector3 (0f, 0f, 0f);
	public float drag = 30f;
	public static Vector2 lowBoundaries = new Vector2(-200f,0f);
	public static Vector2 highBoundaries = new Vector2(200f,200f);
	public static float minDepth = -100f;
	//private float edgeAcceleration=100f;

	public CloudGenerator generator;
    public float zoomVelocity = 0f;
	public float zoomDrag = 10f;

	private float baseSize = 20f;
	private float opacity;

	private float zoomLowLimit=10f;
	private float zoomHighLimit=50f;
    //private float currentZoom = 2;


//    private float zoomAcceleration=100f;
	private float pinchDistance=0f;
	private int prevTouchCount=0;


	private Vector3 startTouchOneWorldPosition;
	private Vector3 previousTouchOneWorldPosition;
	private Vector3 startTouchOneCameraPosition;
	private Vector3 startTouchTwoWorldPosition;
	private Vector3 previousTouchTwoWorldPosition;
	private Vector3 startTouchTwoCameraPosition;
	

	
	
	public static void Enable () {
		enableControls = true;
	}
	
	public static void Disable () {
		enableControls = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!enableControls) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)||(Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Began)) {
			startTouchOneWorldPosition=MathTools.ScreenToWorldPosition(Input.mousePosition);
			startTouchOneCameraPosition=Camera.main.transform.position;
			previousTouchOneWorldPosition=MathTools.ScreenToWorldPosition (Input.mousePosition);
		}

		if ((Input.touchCount>1&&Input.GetTouch(1).phase==TouchPhase.Began)) {
			startTouchTwoWorldPosition=MathTools.ScreenToWorldPosition(Input.mousePosition);
			startTouchTwoCameraPosition=Camera.main.transform.position;
			previousTouchTwoWorldPosition=MathTools.ScreenToWorldPosition (Input.mousePosition);
		}

		UpdatePan ();
		UpdateZoom ();

		if (Input.GetKey (KeyCode.Mouse0)||(Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Moved)) {
			previousTouchOneWorldPosition=MathTools.ScreenToWorldPosition (Input.mousePosition);
		}
		if ((Input.touchCount>1&&Input.GetTouch(1).phase==TouchPhase.Moved)) {
			previousTouchTwoWorldPosition=MathTools.ScreenToWorldPosition (Input.mousePosition);
		}
	}

	bool IsCameraOutOfBounds(){
		return IsOutOfBounds(gameObject.transform.position);
	}

	bool IsOutOfBounds(Vector3 position){
		return (position.x > highBoundaries.x || position.x < lowBoundaries.x || position.y > highBoundaries.y || position.y < lowBoundaries.y);
	}


    void UpdatePan()
    {
        if (!enablePan)
        {
            return;
        }


        if (Input.GetKey(KeyCode.Mouse0) && (startTouchOneWorldPosition - MathTools.ScreenToWorldPosition(Input.mousePosition)).sqrMagnitude > .00001)
        {
            var change = (previousTouchOneWorldPosition - MathTools.ScreenToWorldPosition(Input.mousePosition));

            var _position = transform.position;
            _position += change;
            _position.x = Mathf.Clamp(_position.x, lowBoundaries.x, highBoundaries.x);
            _position.y = Mathf.Clamp(_position.y, lowBoundaries.y, highBoundaries.y);
            gameObject.transform.position = _position;

            /*var _velocity = transform.position;
            _velocity += change/Time.deltaTime;
            _velocity.x = Mathf.Clamp(_velocity.x, -10, 10);
            _velocity.y = Mathf.Clamp(_velocity.y, -10, 10);
            velocity = _velocity;*/
            return;

        }
        else {
            gameObject.transform.position = gameObject.transform.position + Time.deltaTime * velocity;
            velocity = Mathf.Pow(1 / (drag + 1), Time.deltaTime) * velocity;
        }


        
        if (Input.GetKey (KeyCode.Mouse0)&&!Input.GetKeyDown(KeyCode.Mouse0)&&Input.touchCount<=1&&prevTouchCount<=1) {//TODO make this also happen during a touch/drag
            var moveX = -.0438f*Camera.main.orthographicSize*Input.GetAxisRaw ("Mouse X");
            var moveY = -.0438f*Camera.main.orthographicSize*Input.GetAxisRaw ("Mouse Y");


            velocity=(5*velocity+new Vector3(moveX/Time.deltaTime, moveY/Time.deltaTime, 0f))/6;
            gameObject.transform.position=gameObject.transform.position+new Vector3(moveX,moveY,0f);
            //move the camera to keep the point under the cursor/finger
            //this is orthographic so in theory any translation in pixels of the cursor should cause the same
            //if that doesn't work I can project throught the mouse cursor 
            //make the velocity a weighted average with the delta
        } else {

        }
    }
    

    void UpdateZoom() {
        if (!enableZoom) {
            return;
        }

        /*if (currentZoom == 1)
        {
            zoomLowLimit = 10;
            zoomHighLimit = 15;
        }
        else if (currentZoom == 2)
        {
            zoomLowLimit = 20;
            zoomHighLimit = 20;
        }
        else if (currentZoom == 3)
        {
            zoomLowLimit = 25;
            zoomHighLimit = 50;
        }*/
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomLowLimit, zoomHighLimit);
		opacity = (Camera.main.orthographicSize / baseSize);
		opacity = Mathf.Clamp (opacity, 0.1f, 0.9f);
		generator.Opacity (opacity);


        //keyboard
        if (Input.GetKey (KeyCode.Plus)||Input.GetKey (KeyCode.KeypadPlus)||Input.GetKey (KeyCode.Equals)){

			zoomVelocity-=.1f*Camera.main.orthographicSize;
		}
		if(Input.GetKey (KeyCode.Minus)||Input.GetKey (KeyCode.KeypadMinus)){
			zoomVelocity+=.1f*Camera.main.orthographicSize;
		}


        //mouse
        zoomVelocity += -50*Input.GetAxisRaw ("Mouse ScrollWheel");

		//Google style zooming
		Vector3 desiredPos = ClickToDrag.GetCursorWorldLocation ();
		desiredPos.z = Camera.main.transform.position.z;



		//touch
		if (prevTouchCount==1||prevTouchCount==0&&Input.touchCount>=2) {
			if(Input.GetTouch (0).phase==TouchPhase.Moved){
				Debug.Log ("Moved");
			}
			pinchDistance=(Input.GetTouch (0).position-Input.GetTouch (1).position).magnitude;
		}
		if (Input.touchCount >= 2) {
			float deltaDistance= (Input.GetTouch (0).position-Input.GetTouch (1).position).magnitude-pinchDistance;
			pinchDistance+=deltaDistance;

			zoomVelocity-=.01f*Camera.main.orthographicSize*deltaDistance;
		}
		prevTouchCount = Input.touchCount;

		//velocity += (desiredPos - Camera.main.transform.position).normalized*zoomVelocity;

		Camera.main.orthographicSize += Time.deltaTime * zoomVelocity;
		zoomVelocity = Mathf.Pow (1 / (zoomDrag + 1), Time.deltaTime) * zoomVelocity;

        /*if (Camera.main.orthographicSize >= zoomHighLimit && currentZoom != 3 && currentZoom != 2 && zoomVelocity > 5)
        {
            currentZoom += 1;
            Camera.main.orthographicSize = zoomHighLimit + zoomLowLimit / 2;
        }
        else if (Camera.main.orthographicSize <= zoomLowLimit && currentZoom != 1 && currentZoom != 2 && zoomVelocity < -5 )
        {
            currentZoom -= 1;
            Camera.main.orthographicSize = zoomHighLimit + zoomLowLimit / 2;
        }
        else if (Camera.main.orthographicSize >= zoomHighLimit && currentZoom == 2 && zoomVelocity > 10)
        {
            currentZoom += 1;
            Camera.main.orthographicSize = zoomHighLimit + zoomLowLimit / 2;
        }
        else if (Camera.main.orthographicSize <= zoomLowLimit && currentZoom == 2 && zoomVelocity < -10)
        {
            currentZoom -= 1;
            Camera.main.orthographicSize = zoomHighLimit + zoomLowLimit / 2;
        }*/
    }
}
