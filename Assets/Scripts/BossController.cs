using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour {

	private LevelController level;
	private PlayerController player;
	private AutomataNode current;
	private Symbol rest, cast, teleport;
	private int lastHitPoints;
	public int hitPoints;
	private MonoBehaviour currentBehaviour;
	private AutomataNode resting, casting, teleporting;
	private new Renderer renderer;
	private Material painMaterial = null;
	private Material defaultMaterial = null;
	private bool isInvulnerable = false;

	public GameObject magicPrefab;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("Level").GetComponent<LevelController> ();
		this.renderer = GetComponent<Renderer> ();
		this.defaultMaterial = renderer.material;
		this.painMaterial = Resources.Load ("Materials/Pain", typeof(Material)) as Material;
		player = GameObject.Find ("Player").GetComponent<PlayerController>();
		
		resting = new AutomataNode ("idle", typeof(BossIdleBehaviour));
		casting = new AutomataNode ("casting", typeof(BossCastingBehaviour));
		teleporting = new AutomataNode ("teleporting", typeof(BossTeleportBehaviour));

		cast = new Symbol ("cast");
		rest = new Symbol ("rest");
		teleport = new Symbol ("teleport");

		resting.AddTransition (cast, casting);

		casting.AddTransition (rest, resting);
		casting.AddTransition (teleport, teleporting);

		teleporting.AddTransition (rest, resting);

		current = resting;
		currentBehaviour = (MonoBehaviour)gameObject.AddComponent (current.Behaviour);

		StartCoroutine ("checkForPlayerNearby");

		lastHitPoints = hitPoints;
	}

	// Update is called once per frame
	void Update () {
		if (current == casting && lastHitPoints == 21 || lastHitPoints == 11) {
			lastHitPoints = hitPoints;
			this.ChangeState (teleport);
			Invoke ("returnToIdle", 1f);
		} else if (this.hitPoints == 0) {
			level.addKey (this.getTileAt ());
			Destroy (gameObject);
		} 
	}

	private void returnToIdle() {
		this.ChangeState (rest);
	}

	private void ChangeState(Symbol symbol){
		current = current.ApplySymbol (symbol);
		Destroy (currentBehaviour);
		currentBehaviour = (MonoBehaviour)gameObject.AddComponent (current.Behaviour);
	}

	private bool playerIsNearby() {
		List<Tile> pathToPlayer = Pathfinding.AStar (this.getTileAt (), player.getTileAt ());
		if (pathToPlayer != null && pathToPlayer.Count <= 8) {
			return true;
		}
		return false;
	}

	public Tile getTileAt() {
		return Tile.getTileAtPosition ((int)transform.position.x, (int)transform.position.y);
	}

	IEnumerator checkForPlayerNearby() {
		while (true) {
			bool playerNearby = this.playerIsNearby ();
			if (current == resting && playerNearby) {
				this.ChangeState (cast);
			} else if (current == casting && !playerNearby) {
				this.ChangeState (rest);
			}
			yield return new WaitForSeconds (4f);
		}
	}

	private void resetMaterial() {
		renderer.material = this.defaultMaterial;
	}

	private void becomeInvulnerable() {
		this.isInvulnerable = false;
	}

	public void getAttacked() {
		if (!isInvulnerable) {
			lastHitPoints = this.hitPoints;
			this.hitPoints--;
			isInvulnerable = true;
			renderer.material = painMaterial;
			Invoke ("becomeInvulnerable", 0.5f);
			this.Invoke ("resetMaterial", 0.5f);
		}
	}
}
