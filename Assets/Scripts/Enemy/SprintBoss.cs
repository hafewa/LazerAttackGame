using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintBoss : BasicBoss 
{
	private float playerZ;
	private enum CHILD_STATE
	{
		SIDE2SIDE = 0,
		SPRINTING,
		REVERSING
	}
	private SHOOTSTATE m_eShootState;
	private enum SHOOTSTATE{
		ONE = 0,
		TWO
	}

	public float reverseToZ;
	private CHILD_STATE m_chldState;
	private float sprintTimer;
	public float sprintDelay;
	private Transform m_tPlayer;

	// Use this for initialization
	protected override void Start () {
		m_tPlayer = GameObject.Find("Player").transform;
		playerZ = m_tPlayer.position.z;

		if (sprintDelay <= 0)
			sprintDelay = Random.Range(1.5f, 4f);
		reverseToZ = 4f;
		sprintTimer = 0f;
		m_eShootState = SHOOTSTATE.ONE;

		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		switch (GetState ()) 
		{
		case BOSS_STATE.ACTIVE:
			if (m_chldState == CHILD_STATE.SIDE2SIDE) {
				if (transform.position.x > 3)
					targetX = -3.1f;
				else if (transform.position.x < -3f)
					targetX = 3.1f;

				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

				if (bulletTimer > bulletDelay) {
					switch (m_eShootState) {
					case SHOOTSTATE.ONE:
						Shoot (GameObject.Find("FirePos").transform, 30f);
						m_eShootState = SHOOTSTATE.TWO;
						break;
					case SHOOTSTATE.TWO:
					default:
						Shoot (GameObject.Find("FirePos").transform, 25f);
						m_eShootState = SHOOTSTATE.ONE;
						break;
					}

					bulletTimer = 0f;
					bulletDelay = Random.Range (1f, 1.6f);
				}

				if (sprintTimer > sprintDelay) {
					if (transform.position.x > m_tPlayer.position.x - 0.5f && transform.position.x < m_tPlayer.position.x + 0.5f) {
						m_chldState = CHILD_STATE.SPRINTING;
						sprintTimer = 0f;
						sprintDelay = Random.Range (1.5f, 4f);
					}
				}

				sprintTimer += Time.deltaTime;
				bulletTimer += Time.deltaTime;
			} else if (m_chldState == CHILD_STATE.SPRINTING) {
				if (transform.position.z <= playerZ)//reverse now if sprint finished
					m_chldState = CHILD_STATE.REVERSING;
				
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, playerZ), (moveSpeed * 2.25f) * Time.deltaTime);
			} else if (m_chldState == CHILD_STATE.REVERSING) {
				if (transform.position.z >= reverseToZ) {//side2side once reverse is finished
					transform.position = new Vector3(transform.position.x, transform.position.y, reverseToZ);
					m_chldState = CHILD_STATE.SIDE2SIDE;
				}

				transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, reverseToZ), 3.5f * Time.deltaTime);
			}
			break;
		case BOSS_STATE.SETUP:
			if (transform.position.z < setupFinishedZ)
				SetState(BOSS_STATE.ACTIVE);
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, 2.99f), 4f * Time.deltaTime);
			break;
		case BOSS_STATE.DEAD:
			break;
		}

		base.Update();
	}

	protected override void SortDifficulty(int wavesDefeated){
		int timesDefeated = WaveSpawner.Get ().BossDefeatedCount (this.gameObject.name);
		if (timesDefeated > 4) {
			SetDifficulty (DIFFICULTY.EXPERT);
			m_fHealth *= (3.5f + (wavesDefeated/10));
			moveSpeed = 5f;
		} else if (timesDefeated > 3) {
			SetDifficulty (DIFFICULTY.DIFFICULT);
			moveSpeed = 4.75f;
			m_fHealth *= (2.5f + (wavesDefeated/10));
		} else if (timesDefeated > 2) {
			m_fHealth *= (1.75f + (wavesDefeated/10));
			SetDifficulty (DIFFICULTY.MEDIUM);
			moveSpeed = 4.5f;
		} else if (timesDefeated >= 1) {
			m_fHealth *= (1.5f + (wavesDefeated/10));
			SetDifficulty (DIFFICULTY.EASY);
			moveSpeed = 4.2f;
		} else {
			SetDifficulty (DIFFICULTY.BASE);
			moveSpeed = 4f;
		}
	}
}
