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

	private enum COMPONENTS { NOTHING, FLOOR, WALL };

	void Start () {
		int[,] blueprint = Toolbox.singleton.blueprint;

		int rows = blueprint.GetLength(0);
		int cols = blueprint.GetLength(1);
			
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				switch (blueprint[row,col]) {
				case (int) COMPONENTS.FLOOR:
					Instantiate (groundPrefab, new Vector3 (col, row, 0), Quaternion.identity);
					break;
				case (int) COMPONENTS.WALL:
					Instantiate (wallPrefab, new Vector3 (col, row, 0), Quaternion.identity);
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
