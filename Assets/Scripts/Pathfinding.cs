using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

	public static List<Tile> AStar(Tile origin, Tile destination){ 
		List<Tile> visited = new List<Tile> ();
		List<Tile> frontier = new List<Tile> ();
		origin.history = new List<Tile> ();

		visited.Add (origin);
		frontier.Add (origin);

		origin.g = 0;
		origin.f = origin.g + Vector2.Distance (origin.transform.position, destination.transform.position);

		while (frontier.Count > 0) {
			int smallest = 0;

			for (int i = 0; i < frontier.Count; i++) {
				if (frontier [i].f < frontier [smallest].f) {
					smallest = i;
				}
			}

			Tile smallestTile = frontier [smallest];
			frontier.Remove (smallestTile);

			if (smallestTile == destination) {
				List<Tile> result = new List<Tile> (smallestTile.history);
				result.Add (smallestTile);
				return result;
			} else {
				List<Tile> neighbours = Tile.getNeighboursOf (smallestTile);
				for (int i = 0; i < neighbours.Count; i++) {
					Tile currentNeighbour = neighbours [i];

					if (!visited.Contains (currentNeighbour)) {
						visited.Add (currentNeighbour);
						frontier.Add (currentNeighbour);

						currentNeighbour.g = smallestTile.g +
						Vector2.Distance (smallestTile.transform.position, currentNeighbour.transform.position);

						float h = Vector2.Distance (currentNeighbour.transform.position, destination.transform.position);
						currentNeighbour.f = currentNeighbour.g + h;

						currentNeighbour.history = new List<Tile> (smallestTile.history);
						currentNeighbour.history.Add (smallestTile);
					}
				}
			}
		}
		return null;
	}
		
}
