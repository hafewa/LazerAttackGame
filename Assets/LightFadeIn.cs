using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFadeIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (WaveSpawner.Get ().startSpawningTimer > WaveSpawner.Get ().timeBeforeStart || 
			this.GetComponent<Light>().intensity >= 0.4f) {
			this.GetComponent<Light> ().intensity = 0.4f;
			Destroy (this);
		}

		this.GetComponent<Light> ().intensity += (0.2f * Time.deltaTime);
	}
}
