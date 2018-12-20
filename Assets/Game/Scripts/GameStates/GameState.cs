using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Basic game state for the game manager.
// - Carl Jovenn
public class GameState
{
	public enum State
	{
		MENU = 0,
		PLAY = 1,
		RESULT = 2
	}

	public static Dictionary<State,GameState> CreateStates ()
	{
		// Create basic states for the game.
		Dictionary<State, GameState> stateList = new Dictionary<State, GameState> ();

		GameStateMenu menuState = new GameStateMenu ();
		GameStatePlay playState = new GameStatePlay ();
		GameStateResult resultState = new GameStateResult ();

		stateList.Add (menuState.GetStateType (), menuState);
		stateList.Add (playState.GetStateType (), playState);
		stateList.Add (resultState.GetStateType (), resultState);

		return stateList;
	}

	public virtual State GetStateType ()
	{
		return State.MENU;
	}

	public virtual void Start (GameManager gm)
	{
		
	}

	public virtual void Update (GameManager gm)
	{
		
	}

	public virtual void End (GameManager gm)
	{
		
	}

}
