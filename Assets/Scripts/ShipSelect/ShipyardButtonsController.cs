using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipyardButtonsController : MonoBehaviour {
	public GameObject shipyardControllerObj;
	public AudioClip successClip;
	public AudioClip failClip;

	public void NextShip(){
		AudioManager.Get ().PlaySoundEffect (successClip);
		shipyardControllerObj.GetComponent<ShipyardController> ().ChangeShip (1);
	}

	public void PrevShip(){
		AudioManager.Get ().PlaySoundEffect (successClip);
		shipyardControllerObj.GetComponent<ShipyardController> ().ChangeShip (-1);
	}

	public void UpgradeCurrentShip(){
		shipyardControllerObj.GetComponent<ShipyardController> ().LevelUpShip (successClip, failClip);
	}

	public void SetAsActiveShip(){
		AudioManager.Get ().PlaySoundEffect (successClip);
		shipyardControllerObj.GetComponent<ShipyardController> ().SetAsActiveShip ();
	}

	public void UnlockShip(){
		shipyardControllerObj.GetComponent<ShipyardController> ().UnlockShip (successClip, failClip);
	}

	public void ToScene(int i){
		AudioManager.Get ().PlaySoundEffect (successClip);
		SceneManager.LoadScene (i);
	}
}
