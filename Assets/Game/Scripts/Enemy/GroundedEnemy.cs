using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemy : MovingEnemy
{
	public float jumpTime = 1f;
	public float jumpDelay = 0.25f;
	public string idleClipName = "frog_monster_anim";

	public Vector3[] bouncePositions;

	public override void Update ()
	{
		base.Update();

		//if(target != null && state == State.ATTACKING)
		//{
		//	Vector3 targetPos = target.transform.position;
		//	targetPos.y -= carryOffset.y;
		//	AdjustEnd(targetPos);
		//}
	}


	public override void Enter ()
	{ 
		SetStartMovementListener(OnMoveStarted);
		SetState(State.ENTERING);

		SimplePathData startPath = GetRandomStartPath();
		Vector3 point1 = NormalizeVector(startPath.start);
		Vector3 point2 = NormalizeVector(startPath.end);
		this.gameObject.transform.position = point1;
		BounceToPosition(point2);

		SetFinishedListener(OnEntered);

	}

	protected override void ContinuePath ()
	{
		if(enemyAnim != null)
			enemyAnim.Stop();

		OverridePlaying(false);
		Invoke("ContinePathAfterPause", Random.Range(0.5f, 1.5f));
	}

	private void ContinePathAfterPause ()
	{
		if (path.Count > 0)
		{
			PlayIdle();
			StartMoving (path.Dequeue (), pathSpeed.Dequeue());
			OverridePlaying(true);
			SetDelay(jumpDelay);
			OverrideDuration(jumpTime - jumpDelay);
		}
	}

	protected override void OnEntered ()
	{
		Invoke("OnEnteredDelayed", 0.5f);
	}

	protected override void OnStartExit ()
	{
		Invoke("OnStartExitDelayed", 0.5f);
	}

	protected void OnEnteredDelayed ()
	{
		//if attacker, set some delay before trying an attack
		if(data.type == EnemyData.Type.ATTACKER || forcedType == EnemyData.Type.ATTACKER)
		{
			Attack();
		}
		else
		{
			SetState(State.IDLING);

			Vector3 nextPos = GetRandomPostEntryPath();
			BounceToPosition(nextPos);
			SetFinishedListener(OnStartExit);
		}
	}

	public override void Attack ()
	{
		Girl girl = EnemyHandler.GetInstance().GetGirl();
		if(girl == null)
			return;

		if(girl.IsTaken())
			return;

		Stop();
		SetState(State.ATTACKING);
		Vector3 targetPos = girl.transform.position;
		targetPos.y = EnemyHandler.GROUND;
		BounceToPosition(targetPos);
		SetFinishedListener(OnAttackApproach);

		target = girl.transform;
	}

	protected virtual void OnAttackApproach ()
	{
		this.transform.position = NormalizeVector(this.transform.position);
		Invoke("OnAttackApproachDelayed", 0.3f);
	}

	protected void OnAttackApproachDelayed()
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

					Vector3 exitPos = this.transform.position;
					if(exitPos.x > 0f)
						exitPos.x = 12f;
					else
						exitPos.x = -12f;
					exitPos.y = EnemyHandler.GROUND;
					exitPos = NormalizeVector(exitPos);
					BounceToPosition(exitPos);
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

	protected void OnStartExitDelayed ()
	{
		Vector3 exitPos = this.transform.position;
		if(exitPos.x > 0f)
			exitPos.x = 12f;
		else
			exitPos.x = -12f;
		SetState(State.EXITING);
		BounceToPosition(exitPos);
		SetFinishedListener(OnExitFinished);
	}

	private void BounceToPosition (Vector3 nextPos)
	{
		nextPos = NormalizeVector(nextPos);

		Vector3 currPos = transform.position;
		float x = currPos.x;
		float xDist = Mathf.Abs(nextPos.x - x);
		List<Vector3> bpos = new List<Vector3>();
		if(xDist > 3f)
		{
			int div = (int)(xDist/2f);
			float divSpace = xDist/div;
			float dir = (x < nextPos.x) ? 1f : -1f;
			for(int i=1; i <= div; i++)
			{
				Vector3 newPos = new Vector3(x + dir*divSpace * i, currPos.y, currPos.z);
				bpos.Add(newPos);
				if(i == 0)
				{
					MoveAndClearPath(newPos);
				}
				else
				{
					AddToPath(newPos);
				}
			}
		}
		else
		{
			MoveAndClearPath(nextPos);
			bpos.Add(nextPos);
		}
		SetDelay(jumpDelay);
		OverrideDuration(jumpTime);
		PlayIdle();

		bouncePositions = bpos.ToArray();
	}
}
