using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public GameObject patrolMarker;
	public float treshold = 0.7f;
	public float speed = 0.7f;
	public float radius = 1f;

	private GameObject[] patrolMarkers;
	private int current;

	void Start () {
		current = 0;
		patrolMarkers = new GameObject[2];

		float x = transform.position.x;
		float y = transform.position.y;

		float angle = Random.value * 2 * Mathf.PI;
		float x1 = x + (radius * Mathf.Cos (angle));
		float y1 = y + (radius * Mathf.Sin (angle));
		float x2 = -x1;
		float y2 = -y1;

		patrolMarkers [0] = (GameObject) Instantiate (
			patrolMarker, new Vector3 (x1, y1, 0), Quaternion.identity);
		patrolMarkers [1] = (GameObject) Instantiate (
			patrolMarker, new Vector3 (x2, y2, 0), Quaternion.identity);
	}
		
	void Update () {
		float distance = Vector3.Distance (transform.position, patrolMarkers [current].transform.position);
		
		transform.position = Vector3.MoveTowards (
			transform.position, patrolMarkers[current].transform.position, speed * Time.deltaTime);

		if (distance < treshold) {
			current++; 
			current %= patrolMarkers.Length;
		}
	}
}
