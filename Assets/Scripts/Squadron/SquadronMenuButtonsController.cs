using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SquadronMenuButtonsController : MonoBehaviour {
	public GameObject SquadronControllerObject;
	public GameObject panel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToScene(int ind){
		SceneManager.LoadScene (ind);
	}

	public void NextSquadronItem(){
		SquadronControllerObject.GetComponent<SquadronMenuController> ().IncOrDecShopItem (1);
	}

	public void PreviousSquadronItem(){
		SquadronControllerObject.GetComponent<SquadronMenuController> ().IncOrDecShopItem (-1);
	}

	public void AddCurrentItemToLeftBuddy(){
		panel.SetActive (false);
		SquadronControllerObject.GetComponent<SquadronMenuController> ().AddCurrentItemToLeftBuddy ();
	}

	public void AddCurrentItemToRightBuddy(){
		panel.SetActive (false);
		SquadronControllerObject.GetComponent<SquadronMenuController> ().AddCurrentItemToRightBuddy ();
	}
}
