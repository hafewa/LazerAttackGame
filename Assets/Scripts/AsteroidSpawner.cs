using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
	public List<GameObject> asteroids;

	public enum SPAWNERSTATE
	{
		WAITING = 0,
		SPAWNING
	}

	private SPAWNERSTATE m_enState;
	private float timer;
	private float stateLength;
	private GameObject spwnr;

	private float spawnDelay;
	private float lastSpawnTime;
	// Use this for initialization
	void Start () {
		m_enState = SPAWNERSTATE.WAITING;
		timer = 0f;
		stateLength = 5f;
		spwnr = GameObject.Find ("CameraHolder");
		spawnDelay = 1f;
		lastSpawnTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (spwnr.GetComponent<WaveSpawner> ().GetIsBossWave ())
			return;

		if (timer > stateLength) {
			//swap the states if the timer says so
			if (m_enState == SPAWNERSTATE.WAITING)
				m_enState = SPAWNERSTATE.SPAWNING;
			else
				m_enState = SPAWNERSTATE.WAITING;
			timer = 0f;
		}

		if (m_enState == SPAWNERSTATE.SPAWNING) {
			if (timer > lastSpawnTime + spawnDelay) {
				lastSpawnTime = timer;
				var a = Instantiate (asteroids [Random.Range(0, asteroids.Count)], transform.position + new Vector3(Random.Range (-3f, 3f), 0, 0), transform.rotation);
				var s = Random.Range (35, 50);
				a.transform.localScale = new Vector3 (s, s, s);
			}
		}

		timer += Time.deltaTime;
	}
}
