using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelController : MonoBehaviour {

	public GameObject groundPrefab;
	public GameObject wallPrefab;
	public GameObject keyPrefab;
	public int minimumKeyDistance = 6;

	void Start () {
		int[,] blueprint = Toolbox.singleton.blueprint;

		int nRows = blueprint.GetLength(0);
		int nCols = blueprint.GetLength(1);

		GameObject[,] tiles = new GameObject[nRows, nCols];
			
		for (int row = 0; row < nRows; row++) {
			for (int col = 0; col < nCols; col++) {
				switch (blueprint[row,col]) {
				case (int) Toolbox.COMPONENTS.FLOOR:
					tiles[row,col] = Instantiate (groundPrefab, new Vector3 (col, row, 0), Quaternion.identity) as GameObject;
					break;
				case (int) Toolbox.COMPONENTS.WALL:
					tiles[row,col] = Instantiate (wallPrefab, new Vector3 (col, row, 0), Quaternion.identity) as GameObject;
					break;
				default:
					// Add nothing
					break;
				}
			}
		}

		BlueprintPosition keyPosition = Toolbox.singleton.getRandomTileAtDistance (11, 5, minimumKeyDistance);
		Instantiate(keyPrefab, new Vector3(keyPosition.x, keyPosition.y, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
