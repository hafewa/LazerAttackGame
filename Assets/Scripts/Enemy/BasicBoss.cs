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
	public float moveSpeed = 4f;

	public float bulletTimer;
	public float bulletDelay;

	public GameObject m_goMissile;

	public Camera realCamera;
	private GameObject m_goHealthSprite;

	public AudioClip deathSound;
	public AudioClip entranceSound;
	public int entranceSoundTimes;
	public AudioClip missileSound;

	public GameObject deathParticles;
	public GameObject m_goPlayer;

	public enum BOSS_STATE
	{
		SETUP = 0,
		ACTIVE,
		DEAD
	}
	private BOSS_STATE m_eState;

	//each grade of difficulty will change the bosses behaviour
	public enum DIFFICULTY{
		BASE = 0,
		EASY,
		MEDIUM,
		DIFFICULT,
		EXPERT
	}
	private DIFFICULTY m_eBossDifficulty = DIFFICULTY.BASE;

	// Use this for initialization
	protected virtual void Start () {
		if (GameObject.Find ("Player") == null) {
			Destroy (this.gameObject);
		}

		m_goPlayer = GameObject.Find ("Player");

		m_fHealthBoost = 0f;
		m_eState = BOSS_STATE.SETUP;

		//if (inGameChangedHealth > 0)
		//	m_fHealth = inGameChangedHealth;

		if(bulletDelay <= 0)
			bulletDelay = 1.5f;
		
		if (targetX < 3 && targetX > -3)
			targetX = 3.1f;

		SetStartHealth ();
		AudioManager.Get().PlaySoundEffect (entranceSound, 0.75f, entranceSoundTimes);

		SortDifficulty (WaveSpawner.Get().wavesDefeated);
	}

	protected virtual void SortDifficulty (int wavesDefeated){
		
	}

	public void SetDifficulty(DIFFICULTY diff){
		m_eBossDifficulty = diff;
	}

	public DIFFICULTY GetDifficulty(){
		return m_eBossDifficulty;
	}

	public void SetStartHealth(){
		m_fStartHealth = m_fHealth;

		m_goHealthSprite = GameObject.Find ("health-bar");
	}

	public void UpdateHealth(){
		if(m_goHealthSprite)
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
			WaveSpawner.Get ().BossDefeated (this.gameObject.name);
			m_eState = BOSS_STATE.DEAD;
			Instantiate (deathParticles, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
			AudioManager.Get().PlaySoundEffect (deathSound);
		}
	}

	public void Shoot(float rot = 30f, GameObject rocket = null)
	{
		var ang = 120f;
		var max = ang + (rot * 4f);
		while (ang <= max) {
			var m = Instantiate (rocket == null ? m_goMissile : rocket, transform.position, new Quaternion(0,0,0,1));
			m.transform.RotateAround (transform.position, new Vector3 (0, 1, 0), ang);
			m.transform.position += m.transform.forward * 0.2f;
			ang += rot;
		}
		AudioManager.Get().PlaySoundEffect (missileSound);
	}

	public void Shoot(Transform firePos, float rot = 30f, GameObject rocket = null)
	{
		var ang = 120f;
		var max = ang + (rot * 4f);
		while (ang <= max) {
			var m = Instantiate (rocket == null ? m_goMissile : rocket, firePos.position, new Quaternion(0,0,0,1));
			m.transform.RotateAround (transform.position, new Vector3 (0, 1, 0), ang);
			m.transform.position += m.transform.forward * 0.2f;
			ang += rot;
		}
		AudioManager.Get().PlaySoundEffect (missileSound);
	}

	public void ShootSingle(){
		Instantiate (m_goMissile, transform.position, new Quaternion(0,180f,0,1));
		AudioManager.Get().PlaySoundEffect (missileSound);
	}

	public void ShootSingleFromPos(Vector3 pos, GameObject rocket = null){
		Instantiate (rocket == null ? m_goMissile : rocket, pos, new Quaternion(0,180f,0,1));
		AudioManager.Get().PlaySoundEffect (missileSound);
	}

	public void ShootAtAngleFromPos(Vector3 pos, float angle, GameObject rocket = null){
		var r = Instantiate (rocket == null ? m_goMissile : rocket, pos, new Quaternion(0,0f,0,1));
		r.transform.RotateAround (transform.position, new Vector3 (0, 1, 0), angle);
		r.transform.position += r.transform.forward * 0.4f;
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
