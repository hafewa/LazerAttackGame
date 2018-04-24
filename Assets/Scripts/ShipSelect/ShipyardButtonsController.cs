using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;

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

	public void WatchVideo(){
		ShowOptions options = new ShowOptions();
		options.resultCallback = VideoResult;
		shipyardControllerObj.GetComponent<ShipyardController> ().MenuBtn (false);
		Debug.Log (Advertisement.isInitialized);
		Advertisement.Show("video", options);
	}

	private void VideoResult(ShowResult result){
		AudioManager.Get ().PlaySoundEffect (successClip);
		string txt = "";
		if (result == ShowResult.Finished) {
			//give some score
			txt = "Congrats! You earned 50 points!";
			PlayerPrefsManager.Get ().AddScore (50);
		}else if(result == ShowResult.Skipped){
			//no score for you
			txt = "Watch the video without skipping if you want some points!";
		}else if(result == ShowResult.Failed){
			//error
			txt = "An error occurred. Try again later.";
		}

		shipyardControllerObj.GetComponent<ShipyardController> ().Popup (txt, true);
	}

	public void ClosePopup(){
		AudioManager.Get ().PlaySoundEffect (successClip);
		shipyardControllerObj.GetComponent<ShipyardController> ().Popup ("", false);
		shipyardControllerObj.GetComponent<ShipyardController> ().MenuBtn (true);
	}
}
