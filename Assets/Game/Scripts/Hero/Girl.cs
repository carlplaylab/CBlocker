using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tweeners;

public class Girl : MonoBehaviour 
{
	[SerializeField] private Mover mover;
	[SerializeField] private float groundY = -8f;

	private IEnemy captor = null;


	void Update ()
	{
		if(captor != null)
		{
			Vector3 captorPos = captor.GetPosition();
			captorPos.y = captorPos.y - 1f;
			this.transform.position = captorPos;
		}
	}

	public bool IsTaken ()
	{
		return captor != null;
	}

	public void SetCaptor (IEnemy enemy)
	{
		captor = enemy;
	}

}
