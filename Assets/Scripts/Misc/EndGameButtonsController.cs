using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class EndGameButtonsController : MonoBehaviour {
	public AudioClip clip;

	//reload current scene
	public void Restart(){
		//ShowOptions options = new ShowOptions();
		//options.resultCallback = RestartGame;

		AudioManager.Get ().PlaySoundEffect (clip);
		//Advertisement.Show("video", options);

		//if advert code is uncommented, comment this line out
		RestartGame (ShowResult.Finished);
	}

	void RestartGame(ShowResult result){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	//go to main menu
	public void Menu(){
		AudioManager.Get ().PlaySoundEffect (clip);
		SceneManager.LoadScene (0);
	}
}
