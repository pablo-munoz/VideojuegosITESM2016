using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesController : MonoBehaviour {

	public int numEnemies;
	public GameObject enemyPrefab;

	void Start () {
		BlueprintPosition spawnPosition;

		for (int i = 0; i < numEnemies; i++) {
			spawnPosition = Toolbox.singleton.getRandomSpawnPosition ();
			Instantiate (enemyPrefab, new Vector3 (spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
		}
	}

}
