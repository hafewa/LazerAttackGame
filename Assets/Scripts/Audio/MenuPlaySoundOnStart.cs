using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlaySoundOnStart : MonoBehaviour {
	public AudioClip clip;

	// Use this for initialization
	void Start () {
		AudioManager.Get ().PlayMusicLoop (clip);
	}
}
