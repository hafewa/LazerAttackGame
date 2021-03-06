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

	// Use this for initialization
	void Start () {
		Physics.gravity = new Vector3 (0, 0, -9.8f);
		luckLevel = 0;
		luckTimer = 0f;
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
	}

	public int GetLuckLevel(){
		return luckLevel;
	}

	void OnTriggerEnter(Collider other){
		//only bosses can fire projectiles
		if (other.tag == "Enemy" || other.tag == "EnemyProjectile") {
			//kill
			Instantiate (deathParticles, transform.position, Quaternion.identity);
			this.gameObject.SetActive(false);

			AudioManager.Get ().PlaySoundEffect (deathSound);
			GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDead();
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
