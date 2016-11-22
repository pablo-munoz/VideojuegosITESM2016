using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The enemy controller is mainly concerned with motion.
// It makes an enemy behave in two ways or modes. A patrol mode and a chasing mode.
// During its patrol mode the enemy goes back and forth between a set path.
// During the chase mode the enemy calculates the shortest path to the player
// and advances closer to him on a frequent basis.
public class EnemyController : MonoBehaviour {

	public float DEFAULT_SPEED = 0.07f;
	public float CHASING_SPEED = 0.07f * 1.45f;
	public int PLAYER_DETECTION_RANGE = 8;
	public float treshold = 0.5f;
	public int patrolDistance = 6;

	private Rigidbody2D rb;
	private PlayerController player;
	private float speed = 0f;
	private Tile pathDestination;
	private List<Tile> path;
	private int indexInPath;
	private int direction;
	private bool chasingPlayer;

	void Start () {
		this.rb = GetComponent<Rigidbody2D> ();
		
		// On start we get a handle on the player object as we will be doing calculations
		// with the player position
		this.player = GameObject.Find ("Player").GetComponent<PlayerController>();

		this.chasingPlayer = false;
		this.beginPatrolling ();
		StartCoroutine (goTowardsPlayerMaybe());
	}
		
	void Update () {
		float distanceFromNextTarget = Vector3.Distance (transform.position, this.positionOfNextTile ());
		float delta;
		Vector2 dirVector = Vector2.zero;
		if (this.chasingPlayer) {
			if (this.indexInPath < this.path.Count -1 && distanceFromNextTarget < treshold) {
				this.indexInPath++;
			}
			dirVector = (this.positionOfNextTile() - transform.position);
		} else {
			delta = Vector3.Distance (this.transform.position, this.positionOfNextTile ());
			dirVector = this.positionOfNextTile () - this.transform.position;
			if (delta <= treshold) {
				if (this.indexInPath == this.path.Count - 1) {
					// If at end of path, begin backtracking
					this.direction = -1;
				} else if (this.indexInPath == 0) {
					// At beginning of path
					this.direction = 1;
				}
				this.indexInPath += this.direction;
			}
		}
			
		rb.velocity = dirVector.normalized * this.speed;
	}

	public Tile tileAt() {
		return Tile.getTileAtPosition ((int) transform.position.x, (int) transform.position.y);
	}

	private Vector3 positionOfNextTile() {
		return this.path [indexInPath].transform.position;
	}

	private void beginPatrolling() {
		this.chasingPlayer = false;
		Tile currentPosition = this.tileAt ();
		this.pathDestination = Tile.getRandomFloorTile (currentPosition, patrolDistance);
		this.path = Pathfinding.AStar (currentPosition, pathDestination);
		this.indexInPath = 0;
		this.direction = 1;
		this.speed = DEFAULT_SPEED;
	}

	private void beginChasingPlayer() {
		this.chasingPlayer = true;
		this.pathDestination = player.tileAt();
		this.path = Pathfinding.AStar (this.tileAt(), this.pathDestination);
		this.indexInPath = 0;
		this.direction = 1;
		this.speed = CHASING_SPEED;
	}
		
	// Coroutine to check every some seconds if the enemy is within a certain distance from the
	// player and if it is to abandon its patrolling route and begin chasing the player
	// at a faster speed.
	private IEnumerator goTowardsPlayerMaybe() {
		while (true) {
			List<Tile> pathToPlayer = Pathfinding.AStar (this.tileAt (), player.tileAt ());
			bool playerInRange = (pathToPlayer != null) && pathToPlayer.Count <= PLAYER_DETECTION_RANGE;
			
			if (this.chasingPlayer && !playerInRange) {
				this.beginPatrolling ();
			} else {
				if (playerInRange && player.tileAt() != this.pathDestination) {
					this.beginChasingPlayer ();
				}
			}

			yield return new WaitForSeconds (1);
		}
	}
}
