using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

	private int x;
	private int y;
	private Toolbox.COMPONENTS type;
	private static Tile[,] levelTiles;

	public static Tile getTile(int x, int y) {
		return levelTiles[x, y];
	}

	public static Tile makeTile(int x, int y, Toolbox.COMPONENTS type) {
		if (levelTiles [x, y] != null) {
			return levelTiles [x, y];
		} else {
			Tile newTile = new Tile (x, y, type);
			levelTiles [x, y] = newTile;
			return newTile;
		}
	}

	private Tile(int x, int y, Toolbox.COMPONENTS type) {
		this.x = x;
		this.y = y;
	}

	Toolbox.COMPONENTS getType() {
		return this.type;
	}


	public Tile[] getConnectedNeighbours() {
		List<Tile> connectedNeighbours = new List<Tile>();
		// Account for left neighbours
		return connectedNeighbours.ToArray ();
	}


}
