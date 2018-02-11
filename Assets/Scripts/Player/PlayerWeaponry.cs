using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponry : MonoBehaviour {
	public int playerLevel;
	public int powerUp;
	public Transform weaponFirePos;
	private string shipName;

	//track rate of fire
	private float m_fBulletTimer;
	private float m_fBulletDelay;

	[System.Serializable]
	public class Weapon
	{
		public GameObject obj;
		public int levelReq;
	}
	public Weapon[] weaponry;
	private GameObject weapon;
	public int fireAmount;
	// Use this for initialization
	void Start () {
		m_fBulletTimer = 0f;
		m_fBulletDelay = 0.3f;
		powerUp = 0;
		fireAmount = 1;
		shipName = PlayerPrefs.GetString ("ShipName", "SpaceShip2");
		playerLevel = PlayerPrefs.GetInt (shipName + ":PlayerLevel", 0);
		GetInitialWeapon();

		//hide all other ship models attached to gameobject
		if (shipName != "SpaceShip1")
			GameObject.Find ("SpaceShip1").SetActive (false);
		if (shipName != "SpaceShip2")
			GameObject.Find ("SpaceShip2").SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (m_fBulletTimer > m_fBulletDelay) {
			if(fireAmount >= 1)
				Instantiate(weapon, weaponFirePos.position, Quaternion.identity); 
			if(fireAmount >= 2)
				Instantiate(weapon, new Vector3(weaponFirePos.position.x + 0.5f, 0f, weaponFirePos.position.z - 0.5f), Quaternion.identity); 
			if(fireAmount >= 3)
				Instantiate(weapon, new Vector3(weaponFirePos.position.x - 0.5f, 0f, weaponFirePos.position.z - 0.5f), Quaternion.identity); 
			m_fBulletTimer = 0f;
		}

		m_fBulletTimer += Time.deltaTime;
	}

	public void WeaponBoostCollected(){
		powerUp++;
		//powerup basically tracks how many rockets to fire each time, if the player collects three powerups, the level increase
		//i.e. they get the next rocket, but it only spawns one
		//each rockets damage should basically be 3x that of the previous rocket
		GetCurrentWeapon();
	}

	private void GetCurrentWeapon(){
		//if the player has maxed out, and the game doesn't offer any more rockets, just return and don't fiddle
		if (playerLevel >= weaponry.Length)
			return;
		
		for (int i = 0; i < weaponry.Length; i++) {
			//player collects powerups ingame that boost player level, 1 powerup = 1 level
			if (weaponry [i].levelReq == playerLevel + powerUp) {
				weapon = weaponry[i].obj;
				//when the
				fireAmount = 1;
				return;
			}
		}

		if (fireAmount < 3)
			fireAmount++;
	}

	//only to be used in Start() to find what rocket + how many rockets, the player should start with
	private void GetInitialWeapon(){
		//if player level < 0 somehow, give them the first rocket + give them player level 0
		if (playerLevel < 0) {
			weapon = weaponry [0].obj;
			fireAmount = 1;
			playerLevel = 0;
			PlayerPrefs.SetInt (shipName + ":PlayerLevel", playerLevel);
		}

		for (int i = 0; i < weaponry.Length; i++) {
			if (weaponry [i].levelReq == playerLevel) {
				weapon = weaponry [i].obj;
				fireAmount = 1;
				return;
			} else if (weaponry [i].levelReq + 1 == playerLevel) {
				weapon = weaponry [i].obj;
				fireAmount = 2;
				return;
			} else if (weaponry [i].levelReq + 2 == playerLevel) {
				weapon = weaponry [i].obj;
				fireAmount = 3;
				return;
			}

			//if it's got to this point, it means it can't find a valid rocket (player's level hasn't matched, just give them the best one
			if (i == weaponry.Length - 1) {
				weapon = weaponry [i].obj;
				fireAmount = 3;
				return;
			}
		}
	}

	public int GetPlayerLevel(){
		return playerLevel;
	}
}
