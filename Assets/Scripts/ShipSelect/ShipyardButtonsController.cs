using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipyardButtonsController : MonoBehaviour {
	public GameObject shipyardControllerObj;

	public void NextShip(){
		shipyardControllerObj.GetComponent<ShipyardController> ().ChangeShip (1);
	}

	public void PrevShip(){
		shipyardControllerObj.GetComponent<ShipyardController> ().ChangeShip (-1);
	}

	public void UpgradeCurrentShip(){
		shipyardControllerObj.GetComponent<ShipyardController> ().LevelUpShip ();
	}

	public void SetAsActiveShip(){
		shipyardControllerObj.GetComponent<ShipyardController> ().SetAsActiveShip ();
	}

	public void UnlockShip(){
		shipyardControllerObj.GetComponent<ShipyardController> ().UnlockShip ();
	}

	public void ToScene(int i){
		SceneManager.LoadScene (i);
	}
}
