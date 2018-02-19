using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBoss : MonoBehaviour {
	public float m_fHealth;
	private float m_fStartHealth;
	private float m_fHealthBoost;	//when boss has been defeated multiple times, it gets harder
	public float targetX;
	public float inGameChangedHealth;
	public float setupFinishedZ;

	public float bulletTimer;
	public float bulletDelay;

	public GameObject m_goMissile;

	public Camera realCamera;
	private GameObject m_goHealthSprite;

	public AudioClip deathSound;
	public AudioClip entranceSound;
	public AudioClip missileSound;

	public GameObject deathParticles;

	public enum BOSS_STATE
	{
		SETUP = 0,
		ACTIVE,
		DEAD
	}
	private BOSS_STATE m_eState;

	// Use this for initialization
	protected virtual void Start () {
		m_fHealthBoost = 0f;
		m_eState = BOSS_STATE.SETUP;

		//if (inGameChangedHealth > 0)
		//	m_fHealth = inGameChangedHealth;

		if(bulletDelay <= 0)
			bulletDelay = 1.5f;
		
		if (targetX < 3 && targetX > -3)
			targetX = 3.1f;

		SetStartHealth ();

		AudioManager.Get().PlaySoundEffect (entranceSound);
	}

	public void SetStartHealth(){
		m_fStartHealth = m_fHealth;

		m_goHealthSprite = GameObject.Find ("health-bar");
	}

	public void UpdateHealth(){
		m_goHealthSprite.transform.localScale = new Vector3 (m_fHealth / m_fStartHealth * 100, m_goHealthSprite.transform.localScale.y, m_goHealthSprite.transform.localScale.z);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
		UpdateHealth ();
	}

	protected virtual void IncreaseDifficulty(int waveNum){

	}

	public BOSS_STATE GetState(){
		return m_eState;
	}

	public void SetState(BOSS_STATE s){
		m_eState = s;
	}

	public float GetHealth(){
		return m_fHealth;
	}

	public void Hurt(float dmg){
		if (m_eState == BOSS_STATE.ACTIVE) {
			if (m_fHealthBoost > 0)
				m_fHealthBoost -= dmg;
			else
				m_fHealth -= dmg;

			CheckDead ();
		}
	}

	public void CheckDead(){
		if (m_fHealth <= 0) {
			this.gameObject.GetComponent<EnemyDeath> ().Kill ();
			m_eState = BOSS_STATE.DEAD;
			Instantiate (deathParticles, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
			AudioManager.Get().PlaySoundEffect (deathSound);
		}
	}

	public void Shoot()
	{
		var ang = 120f;
		while (ang <= 210f) {
			var m = Instantiate (m_goMissile, transform.position, new Quaternion(0,0,0,1));
			m.transform.RotateAround (transform.position, new Vector3 (0, 1, 0), ang);
			m.transform.position += m.transform.forward * 0.2f;
			ang += 30f;
		}
		AudioManager.Get().PlaySoundEffect (missileSound);
	}

	public void ShootSingle(){
		Instantiate (m_goMissile, transform.position, new Quaternion(0,180f,0,1));
		AudioManager.Get().PlaySoundEffect (missileSound);
	}

	public void ShootSingleFromPos(Vector3 pos){
		Instantiate (m_goMissile, pos, new Quaternion(0,180f,0,1));
		AudioManager.Get().PlaySoundEffect (missileSound);
	}

	public void ShootAtRandomAngle(float min, float max){
		float ang = Random.Range (min, max);
		var m = Instantiate (m_goMissile, transform.position, new Quaternion(0,0,0,1));
		AudioManager.Get().PlaySoundEffect (missileSound);
		m.transform.RotateAround (transform.position, new Vector3 (0, 1, 0), ang);
	}

	public void ShootNearPlayer(float min, float max, Transform player){
		float ang = Random.Range (min, max);
		var m = Instantiate (m_goMissile, transform.position, new Quaternion(0,0,0,1));
		AudioManager.Get().PlaySoundEffect (missileSound);
		m.transform.LookAt (player);
		m.transform.RotateAround (transform.position, new Vector3 (0, 1, 0), ang);
	}

	public void HealthBoost(){
		float inc = Random.Range (m_fHealth/10, m_fHealth/3);
		m_fHealthBoost = inc;
	}


}
