using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopButtonsController : MonoBehaviour {
	public GameObject ShopControllerObject;
	/*
	 * This script is attached to multiple buttons to be able to submit user requests like
	 * buy items
	 * back to main menu
	 * view next item in shop etc..
	 * 
	 * DO NOT USE START/UPDATE, they will get called by all buttons that use the script.
	 */

	// Use this for initialization
	void Start () {
		//do not use
	}
	
	// Update is called once per frame
	void Update () {
		//do not use
	}

	//gui functions
	public void IncShopIndex(){
		ShopControllerObject.GetComponent<ShopController>().ChangeIndex(1);
	}

	public void DecShopIndex(){
		ShopControllerObject.GetComponent<ShopController>().ChangeIndex(-1);
	}

	public void BuyItem(){
		ShopController.ShopItem si = ShopControllerObject.GetComponent<ShopController> ().GetCurrentShopItem ();
		int totalScore = PlayerPrefsManager.Get().TotalScore;

		if (totalScore < si.price)
			return;//only return for now, figure out a 'you are too poo' popup

		//open crate
		ShopControllerObject.GetComponent<ShopController>().BuyShopItem();
	}

	public void GoToScene(int i)
	{
		SceneManager.LoadScene (i);
	}

	//called To Shop Button on main shop canvas
	public void GoBackToNormalShop(){
		ShopControllerObject.GetComponent<ShopController> ().BackToMainShop ();
	}
}
