using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

	private static GameManager instance;
	public static GameManager GetInstance()
	{
		return instance;
	}

	[SerializeField] public EnemyHandler enemyHandler;

	private Dictionary<GameState.State, GameState> stateList;
	private GameState currentState;

	public static bool Exists()
	{
		return instance != null;
	}


	#region MONO

 	// Use this for initialization
	void Start () 
	{
		instance = this;
		stateList = GameState.CreateStates ();
		currentState = stateList [GameState.State.MENU];
		currentState.Start (this);

		UIManager.GetInstance ().OnGameStateChange ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(currentState != null)
			currentState.Update (this);
	}

	void OnDestroy ()
	{
		instance = null;
	}

	#endregion
	
	public void SwitchState(GameState.State nextState)
	{
		if (currentState != null && currentState.GetStateType () == nextState)
			return;

		currentState.End (this);
		currentState = stateList [nextState];
		currentState.Start (this);
		UIManager.GetInstance ().OnGameStateChange ();

		//Debug.Log (this.name + " SwitchState : " + nextState.ToString ());
	}

	public GameState.State GetState ()
	{
		return (currentState != null) ? currentState.GetStateType () : GameState.State.MENU;
	}

	public bool StartGame ()
	{
		if(currentState.GetStateType() == GameState.State.MENU)
		{
			SwitchState (GameState.State.PLAY);
			return true;
		}

		return false;
	}

	public bool StartResults ()
	{
		if(currentState.GetStateType() == GameState.State.PLAY)
		{
            SwitchState (GameState.State.RESULT);
			return true;
		}

		return false;
	}

	public bool StartMenu ()
	{
		if(currentState.GetStateType() == GameState.State.RESULT)
		{
            SwitchState (GameState.State.MENU);
			return true;
		}

		return false;
	}
}
