using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AudioManager : MonoBehaviour {
	private static AudioManager instance;
	private AudioSource audioSource;	//the audiomanager itself has an audio source, so the game can play one looping theme/track over and over, across scenes if needed.
	public GameObject soundEffectObj;	//a temp obj used to play one off sound effects

	void Awake(){
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		audioSource = GetComponent<AudioSource> ();
		DontDestroyOnLoad(this.gameObject);
	}

	void Start(){
		if(Advertisement.isSupported){
			Advertisement.Initialize ("1706898", true);
		}
	}

	public static AudioManager Get(){
		return instance;
	}

	public void PlayMusicLoop(AudioClip ac){
		//if it's the same one, don't bother with this
		if (audioSource.clip && audioSource.clip.name == ac.name)
			return;
		
		audioSource.Stop ();

		audioSource.loop = true;
		audioSource.clip = ac;

		audioSource.Play ();
	}

	public void PlaySoundEffect(AudioClip ac, float vol = 0.75f, int amount = 1){
		var e = Instantiate (soundEffectObj, transform.position, transform.rotation);
		e.GetComponent<PlaySoundScript> ().SetAudioClip (ac, vol, amount);
	}
}
