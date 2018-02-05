using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
	public float m_fHealth;
	public int minWaveSpawn;
	public float targetZ;

	public bool canBuff;
	public float healthBuffAmount;
	public GameObject buffParticles;

	// Use this for initialization
	void Start () {
		targetZ = -11f;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < -10f)
			Destroy (this.gameObject);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, targetZ), 7f * Time.deltaTime);
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

	public void DoBuff(float increase){
		if (!canBuff)
			return;
		//increase health
		m_fHealth *= increase;
		//spawn particles on enemy to show buffed state
		Instantiate(buffParticles, transform);
		Debug.Log ("do buff:" + increase);
	}
}
