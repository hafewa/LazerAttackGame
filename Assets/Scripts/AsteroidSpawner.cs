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
	private float m_fTimer;
	public float m_fSpawnBetweenDelay;

	// Use this for initialization
	void Start () {
		m_enState = SPAWNERSTATE.WAITING;
	}
	
	// Update is called once per frame
	void Update () {
		switch (m_enState) {
		case SPAWNERSTATE.WAITING:
			
			break;
		case SPAWNERSTATE.SPAWNING:
			
			break;
		}

		m_fTimer += Time.deltaTime;
	}

	public void ChangeState(SPAWNERSTATE s){
		m_enState = s;
	}
}
