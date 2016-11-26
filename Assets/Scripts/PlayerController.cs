using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public const int NORTH = 1;
	public const int WEST = 2;
	public const int SOUTH = 3;
	public const int EAST = 4;

	public const int INITIAL_HP = 5;
	public const float INVULNERABILITY_SECONDS = 1.5f;
	public const float ATTACK_MODE_DURATION = 0.5f;
	public const float ATTACK_MODE_COOLDOWN = 3f;

	private Material painMaterial = null;
	private Material defaultMaterial = null;

	public float movementSpeed;
	private int hitPoints;

	private Rigidbody2D rb;
	Animator anim;
	private LevelController levelController;
	private bool isInAttackMode;
	private bool canEnterAttackMode;
	private bool isInvulnerable;
	private int numPickaxes = 2;
	private int direction;
    private int key;
	private Renderer renderer;

	private void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
		anim = this.GetComponent<Animator> ();
		levelController = GameObject.Find ("Level").GetComponent<LevelController>();
		renderer = this.GetComponent<Renderer> ();
		defaultMaterial = renderer.material;
		painMaterial = Resources.Load ("Materials/Pain", typeof(Material)) as Material;
		hitPoints = INITIAL_HP;
		isInvulnerable = false;
		isInAttackMode = false;
		canEnterAttackMode = true;
		direction = WEST;
        key = 0;
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

        if (key == 1){
            if (other.gameObject.CompareTag("Goal"))
            {
                key = 0;    
                this.levelController.loadNextLevel();
                Debug.Log("Key used");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			if (!isInvulnerable) {
				// Lose hp
				this.hitPoints--;
                //You die
                if(this.hitPoints == 0){
                    SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
                }
				// Become invulnerable for a little while
				isInvulnerable = true;
				anim.SetBool ("isInvulnerable", this.isInvulnerable);
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
			this.numPickaxes += 2;
		} else if (other.gameObject.CompareTag("Key")) {
            Destroy(other.gameObject);
            key++;
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
			rayDirection = this.rb.velocity;
			Vector2 center = (Vector2) transform.position;
			RaycastHit2D hit = Physics2D.Raycast (center, rayDirection, 1f);
			if (hit && hit.collider.gameObject.CompareTag ("Wall")) {
				Tile candidate = hit.collider.gameObject.GetComponent<Tile> ();
				if (!candidate.isBoundary ()) {
					this.numPickaxes--;
					int x = (int)hit.collider.transform.position.x;
					int y = (int)hit.collider.transform.position.y;
					Tile.replace (levelController.groundPrefab, x, y, 1);
				}
			}
		}
	}

	private void checkForAttackCommand() {
		if (canEnterAttackMode && Input.GetKeyUp (KeyCode.Space)) {
			// Enter attack mode
			isInAttackMode = true;
			anim.SetBool ("isInAttackMode", this.isInAttackMode);
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
		anim.SetBool ("isInvulnerable", this.isInvulnerable);
	}

	private void endAttackMode() {
		isInAttackMode = false;
		anim.SetBool ("isInAttackMode", this.isInAttackMode);
	}

	private void allowAttackMode() {
		canEnterAttackMode = true;
	}

	public Tile tileAt() {
		return Tile.getTileAtPosition ((int) Mathf.Round(transform.position.x), (int) Mathf.Round(transform.position.y));
	}

	public bool isAttacking() {
		return this.isInAttackMode;
	}
}
