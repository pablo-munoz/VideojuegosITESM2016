using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public int attackFramesDuration;

	private Rigidbody2D rb;
	private int attackModeFrames;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		attackModeFrames = 0;
	}

	void Update () {
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");
		rb.velocity = new Vector2 (x * speed, y * speed);

		checkForAttackCommand ();
		attackModeFrames--;
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			if (inAttackMode()) {
				Destroy (other.gameObject);
			} else {
				
			}
		}
	}

	void checkForAttackCommand() {
		if (Input.GetKeyUp (KeyCode.Space)) {
			attackModeFrames = attackFramesDuration;
		}
	}

	bool inAttackMode () {
		return attackModeFrames > 0;
	}
}
