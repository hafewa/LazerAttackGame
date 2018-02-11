using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipChild : MonoBehaviour {
	public GameObject motherShip;
	public Transform player;
	public GameObject rocket;
	private Vector3 targetPos;

	public enum MOTHERSHIPCHILD_STATE{
		SETUP = 0,
		ROTATING,
	}
	private MOTHERSHIPCHILD_STATE m_chldState;

	private float shootTimer;
	public float shootDelay;

	// Use this for initialization
	void Start () {
		//setup means player moves towards target pos, where they will then start attacking
		m_chldState = MOTHERSHIPCHILD_STATE.SETUP;
		shootTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!motherShip)
			Kill ();
		switch (m_chldState) {
		case MOTHERSHIPCHILD_STATE.SETUP:
			transform.position = Vector3.MoveTowards (transform.position, targetPos, 5f * Time.deltaTime);
			if (Vector3.Distance (transform.position, targetPos) < 0.03f)
				m_chldState = MOTHERSHIPCHILD_STATE.ROTATING;
			break;
		case MOTHERSHIPCHILD_STATE.ROTATING:
			if (motherShip)
				transform.RotateAround (motherShip.transform.position, new Vector3 (0, 1, 0), 1f);
			if(player)
				transform.LookAt (player.position);

			if (shootTimer > shootDelay) {
				shootTimer = 0f;
				shootDelay = Random.Range (1.2f, 2.5f);
				Shoot ();
			}

			shootTimer += Time.deltaTime;
			break;
		}

	}

	public void SetMotherShipPos(GameObject go){
		motherShip = go;
	}

	public void SetPlayer(Transform p){
		player = p;
	}

	public void SetTargetPos(float x, float y, float z){
		targetPos = new Vector3 (x, y, z);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "PlayerBullet") {
			Kill ();
		}
	}

	private void Kill(){
		if(motherShip)
			motherShip.GetComponent<MothershipBoss>().KillChild ();
		Destroy (this.gameObject);
	}

	private void Shoot(){
		var l = Instantiate (rocket, transform.position, transform.rotation);
		//l.transform.position += Vector3.forward;
		l.transform.localScale *= 0.75f;
	}
}
