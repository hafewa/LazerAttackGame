using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipyardController : MonoBehaviour {
	private float constant = 0.4f;
	public int currentLevel;
	private int currShipIndex;
	public Transform m_tDisplayPos;
	private GameObject currentShipObj;

	[System.Serializable]
	public class Ship{
		public string name;
		public GameObject obj;
		public float constant; //for xp/levelling (more expensive for better ships)
		public int unlockPrice;
		public Vector3 appliedRotation;
	}
	public Ship[] allShips;

	private int coinsForNextLevel;
	private int currentPoints;

	//buttons
	public Button upgradeBtn;
	public Button setActiveBtn;
	public Button unlockBtn;
	public Text scoreText;
	public Text shipLevelText;
	public Text UpgradeBtnText;
	public Text UnlockBtnText;

	// Use this for initialization
	void Start () {
		scoreText.text = PlayerPrefs.GetInt ("points", 0) + "";
		if (PlayerPrefs.GetString ("ShipName", "") == "")
			PlayerPrefs.SetString ("ShipName", "SpaceShip1");
		if (PlayerPrefs.GetString ("SpaceShip1:Unlocked", "") == "")
			PlayerPrefs.SetString ("SpaceShip1:Unlocked", "true");
		string shipname = PlayerPrefs.GetString ("ShipName");
		currShipIndex = -1;
		//get currently selected ship
		for (int i = 0; i < allShips.Length; i++) {
			if (allShips [i].name == shipname) {
				currShipIndex = i;
			}	
		}
		if (currShipIndex < 0)
			currShipIndex = 0;
		currentPoints = PlayerPrefs.GetInt ("points", 0);

		UpdateShip ();
	}

	void Update(){
//		if (Input.GetKeyDown(KeyCode.L)) {
//			PlayerPrefs.SetInt ("SpaceShip1:PlayerLevel", 0);
//		}
	}
	
	private void UpdateShip(){
		if(currentShipObj != null)
			Destroy (currentShipObj);
		currentShipObj = Instantiate (allShips [currShipIndex].obj, m_tDisplayPos.position, Quaternion.Euler(allShips[currShipIndex].appliedRotation));
		currentShipObj.transform.localScale = new Vector3 (1f, 1f, 1f);

		currentLevel = PlayerPrefs.GetInt (allShips [currShipIndex].name + ":PlayerLevel", 0);
		shipLevelText.text = currentLevel + "";
		scoreText.text = currentPoints + "";


		int nextLvl = currentLevel + 1;

		int xpRequired = Mathf.FloorToInt((nextLvl * nextLvl) / (allShips[currShipIndex].constant*allShips[currShipIndex].constant) - 
			(currentLevel * currentLevel) / (allShips[currShipIndex].constant*allShips[currShipIndex].constant));

		UpgradeBtnText.text = "Upgrade for " + xpRequired + " points";
		UnlockBtnText.text = "Unlock for " + allShips [currShipIndex].unlockPrice + " points";
		//buy this ship button
		if (PlayerPrefs.GetString (allShips [currShipIndex].name + ":Unlocked", "") == "") {
			//show 'unlock' button, they are yet to buy it
			unlockBtn.gameObject.SetActive(true);
			setActiveBtn.gameObject.SetActive(false);
			upgradeBtn.gameObject.SetActive(false);
		} else {
			//hide 'unlock' button, already bought it
			unlockBtn.gameObject.SetActive(false);
			//set to active ship button
			if (PlayerPrefs.GetString ("ShipName", "") == allShips [currShipIndex].name) {
				//hide/deactivate 'choose' button
				setActiveBtn.gameObject.SetActive(false);
			} else {
				//show it
				setActiveBtn.gameObject.SetActive(true);
			}

			//upgrade this ship button
			if (CanAffordUpgrade ()) {
				//activate the button and allow click
				upgradeBtn.gameObject.SetActive(true);
			} else {
				//grey out the button
				upgradeBtn.gameObject.SetActive(false);
			}
		}
	}

	public void ChangeShip(int i){
		currShipIndex += i;

		if (currShipIndex < 0)
			currShipIndex = allShips.Length - 1;

		if (currShipIndex >= allShips.Length)
			currShipIndex = 0;

		UpdateShip ();
	}

	public void SetAsActiveShip(){
		//set current ship to active ship
		PlayerPrefs.SetString("ShipName", allShips[currShipIndex].name);
		UpdateShip ();
	}

	public void LevelUpShip(){
		int currShipLevel = PlayerPrefs.GetInt (allShips[currShipIndex].name + ":PlayerLevel", 0);
		int nextLvl = currShipLevel + 1;

		int xpRequired = Mathf.FloorToInt((nextLvl * nextLvl) / (allShips[currShipIndex].constant*allShips[currShipIndex].constant) - 
			(currShipLevel * currShipLevel) / (allShips[currShipIndex].constant*allShips[currShipIndex].constant));
		
		currentPoints = PlayerPrefs.GetInt ("points", 0);
		if (currentPoints > xpRequired) {
			PlayerPrefs.SetInt ("points", PlayerPrefs.GetInt ("points", 0) - xpRequired);
			currentPoints = PlayerPrefs.GetInt ("points", 0);
			//do upgrade to ship
			PlayerPrefs.SetInt(allShips[currShipIndex].name + ":PlayerLevel", nextLvl);

			UpdateShip ();
		}
	}

	public void UnlockShip(){
		if (currentPoints > allShips [currShipIndex].unlockPrice) {
			PlayerPrefs.SetString (allShips [currShipIndex].name + ":Unlocked", "true");
			PlayerPrefs.SetInt("points", currentPoints - allShips[currShipIndex].unlockPrice);

			UpdateShip ();
		}
	}

	public bool CanAffordUpgrade(){
		Ship ship = allShips [currShipIndex];

		int currShipLevel = PlayerPrefs.GetInt (ship.name + ":PlayerLevel", 0);
		int nextLvl = currShipLevel + 1;

		int xpDiff = Mathf.FloorToInt((nextLvl * nextLvl) / (constant*constant) - (currShipLevel * currShipLevel) / (constant*constant));

		//get real cost of next level up
		if (PlayerPrefs.GetInt ("points", 0) > xpDiff)
			return true;

		return false;
	}
}
