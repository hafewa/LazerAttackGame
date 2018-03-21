using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour {
	public GameObject asteroidSprite;
	private GameObject asteroidIcon;
	// Use this for initialization
	void Start () {
		asteroidIcon = Instantiate (asteroidSprite, new Vector3 (transform.position.x, 1f, 4.5f), Quaternion.Euler (new Vector3 (90f, 0, 0)));

		//get faster
		var wavesDefeated = WaveSpawner.Get ().wavesDefeated;
		this.GetComponent<GravityWithSpeed> ().speedMultiplier += (wavesDefeated * 0.2f);
	}
	
	// Update is called once per frame
	void OnDestroy(){
		Destroy (asteroidIcon);
	}
}
