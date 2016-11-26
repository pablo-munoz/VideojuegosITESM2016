using UnityEngine;
using System.Collections;

public class BossCastingBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("castMagic");
	}

	IEnumerable castMagic() {
		yield return new WaitForSeconds(3f);
	}
}
