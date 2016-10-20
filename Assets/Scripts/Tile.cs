using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public int x, y, type;
	public List<Tile> history;

	public static List<Tile> allTiles = new List<Tile> ();
	public static List<Tile> wallTiles = new List<Tile> ();
	public static List<Tile> floorTiles = new List<Tile> ();
	public static Tile[,] indexedTiles = new Tile[100,100];   // Ugly hack, lets try not to hardcode the size
	public static int numRows = 0;
	public static int numCols = 0;

	public float f, g;

	public static Tile make(GameObject prefab, int x, int y, int type) {
		if (x > numCols)
			numCols = x;
		if (y > numRows)
			numRows = y;
		
		GameObject obj = Instantiate (prefab, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
		obj.AddComponent<Tile> ();
		Tile tile = obj.GetComponent<Tile> ();
		tile.type = type;
		tile.x = x;
		tile.y = y;
		allTiles.Add (tile);
		indexedTiles [x,y] = tile;

		if (type == 1) {
			floorTiles.Add (tile);
		} else if (type == 2) {
			wallTiles.Add (tile);
		}

		return tile;
	}

	public static Tile getRandomFloorTile() {
		return floorTiles [Random.Range (0, floorTiles.Count - 1)];
	}

	public static Tile getRandomFloorTile(int x, int y, int delta) {
		Vector2 reference = new Vector2 (x, y);
		List<Tile> candidates = new List<Tile> ();
		for (int i = 0; i < floorTiles.Count; i++) {
			if (Vector2.Distance (reference, floorTiles [i].transform.position) >= delta) {
				candidates.Add (floorTiles [i]);
			}
		}
		if (candidates.Count > 0) {
			return candidates [Random.Range (0, candidates.Count - 1)];
		}

		return null;
	}

	public static Tile getRandomFloorTile(Tile origin, int delta) {
		return Tile.getRandomFloorTile ((int) origin.transform.position.x, (int) origin.transform.position.y, delta);
	}

	public static Tile getTileAtPosition(int x, int y) {
		return indexedTiles [x,y];
	}

	public static List<Tile> getNeighboursOf(Tile tile) {
		int x = (int) tile.transform.position.x;
		int y = (int) tile.transform.position.y;
		List<Tile> neighbours = new List<Tile> ();
		// might be wrong, lower bound might be zero, same with upper, might be off by 1
		if (x > 1 && indexedTiles [x - 1, y].type == 1)
			neighbours.Add (indexedTiles [x - 1, y]);
		
		if (x < numCols - 1 && indexedTiles [x + 1, y].type == 1)
			neighbours.Add (indexedTiles [x + 1, y]);
		
		if (y > 1 && indexedTiles [x, y-1].type == 1)
			neighbours.Add (indexedTiles [x, y-1]);
		
		if (y < numRows - 1 && indexedTiles [x, y+1].type == 1)
			neighbours.Add (indexedTiles [x, y+1]);

		return neighbours;
	}
}
