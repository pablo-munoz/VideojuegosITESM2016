using UnityEngine;
using System.Collections;

// When the boss is idel it periodically summons a new skeleton every some seconds
public class BossIdleBehaviour : MonoBehaviour {

	LevelController level;
	BossController bc;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("Level").GetComponent<LevelController>();
		bc = GetComponent<BossController> ();
		StartCoroutine ("summonEnemy");
		Debug.Log ("Boss resting");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator summonEnemy() {
		while (true) {
			GameObject summon = level.addEnemy (Tile.getTileAtPosition ((int)transform.position.x, (int)transform.position.y));
			yield return new WaitForSeconds (15f);
		}
	}
}
