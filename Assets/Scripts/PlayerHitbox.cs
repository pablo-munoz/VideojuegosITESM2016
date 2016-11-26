using UnityEngine;
using System.Collections;

public class PlayerHitbox : MonoBehaviour {

	private PlayerController pc;
	private new BoxCollider2D collider;

	// Use this for initialization
	void Start () {
		pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
		collider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = pc.transform.position;
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag("Goal")) {
			Physics2D.IgnoreCollision (collider, other.gameObject.GetComponent<BoxCollider2D> ());
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			Physics2D.IgnoreCollision (collider, other.gameObject.GetComponent<BoxCollider2D> ());
		} else if (other.gameObject.CompareTag ("Enemy")) {
			if (pc.isAttacking ()) {
				other.gameObject.GetComponent<EnemyController> ().getAttacked ();
			}
		} else if (other.gameObject.CompareTag ("Boss")) {
			if (pc.isAttacking ()) {
				other.gameObject.GetComponent<BossController> ().getAttacked ();
			}
		}
	}
}