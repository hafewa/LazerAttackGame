using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundScript : MonoBehaviour {
	private AudioSource source;
	private bool hasClip = false;
	private int playTimes = 0;
	private int played = 0;
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying) {
			playTimes--;
			source.Play ();//basically a manual loop
			if (playTimes == 0) {
				Destroy (this.gameObject);
			}
		}
	}

	public void SetAudioClip(AudioClip ac, float vol, int amount = 1){
		source = GetComponent<AudioSource> ();
		source.volume = vol;
		DontDestroyOnLoad (this.gameObject);
		hasClip = true;
		playTimes = amount;
		source.clip = ac;
		source.volume = vol;
		source.Play ();
	}
}
