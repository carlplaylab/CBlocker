﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private StateUI [] uiObjects;

	private static UIManager instance;
	public static UIManager GetInstance()
	{
		return instance;
	}

	void Awake ()
	{
		instance = this;
	}

	void OnDestroy ()
	{
		instance = null;
	}

	#region BUTTON CALLBACKS
	public void OnPlay ()
	{
		GameManager.GetInstance ().StartGame ();
	}

	public void OnResultConfirm ()
	{
		GameManager.GetInstance ().StartResults ();
	}
 	#endregion

	public void OnGameStateChange ()
	{
		GameState.State newState = GameManager.GetInstance ().GetState ();
		for (int i = 0; i < uiObjects.Length; i++)
		{
			uiObjects [i].UpdateState (newState);
		}
	}


}