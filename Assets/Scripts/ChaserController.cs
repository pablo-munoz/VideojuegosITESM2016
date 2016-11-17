using UnityEngine;
using System.Collections;

public class ChaserController : MonoBehaviour {

	//necesita hacer contacto con las paredes.

	public float treshold = 0.7f;
	public float speed = 0.7f;
	public Transform player;
	public float separation;
	public float rangeOfView;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		separation = Vector2.Distance(transform.position, player.position);

		if (separation < rangeOfView) {
			transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

		}
		Debug.Log (separation);
			
	}
}
