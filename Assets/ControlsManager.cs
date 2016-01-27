using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO make it so you can register onkey, onkeydown or onkeyup for each and add support for mouse axes
//TODO add support for custom events like dragging, pinching, holding, double clicking
//TODO add state transitions so we can only go between states for which a transition is defined
//TODO add support for controller axes and touch controls
//TODO add the ability to enable/disable certain virtual calls


public class ControlsState {
	public string name;
	public Dictionary<string, UnityEngine.Events.UnityEvent> events = new Dictionary<string, UnityEngine.Events.UnityEvent>();
	public Dictionary<string, KeyCode> reverseKeyMappings = new Dictionary<string, KeyCode>();

	public Dictionary<KeyCode, string> onKeyDownEvents = new Dictionary<KeyCode, string>();
	public Dictionary<KeyCode, string> onKeyEvents = new Dictionary<KeyCode, string>();
	public Dictionary<KeyCode, string> onKeyUpEvents = new Dictionary<KeyCode, string>();

	public List<KeyCode> keys = new List<KeyCode>();//TODO only check tracked keys

	public ControlsState(string a_name){
		name = a_name;
	}

	public virtual void EnterState(ControlsState prevState){
	}

	public virtual void ExitState(ControlsState nextState){
	}

}

public class PlayControls : ControlsState {
	public PlayControls(string a_name) : base(a_name) {
		name = a_name;
	}

	public override void EnterState(ControlsState prevState){
		Debug.Log ("Configuring controls for play state");
		CameraPanningScript.Enable ();
	}
}

public class EditControls : ControlsState {
	public EditControls(string a_name) : base(a_name) {
		name = a_name;
	}

	public override void EnterState(ControlsState prevState){
		Debug.Log ("Configuring controls for edit state");
		CameraPanningScript.Enable ();
		
	}
}

public class MenuControls : ControlsState {
	public MenuControls(string a_name) : base(a_name) {
		name = a_name;
	}
	
	public override void EnterState(ControlsState prevState){
		Debug.Log ("Configuring controls for menu state");
		CameraPanningScript.Disable ();
		
	}
}


public class ControlsManager : MonoBehaviour {
	public enum KeyStatuses{Press, Pressed, Release, Released};
	private static Dictionary<string, ControlsState> states = new Dictionary<string, ControlsState>() ;
	private static List<ControlsState> stateStack = new List<ControlsState> ();
	private static ControlsState currentState;

	// Use this for initialization
	void Start () {
		Debug.Log ("setting up controls");
		AddState (new PlayControls("Play"));
		SetState ("Play");
		AddVirtualKey ("Click");
		SetVirtualKey ("Click", KeyCode.Mouse0);
		AddVirtualKey ("Edit");
		SetVirtualKey ("Edit", KeyCode.E);
		AddListener ("Edit", SwitchToEditMode);
		AddVirtualKey ("Menu");
		SetVirtualKey ("Menu", KeyCode.Escape);
		AddListener ("Menu", SwitchToMenuMode);
		AddState (new EditControls("Edit"));
		SetState ("Edit");
		AddVirtualKey ("Play");
		SetVirtualKey ("Play", KeyCode.E);
		AddListener ("Play", SwitchToPlayMode);
		AddVirtualKey ("Menu");
		SetVirtualKey ("Menu", KeyCode.Escape);
		AddListener ("Menu", SwitchToMenuMode);
		AddState (new MenuControls ("Menu"));
		SetState ("Menu");
		AddVirtualKey ("ExitMenu");
		SetVirtualKey ("ExitMenu", KeyCode.Escape);
		AddListener ("ExitMenu", SwitchToPlayMode);
		SetState ("Play");


	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == null)
			return;
		foreach (var key in currentState.keys) {

			if(currentState.onKeyEvents.ContainsKey (key)){
				if(Input.GetKey (key)){
					currentState.events[currentState.onKeyEvents[key]].Invoke();
				}
			}
			if(currentState.onKeyDownEvents.ContainsKey (key)){
				if(Input.GetKeyDown (key)){
					currentState.events[currentState.onKeyDownEvents[key]].Invoke();
				}
			}
			if(currentState.onKeyUpEvents.ContainsKey (key)){
				if(Input.GetKeyUp (key)){
					currentState.events[currentState.onKeyUpEvents[key]].Invoke();
				}
			}
		}
		if (Input.GetKey (KeyCode.Mouse0)){//Input.GetTouch (0).phase == TouchPhase.Began&&TouchPhase.) {//TouchPhase.Began 
		}

