using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tweeners;

namespace Characters
{
	/// <summary>
	/// Character touch controlled - Moves the character on points touched on the screen.
	/// - Carl Joven
	/// </summary>
	public class CharacterTouchControlled : MonoBehaviour
	{
		[SerializeField] private GameObject vipObject;
		[SerializeField] private float attackSpeed;
		[SerializeField] private float returnSpeed;

		private MoverPath pathMover;
		private bool movingToVip = false;

		#region Mono

		// Use this for initialization
		void Start ()
		{
			TouchListener.AddTouchListener (this.gameObject);
			pathMover = GetComponent<MoverPath> ();
			pathMover.AddFinishedListener (OnMoveEnd);
		}

		void OnDestroy ()
		{
			TouchListener.RemoveTouchListener (this.gameObject);
		}

		// Update is called once per frame
		void Update ()
		{

		}

		#endregion

		#region Touch listener

		public void OnTouch (Vector3 pos)
		{
			pos.z = this.transform.position.z;
			if (movingToVip)
			{
				pathMover.MoveAndClearPath (pos, attackSpeed);
			}
			else
			{
				pathMover.AddToPath (pos, attackSpeed);
			}
			movingToVip = false;
		}

		public void OnMoveEnd ()
		{
			Vector3 targetPos = vipObject.transform.position;
			Vector3 currentPos = this.transform.position;
			bool isVertical = (Mathf.Abs (currentPos.y - targetPos.y) > Mathf.Abs (currentPos.x - targetPos.x));
			float nearDistance = isVertical ? 0.8f : 1.2f;

			Vector3 heading = targetPos - currentPos;
			float dist = heading.magnitude;

			Vector3 direction = heading / dist;
			dist -= nearDistance;
			Vector3 nearTarget = direction * dist;

			if (Mathf.Abs (dist) < 0.2f)
			{
				movingToVip = false;
				return;
			}

			Vector3 newPos = this.transform.position + nearTarget;
			Vector3 newHeading = targetPos - currentPos;
			if (newHeading.magnitude < 0.2f)
			{
				movingToVip = false;
				return;
			}

			movingToVip = true;
			pathMover.AddToPath (newPos, returnSpeed);
		}

		#endregion
	}

}
