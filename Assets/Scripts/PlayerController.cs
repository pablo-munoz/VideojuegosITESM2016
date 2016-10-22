using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public const int NORTH = 1;
	public const int WEST = 2;
	public const int SOUTH = 3;
	public const int EAST = 4;

	public const int INITIAL_HP = 5;
	public const float INVULNERABILITY_SECONDS = 1.5f;
	public const float ATTACK_MODE_DURATION = 1f;
	public const float ATTACK_MODE_COOLDOWN = 3f;

	public float movementSpeed;
	private int hitPoints;

	private Rigidbody2D rb;
	Animator anim;
	private LevelController levelController;
	private bool isInAttackMode;
	private bool canEnterAttackMode;
	private bool isInvulnerable;
	private int numPickaxes = 1;
	private int direction;

	private void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = this.GetComponent<Animator> ();
		levelController = GameObject.Find ("Level").GetComponent<LevelController>();
		hitPoints = INITIAL_HP;
		isInvulnerable = false;
		isInAttackMode = false;
		canEnterAttackMode = true;
		direction = WEST;
	}

	private void Update () {
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");
		Vector2 speedVector = new Vector2 (0f, 0f);

		if (x > 0) {
			direction = WEST;
			speedVector.x = 1f;
		} else if (x < 0) {
			direction = EAST;
			speedVector.x = -1f;
		} else if (y > 0) {
			direction = NORTH;
			speedVector.y = 1f;
		} else if (y < 0) {
			direction = SOUTH;
			speedVector.y = -1f;
		} else {
			speedVector.x = 0;
			speedVector.y = 0;
		}

		rb.velocity = speedVector * movementSpeed;

		checkForAttackCommand ();
		checkForPickAxeUse ();
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Goal")) {
			this.levelController.loadNextLevel ();
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			if (isInAttackMode) {
				Destroy (other.gameObject);
			} else if (!isInvulnerable) {
				// Lose hp
				this.hitPoints--;
				// Become invulnerable for a little while
				isInvulnerable = true;
				this.Invoke ("loseInvulnerability", INVULNERABILITY_SECONDS);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Food")) {
			if (this.hitPoints < INITIAL_HP) {
				Destroy (other.gameObject);
				this.hitPoints++;
			}
		} else if (other.gameObject.CompareTag ("PickAxe")) {
			Destroy (other.gameObject);
			this.numPickaxes++;
		}
	}

	public int getHitPoints() {
		return this.hitPoints;
	}

	public int getNumPickaxes() {
		return this.numPickaxes;
	}

	private void checkForPickAxeUse() {
		if (this.numPickaxes > 0 && Input.GetKeyUp (KeyCode.F)) {
			Vector2 rayDirection = new Vector2 ();
			if (direction == NORTH) {
				rayDirection = Vector2.up;
			} else if (direction == WEST) {
				rayDirection = Vector2.right;
			} else if (direction == SOUTH) {
				rayDirection = Vector2.down;
			} else if (direction == EAST) {
				rayDirection = Vector2.left;
			}

			RaycastHit2D hit = Physics2D.Raycast (new Vector2(this.transform.position.x, this.transform.position.y) + rayDirection, rayDirection, 0);
			if (hit && hit.collider.gameObject.CompareTag ("Wall")) {
				this.numPickaxes--;
				int x = (int) hit.collider.transform.position.x;
				int y = (int) hit.collider.transform.position.y;
				Tile.replace (levelController.groundPrefab, x, y, 1);
			}
		}
	}

	private void checkForAttackCommand() {
		if (canEnterAttackMode && Input.GetKeyUp (KeyCode.Space)) {
			// Enter attack mode
			isInAttackMode = true;
			// Mark an attack mode cooldown so user cannot spam attack key
			canEnterAttackMode = true;
			anim.Play ("Attack");
			// Schedule attack mode to end
			Invoke("endAttackMode", ATTACK_MODE_DURATION);
			// Schedule attack mode cooldown to end a.k.a. user is allowed to enter attack mode again
			Invoke ("allowAttackMode", ATTACK_MODE_DURATION + ATTACK_MODE_COOLDOWN);
		}
	}

	private void loseInvulnerability() {
		isInvulnerable = false;
	}

	private void endAttackMode() {
		isInAttackMode = false;
		anim.Play ("PlayerLoop");
	}

	private void allowAttackMode() {
		canEnterAttackMode = true;
	}
}
