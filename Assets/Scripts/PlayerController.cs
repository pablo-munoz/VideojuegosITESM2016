using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour {

	public const int NORTH = 1;
	public const int WEST = 2;
	public const int SOUTH = 3;
	public const int EAST = 4;

	public const int INITIAL_HP = 5;
	public const float INVULNERABILITY_SECONDS = 1.5f;
	public const float ATTACK_MODE_DURATION = 1f;
	public const float ATTACK_MODE_COOLDOWN = 1f;

	private Material painMaterial = null;
	private Material defaultMaterial = null;

	public float movementSpeed;
	private int hitPoints;

	//audio
	private AudioSource audio;
	public AudioClip fruitAudio, dieAudio, attackAudio, damageAudio, itemAudio;
    
	private Rigidbody2D rb;
	Animator anim;
	private LevelController levelController;
	private bool isInAttackMode;
	private bool canEnterAttackMode;
	private bool isInvulnerable;
	private int numPickaxes = 2;
	private int direction;
    private int key;
	private new Renderer renderer;
    private bool MoveL;

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
        MoveL = false;
		direction = WEST;
        key = 0;
		audio = this.GetComponent<AudioSource> ();
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
        if (rb.velocity.x < 0) {
            MoveL = true;
            anim.SetBool("MoveL", this.MoveL);
        }else if(rb.velocity.x > 0){
            MoveL = false;
            anim.SetBool("MoveL", this.MoveL);
        }


     
	}

	private void OnCollisionEnter2D(Collision2D other) {
        if (this.key > 0){
            if (other.gameObject.CompareTag("Goal"))
            {
                this.key = 0;    
                this.levelController.loadNextLevel();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			if (!isInvulnerable) {
				// Lose hp
				this.hitPoints--;

                //You die
				this.checkForDeath();

				// Become invulnerable for a little while
				isInvulnerable = true;

				//play sound
				audio.PlayOneShot(damageAudio, 0.9f);

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

				//play sound
				audio.PlayOneShot(fruitAudio, 1.7f);
			}
		} else if (other.gameObject.CompareTag ("PickAxe")) {
			Destroy (other.gameObject);
			this.numPickaxes += 2;

			//play sound
			audio.PlayOneShot(itemAudio, 1.7f);

		} else if (other.gameObject.CompareTag ("Key")) {
			Destroy (other.gameObject);
			key++;

			//play sound
			audio.PlayOneShot(itemAudio, 1.7f);

		} else if (other.gameObject.CompareTag ("BossMagic")) {
			Destroy (other.gameObject);
			if (!isInvulnerable) {
				// Lose hp
				this.hitPoints--;

				//play sound
				audio.PlayOneShot (damageAudio, 1.7f);

				//You die
				this.checkForDeath();

				// Become invulnerable for a little while
				isInvulnerable = true;
				anim.SetBool ("isInvulnerable", this.isInvulnerable);
				this.Invoke ("loseInvulnerability", INVULNERABILITY_SECONDS);
			}
		}
    }

	private void checkForDeath() {
		if(this.hitPoints == 0){
			SceneManager.LoadScene("Gameover", LoadSceneMode.Single);

			//play sound
			audio.PlayOneShot(dieAudio,1.7f);
		}
	}
		
	public int getHitPoints() {
		return this.hitPoints;
	}
            
	public int getNumPickaxes() {
		return this.numPickaxes;
	}

	private void checkForPickAxeUse() {
		if (this.numPickaxes > 0 && (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))) {
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
		if (Input.GetKeyDown(KeyCode.Space)) {
			// Enter attack mode
			isInAttackMode = true;
			anim.SetBool ("isInAttackMode", this.isInAttackMode);
			//play sound
			audio.PlayOneShot(attackAudio, 0.9f);
			// Schedule attack mode to end
			Invoke("endAttackMode", ATTACK_MODE_DURATION);
			// Schedule attack mode cooldown to end a.k.a. user is allowed to enter attack mode again
			//Invoke ("allowAttackMode", ATTACK_MODE_DURATION + ATTACK_MODE_COOLDOWN);
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

	// alias
	public Tile getTileAt() {
		return this.tileAt ();
	}

	public bool isAttacking() {
		return this.isInAttackMode;
	}
}
