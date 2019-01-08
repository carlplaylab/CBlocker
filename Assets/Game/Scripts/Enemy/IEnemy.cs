using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
	// Enemy creation 
	IEnemy Create (Transform parent, Transform target);

	long GetId ();

	void SetId (long id);

	void Enter ();

	void Kill ();

	Vector3 GetPosition ();

	bool CheckHit (Vector3 heroPos, int hitId = 0);
	int GetHP ();
	void Hit ();	// Reduce life
	bool IsAlive ();
		
	void DestroyObject ();

	int GetHitScore ();
	int GetKillScore ();

	bool IsBomb ();
}
