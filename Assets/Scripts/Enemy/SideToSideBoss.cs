using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideBoss : BasicBoss {
	public GameObject level2Rocket;
	public GameObject level3Rocket;
	public GameObject level4Rocket;
	public GameObject level5Rocket;

	private GameObject currRocket;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		switch (GetState()) {
		case BOSS_STATE.SETUP:
			if (transform.position.z < setupFinishedZ) {
				SetState (BOSS_STATE.ACTIVE);
			}
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, 2.99f), 4f * Time.deltaTime);
			break;
		case BOSS_STATE.ACTIVE:
			if (transform.position.x > 3) {
				targetX = -3.1f;
			} else if (transform.position.x < -3f) {
				targetX = 3.1f;
			}
			
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
			if (bulletTimer > bulletDelay) {
				Shoot (currRocket);
				bulletTimer = 0f;
				bulletDelay = Random.Range (1.2f, 1.5f);
			}

			bulletTimer += Time.deltaTime;
			break;
		case BOSS_STATE.DEAD:
			break;
		}

		base.Update ();
	}

	protected override void IncreaseDifficulty(int waveNum){

	}

	protected override void SortDifficulty(int wavesDefeated){
		int timesDefeated = WaveSpawner.Get ().BossDefeatedCount (this.gameObject.name);
		if (timesDefeated > 4) {
			SetDifficulty (DIFFICULTY.EXPERT);
			m_fHealth *= (3.5f + (wavesDefeated/10));
			moveSpeed = 5.5f;
			currRocket = level5Rocket;
		} else if (timesDefeated > 3) {
			SetDifficulty (DIFFICULTY.DIFFICULT);
			moveSpeed = 5f;
			m_fHealth *= (2.5f + (wavesDefeated/10));
			currRocket = level4Rocket;
		} else if (timesDefeated > 2) {
			m_fHealth *= (1.75f + (wavesDefeated/10));
			SetDifficulty (DIFFICULTY.MEDIUM);
			moveSpeed = 4.5f;
			currRocket = level3Rocket;
		} else if (timesDefeated >= 1) {
			m_fHealth *= (1.5f + (wavesDefeated/10));
			SetDifficulty (DIFFICULTY.EASY);
			moveSpeed = 4f;
			currRocket = level2Rocket;
		} else {
			SetDifficulty (DIFFICULTY.BASE);
			moveSpeed = 3f;
		}
	}
}
