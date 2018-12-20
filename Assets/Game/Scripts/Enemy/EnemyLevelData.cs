using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevelData 
{
	public int level;

	// Enemies spawn scheduling
	public EnemySpawnData[] spawnData;
}

public class EnemySpawnData
{
	public EnemyData.Type type;
	public int startCount;

	public float startSpawnTime;
	public float endSpawnTime;

	public float spawnIntervals;
	public int maxEnemies;
}
