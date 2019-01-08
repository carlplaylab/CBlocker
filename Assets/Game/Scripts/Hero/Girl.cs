using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tweeners;

public class Girl : MonoBehaviour 
{
	public const float OUTER_X = 9.8f;

	[SerializeField] private Mover mover;
	[SerializeField] private float groundY = -4f;

	public enum State 
	{
		IDLE = 0,
		TAKEN = 1,
		LANDING = 2,
		LOST = 3
	}
	private IEnemy captor = null;
	private State state = State.IDLE;


	void Update ()
	{
		if(captor != null)
		{
			Vector3 captorPos = captor.GetPosition();
			captorPos.y = captorPos.y - 1f;
			this.transform.position = captorPos;

			if(state == State.TAKEN)
			{
				float x = this.transform.position.x;
				if(x < -OUTER_X || x > OUTER_X)
				{
					LostTheGirl();
				}
			}
		}
	}

	public bool IsTaken ()
	{
		return (captor != null && state == State.TAKEN) || state == State.LOST;
	}

	public void SetCaptor (IEnemy enemy)
	{
		captor = enemy;
		if(captor != null)
		{
			SetState(State.TAKEN);
		}
	}

	public void Release (IEnemy assumedCaptor)
	{
		if(captor == assumedCaptor)
		{
			captor = null;
			 
			// now move to ground
			Vector3 pos = this.transform.position;
			pos.y = groundY;
			mover.StartMoving(pos);
			mover.SetFinishedListener(OnLanded);
		}
	}

	private void OnLanded ()
	{
		SetState(State.IDLE);
	}

	private void SetState (State newState)
	{
		state = newState;
	}

	private void LostTheGirl ()
	{
		SetState(State.LOST);
		EnemyHandler.GetInstance().LostTheGirl ();
	}

	public void StartGame ()
	{
		mover.Stop();
		captor = null;
		SetState(State.IDLE);
		this.transform.position = new Vector3(0f, groundY, 0f);
	}

}
