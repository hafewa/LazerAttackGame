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
	public AudioClip deathSound;

	public float speed = 5f;
	private float immuneZ = 6f;

	public enum DIFFICULTY{
		BASE = 0,
		EASY,
		MEDIUM,
		DIFFICULT,
		EXPERT,
		MENTAL
	}
	private DIFFICULTY m_eDifficulty = DIFFICULTY.BASE;

	// Use this for initialization
	void Start () {
		targetZ = -11f;
		m_eDifficulty = DIFFICULTY.BASE;

		SortDifficulty ();
	}

	private void SortDifficulty(){
		int wavesDefeated = WaveSpawner.Get ().wavesDefeated;

		//chance of buff being applied
		int buffChance = 0;
		if (wavesDefeated > 20) {
			m_eDifficulty = DIFFICULTY.MENTAL;
			m_fHealth *= 3f;
			buffChance = 100;
			speed = 6f;
		} else if (wavesDefeated > 14) {
			m_eDifficulty = DIFFICULTY.EXPERT;
			m_fHealth *= 2f;
			buffChance = 90;
			speed = 5.8f;
		} else if (wavesDefeated > 8) {
			m_eDifficulty = DIFFICULTY.DIFFICULT;
			m_fHealth *= 1.75f;
			buffChance = 80;
			speed = 5.6f;
		} else if (wavesDefeated > 4) {
			m_eDifficulty = DIFFICULTY.MEDIUM;
			m_fHealth *= 1.5f;
			buffChance = 70;
			speed = 5.4f;
		} else if (wavesDefeated > 2) {
			m_eDifficulty = DIFFICULTY.EASY;
			m_fHealth *= 1.1f;
			buffChance = 50;
			speed = 5.2f;
		} else {
			m_eDifficulty = DIFFICULTY.BASE;
			canBuff = false;
			speed = 5f;
		}

		//do buff if a random seed is less than buff chance
		if (m_eDifficulty != DIFFICULTY.BASE) {
			canBuff = buffChance > 0;
			if (Random.Range (0, 100) <= buffChance)
				DoBuff ();

			IncreaseBaseHealth (wavesDefeated/10f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < -10f) {
			Destroy (this.gameObject);
		}

		//move towards player
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, targetZ), speed * Time.deltaTime);
	}

	public float GetHealth(){
		return m_fHealth;
	}

	public void Hurt(float dmg){
		if (transform.position.z > immuneZ)
			return;
		
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
				AudioManager.Get ().PlaySoundEffect (deathSound);
				this.gameObject.GetComponent<EnemyDeath> ().Kill ();
				Destroy (this.gameObject);
			}
		}
	}

	public void UpdateHealthBarSprite(){
		//health bar starts at 100 scale
	}

	public void DoBuff(){
		if (!canBuff)
			return;

		isBuffed = true;
		//spawn particles on enemy to show buffed state
		buffParticles.SetActive(true);
	}

	public void IncreaseBaseHealth(float increase){
		m_fHealth += increase;
	}

	public void IncreaseSpeed(float increase){
		speed += increase;
	}
}
