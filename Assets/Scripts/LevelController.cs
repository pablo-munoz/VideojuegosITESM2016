using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class LevelController : MonoBehaviour {

	private int MINIMUM_KEY_DELTA_DISTANCE = 15;

	public GameObject groundPrefab;
	public GameObject wallPrefab;
	public GameObject keyPrefab;
	public GameObject foodPrefab;
	public GameObject goalPrefab;
	public int numFoodPerLevel;
	public int minimumKeyDistance = 6;

	private JsonData levelData;
	private int _playerSpawnX;
	private int _playerSpawnY;

	public int playerSpawnX {
		private set { _playerSpawnX = value; }
		get { return _playerSpawnX; }
	}

	public int playerSpawnY {
		private set { _playerSpawnY = value; }
		get { return _playerSpawnY; }
	}


	void Awake () {
		TextAsset levelJson = Resources.Load<TextAsset> ("level1");
		levelData = JsonMapper.ToObject (levelJson.text);
		int nRows = (int)levelData ["rows"];
		int nCols = (int)levelData ["cols"];
		playerSpawnX = (int) levelData ["playerSpawnX"];
		playerSpawnY = (int) levelData ["playerSpawnY"];
		int goalX = (int)levelData ["goalX"];
		int goalY = (int)levelData ["goalY"];
		GameObject tilePrefab = groundPrefab;

		for (int i = 0; i < nRows; i++) {
			for (int j = 0; j < nCols; j++) {
				int type = (int)levelData ["blueprint"] [i] [j];

				if (type > 0) {
					if (type == (int) Toolbox.COMPONENTS.FLOOR) {
						tilePrefab = groundPrefab;
					} else if (type == (int) Toolbox.COMPONENTS.WALL) {
						tilePrefab = wallPrefab;
					}
					Tile.make (tilePrefab, j, nRows - i, type);
				}
			}
		}

		// Spawn the goal
		Instantiate(goalPrefab, new Vector2(goalX, goalY), Quaternion.identity);

		// Spawn the key to the next level
		Tile keySpawn = Tile.getRandomFloorTile (playerSpawnX, playerSpawnY, MINIMUM_KEY_DELTA_DISTANCE);
		Instantiate(keyPrefab, new Vector2(keySpawn.x, keySpawn.y), Quaternion.identity);

		// Spawn the food (power ups)
		for (int i = 0; i < numFoodPerLevel; i++) {
			Tile foodSpawn = Tile.getRandomFloorTile ();
			Instantiate (foodPrefab, new Vector2 (foodSpawn.x, foodSpawn.y), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
