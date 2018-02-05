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
			if (transform.position.z < setupFinishedZ)
				SetState(BOSS_STATE.ACTIVE);
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, transform.position.y, 2.99f), 0.1f);
			break;
		case BOSS_STATE.ACTIVE:
			if (transform.position.x > 3)
				targetX = -3.1f;
			else if (transform.position.x < -3f)
				targetX = 3.1f;

			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), 4f * Time.deltaTime);
			if (bulletTimer > bulletDelay) {
				Shoot ();
				bulletTimer = 0f;
			}

			bulletTimer += Time.deltaTime;
			break;
		case BOSS_STATE.DEAD:
			break;
		}

		base.Update ();
	}
}
