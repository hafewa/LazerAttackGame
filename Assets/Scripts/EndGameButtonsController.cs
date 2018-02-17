using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameButtonsController : MonoBehaviour {
	//reload current scene
	public void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	//go to main menu
	public void Menu(){
		SceneManager.LoadScene (0);
	}
}
