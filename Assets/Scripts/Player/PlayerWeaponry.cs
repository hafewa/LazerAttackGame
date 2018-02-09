using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponry : MonoBehaviour {
	public int playerLevel;
	public int powerUp;
	public Transform weaponFirePos;

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
		m_fBulletDelay = 0.4f;
		powerUp = 0;
		fireAmount = 1;
		playerLevel = PlayerPrefs.GetInt ("Ship1_PlayerLevel", 0);
		GetCurrentWeapon ();
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
		if(Input.GetKeyDown(KeyCode.L)){
			WeaponBoostCollected ();
		}
	}

	public void WeaponBoostCollected(){
		powerUp++;
		//powerup basically tracks how many rockets to fire each time, if the player collects three powerups, the level increase
		//i.e. they get the next rocket, but it only spawns one
		//each rockets damage should basically be 3x that of the previous rocket
		GetCurrentWeapon();
	}

	private void GetCurrentWeapon(){
		if (playerLevel >= weaponry.Length)
			return;

		for (int i = 0; i < weaponry.Length; i++) {
			if (weaponry [i].levelReq == playerLevel + powerUp) {
				weapon = weaponry[i].obj;
				fireAmount = 1;
				return;
			}
		}

		if (fireAmount < 3)
			fireAmount++;
	}

	public int GetPlayerLevel(){
		return playerLevel;
	}
}
