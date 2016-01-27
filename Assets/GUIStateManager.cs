using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIState{
	public virtual void EnterState(GUIState prevState){
	}
	
	public virtual void ExitState(GUIState nextState){
	}
}

public class GUIStateManager : MonoBehaviour {
	static GUIState currentState;
	static Dictionary<string, GUIState> states = new Dictionary<string, GUIState>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void SetState(string state_name){
		if (!states.ContainsKey (state_name)) {
			Debug.Log ("no definition found for GUIstate "+state_name);
			return;
		}
		SetState (states [state_name]);
	}

	public static void SetState(GUIState state){
		if (currentState != null) {
			currentState.ExitState(state);
		}
		state.EnterState (currentState);
		currentState=state;
	}
	
}
