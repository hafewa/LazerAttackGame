using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to handle movement of gesture instructions at the start of the game, only bother displaying it if they have played less than 3 games
public class SwipeGestureMovement : MonoBehaviour {
	private float targetX = 1.1f;

	// Use this for initialization
	void Start () {
//		if (PlayerPrefsManager.Get ().GetGamesPlayed() >= 3)
//			Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (WaveSpawner.Get ().startSpawningTimer < WaveSpawner.Get ().timeBeforeStart * 0.25f)
			return;
		
		if (WaveSpawner.Get ().startSpawningTimer < WaveSpawner.Get ().timeBeforeStart) {
			if (transform.position.x > 1) {
				targetX = -1.1f;
			} else if (transform.position.x < -1f) {
				targetX = 1.1f;
			}

			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), 1.2f * Time.deltaTime);
		} else {
			Destroy (this.gameObject);
		}
	}
}
