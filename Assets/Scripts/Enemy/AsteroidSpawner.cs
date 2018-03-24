using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
	public List<GameObject> asteroids;
	public Transform asteroidSpawnPoint;

	public enum SPAWNERSTATE
	{
		INACTIVE = 0,
		WAITING,
		SPAWN_INDIVIDUALS,	//individual asteroids in a queue, spawn between = high
		SPAWN_CLUSTER	//a bunch in quick succession, spawn between = low
	}

	public enum ASTEROID_SPAWNER_DIFFICULTY
	{
		BASE = 0,
		EASY,
		MEDIUM,
		HARD,
		EXPERT
	}

	private ASTEROID_SPAWNER_DIFFICULTY m_eDifficulty;
	private SPAWNERSTATE m_enState;
	private float m_fTimer;
	public float m_fSpawnBetweenDelay;
	private int asteroidSpawnAmount;
	private int clusterMax;
	private int asteroidSequenceMax;
	private int asteroidsSpawnedThisTime;
	private float spawnStartDelay;
	private float spawnStartTimer;

	// Use this for initialization
	void Start () {
		asteroidSequenceMax = 10;
		clusterMax = 4;
		asteroidSpawnAmount = 5;
		spawnStartDelay = 2f;
		spawnStartTimer = 0f;
		m_enState = SPAWNERSTATE.INACTIVE;
		m_eDifficulty = ASTEROID_SPAWNER_DIFFICULTY.BASE;
	}
	
	// Update is called once per frame
	void Update () {
		switch (m_enState) {
		case SPAWNERSTATE.INACTIVE:
			//do nothin'
			break;
		case SPAWNERSTATE.WAITING:
			if (spawnStartTimer > spawnStartDelay)
				StartSpawning ();

			spawnStartTimer += Time.deltaTime;
			break;
		case SPAWNERSTATE.SPAWN_INDIVIDUALS:
		case SPAWNERSTATE.SPAWN_CLUSTER:
			if (SpawnCheck ()) {
				Vector3 randomPos = new Vector3(Random.Range (-2f, 2f), 0f, asteroidSpawnPoint.position.z);
				var a = Instantiate (asteroids [Random.Range (0, asteroids.Count - 1)], randomPos, Quaternion.Euler(new Vector3(0, 0, 0)));
				a.transform.localScale = new Vector3 (40, 40, 40);
				asteroidsSpawnedThisTime++;
			}
			break;
		}

		m_fTimer += Time.deltaTime;
	}

	private bool SpawnCheck(){
		//if (spawnStartTimer > spawnStartDelay) {
			if (m_fTimer > m_fSpawnBetweenDelay) {
			if (asteroidsSpawnedThisTime > asteroidSpawnAmount || this.gameObject.GetComponent<WaveSpawner> ().GetIsBossWave ()) {
					StopSpawning ();
					return false;
				}
				m_fTimer = 0f;

				return true;
			}
		//}

		return false;
	}

	public void StartSpawning()
	{
		int w = WaveSpawner.Get ().wavesDefeated;
		if (Random.Range (0, 10) > 5) {
			StartSpawningIndividuals ();
		}else{
			StartSpawningCluster (4,7);
		}

		//sort difficulty
		SortDifficulty(w);
	}

	//w = waves defeated
	public void SortDifficulty(int w){
		if (w < 3)
			m_eDifficulty = ASTEROID_SPAWNER_DIFFICULTY.BASE;
		else if (w < 5)
			m_eDifficulty = ASTEROID_SPAWNER_DIFFICULTY.EASY;
		else if (w < 7)
			m_eDifficulty = ASTEROID_SPAWNER_DIFFICULTY.MEDIUM;
		else if (w < 11)
			m_eDifficulty = ASTEROID_SPAWNER_DIFFICULTY.HARD;
		else
			m_eDifficulty = ASTEROID_SPAWNER_DIFFICULTY.EXPERT;
	}

	public void ActivateAsteroidSpawner(int wavesDefeated){
		SortDifficulty (wavesDefeated);

		//use difficulty to sort chance
		if (m_eDifficulty != ASTEROID_SPAWNER_DIFFICULTY.BASE) {
			float r = Random.Range (0, 10);
			switch (m_eDifficulty) {
			case ASTEROID_SPAWNER_DIFFICULTY.EASY:
				clusterMax = 4;
				asteroidSequenceMax = Random.Range (2, 5);
				if (r > 4)
					return;
				break;
			case ASTEROID_SPAWNER_DIFFICULTY.MEDIUM:
				clusterMax = Random.Range (4,6);
				asteroidSequenceMax = Random.Range (4, 8);
				if (r > 5)
					return;
				break;
			case ASTEROID_SPAWNER_DIFFICULTY.HARD:
				clusterMax = Random.Range (4,6);
				asteroidSequenceMax = Random.Range (6, 10);
				if (r > 6)
					return;
				break;
			case ASTEROID_SPAWNER_DIFFICULTY.EXPERT:
				clusterMax = Random.Range (6,8);
				asteroidSequenceMax = Random.Range (10, 18);
				if (r > 8)
					return;
				break;
			}

			asteroidSpawnAmount = Random.Range (1, clusterMax);
			m_enState = SPAWNERSTATE.WAITING;
			spawnStartDelay = Random.Range (1.5f, 4f);
			spawnStartTimer = 0f;			
		}
	}

	public void DeactivateAsteroidSpawner(){
		m_enState = SPAWNERSTATE.INACTIVE;
	}

	//spawning individuals, time between is higher
	public void StartSpawningIndividuals(){
		spawnStartTimer = m_fTimer = 0f;
		asteroidSpawnAmount = Random.Range (3, asteroidSequenceMax);
		m_enState = SPAWNERSTATE.SPAWN_INDIVIDUALS;
		m_fSpawnBetweenDelay = Random.Range (2.5f, 4.5f);
		spawnStartDelay = Random.Range (1f, 3.2f);
	}

	//cluster doesn't last long
	public void StartSpawningCluster(int clusterCountMin, int clusterCountMax){
		spawnStartTimer = m_fTimer = 0f;
		asteroidSpawnAmount = Random.Range (clusterCountMin, clusterCountMax);
		if (asteroidSpawnAmount > clusterMax)
			asteroidSpawnAmount = clusterMax;
		
		m_enState = SPAWNERSTATE.SPAWN_CLUSTER;
		m_fSpawnBetweenDelay = Random.Range (0f, 0.3f);
		spawnStartDelay = Random.Range (0, 7f);
	}

	public void StopSpawning(){
		m_enState = SPAWNERSTATE.WAITING;

		//reset counts
		asteroidsSpawnedThisTime = 0;
		asteroidSpawnAmount = 0;

		//reset timer
		m_fTimer = 0f;
		spawnStartTimer = 0f;

		if (WaveSpawner.Get().wavesDefeated < 5) {
			spawnStartDelay = 500f;
		} else if(Random.Range(0,5) > 5){
			spawnStartDelay = Random.Range (7.5f, 15f);
		}
	}
}
