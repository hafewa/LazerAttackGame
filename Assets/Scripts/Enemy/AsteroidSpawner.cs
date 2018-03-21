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
	private int clusterAmount;
	private int clusterMax;
	private int asteroidsSpawnedThisTime;
	private float spawnStartDelay;
	private float spawnStartTimer;

	// Use this for initialization
	void Start () {
		clusterMax = 10;
		clusterAmount = 5;
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
				if (asteroidsSpawnedThisTime > clusterAmount || this.gameObject.GetComponent<WaveSpawner> ().GetIsBossWave ()) {
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
		Debug.Log ("start spawning");
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

		Debug.Log ("difficulty: " + m_eDifficulty);
	}

	public void ActivateAsteroidSpawner(int wavesDefeated){
		Debug.Log ("Activate asteroid spawner");
		SortDifficulty (wavesDefeated);

		//use difficulty to sort chance
		if (m_eDifficulty != ASTEROID_SPAWNER_DIFFICULTY.BASE) {
			float r = Random.Range (0, 10);
			switch (m_eDifficulty) {
			case ASTEROID_SPAWNER_DIFFICULTY.EASY:
				clusterMax = Random.Range (7, 9);
				if (r > 4)
					return;
				break;
			case ASTEROID_SPAWNER_DIFFICULTY.MEDIUM:
				clusterMax = Random.Range (8, 11);
				if (r > 5)
					return;
				break;
			case ASTEROID_SPAWNER_DIFFICULTY.HARD:
				clusterMax = Random.Range (9, 13);
				if (r > 6)
					return;
				break;
			case ASTEROID_SPAWNER_DIFFICULTY.EXPERT:
				clusterMax = Random.Range (14, 18);
				if (r > 8)
					return;
				break;
			}

			clusterAmount = Random.Range (1, clusterMax);
			m_enState = SPAWNERSTATE.WAITING;
			spawnStartDelay = Random.Range (1.5f, 4f);
			spawnStartTimer = 0f;			
		}
	}

	public void DeactivateAsteroidSpawner(){
		Debug.Log ("Deactivate asteroid spawner");
		m_enState = SPAWNERSTATE.INACTIVE;
	}

	//spawning individuals, time between is higher
	public void StartSpawningIndividuals(){
		spawnStartTimer = m_fTimer = 0f;
		clusterAmount = Random.Range (3, clusterMax);
		m_enState = SPAWNERSTATE.SPAWN_INDIVIDUALS;
		m_fSpawnBetweenDelay = Random.Range (2.5f, 4.5f);
		spawnStartDelay = Random.Range (1f, 3.2f);
	}

	//cluster doesn't last long
	public void StartSpawningCluster(int clusterCountMin, int clusterCountMax){
		spawnStartTimer = m_fTimer = 0f;
		clusterAmount = Random.Range (clusterCountMin, clusterCountMax);
		if (clusterAmount > clusterMax)
			clusterAmount = clusterMax;
		
		m_enState = SPAWNERSTATE.SPAWN_CLUSTER;
		m_fSpawnBetweenDelay = Random.Range (0f, 0.2f);
		spawnStartDelay = Random.Range (0, 5f);
	}

	public void StopSpawning(){
		Debug.Log ("stop spawning");
		m_enState = SPAWNERSTATE.WAITING;

		//reset counts
		asteroidsSpawnedThisTime = 0;
		clusterAmount = 0;

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
