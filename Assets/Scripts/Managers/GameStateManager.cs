using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState{
	public List<IEnumerator> startupCoroutines = new List<IEnumerator> ();
	
	public void EnterState(GameState prevState){
		SetupControls (prevState);
		SetupGUI (prevState);
		SetupWorld (prevState);
		SetupCamera (prevState);
	}
	
	protected virtual void SetupCamera(GameState prevState){
	}
	
	protected virtual void SetupGUI(GameState prevState){
	}
	
	protected virtual void SetupControls(GameState prevState){
	}
	
	protected virtual void SetupWorld(GameState prevState){
	}
	
	public void ExitState(GameState nextState){
		if (startupCoroutines != null && startupCoroutines.Count > 0) {
			foreach(var co in startupCoroutines){
				GameStateManager.instance.StopCoroutine(co);
			}
		}
		CleanupControls (nextState);
		CleanupGUI (nextState);
		CleanupWorld (nextState);
		CleanupCamera (nextState);
	}
	
	protected virtual void CleanupCamera(GameState nextState){
	}
	
	protected virtual void CleanupGUI(GameState nextState){
	}
	
	protected virtual void CleanupControls(GameState nextState){
	}
	
	protected virtual void CleanupWorld(GameState nextState){
	}
}

public class EditState : GameState{
	protected override void SetupControls (GameState prevState)
	{
		base.SetupControls (prevState);
		ControlsManager.SetState ("Edit");
	}
	protected override void SetupGUI(GameState prevState){
		GUIStateManager.SetState ("Edit");
	}

}

public class PlayState : GameState{
	protected override void SetupControls (GameState prevState)
	{
		base.SetupControls (prevState);
		ControlsManager.SetState ("Play");
		GUIStateManager.SetState ("Play");
	}
	protected override void SetupGUI(GameState prevState){

	}

}

public class MenuState : GameState{

	protected override void SetupControls (GameState prevState)
	{
		base.SetupControls (prevState);
		ControlsManager.SetState ("Menu");
		GUIStateManager.SetState ("Menu");
	}
	protected override void SetupGUI(GameState prevState){
		
	}

}

public class GameStateManager : MonoBehaviour {
	public static GameStateManager instance;

	protected static List<GameState> stateStack = new List<GameState> ();
	protected static GameState currentState;
	protected static GameState previousState;


	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	
	public static void ChangeToState(GameState a_newState){
		if (currentState == null) {
			stateStack.Add (currentState);
		} else {
			currentState.ExitState (a_newState);
			stateStack[stateStack.Count-1]=a_newState;
		}
		a_newState.EnterState(currentState);
		currentState=a_newState;
	}
	
	public static void StackState(GameState a_newState){
		if (currentState != null) {
			currentState.ExitState (a_newState);
		}
		stateStack.Add (a_newState);
		a_newState.EnterState(currentState);
		currentState=a_newState;
	}
}
