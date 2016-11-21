using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	private const int MINIMUM_KEY_DELTA_DISTANCE = 10;
	private const int MINIMUM_ENEMY_SPAWN_DISTANCE = 5;

	public GameObject groundPrefab;
	public GameObject wallPrefab;
	public GameObject keyPrefab;
	public GameObject foodPrefab;
	public GameObject pickaxePrefab;
	public GameObject goalPrefab;
	public GameObject enemyPrefab;
	public int numFoodPerLevel = 4;
	public int numPickaxesPerLevel = 1;
	public int numLevels;
	public int minimumKeyDistance = 6;
	public int numEnemies = 5;
	public int levelSize = 15; // must be an odd number

	private GameObject player;
	private JsonData levelData;
	private List<Maze> levels;
	private List<GameObject> levelObjects;
	private int currentLevel;
	private bool newLevelCooldown;

	void Awake () {
//		TextAsset levelJson = Resources.Load<TextAsset> ("nivel3");
//		levelData = JsonMapper.ToObject (levelJson.text);
		levels = new List<Maze> ();

		for (int i = 0; i < this.numLevels; i++) {
			levels.Add (new Maze (this.levelSize, this.levelSize));
		}

		currentLevel = 0;

		player = GameObject.Find ("Player");
		levelObjects = new List<GameObject> ();
		this.constructLevel();
	}
	
	public void constructLevel() {
		Tile.destroyAllTiles ();
		for (int i = 0; i < levelObjects.Count; i++) {
			Destroy (levelObjects [i]);
		}
		levelObjects = new List<GameObject> ();

		int nRows = this.levels [this.currentLevel].nRows;
		int nCols = this.levels [this.currentLevel].nCols;
		int playerSpawnX = 1;
		int playerSpawnY = 2;
		int goalX = 0;
		int goalY = 0;
		GameObject tilePrefab = groundPrefab;

		int[,] lab = this.levels [this.currentLevel].getMaze ();

		for (int i = 0; i < nRows; i++) {
			for (int j = 0; j < nCols; j++) {
				int type = lab[i,j];

				if (type > 0) {
					// Make 0 also be an alias for ground
					if (type == 1 || type == 0) {
						tilePrefab = groundPrefab;
					} else if (type == 4) {
						tilePrefab = groundPrefab;
						goalX = j;
						goalY = i;
					} else if (type == 2) {
						tilePrefab = wallPrefab;
					}
					Tile.make (tilePrefab, j, nRows - i, type);
				}
			}
		}

		// Spawn the goal
		levelObjects.Add (
			Instantiate (goalPrefab, new Vector2 (goalX, goalY), Quaternion.identity) as GameObject);

		// Spawn the key to the next level
		Tile keySpawn = Tile.getRandomFloorTile (playerSpawnX, playerSpawnY, MINIMUM_KEY_DELTA_DISTANCE);
		levelObjects.Add(
			Instantiate(this.keyPrefab, new Vector2(keySpawn.x, keySpawn.y), Quaternion.identity) as GameObject);

		// Spawn the food (power ups)
		for (int i = 0; i < numFoodPerLevel; i++) {
			Tile foodSpawn = Tile.getRandomFloorTile ();
			levelObjects.Add(
				Instantiate (this.foodPrefab, new Vector2 (foodSpawn.x, foodSpawn.y), Quaternion.identity) as GameObject);
		}

		// Spawn pickaxes
		for (int i = 0; i < numPickaxesPerLevel; i++) {
			Tile pickaxeSpawn = Tile.getRandomFloorTile ();
			levelObjects.Add(
				Instantiate (this.pickaxePrefab, new Vector2 (pickaxeSpawn.x, pickaxeSpawn.y), Quaternion.identity) as GameObject);
		}

		// Spawn the enemies
		for (int i = 0; i < numEnemies; i++) {
			Tile enemySpawnPosition = Tile.getRandomFloorTile (
				playerSpawnX, playerSpawnY, MINIMUM_ENEMY_SPAWN_DISTANCE);
			levelObjects.Add(
				Instantiate (enemyPrefab, new Vector3 (enemySpawnPosition.x, enemySpawnPosition.y, 0), Quaternion.identity)
				as GameObject);
		}
			
		player.transform.position = new Vector2 (playerSpawnX, playerSpawnY);
	}

	public void loadNextLevel() {
		++this.currentLevel;

		if (this.currentLevel >= this.numLevels) {
			SceneManager.LoadScene ("WinScreen", LoadSceneMode.Single);
		} else {
			this.constructLevel ();
		}
	}

	private void resetNewLevelCooldown() {
		this.newLevelCooldown = false;
	}
		
}
