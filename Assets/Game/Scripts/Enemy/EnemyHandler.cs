using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
	// Singletonn
	private static EnemyHandler instance;
	public static EnemyHandler GetInstance ()
	{
		return instance;
	}

	// Handling of child objects
	private List<IEnemy> objectList = new List<IEnemy> ();
	private int counter;

	// Enemy references
	private GameObject [] enemyReferences;

	// Enemy levels
	private EnemyLevelData [] levelData;
	private float levelTime = 0f;

	public static bool Exists ()
	{
		return instance != null;
	}

	#region MONO

	void Start ()
	{
		instance = this;
	}

	void OnDestroy ()
	{
		instance = null;
	}

	void Update ()
	{
		
	}

	#endregion

	#region ENEMY MANAGEMENT

	public int AddEnemy (IEnemy enemy)
	{
		int id = -1;
		if (instance != null && enemy != null && enemy.GetId () < 0)
		{
			objectList.Add (enemy);
			counter++;
			id = counter;
			enemy.SetId (counter);
		}
		return id;	
	}

	public void RemoveEnemy (IEnemy enemy)
	{
		if(objectList.Contains(enemy))
		{
			objectList.Remove (enemy);
			enemy.Kill ();
		}
	}
	
	#endregion


}
