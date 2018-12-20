using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
	// Enemy creation 
	IEnemy Create (Transform parent, Transform target);

	int GetId ();

	void SetId (int id);

	void Enter ();

	void Kill ();


}
