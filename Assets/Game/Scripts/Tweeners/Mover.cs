using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tweeners 
{
	/// <summary>
	/// A basic tweener for moving an objects transform.
	/// - Carl Joven
	/// </summary>
	public class Mover : MonoBehaviour, ITweener
	{
		[SerializeField] private float speed = 1f;

		private Vector3 start;
		private Vector3 end;

		private bool playing = false;
		private float timer = 0f;
		private float duration = 0f;

		private System.Action onEnd;

		#region Mono

		void Update ()
		{
			if (IsPlaying ())
			{
				UpdateTweener ();
			}
		}

		void OnDestroy ()
		{
			onEnd = null; 	// Ensure no references are kept
		}

		#endregion

		#region Tweeners

		public void Play ()
		{
			playing = true;
			Vector2 startV2 = new Vector2 (start.x, start.y);
			Vector2 endV2 = new Vector2 (end.x, end.y);

			duration =  Vector2.Distance (startV2, endV2)/GetSpeed (); // Duration only considers 2D distance, x and y
			timer = 0f;
		}

		public void Stop ()
		{
			playing = false;
		}

		public void AddFinishedListener (System.Action callback)
		{
			onEnd = callback;
		}

		public bool IsPlaying ()
		{
			return playing;
		}

		public float GetSpeed ()
		{
			return speed;
		}

		public void UpdateTweener ()
		{
			timer += Time.deltaTime;
			float t = Mathf.Clamp(timer / duration, 0f, 1f);
			this.transform.position = Vector3.Lerp (start, end, t);
			playing = (t >= 0f && t < 1f);

			if (!playing)
			{
				OnFinished ();
			}
		}

		public virtual void OnFinished ()
		{
			if (onEnd != null)
			{
				onEnd ();
			}
		}

		#endregion
		public void StartMoving (Vector3 start, Vector3 end, float speed)
		{
			this.speed = speed;
			this.start = start;
			this.end = end;
			Play ();
		}

		public void StartMoving (Vector3 start, Vector3 end)
		{
			StartMoving (start, end, GetSpeed ());
		}

		public void StartMoving (Vector3 target, float speed)
		{
			this.speed = speed;
			this.start = this.transform.position;
			this.end = target;
			Play ();
		}

		public void StartMoving (Vector3 target)
		{
			StartMoving (target, GetSpeed());
		}

	}
}
