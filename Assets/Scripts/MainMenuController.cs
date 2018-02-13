using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//make sure the very first ship is unlocked by default
		if(PlayerPrefs.GetString ("SpaceShip1:Unlocked", "") == "")
			PlayerPrefs.SetString ("SpaceShip1:Unlocked", "true");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToScene(int i){
		SceneManager.LoadScene (i);
	}
}
