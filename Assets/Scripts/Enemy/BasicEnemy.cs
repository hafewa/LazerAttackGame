﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
	public float m_fHealth;
	public int minWaveSpawn;
	public float targetZ;

	// Use this for initialization
	void Start () {
		targetZ = -11f;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < -10f)
			Destroy (this.gameObject);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, targetZ), 0.1f);
	}

	public float GetHealth(){
		return m_fHealth;
	}

	public void Hurt(float dmg){
		m_fHealth -= dmg;

		CheckDead ();
	}

	public void CheckDead(){
		if (m_fHealth <= 0) {
			this.gameObject.GetComponent<EnemyDeath> ().Kill ();
			Destroy (this.gameObject);
		}
	}

	public void UpdateHealthBarSprite(){
		//health bar starts at 100 scale
	}
}
