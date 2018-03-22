﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public GameObject m_goBullet;
	public GameObject m_goBulletSpawnPoint;
	private int luckLevel;
	private float luckTimer;
	public Camera mainCamera;
	[System.Serializable]
	public class Buddy{
		public string name;
		public GameObject buddy;
		public string codename;
	}
	public List<Buddy> m_lBuddies;

	public Transform m_tRightBuddySpawnPos;
	public Transform m_tLeftBuddySpawnPos;

	public AudioClip deathSound;
	public AudioClip treasureSound;
	public GameObject deathParticles;

	private float extraLifeUsedTimer;
	private float extraLifeUsedDelay;
	private float extraLifeUsedFlash;
	private bool extraLifeJustUsed;

	public GameObject currShipObj;

	// Use this for initialization
	void Start () {
		Physics.gravity = new Vector3 (0, 0, -9.8f);
		luckLevel = 0;
		luckTimer = 0f;
		extraLifeUsedTimer = 0f;
		extraLifeUsedDelay = 2f;
		extraLifeUsedFlash = extraLifeUsedTimer + 0.2f;
		extraLifeJustUsed = false;
//		string lBuddy = PlayerPrefs.GetString ("LeftBuddy", "");
//		string rBuddy = PlayerPrefs.GetString ("RightBuddy", "");
//
//		if (lBuddy == "" && rBuddy == "")
//			return;
//		float buddiesSpawned = 0;
//		foreach (Buddy b in m_lBuddies) {
//			if (buddiesSpawned >= 2)
//				break;
//			
//			if (b.codename == lBuddy) {
//				Instantiate (b.buddy, m_tLeftBuddySpawnPos);
//				buddiesSpawned++;
//			}
//
//			if (b.codename == rBuddy) {
//				Instantiate (b.buddy, m_tRightBuddySpawnPos);
//				buddiesSpawned++;
//			}
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (WaveSpawner.Get ().startSpawningTimer < WaveSpawner.Get ().timeBeforeStart)
			return;
		
		//if (Application.platform != RuntimePlatform.IPhonePlayer) {
			if (Input.GetAxis ("Fire1") > 0) {
				var ray = mainCamera.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 11)){
					Debug.DrawLine (transform.position, hit.point);
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (hit.point.x, transform.position.y, transform.position.z), 150f * Time.deltaTime);
				}
			}
		//}

		//decay luck level if there is any
		if (luckLevel > 0) {
			if (luckTimer > 30f) {
				luckLevel--;
				luckTimer = 0f;
			}
			luckTimer += Time.deltaTime;
		}

		if (extraLifeJustUsed) {
			if (extraLifeUsedTimer > extraLifeUsedDelay) {
				extraLifeJustUsed = false;
				extraLifeUsedTimer = 0f;
				this.gameObject.SetActive (true);
			}

			if (extraLifeUsedTimer > extraLifeUsedFlash) {
				currShipObj.SetActive(!currShipObj.activeInHierarchy);
				extraLifeUsedFlash = extraLifeUsedTimer + 0.1f;
				
			}

			extraLifeUsedTimer += Time.deltaTime;
		}
	}

	public int GetLuckLevel(){
		return luckLevel;
	}

	void OnTriggerEnter(Collider other){
		//only bosses can fire projectiles
		if (other.tag == "Enemy" || other.tag == "EnemyProjectile") {
			if (extraLifeJustUsed) {
				Destroy (other.gameObject);
				return;
			}
			//kill
			Instantiate (deathParticles, transform.position, Quaternion.identity);
			if (gameObject.GetComponent<ExtraLife> () != null) {
				if(gameObject.GetComponent<ExtraLife>().UseExtraLife()){
					extraLifeJustUsed = true;
					Destroy (other.gameObject);
					return;
				}
			}
			this.gameObject.SetActive (false);
			AudioManager.Get ().PlaySoundEffect (deathSound);
			GameObject.Find ("GameManager").GetComponent<GameManager> ().PlayerDead (gameObject.GetComponent<DoublePoints>() == null ? 1f : gameObject.GetComponent<DoublePoints>().boost);
		}else if (other.tag == "Treasure") {
			//add to current game score
			if (!other.GetComponent<Pickup> ().used) {
				AudioManager.Get ().PlaySoundEffect (treasureSound, 0.2f);
				other.GetComponent<Pickup> ().used = true;
				switch (other.GetComponent<Pickup> ().GetPickupType ()) {
				case Pickup.PICKUPTYPE.POWERUP:
					ActivatePowerup (other.gameObject);
					break;
				case Pickup.PICKUPTYPE.TREASURE:
					GameObject.Find ("GameManager").GetComponent<GameManager> ().AddGameScore (other.GetComponent<Treasure> ().GetValue ());
					break;
				}
			}
			Destroy (other.gameObject);
		}
	}

	public void ActivatePowerup(GameObject p){
		switch (p.GetComponent<Powerup> ().m_eType) {
		case Powerup.POWERUPTYPE.WEAPON:
			this.gameObject.GetComponent<PlayerWeaponry> ().WeaponBoostCollected ();
			break;
		case Powerup.POWERUPTYPE.LUCK:
			//Chances of good drops from all types of enemies increases
			break;
		case Powerup.POWERUPTYPE.MAGNET:
			//dropped treasure moves slightly towards player
			break;
		default:
			//nah
			break;
		}
	}
}
