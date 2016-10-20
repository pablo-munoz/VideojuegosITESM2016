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

		// Spawn the key to the next level
		Tile keySpawn = Tile.getRandomFloorTile (playerSpawnX, playerSpawnY, MINIMUM_KEY_DELTA_DISTANCE);
		Instantiate(keyPrefab, new Vector3(keySpawn.x, keySpawn.y, 0), Quaternion.identity);

//		int[,] blueprint = Toolbox.singleton.blueprint;
//
//		int nRows = blueprint.GetLength(0);
//		int nCols = blueprint.GetLength(1);
//
//		GameObject[,] tiles = new GameObject[nRows, nCols];
//			
//		for (int row = 0; row < nRows; row++) {
//			for (int col = 0; col < nCols; col++) {
//				switch (blueprint[row,col]) {
//				case (int) Toolbox.COMPONENTS.FLOOR:
//					tiles[row,col] = Instantiate (groundPrefab, new Vector3 (col, row, 0), Quaternion.identity) as GameObject;
//					break;
//				case (int) Toolbox.COMPONENTS.WALL:
//					tiles[row,col] = Instantiate (wallPrefab, new Vector3 (col, row, 0), Quaternion.identity) as GameObject;
//					break;
//				default:
//					// Add nothing
//					break;
//				}
//			}
//		}
//
//		BlueprintPosition keyPosition = Toolbox.singleton.getRandomTileAtDistance (11, 5, minimumKeyDistance);
//		Instantiate(keyPrefab, new Vector3(keyPosition.x, keyPosition.y, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
