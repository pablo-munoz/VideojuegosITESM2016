using UnityEngine;
using System.Collections;

public class BossTeleportBehaviour : MonoBehaviour {

	LevelController level;
	BossController bc;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("Level").GetComponent<LevelController> ();
		bc = GetComponent<BossController> ();
		Tile current = bc.getTileAt ();

		// Summon 4 enemies for the hero to fight and teleport to a random location
		for (int i = 0; i < 4; i++) {
			Tile spawn = Tile.getRandomFloorTile (current.x, current.y, 1, 4);
			GameObject summon = level.addEnemy (spawn);
			summon.GetComponent<EnemyController> ().beginPatrolling (GameConstants.levelSize);
		}

		transform.position = Tile.getRandomFloorTile (current.x, current.y, 11).getPosition3();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
