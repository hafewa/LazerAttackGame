using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class EndGameButtonsController : MonoBehaviour {
	//reload current scene
	public void Restart(){
		ShowOptions options = new ShowOptions();
		options.resultCallback = RestartGame;

		Advertisement.Show("video", options);

	}

	void RestartGame(ShowResult result){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	//go to main menu
	public void Menu(){
		SceneManager.LoadScene (0);
	}
}
