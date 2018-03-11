using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFadeIn : MonoBehaviour {
	public float maxLight;
	public float fadeRate;

	// Use this for initialization
	void Start () {
		if (maxLight <= 0f)
			maxLight = 0.6f;
		if (fadeRate <= 0f)
			fadeRate = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
		if (WaveSpawner.Get ().startSpawningTimer > WaveSpawner.Get ().timeBeforeStart || 
			this.GetComponent<Light>().intensity >= maxLight) {
			this.GetComponent<Light> ().intensity = maxLight;
			Destroy (this);
		}

		this.GetComponent<Light> ().intensity += (fadeRate * Time.deltaTime);
	}
}
