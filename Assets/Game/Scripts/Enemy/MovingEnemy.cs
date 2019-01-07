using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tweeners;

public class MovingEnemy : MoverPath, IEnemy
{
	[SerializeField] private EnemyData data;

	public long id = -1;
	private int hp = 1;
	private int hitId = -1;

	private float squareSize = 1f;

	#region ENEMY 
	// Enemy creation 
	public virtual IEnemy Create (Transform parent, Transform target)
	{
		MovingEnemy newEnemy = Instantiate (this, parent);
		newEnemy.hp = data.life;
		return (IEnemy)newEnemy;
	}

	public long GetId ()
	{
		return id;
	}

	public void SetId (long id)
	{
		this.id = id;
	}

	public Vector3 GetPosition ()
	{
		return this.transform.position;
	}

	public bool CheckHit (Vector3 heroPos, int hitId)
	{
		if(this.hitId == hitId || !IsAlive())
			return false; 

		// Hit id is used to allow one hero movement per hit 
		// (since each update scans for hits, a hero moving across an enemy may trigger multiple hits.
		Vector3 pos = GetPosition();
		if (EnemyUtils.CheckBoxHit(pos, heroPos, data.size))
		{
			if( EnemyUtils.CheckDistanceHit(pos, heroPos, data.size) )
			{
				this.hitId = hitId;
				return true;
			}
		}
		return false;
	}

	public int GetHP ()
	{
		return hp;
	}

	public void Hit ()
	{
		if(hp > 0)
			hp--;
	}

	public bool IsAlive ()
	{
		return hp > 0;
	}

	public int GetHitScore ()
	{
		return data.hitScore;
	}

	public int GetKillScore ()
	{
		return data.killScore;
	}

	public void DestroyObject ()
	{
		this.gameObject.SetActive(false);
		GameObject.Destroy(this.gameObject);
	}

	public virtual void Enter ()
	{ 
		SimplePathData startPath = GetRandomStartPath();
		AddToPath(startPath.start, data.speed);
		AddToPath(startPath.end);
		SetFinishedListener(OnEntered);
	}

	public virtual void Kill ()
	{
		// Destroy this enemy
		Stop();
	}

	private void OnEntered ()
	{
		Vector3 nextPos = GetRandomPostEntryPath();
		MoveAndClearPath(nextPos);
		SetFinishedListener(OnStartExit);
	}

	private void OnStartExit ()
	{
		Vector3 exitPos = GetRandomExitPath();
		MoveAndClearPath(exitPos);
		SetFinishedListener(OnExitFinished);
	}

	private void OnExitFinished ()
	{
		SendMessageUpwards("OnEnemyExit", this);
	}

	private SimplePathData GetRandomStartPath ()
	{
		if(data.startingPaths != null && data.startingPaths.Length > 0)
		{
			int rand = Random.Range(0, data.startingPaths.Length);
			return data.startingPaths[rand];
		}
		else
		{
			return new SimplePathData();
		}
	}

	private Vector3 GetRandomPostEntryPath ()
	{
		if(data.pathsAfterEntrance != null && data.pathsAfterEntrance.Length > 0)
		{
			int rand = Random.Range(0, data.pathsAfterEntrance.Length);
			return data.pathsAfterEntrance[rand];
		}
		else
		{
			return Vector3.zero;
		}
	}

	private Vector3 GetRandomExitPath ()
	{
		if(data.exitPositions != null && data.exitPositions.Length > 0)
		{
			int rand = Random.Range(0, data.exitPositions.Length);
			return data.exitPositions[rand];
		}
		else
		{
			return Vector3.zero;
		}
	}

	#endregion
}
