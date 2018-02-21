using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBoss : BasicBoss {
	public GameObject ufoBody;
	private Transform m_tPlayer;

	private enum CHILD_STATE
	{
		STILL = 0,
		SPIN
	}

	private CHILD_STATE m_chldState;
	private float spinTimer;
	public float spinDelay;
	public float spinForTime;
	private float oldBulletDelay;

	private float min, max;

	// Use this for initialization
	protected override void Start () {
		m_chldState = CHILD_STATE.STILL;
		base.Start();

		if (spinDelay <= 0)
			spinDelay = 8.2f;

		if (spinForTime <= 0)
			spinForTime = 4.2f;

		oldBulletDelay = bulletDelay;
		if(GameObject.FindGameObjectWithTag ("Player"))
			m_tPlayer = GameObject.FindGameObjectWithTag ("Player").transform;
		
		GetMinMaxShootRange (out min, out max);
	}
	
	// Update is called once per frame
	protected override void Update () {
		switch (GetState ()) {
		case BOSS_STATE.ACTIVE:
				//do specific fun
			switch (m_chldState) {
			case CHILD_STATE.STILL:
				if (spinTimer > spinDelay) {
					spinTimer = 0;
					m_chldState = CHILD_STATE.SPIN;
					bulletDelay = Random.Range (0.2f, 0.5f);
				}
				if (bulletTimer > bulletDelay) {
					ShootNearPlayer (min, max, m_tPlayer);
					bulletTimer = 0f;
				}

				spinTimer += Time.deltaTime;
				ufoBody.transform.RotateAround (ufoBody.transform.position, new Vector3 (0, 1, 0), 90f * Time.deltaTime);
				break;
			case CHILD_STATE.SPIN:
				if (spinTimer > spinForTime) {
					spinTimer = 0f;
					m_chldState = CHILD_STATE.STILL;
					bulletDelay = oldBulletDelay;
				}

				if (bulletTimer > bulletDelay) {
					
					ShootNearPlayer (min, max, m_tPlayer);
					bulletTimer = 0f;
				}

				spinTimer += Time.deltaTime;
				ufoBody.transform.RotateAround (ufoBody.transform.position, new Vector3 (0, 1, 0), 360f * Time.deltaTime);
				break;
			}

			bulletTimer += Time.deltaTime;
			break;
		case BOSS_STATE.SETUP:
			if (transform.position.z < setupFinishedZ)
				SetState (BOSS_STATE.ACTIVE);
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, 2.99f), 4f * Time.deltaTime);
			break;
		case BOSS_STATE.DEAD:
			break;
		}

		base.Update();
	}

	private void GetMinMaxShootRange(out float min, out float max){
		switch (GetDifficulty ()) {
		case DIFFICULTY.EASY:
			min = -20f;
			max = 20f;
			break;
		case DIFFICULTY.MEDIUM:
			min = -12f;
			max = 12f;
			break;
		case DIFFICULTY.DIFFICULT:
			min = -5f;
			max = 5f;
			break;
		case DIFFICULTY.EXPERT:
			min = -1f;
			max = 1f;
			break;
		case DIFFICULTY.BASE:
		default:
			min = -25f;
			max = 25f;
			break;
		}
	}

	protected override void SortDifficulty(int wavesDefeated){
		int timesDefeated = WaveSpawner.Get ().BossDefeatedCount (this.gameObject.name);
		if (timesDefeated > 4) {
			SetDifficulty (DIFFICULTY.EXPERT);
			m_fHealth *= 2.5f;
		} else if (timesDefeated > 3) {
			SetDifficulty (DIFFICULTY.DIFFICULT);
			m_fHealth *= 2f;
		} else if (timesDefeated > 2) {
			m_fHealth *= 1.5f;
			SetDifficulty (DIFFICULTY.MEDIUM);
		} else if (timesDefeated >= 1) {
			m_fHealth *= 1.1f;
			SetDifficulty (DIFFICULTY.EASY);
		} else {
			SetDifficulty (DIFFICULTY.BASE);
		}
	}
}
