﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public const int INITIAL_HP = 5;
	public const float INVULNERABILITY_SECONDS = 1.5f;
	public const float ATTACK_MODE_DURATION = 1f;
	public const float ATTACK_MODE_COOLDOWN = 3f;

	public float movementSpeed;
	private int hitPoints;

	private Rigidbody2D rb;
	private bool isInAttackMode;
	private bool canEnterAttackMode;
	private bool isInvulnerable;

	private void Start () {
		rb = GetComponent<Rigidbody2D> ();
		hitPoints = INITIAL_HP;
		isInvulnerable = false;
		isInAttackMode = false;
		canEnterAttackMode = true;
	}

	private void Update () {
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");
		rb.velocity = new Vector2 (x * movementSpeed, y * movementSpeed);

		checkForAttackCommand ();
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

	public int getHitPoints() {
		return this.hitPoints;
	}

	private void checkForAttackCommand() {
		if (canEnterAttackMode && Input.GetKeyUp (KeyCode.Space)) {
			// Enter attack mode
			isInAttackMode = true;
			// Mark an attack mode cooldown so user cannot spam attack key
			canEnterAttackMode = true;
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
	}

	private void allowAtackMode() {
		canEnterAttackMode = true;
	}
}
