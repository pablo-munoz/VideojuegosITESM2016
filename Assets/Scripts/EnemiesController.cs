using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesController : MonoBehaviour {

	public int numEnemies;
	public GameObject enemyPrefab;

	private int MINIMUM_ENEMY_SPAWN_DISTANCE = 8;
	private LevelController levelController;

	void Start () {
		levelController = GameObject.Find ("Level").GetComponent<LevelController> ();
		Tile spawnPosition;

		for (int i = 0; i < numEnemies; i++) {
			spawnPosition = Tile.getRandomFloorTile (
				levelController.playerSpawnX, levelController.playerSpawnY, MINIMUM_ENEMY_SPAWN_DISTANCE);
			Instantiate (enemyPrefab, new Vector3 (spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
		}
	}

}
