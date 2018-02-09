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
	// Use this for initialization
	protected override void Start () {
		m_chldState = CHILD_STATE.STILL;
		base.Start();

		if (spinDelay <= 0)
			spinDelay = 8.2f;

		if (spinForTime <= 0)
			spinForTime = 4.2f;

		oldBulletDelay = bulletDelay;
		m_tPlayer = GameObject.FindGameObjectWithTag ("Player").transform;
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
					ShootNearPlayer (-10f, 10f, m_tPlayer);
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
					ShootNearPlayer (-25f, 25f, m_tPlayer);
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
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, 2.99f), 10f * Time.deltaTime);
			break;
		case BOSS_STATE.DEAD:
			break;
		}

		base.Update();
	}
}
