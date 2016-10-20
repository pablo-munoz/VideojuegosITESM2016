using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public const int INITIAL_HP = 5;
	public const float INVULNERABILITY_SECONDS = 1.5f;

	public float speed;
	public int attackFramesDuration;
	private int hp;

	private Rigidbody2D rb;
	private int attackModeFrames;
	private bool isInvulnerable;

	private void Start () {
		rb = GetComponent<Rigidbody2D> ();
		attackModeFrames = 0;
		hp = INITIAL_HP;
		isInvulnerable = false;
	}

	private void Update () {
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");
		rb.velocity = new Vector2 (x * speed, y * speed);

		checkForAttackCommand ();
		attackModeFrames--;
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			if (inAttackMode()) {
				Destroy (other.gameObject);
			} else if (!isInvulnerable) {
				// Lose hp
				this.hp--;
				// Become invulnerable for a little while
				isInvulnerable = true;
				this.Invoke("loseInvulnerability", INVULNERABILITY_SECONDS);
			}
		}
	}

	public int getHitPoints() {
		return this.hp;
	}

	private void checkForAttackCommand() {
		if (Input.GetKeyUp (KeyCode.Space)) {
			attackModeFrames = attackFramesDuration;
		}
	}

	private bool inAttackMode () {
		return attackModeFrames > 0;
	}

	private void loseInvulnerability() {
		isInvulnerable = false;
	}
}
