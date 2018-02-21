using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipBoss : BasicBoss {
	public GameObject[] children;
	private int spawnedChildren;

	private float spawnChildrenTimer = 0f;
	public float spawnChildDelay;

	public Transform rightSpawnTarget;
	public Transform leftSpawnTarget;
	public Transform centreSpawnTarget;
	public Transform leftSpawnTargetExpert;
	public Transform rightSpawnTargetExpert;

	public enum MOTHERSHIP_STATE{
		IDLE = 0,
		SPAWNING,
	}
	private MOTHERSHIP_STATE m_chldState;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		spawnedChildren = 0;
		m_chldState = MOTHERSHIP_STATE.IDLE;
	}
	
	// Update is called once per frame
	protected override void Update () {
		switch (GetState ()) {
		case BOSS_STATE.SETUP:
			if (transform.position.z < setupFinishedZ) {
				SetState (BOSS_STATE.ACTIVE);
				m_chldState = MOTHERSHIP_STATE.IDLE;
			}
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, 2.99f), 4f * Time.deltaTime);
			break;
		case BOSS_STATE.ACTIVE:
			switch (m_chldState) {
			case MOTHERSHIP_STATE.IDLE:
				if (spawnChildrenTimer > spawnChildDelay) {
					spawnChildDelay = Random.Range (0.8f, 2.0f);
					spawnChildrenTimer = 0f;
					m_chldState = MOTHERSHIP_STATE.SPAWNING;
				}

				if (spawnedChildren == 0)
					spawnChildrenTimer += Time.deltaTime;
				
				if (bulletTimer > bulletDelay) {
					bulletTimer = 0f;
					bulletDelay = Random.Range (1.5f, 3.5f);
					ShootSingleFromPos (GameObject.Find ("LeftLaserSpawn").transform.position);
					ShootSingleFromPos (GameObject.Find ("RightLaserSpawn").transform.position);
				}
				bulletTimer += Time.deltaTime;
				break;
			case MOTHERSHIP_STATE.SPAWNING:
				//spawn the children
				if (spawnedChildren == 0) {
					if (GameObject.FindGameObjectWithTag ("Player")) {
						Transform playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
						var c = Instantiate (children [Random.Range (0, children.Length - 1)], transform.position, transform.rotation);
						c.GetComponent<MothershipChild> ().SetMotherShipPos (this.gameObject);
						c.GetComponent<MothershipChild> ().SetPlayer (playerTransform);
						c.GetComponent<MothershipChild> ().SetTargetPos (leftSpawnTarget.position.x, leftSpawnTarget.position.y, leftSpawnTarget.position.z);

						var c2 = Instantiate (children [Random.Range (0, children.Length - 1)], transform.position, transform.rotation);
						c2.GetComponent<MothershipChild> ().SetMotherShipPos (this.gameObject);
						c2.GetComponent<MothershipChild> ().SetPlayer (playerTransform);
						c2.GetComponent<MothershipChild> ().SetTargetPos (rightSpawnTarget.position.x, rightSpawnTarget.position.y, rightSpawnTarget.position.z);
						spawnedChildren = 2;
						DIFFICULTY diff = GetDifficulty ();
						if (diff == DIFFICULTY.EASY || diff == DIFFICULTY.MEDIUM) {
							var c3 = Instantiate (children [Random.Range (0, children.Length - 1)], transform.position, transform.rotation);
							c3.GetComponent<MothershipChild> ().SetMotherShipPos (this.gameObject);
							c3.GetComponent<MothershipChild> ().SetPlayer (playerTransform);
							c3.GetComponent<MothershipChild> ().SetTargetPos (centreSpawnTarget.position.x, centreSpawnTarget.position.y, centreSpawnTarget.position.z);
							spawnedChildren = 3;
						} else if (diff == DIFFICULTY.EXPERT || diff == DIFFICULTY.DIFFICULT) {
							var c4 = Instantiate (children [Random.Range (0, children.Length - 1)], transform.position, transform.rotation);
							c4.GetComponent<MothershipChild> ().SetMotherShipPos (this.gameObject);
							c4.GetComponent<MothershipChild> ().SetPlayer (playerTransform);
							c4.GetComponent<MothershipChild> ().SetTargetPos (rightSpawnTargetExpert.position.x, rightSpawnTargetExpert.position.y, rightSpawnTargetExpert.position.z);

							var c5 = Instantiate (children [Random.Range (0, children.Length - 1)], transform.position, transform.rotation);
							c5.GetComponent<MothershipChild> ().SetMotherShipPos (this.gameObject);
							c5.GetComponent<MothershipChild> ().SetPlayer (playerTransform);
							c5.GetComponent<MothershipChild> ().SetTargetPos (leftSpawnTargetExpert.position.x, leftSpawnTargetExpert.position.y, leftSpawnTargetExpert.position.z);
							spawnedChildren = 4;
						}

					}
				}

				m_chldState = MOTHERSHIP_STATE.IDLE;
				break;
			}

			break;
		case BOSS_STATE.DEAD:
			
			break;
		}

		base.Update ();
	}

	public void KillChild(){
		spawnedChildren--;
	}

	protected override void SortDifficulty(int wavesDefeated){
		Debug.Log ("mothership Boss Health starts at: " + m_fHealth);
		int timesDefeated = WaveSpawner.Get ().BossDefeatedCount (this.gameObject.name);
		if (timesDefeated > 4) {
			SetDifficulty (DIFFICULTY.EXPERT);
			m_fHealth *= 2.5f;
			moveSpeed = 5.5f;
		} else if (timesDefeated > 3) {
			SetDifficulty (DIFFICULTY.DIFFICULT);
			moveSpeed = 5f;
			m_fHealth *= 2f;
		} else if (timesDefeated > 2) {
			m_fHealth *= 1.5f;
			SetDifficulty (DIFFICULTY.MEDIUM);
			moveSpeed = 4.5f;
		} else if (timesDefeated >= 1) {
			m_fHealth *= 1.1f;
			SetDifficulty (DIFFICULTY.EASY);
			moveSpeed = 4f;
		} else {
			SetDifficulty (DIFFICULTY.BASE);
			moveSpeed = 3f;
		}
		Debug.Log ("mothership Boss Health now at: " + m_fHealth);
	}
}
