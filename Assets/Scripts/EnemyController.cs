using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public GameObject patrolMarker;
	public float treshold = 0.7f;
	public float speed = 0.7f;
	public float radius = 1f;
	public int patrolDistance = 6;

	private GameObject[] patrolMarkers;
	private int current;
	private int direction = 1;

	void Start () {
		current = 0;
		patrolMarkers = new GameObject[patrolDistance];

		float x = transform.position.x;
		float y = transform.position.y;

		List<BlueprintPosition> patrolPath = new List<BlueprintPosition> ();
		Toolbox.singleton.findTraversablePath ((int) x, (int) y, patrolDistance, patrolPath, null);

		int i = 0;
		foreach (BlueprintPosition patrolPos in patrolPath) {
			patrolMarkers [i] = (GameObject) Instantiate (
				patrolMarker, new Vector3 (patrolPos.x, patrolPos.y, 0), Quaternion.identity);
			i++;
		}

	}
		
	void Update () {
		float distance = Vector3.Distance (transform.position, patrolMarkers [current].transform.position);
		
		transform.position = Vector3.MoveTowards (
			transform.position, patrolMarkers[current].transform.position, speed * Time.deltaTime);

		if (current == 0) {
			direction = 1;
		} else if (current == patrolMarkers.Length - 1) {
			direction = -1;
		}

		if (distance < treshold) {
			current += direction; 
		}
	}
}
