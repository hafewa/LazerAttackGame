using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeGestureMovement : MonoBehaviour {
	private float targetX = 1.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (WaveSpawner.Get ().startSpawningTimer < WaveSpawner.Get ().timeBeforeStart * 0.25)
			return;
		
		if (WaveSpawner.Get ().startSpawningTimer < WaveSpawner.Get ().timeBeforeStart) {
			Debug.Log ("moving finger");
			if (transform.position.x > 1) {
				targetX = -1.1f;
			} else if (transform.position.x < -1f) {
				targetX = 1.1f;
			}

			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), 1.5f * Time.deltaTime);
		} else {
			Destroy (this.gameObject);
		}
	}
}
