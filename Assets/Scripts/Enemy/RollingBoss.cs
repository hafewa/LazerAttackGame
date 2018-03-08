using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBoss : BasicBoss {
	public GameObject laser;
	public Transform shootPosOne;
	public Transform shootPosTwo;
	public Transform RotateAroundPos;

	private float targetRot = 180f;
	public enum CHILDSTATE
	{
		SIDE2SIDE = 0,
		ROLLING,	//burst of speed in one direction, rolls as it does it - ~maybe~ also fires more rapidly in this mode
		BEAM	//at a standstill, shooting a burst/beam of rockets/laser
	}
	private CHILDSTATE m_eNextState;
	private CHILDSTATE m_eChildState;
	private float nextStateTimer;
	private float nextStateDelay;

	// Use this for initialization
	protected override void  Start () {
		m_eChildState = CHILDSTATE.SIDE2SIDE;
		m_eNextState = CHILDSTATE.ROLLING;
		nextStateTimer = 0f;
		nextStateDelay = Random.Range (1f, 6f);
		transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void  Update () {
		switch (GetState ()) {
		case BOSS_STATE.ACTIVE:
			switch (m_eChildState) {
			case CHILDSTATE.SIDE2SIDE:
				if (transform.position.x > 3) {
					targetX = -3.1f;
					if (nextStateTimer > nextStateDelay) {
						NextState ();
						bulletDelay = Random.Range(0.15f, 0.25f);
					}
				} else if (transform.position.x < -3f) {
					targetX = 3.1f;
					if (nextStateTimer > nextStateDelay) {
						NextState ();
						bulletDelay = Random.Range(0.15f, 0.25f);
					}
				}
				Vector3 newDir = new Vector3 (transform.position.x, transform.position.y, -3f) - transform.position;
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards (transform.forward, newDir, (1f * Mathf.Deg2Rad) * Time.deltaTime, 0f));

				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
				if (bulletTimer > bulletDelay) {
					
					ShootSingleFromPos (shootPosOne.position);
					ShootSingleFromPos (shootPosTwo.position);
					bulletTimer = 0f;
					bulletDelay = Random.Range (1.2f, 1.5f);
				}

				bulletTimer += Time.deltaTime;
				break;
//			case CHILDSTATE.BEAM:
//				//fire beam and stay still
//				if (nextStateTimer > nextStateDelay) {
//					m_eNextState = CHILDSTATE.SIDE2SIDE;
//					NextState ();
//				}
//				break;
			case CHILDSTATE.ROLLING:
				if (transform.position.x > 3) {
					targetX = -3.1f;
					m_eNextState = CHILDSTATE.SIDE2SIDE;
					transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);//the rolling moves in y
					NextState ();
				} else if (transform.position.x < -3f) {
					targetX = 3.1f;
					m_eNextState = CHILDSTATE.SIDE2SIDE;
					transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);
					NextState ();
				}

				transform.RotateAround (RotateAroundPos.position, new Vector3 (0, 0, 1), 450f * Time.deltaTime);
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), (moveSpeed * 2.5f) * Time.deltaTime);
				if (bulletTimer > bulletDelay) {
					Debug.Log (bulletDelay);
					ShootSingleFromPos (shootPosOne.position, laser);
					ShootSingleFromPos (shootPosTwo.position, laser);
					bulletTimer = 0f;
					bulletDelay = Random.Range (0.15f, 0.28f);
				}

				bulletTimer += Time.deltaTime;
				break;
			}

			nextStateTimer += Time.deltaTime;
			break;
		case BOSS_STATE.SETUP:
			if (transform.position.z < setupFinishedZ) {
				SetState (BOSS_STATE.ACTIVE);
			}
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, 2.99f), 4f * Time.deltaTime);
			break;
		}

		base.Update ();
	}

	public void NextState(){
		m_eChildState = m_eNextState;
		bulletTimer = nextStateTimer = 0f;
		nextStateDelay = Random.Range (3f, 6f);
		m_eNextState = CHILDSTATE.ROLLING;
	}
}
