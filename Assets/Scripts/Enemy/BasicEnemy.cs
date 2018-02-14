using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
	public float m_fHealth;
	public int minWaveSpawn;
	public float targetZ;

	public bool canBuff;
	public bool isBuffed = false;
	public float healthBuffAmount = 0f;
	public GameObject buffParticles;

	// Use this for initialization
	void Start () {
		targetZ = -11f;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < -10f) {
			Destroy (this.gameObject);
		}
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, targetZ), 7f * Time.deltaTime);
	}

	public float GetHealth(){
		return m_fHealth;
	}

	public void Hurt(float dmg){
		if (isBuffed && canBuff) {
			if (healthBuffAmount > 0) {
				healthBuffAmount -= dmg;
			} else {
				isBuffed = false;
			}
		} else {
			m_fHealth -= dmg;
		}

		CheckDead ();
	}

	public void CheckDead(){
		if (m_fHealth <= 0 ) {
			if (!this.gameObject.GetComponent<EnemyDeath> ().killed) {
				this.gameObject.GetComponent<EnemyDeath> ().Kill ();
				Destroy (this.gameObject);
			}
		}
	}

	public void UpdateHealthBarSprite(){
		//health bar starts at 100 scale
	}

	public void DoBuff(float increase){
		if (!canBuff)
			return;

		isBuffed = true;
		//spawn particles on enemy to show buffed state
		buffParticles.SetActive(true);
	}

	public void IncreaseBaseHealth(float increase){
		m_fHealth += increase;
	}
}
