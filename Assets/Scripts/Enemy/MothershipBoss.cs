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
					spawnChildDelay = Random.Range (0.8f, 3.7f);
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
					var c = Instantiate (children [Random.Range (0, children.Length - 1)], transform.position, transform.rotation);
					c.GetComponent<MothershipChild> ().SetMotherShipPos (this.gameObject);
					c.GetComponent<MothershipChild> ().SetPlayer (GameObject.FindGameObjectWithTag ("Player").transform);
					c.GetComponent<MothershipChild> ().SetTargetPos (leftSpawnTarget.position.x, leftSpawnTarget.position.y, leftSpawnTarget.position.z);

					var c2 = Instantiate (children [Random.Range (0, children.Length - 1)], transform.position, transform.rotation);
					c2.GetComponent<MothershipChild> ().SetMotherShipPos (this.gameObject);
					c2.GetComponent<MothershipChild> ().SetPlayer (GameObject.FindGameObjectWithTag ("Player").transform);
					c2.GetComponent<MothershipChild> ().SetTargetPos (rightSpawnTarget.position.x, rightSpawnTarget.position.y, rightSpawnTarget.position.z);
					spawnedChildren += 2;
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
}
