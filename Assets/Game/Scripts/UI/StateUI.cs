using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUI : MonoBehaviour 
{
	public GameState.State state;

	public void Show ()
	{
		gameObject.SetActive (true);
	}

	public void Hide ()
	{
		gameObject.SetActive (false);
	}

	public void UpdateState (GameState.State newState)
	{
		if(state != newState)
		{
			if (gameObject.activeSelf)
				Hide ();
		}
		else
		{
			if (!gameObject.activeSelf)
				Show ();
		}
	}
}
