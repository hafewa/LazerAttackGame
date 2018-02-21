using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideBoss : BasicBoss {
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
			if (GetDifficulty () == DIFFICULTY.EXPERT)
				transform.LookAt (base.m_goPlayer.transform);
			
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
			if (bulletTimer > bulletDelay) {
				Shoot ();
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
	}
}