		if (Input.touchCount >= 2) {
		}
		//have codes for certain events related to touching, dragging and moving the cursor and provide OnDrag, OnDragStart, OnDragStop, OnClick, OnPinch, OnDoubleClick, 
	}
	
	public static void AddState(string name){
		if (states.ContainsKey (name))
			return;
		states.Add (name, new ControlsState(name));
	}

	public static void AddState(ControlsState state){
		if (states.ContainsKey (state.name))
			return;
		states.Add (state.name, state);
	}

	public static void RemoveState(string name){
		if(states.ContainsKey(name))
			states.Remove (name);
	}
	public static void RemoveState(ControlsState state){
		RemoveState (state.name);
	}

	public static void SetState(ControlsState state){
		SetState (state.name);
	}
	
	public static void SetState(string name){
		if (!states.ContainsKey (name))
			return;
		if (currentState != null) {
			currentState.ExitState(states[name]);
		}
		states [name].EnterState (currentState);
		currentState=states[name];
	}
	public static void AddVirtualKey(string name){
		if (currentState == null)
			return;
		currentState.events.Add (name, new UnityEngine.Events.UnityEvent ());
	}
	public static void SetVirtualKey(string name, KeyCode key){
		if (currentState == null||!currentState.events.ContainsKey (name)||currentState.keys.Contains (key))
			return;
		currentState.onKeyUpEvents.Add (key, name);
		currentState.onKeyEvents.Add (key, name);
		currentState.onKeyDownEvents.Add (key, name);
		currentState.keys.Add (key);
		currentState.reverseKeyMappings.Add (name, key);
	}

	public static void AddListener(string name, UnityEngine.Events.UnityAction action){
		if (currentState == null || !currentState.events.ContainsKey (name))
			return;
		currentState.events[name].AddListener (action);
	}
	public static void RemoveListener(string name, UnityEngine.Events.UnityAction action){
		if (currentState == null || !currentState.events.ContainsKey (name))
			return;
		currentState.events[name].RemoveListener (action);
	}

	public static bool GetKey(string button){
		if (!currentState.reverseKeyMappings.ContainsKey (button))
			return false;
		if (Input.GetKey (currentState.reverseKeyMappings [button])) {
			return true;
		}
		return false;
	}

	public static bool GetKeyDown(string button){
		if (!currentState.reverseKeyMappings.ContainsKey (button))
			return false;
		if (Input.GetKeyDown (currentState.reverseKeyMappings [button])) {
			return true;
		}
		return false;
	}


	public static bool GetKeyUp(string button){
		if (!currentState.reverseKeyMappings.ContainsKey (button))
			return false;
		if (Input.GetKeyUp (currentState.reverseKeyMappings [button])) {
			return true;
		}
		return false;
	}


	//TODO remove this after testing
	private void SwitchToEditMode(){
		Debug.Log ("Switching to edit mode");
		GameStateManager.ChangeToState (new EditState ());
	}
	
	private void SwitchToPlayMode(){
		Debug.Log ("Switching to play mode");
		GameStateManager.ChangeToState (new PlayState ());
	}

	private void SwitchToMenuMode(){
		Debug.Log ("Switching to play mode");
		GameStateManager.ChangeToState (new MenuState ());
	}


	public IEnumerator TestIEnumeratorFunctionality(){
		yield return StartCoroutine(InteriorCoroutine ());
		Debug.Log ("finished counting to 100");
	}

	/* WARNING: THIS HAS ISSUES IF ANY OF THE COROUTINES THROW AN EXCEPTION AND POSSIBLY WITH STATIC IEnumerators

	AN ALTERNATIVE APPROACH:

	IEnumerator Func2()
    {
        //wait for the completion of Func1
        IEnumerator e = Func1();
        while (e.MoveNext()) yield return e.Current;
    }
	 */

	public static IEnumerator InteriorCoroutine(){
		for (int i = 1; i<101; ++i) {
			Debug.Log (i);
			yield return new WaitForEndOfFrame();
		}
	}


}
