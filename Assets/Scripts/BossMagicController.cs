using UnityEngine;
using System.Collections;

public class BossMagicController : MonoBehaviour {

	LevelController level;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("Level").GetComponent<LevelController> ();
		StartCoroutine ("destroyIfOutOfBounds");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator destroyIfOutOfBounds() {
		while (true) {
			if (this.transform.position.x < 0 ||
				this.transform.position.x > (GameConstants.levelSize * 2) ||
				this.transform.position.y < 0 ||
				this.transform.position.y > (GameConstants.levelSize * 2)) {
				Destroy (gameObject);
			}
			yield return new WaitForSeconds (2f);
		}
	}
}
