using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tweeners;

public class MovingEnemy : MoverPath, IEnemy
{
	[SerializeField] private EnemyData data;

	private int id = -1;


	#region ENEMY 
	// Enemy creation 
	public virtual IEnemy Create (Transform parent, Transform target)
	{
		MovingEnemy newEnemy = Instantiate (this, parent);
		return (IEnemy)newEnemy;
	}

	public int GetId ()
	{
		return id;
	}

	public void SetId (int id)
	{
		this.id = id;
	}

	public virtual void Enter ()
	{ 
	}

	public virtual void Kill ()
	{
		
	}

	#endregion
}
