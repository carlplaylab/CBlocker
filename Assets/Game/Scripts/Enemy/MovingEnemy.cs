using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tweeners;

public class MovingEnemy : MoverPath, IEnemy
{
	public const float OUTER_X = 10f;

	[SerializeField] private EnemyData data;

	public enum State
	{
		ENTERING = 0,
		IDLING = 1,
		EXITING = 2,
		ATTACKING = 3,
		KIDNAPPING = 4,
		DEAD = 5
	}

	public long id = -1;
	private int hp = 1;
	private int hitId = -1;

	private float squareSize = 1f;

	private State state = State.ENTERING;
	private float attackTimer = 0f;

	private Transform target;

	public override void Update ()
	{
		base.Update();

		if(attackTimer > 0f)
		{
			attackTimer -= Time.deltaTime;
			if(attackTimer <= 0f)
				Attack();
		}

		if(target != null && state == State.ATTACKING)
		{
			AdjustEnd(target.transform.position);
		}
	}


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
		if(state == State.KIDNAPPING)
		{
			Girl girl = EnemyHandler.GetInstance().GetGirl();
			if(girl.IsTaken())
			{
				girl.Release(this);
			}
		}
		// Destroy this enemy
		SetState(State.DEAD);
		Stop();
	}

	private void OnEntered ()
	{
		Vector3 nextPos = GetRandomPostEntryPath();
		MoveAndClearPath(nextPos);
		SetFinishedListener(OnStartExit);

		SetState(State.IDLING);

		// if attacker, set some delay before trying an attack
		if(data.type == EnemyData.Type.ATTACKER)
		{
			attackTimer = UnityEngine.Random.Range(0.5f, 3f);
		}
	}

	private void OnStartExit ()
	{
		Vector3 exitPos = GetRandomExitPath();
		SetState(State.EXITING);
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

	private Vector3 GetNearExitPath ()
	{
		float x = OUTER_X;
		if(this.transform.position.x < 0f)
			x = -OUTER_X;
		
		if(data.exitPositions != null && data.exitPositions.Length > 0)
		{
			List<Vector3> choices = new List<Vector3>();
			for(int i=0; i < data.exitPositions.Length; i++)
			{
				if((x <= 0f && data.exitPositions[i].x <= 0f) || (x > 0f && data.exitPositions[i].x > 0f))
				{
					choices.Add(data.exitPositions[i]);
				}
			}

			int randIdx = UnityEngine.Random.Range(0, choices.Count);
			if(randIdx < choices.Count)
				return choices[randIdx];
		}

		return new Vector3(x, 0f, 0f);
	}

	private void SetState (State newState)
	{
		state = newState;
	}

	#endregion

	private void Attack ()
	{
		Girl girl = EnemyHandler.GetInstance().GetGirl();
		if(girl == null)
			return;

		if(girl.IsTaken())
			return;

		Stop();
		SetState(State.ATTACKING);
		StartMoving(girl.transform.position, data.attackSpeed);
		SetFinishedListener(OnAttackApproach);

		target = girl.transform;
	}

	private void OnAttackApproach ()
	{
		Girl girl = EnemyHandler.GetInstance().GetGirl();
		bool exit = true;
		if(state == State.ATTACKING)
		{
			if(girl != null)
			{
				if(!girl.IsTaken())
				{
					girl.SetCaptor(this);
					exit = false;

					Vector3 exitPos = GetNearExitPath();
					StartMoving(exitPos);
					SetState(State.KIDNAPPING);
					SetFinishedListener(OnEscaped);
				}
			}
		}

		if(exit)
		{
			OnStartExit();
		}
	}

	private void OnEscaped ()
	{
	}

}
