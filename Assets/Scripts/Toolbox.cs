using UnityEngine;
using System.Collections;

public struct BoardPosition {
	public int x;
	public int y;
}

public class Toolbox : MonoBehaviour {
	public static Toolbox singleton;
	public int[,] blueprint;

	void Awake () {
		if (!singleton) {
			singleton = this;
			blueprint = new int[,]{
				{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0 },
				{ 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 2, 0 },
				{ 0, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2 },
				{ 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
				{ 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 2, 2 },
				{ 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 },
				{ 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 0, 0, 0 },
				{ 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 0, 0, 0 },
				{ 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0 },
				{ 2, 1, 1, 1, 1, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 2, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				{ 2, 2, 1, 1, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			};
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	public BoardPosition[] getAllValidSpawnPositions() {
		ArrayList validPositions = new ArrayList ();
		for (int i = 0; i < blueprint.GetLength(1); i++) {
			for (int j = 0; j < blueprint.GetLength (0); j++) {
				if (blueprint [j, i] == 1) {
					BoardPosition p = new BoardPosition ();
					p.x = i;
					p.y = j;
					validPositions.Add (p);
				}
			}
		}
		return (BoardPosition[])validPositions.ToArray (typeof(BoardPosition));
	}

	public BoardPosition getRandomSpawnPosition() {
		BoardPosition[] positions = getAllValidSpawnPositions ();
		return positions [Random.Range (0, positions.Length)];
	}
}
