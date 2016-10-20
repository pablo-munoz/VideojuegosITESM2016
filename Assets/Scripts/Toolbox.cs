using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct BlueprintPosition {
	public int x;
	public int y;

	public BlueprintPosition(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public string ToString() {
		return x + ", " + y;
	}
}

public class Toolbox : MonoBehaviour {
	public static Toolbox singleton;
	public int[,] blueprint;
	public int numRows;
	public int numCols;
	public enum COMPONENTS { NOTHING, FLOOR, WALL };

	void Awake () {
		if (!singleton) {
			singleton = this;

//			TextAsset levelJson = Resources.Load("level1") as TextAsset;
//			blueprint = JsonHelper.getJsonArray(levelJson.text);

			blueprint = new int[,]{
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0 },
				{ 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 2, 0 },
				{ 0, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2 },
				{ 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
				{ 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 2 },
				{ 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 },
				{ 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 0, 0, 0 },
				{ 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 0, 0, 0 },
				{ 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 },
				{ 2, 1, 1, 1, 1, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 2, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 2, 2, 1, 1, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			};

			numRows = blueprint.GetLength (0);
			numCols = blueprint.GetLength (1);
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public BlueprintPosition[] getAllValidSpawnPositions() {
		ArrayList validPositions = new ArrayList ();
		for (int i = 0; i < numCols; i++) {
			for (int j = 0; j < numRows; j++) {
				if (blueprint [j, i] == 1) {
					BlueprintPosition p = new BlueprintPosition ();
					p.x = i;
					p.y = j;
					validPositions.Add (p);
				}
			}
		}
		return (BlueprintPosition[])validPositions.ToArray (typeof(BlueprintPosition));
	}

	public BlueprintPosition getRandomSpawnPosition() {
		BlueprintPosition[] positions = getAllValidSpawnPositions ();
		return positions [Random.Range (0, positions.Length)];
	}
		
	/*
	 * Modifies a given ArrayList (path) such that it contains, in order, BlueprintPositions that describe
	 * a traversable path from a starting x and y position, and that spans for at least desiredDistance.
	 * For example, if startX is 2 and startY is 1, the endpoint of a path with desiredDistance 3 could be
	 * x = 5, y = 1 or x = 4 y = 2 or x = 1 y = 5, etc.
	 * 
	 * Precondition: startX and startY are valid coordinates.
	 */
	public void findTraversablePath(int startX, int startY, int desiredDistance, List<BlueprintPosition> path, List<BlueprintPosition> alreadyChecked) {
		if (alreadyChecked == null) {
			alreadyChecked = new List<BlueprintPosition> ();
		}

		BlueprintPosition positionOfInterest = new BlueprintPosition (startX, startY);

		// Base case when the path contains desiredDistance board positions. In such a case a path
		// has been found.
		if (path.Count == desiredDistance) {
			return;
		} else if (alreadyChecked.Contains(positionOfInterest) || 
			!blueprintPositionHasTraversibleAdjacent (startX, startY) ||
			blueprint[startY, startX] != 1) {
			return;
		} else {
			path.Add (positionOfInterest);
			alreadyChecked.Add (positionOfInterest);
			findTraversablePath (startX, startY - 1, desiredDistance, path, alreadyChecked);
			findTraversablePath (startX - 1, startY, desiredDistance, path, alreadyChecked);
			findTraversablePath (startX + 1, startY, desiredDistance, path, alreadyChecked);
			findTraversablePath (startX, startY + 1, desiredDistance, path, alreadyChecked);
		}
	}

	public bool blueprintPositionIsValid(int x, int y) {
		return (0 <= x && x <= numCols && 0 <= y && y <= numRows);
	}

	public bool blueprintPositionHasTraversibleAdjacent(int x, int y) {
		for (int deltaX = -1; deltaX <= 1; deltaX++) {
			for (int deltaY = -1; deltaY <= 1; deltaY++) {
				if (!(deltaX == 0 && deltaY == 0) && blueprintPositionIsValid (x + deltaX, y + deltaY) && blueprint [y + deltaY, x + deltaX] == 1)
					return true;
			}
		}
		return false;
	}

	public BlueprintPosition getRandomTileAtDistance(int fromX, int fromY, int desiredDistance) {
		List<BlueprintPosition> candidatePositions = new List<BlueprintPosition> ();
		
		for (int i = 0; i < numCols; i++) {
			for (int j = 0; j < numRows; j++) {
				if (blueprint [j, i] == 1 && (Mathf.Abs (fromX - i) + Mathf.Abs (fromY - j)) >= desiredDistance) {
					candidatePositions.Add (new BlueprintPosition (i, j));
				}
			}
		}

		return candidatePositions [Random.Range (0, candidatePositions.Count - 1)];
	}

}
