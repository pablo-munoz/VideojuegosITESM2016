using UnityEngine;
using System.Collections;

public class PlayerHitbox : MonoBehaviour {

	PlayerController pc;
	BoxCollider2D collider;

	// Use this for initialization
	void Start () {
		pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
		collider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			Physics2D.IgnoreCollision(collider, other.gameObject.GetComponent<BoxCollider2D>());
		} else if (other.gameObject.CompareTag ("Enemy")) {
			if (pc.isAttacking() && Input.GetKeyDown(KeyCode.Space)) {
				other.gameObject.GetComponent<EnemyController> ().getAttacked ();
			}
		}
	}
}
