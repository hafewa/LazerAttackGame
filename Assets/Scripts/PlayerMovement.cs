using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	private float m_fBulletTimer;
	private float m_fBulletDelay;

	public GameObject m_goBullet;
	public GameObject m_goBulletSpawnPoint;

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

	// Use this for initialization
	void Start () {
		m_fBulletTimer = 0f;
		m_fBulletDelay = 0.4f;
		Physics.gravity = new Vector3 (0, 0, -9.8f);
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

		m_fBulletTimer += Time.deltaTime;

		//if (Application.platform != RuntimePlatform.IPhonePlayer) {
			if (Input.GetAxis ("Fire1") > 0) {
				var ray = mainCamera.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 11)){
					Debug.DrawLine (transform.position, hit.point);
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (hit.point.x, transform.position.y, transform.position.z), 0.75f);
				}
			}
		//}

		if (m_fBulletTimer > m_fBulletDelay) {
			Instantiate(m_goBullet, m_goBulletSpawnPoint.transform.position, Quaternion.identity); 
			Instantiate(m_goBullet, new Vector3(m_goBulletSpawnPoint.transform.position.x + 0.5f, m_goBulletSpawnPoint.transform.position.y, m_goBulletSpawnPoint.transform.position.z - 0.5f), Quaternion.identity); 
			Instantiate(m_goBullet, new Vector3(m_goBulletSpawnPoint.transform.position.x - 0.5f, m_goBulletSpawnPoint.transform.position.y, m_goBulletSpawnPoint.transform.position.z - 0.5f), Quaternion.identity); 
			m_fBulletTimer = 0f;
		}

		if(Input.GetKeyDown(KeyCode.L)){
			PlayerPrefs.SetString("RightBuddy", "");
			PlayerPrefs.SetString("LeftBuddy", "");
		}
	}

	void OnTriggerEnter(Collider other){
		//only bosses can fire projectiles
		if (other.tag == "Enemy" || other.tag == "EnemyProjectile") {
			//kill
			Debug.Log("hit player");
			Destroy (this.gameObject);
			GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDead();
		}else if (other.tag == "Treasure") {
			//add to current game score
			GameObject.Find("GameManager").GetComponent<GameManager>().AddGameScore(other.GetComponent<Powerup>().GetValue());
			Destroy (other.gameObject);
		} else if (other.tag == "Boost") {
			//boost, could be increase damage
			//this.gameObject.GetComponent<PlayerCollectStuff>().AddBoost(other.gameObject);
		}
	}
}
