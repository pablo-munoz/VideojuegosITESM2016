using UnityEngine;
using System.Collections;

public class TilePosition : MonoBehaviour {

	public Tile tileAt() {
		return Tile.getTileAtPosition ((int) Mathf.Round(transform.position.x), (int) Mathf.Round(transform.position.y));
	}

}
