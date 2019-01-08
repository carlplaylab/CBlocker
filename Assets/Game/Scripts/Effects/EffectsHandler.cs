using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsHandler : MonoBehaviour 
{

	private static EffectsHandler instance;
	public static EffectsHandler GetInstance ()
	{
		return instance;
	}

	[SerializeField] private Slash slash;

	void Awake ()
	{
		instance = this;
	}

	void OnDestroy ()
	{
		instance = null;
	}

	public static void PlaySlash (Vector3 pos, Vector3 dir)
	{
		if(instance == null)
			return;

		instance.slash.Create(pos, dir);
	}
}
