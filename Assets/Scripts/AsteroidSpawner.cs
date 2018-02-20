using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
	public List<GameObject> asteroids;

	public enum SPAWNERSTATE
	{
		WAITING = 0,
		SPAWN_INDIVIDUALS,	//individual asteroids in a queue, spawn between = high
		SPAWN_CLUSTER	//a bunch in quick succession, spawn between = low
	}

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
		m_enState = SPAWNERSTATE.WAITING;
	}
	
	// Update is called once per frame
	void Update () {
		switch (m_enState) {
		case SPAWNERSTATE.WAITING:
			//do nothin'
			break;
		case SPAWNERSTATE.SPAWN_INDIVIDUALS:
		case SPAWNERSTATE.SPAWN_CLUSTER:
			if (SpawnCheck ()) {
				Vector3 randomPos = new Vector3(Random.Range (-2f, 2f), 0f, this.gameObject.GetComponent<WaveSpawner>().spawnPoints[0].position.z);
				var a = Instantiate (asteroids [Random.Range (0, asteroids.Count - 1)], randomPos, Quaternion.Euler(new Vector3(180f, 0, 0)));
				a.transform.localScale = new Vector3 (40, 40, 40);
				asteroidsSpawnedThisTime++;
			}else
				spawnStartTimer += Time.deltaTime;
			break;
		}

		m_fTimer += Time.deltaTime;
	}

	private bool SpawnCheck(){
		if (spawnStartTimer > spawnStartDelay) {
			if (m_fTimer > m_fSpawnBetweenDelay) {
				if (asteroidsSpawnedThisTime > clusterAmount || this.gameObject.GetComponent<WaveSpawner> ().GetIsBossWave ()) {
					StopSpawning ();
					return false;
				}
				m_fTimer = 0f;

				return true;
			}
		}

		return false;
	}

	//spawning individuals, time between is higher
	public void StartSpawningIndividuals(){
		spawnStartTimer = m_fTimer = 0f;
		clusterAmount = Random.Range (3, clusterMax);
		m_enState = SPAWNERSTATE.SPAWN_INDIVIDUALS;
		m_fSpawnBetweenDelay = Random.Range (1f, 3f);
		spawnStartDelay = Random.Range (1f, 3.2f);
	}

	//cluster doesn't last long
	public void StartSpawningCluster(int clusterCountMin, int clusterCountMax){
		spawnStartTimer = m_fTimer = 0f;
		clusterAmount = Random.Range (clusterCountMin, clusterCountMax);
		if (clusterAmount > clusterMax)
			clusterAmount = clusterMax;
		
		m_enState = SPAWNERSTATE.SPAWN_CLUSTER;
		m_fSpawnBetweenDelay = Random.Range (0.5f, 1f);
		spawnStartDelay = Random.Range (0, 3f);
	}

	public void StopSpawning(){
		m_enState = SPAWNERSTATE.WAITING;

		//reset counts
		asteroidsSpawnedThisTime = 0;

		//reset timer
		m_fTimer = 0f;
		spawnStartTimer = 0f;
	}
}
