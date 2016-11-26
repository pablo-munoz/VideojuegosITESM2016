using UnityEngine;
using System.Collections;

public class BossCastingBehaviour : MonoBehaviour {
	BossController bc;

	// Use this for initialization
	void Start () {
		bc = GetComponent<BossController> ();
		StartCoroutine ("castMagic");
	}

	IEnumerator castMagic() {
		while (true) {
			// cast 8 magic balls in all directions
			for (int i = 0; i < 8; i++) {
				GameObject magic = Instantiate (bc.magicPrefab, transform.position, Quaternion.identity) as GameObject;
				Rigidbody2D rb = magic.GetComponent<Rigidbody2D> ();
				Vector2 velocity = new Vector2 (0, 0);
				velocity.x = Mathf.Cos (i * (Mathf.PI / 4));
				velocity.y = Mathf.Sin (i * (Mathf.PI / 4));
				rb.velocity = velocity;
			}
			yield return new WaitForSeconds (7f);
		}
	}
}
