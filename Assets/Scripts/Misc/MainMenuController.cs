using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	// Use this for initialization
	void Start () {
		//make sure the very first ship is unlocked by default
		if(PlayerPrefsManager.Get().IsShipLocked ("SpaceShip1"))
			PlayerPrefsManager.Get().UnlockShip ("SpaceShip1");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToGame(AudioClip s){
		AudioManager.Get ().PlaySoundEffect (s);
		SceneManager.LoadScene (1);
	}

	public void GoToShips(AudioClip s){
		AudioManager.Get ().PlaySoundEffect (s);
		SceneManager.LoadScene (2);
	}
}
