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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator summonEnemy() {
		while (true) {
			GameObject summon = level.addEnemy (bc.getTileAt());
			summon.GetComponent<EnemyController> ().beginPatrolling (level.levelSize);
			yield return new WaitForSeconds (15f);
		}
	}
}
