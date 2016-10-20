using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public float treshold = 0.7f;
	public float speed = 0.7f;
	public int patrolDistance = 6;

	Tile spawnTile;
	private Tile patrolDestination;
	private List<Tile> patrolPath;
	private int currentPatrolTileIndex;
	private int direction;

	void Start () {
		direction = 1;
		spawnTile = Tile.getTileAtPosition ((int) transform.position.x, (int) transform.position.y);
		patrolDestination = Tile.getRandomFloorTile (spawnTile, patrolDistance);
		patrolPath = Pathfinding.AStar (spawnTile, patrolDestination);
	}
		
	void Update () {
		float distance = Vector3.Distance (transform.position, patrolPath [currentPatrolTileIndex].transform.position);
		
		transform.position = Vector3.MoveTowards (
			transform.position, patrolPath[currentPatrolTileIndex].transform.position, speed * Time.deltaTime);

		if (currentPatrolTileIndex == 0) {
			direction = 1;
		} else if (currentPatrolTileIndex == patrolPath.Count - 1) {
			direction = -1;
		}

		if (distance < treshold) {
			currentPatrolTileIndex += direction; 
		}
	}
}
