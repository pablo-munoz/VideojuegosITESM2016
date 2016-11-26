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
	private Renderer renderer;
	private Material painMaterial = null;
	private Material defaultMaterial = null;
	private PlayerController player;
	private float speed = 0f;
	private Tile pathDestination;
	private List<Tile> path;
	private List<Tile> pathToPlayer;
	private Tile playerLastSeenAt;
	private int indexInPath;
	private int direction;
	private bool chasingPlayer;
	private int hp;
	private Animator anim;
	private Collider2D collider;

	void Start () {
		this.renderer = GetComponent<Renderer> ();
		this.defaultMaterial = renderer.material;
		this.painMaterial = Resources.Load ("Materials/Pain", typeof(Material)) as Material;
		this.rb = GetComponent<Rigidbody2D> ();
		this.anim = GetComponent<Animator> ();
		this.collider = GetComponent<Collider2D> ();
		
		// On start we get a handle on the player object as we will be doing calculations
		// with the player position
		this.player = GameObject.Find ("Player").GetComponent<PlayerController>();

		this.hp = 2;

		this.beginPatrolling ();
		StartCoroutine (goTowardsPlayerMaybe());
	}
		
	void Update () {
		float distanceFromNextTarget = Vector3.Distance (transform.position, this.positionOfNextTile ());
		float delta;
		Vector2 dirVector = Vector2.zero;
		Tile next;

//		if (rb.velocity == Vector2.zero) {
//			this.beginPatrolling ();
//		}

		if (this.chasingPlayer) {
			if (player.tileAt () != this.playerLastSeenAt) {
				this.pathToPlayer.Add (player.tileAt ());
				this.playerLastSeenAt = player.tileAt ();
			}
			next = pathToPlayer [indexInPath];
			dirVector = (Vector2)next.transform.position - (Vector2)transform.position;
			delta = Vector3.Distance (next.transform.position, this.transform.position);
			if (this.indexInPath < this.pathToPlayer.Count - 1 && delta <= treshold) {
				this.indexInPath++;
			}
		} else {
			next = path [indexInPath];
			delta = Vector3.Distance (next.transform.position, this.transform.position);
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
		anim.SetBool ("chasingPlayer", this.chasingPlayer);
		Tile currentPosition = this.tileAt ();
		this.pathDestination = Tile.getRandomFloorTile (currentPosition, patrolDistance);
		this.path = Pathfinding.AStar (currentPosition, pathDestination);
		this.indexInPath = 0;
		this.direction = 1;
		this.speed = DEFAULT_SPEED;
	}

	private void beginChasingPlayer() {
		this.chasingPlayer = true;
		anim.SetBool ("chasingPlayer", this.chasingPlayer);
		this.speed = CHASING_SPEED;
		this.pathToPlayer = Pathfinding.AStar (this.tileAt (), player.tileAt ());
		this.playerLastSeenAt = player.tileAt ();
		this.indexInPath = 0;
	}

	private bool isPlayerInSight(Vector2 rayDirection) {
		RaycastHit2D hit = Physics2D.Raycast ((Vector2) transform.position, rayDirection, PLAYER_DETECTION_RANGE);
		if (hit && hit.collider.gameObject.CompareTag ("Player")) {
			return true;
		}
		return false;
	}
		
	// Coroutine to check every some seconds if the enemy is within a certain distance from the
	// player and if it is to abandon its patrolling route and begin chasing the player
	// at a faster speed.
	private IEnumerator goTowardsPlayerMaybe() {
		while (true) {
			if (!this.chasingPlayer) {
				if (this.isPlayerInSight (Vector2.left) ||
				    this.isPlayerInSight (Vector2.right) ||
				    this.isPlayerInSight (Vector2.up) ||
				    this.isPlayerInSight (Vector2.down)) {
					this.beginChasingPlayer ();
				} else if (this.chasingPlayer && Vector2.Distance (transform.position, player.transform.position) > PLAYER_DETECTION_RANGE) {
					this.beginPatrolling ();
				}
			} else {
				if (this.pathToPlayer.Count >= PLAYER_DETECTION_RANGE) {
					this.beginPatrolling ();
				}
			}
			yield return new WaitForSeconds (1.5f);
		}
	}

	private void resetMaterial() {
		this.renderer.material = this.defaultMaterial;
	}

	public void getAttacked() {
		this.hp--;
		this.renderer.material = painMaterial;
		this.Invoke ("resetMaterial", 0.5f);
		if (this.hp == 0) {
			Destroy (this.gameObject);
		}
	}

	public void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "NonObstacle") {
			Physics2D.IgnoreCollision(collision.collider, collider);
		}

	}
}
