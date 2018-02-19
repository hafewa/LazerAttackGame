using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundScript : MonoBehaviour {
	private AudioSource source;
	private bool hasClip = false;
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying && hasClip) {
			Destroy (this.gameObject);
		}
	}

	public void SetAudioClip(AudioClip ac, float vol = 1f){
		source = GetComponent<AudioSource> ();
		DontDestroyOnLoad (this.gameObject);
		hasClip = true;
		source.clip = ac;
		source.Play ();
	}
}
