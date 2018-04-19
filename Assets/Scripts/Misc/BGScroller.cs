using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {
	public float scrollSpeed;
	private Vector2 savedOffset;

	// Use this for initialization
	void Start () {
		savedOffset = this.gameObject.GetComponent<Renderer> ().material.GetTextureOffset ("_MainTex");
	}
	
	// Update is called once per frame
	void Update () {
		float tmpSpeed = scrollSpeed;

		if (WaveSpawner.Get () != null) {
			if (WaveSpawner.Get ().IsInSpeedMode ()) {
				tmpSpeed *= 5f;
			}
		}

		float x = Mathf.Repeat (Time.time * tmpSpeed, 1);
		Vector2 offset = new Vector2 (-x, savedOffset.y);
		GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
	}

	void OnDisable(){
		GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", savedOffset);
	}
}
