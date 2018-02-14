using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {
	public bool canSpawn;
	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform[] enemy;
		public int count;
		public float rate;
		public bool canIncreaseCount;
	}

	public bool m_bBossWave;
	public Wave[] waves;
	private int nextWave = 0;
	private int wavesDefeated = 0;
	public int NextWave
	{
		get { return nextWave + 1; }
	}

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;
	public SpawnState State
	{
		get { return state; }
	}

	public GameObject m_goPlayer;

	void Start()
	{
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
		}

		waveCountdown = timeBetweenWaves;
		m_bBossWave = false;
		//GameManager.Instance.SetGameState (GameManager.STATES.PLAYING);
	}

	void Update()
	{
		if (!canSpawn)
			return;
		
		if (state == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				WaveCompleted();
			}
			else
			{
				return;
			}
		}

		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
				//UIControllerScript.Instance.SetWaveTextVisibility (false);
				StartCoroutine( SpawnWave ( waves[nextWave] ) );
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		//Debug.Log("Wave Completed!");
		//UIControllerScript.Instance.SetWaveTextVisibility (true);
		wavesDefeated++;
		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			//Debug.Log("ALL WAVES COMPLETE! Looping...");
			for(int i = 0; i < waves.Length; i++){
				Debug.Log ("WAVE COMPLETE");
				if (waves[i].canIncreaseCount) {
					float multiplier = Random.Range (1.5f, 2f);

					waves [i].count = (int)(waves [i].count * multiplier);
				}

				if (waves [i].name.Contains ("Boss")) {
					//alter health at some point
				} else {
					
				}
			}
		}
		else
		{
			nextWave++;
		}
		//UIControllerScript.Instance.AlterWaveNumBy(1);
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}

	public bool GetIsBossWave(){
		return m_bBossWave;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log(wavesDefeated);
		state = SpawnState.SPAWNING;
		if (_wave.name.Contains ("Boss")) {
			m_bBossWave = true;
		} else {
			m_bBossWave = false;
		}

		for (int i = 0; i < _wave.count; i++)
		{
			//get random enemy in list
			if (_wave.name.Contains("Basic")) {
				if (_wave.enemy.Length == 1) {
					SpawnEnemy (_wave.enemy [0], 0.7f);
					SpawnEnemy (_wave.enemy [0], -0.7f);
					SpawnEnemy (_wave.enemy [0], 2.1f);
					SpawnEnemy (_wave.enemy [0], -2.1f);
				}else {
					int e = (int)Random.Range (0, _wave.enemy.Length);
					SpawnEnemy (_wave.enemy [e], 0.7f);
					e = (int)Random.Range (0, _wave.enemy.Length);
					SpawnEnemy (_wave.enemy [e], -0.7f);
					e = (int)Random.Range (0, _wave.enemy.Length);
					SpawnEnemy (_wave.enemy [e], 2.1f);
					e = (int)Random.Range (0, _wave.enemy.Length);
					SpawnEnemy (_wave.enemy [e], -2.1f);
				}
			} else if (_wave.name.Contains ("Boss")) {
				SpawnEnemy (_wave.enemy [0], 0f);
			}

			yield return new WaitForSeconds( 1f/_wave.rate );
		}

		state = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform _enemy, float spacing)
	{
		Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		var enemy = Instantiate(_enemy, new Vector3(_sp.position.x + spacing, 0f, _sp.position.z), _enemy.rotation);

		if (!GetIsBossWave () && (wavesDefeated > 3)) {
			//calculate chances of enemy getting being buffed
			if (wavesDefeated < 10) {
				if (Random.Range (0, 100) > wavesDefeated * 10)
					return;
			} else {
				//if they're past wave 10 then it's always 90%
				if (Random.Range (0, 100) < 10)
					return;
			}

			enemy.GetComponent<BasicEnemy> ().canBuff = true;
			enemy.GetComponent<BasicEnemy> ().DoBuff ((3*0.1f) + (m_goPlayer.GetComponent<PlayerWeaponry>().GetPlayerLevel() * 0.5f));

			//means they've defeated every boss + every basic wave at least once
			if (wavesDefeated > waves.Length/2)
			{
				Debug.Log ("health inc by " + wavesDefeated / 10f);
				enemy.GetComponent<BasicEnemy> ().IncreaseBaseHealth (wavesDefeated/10f);
			}
		}
	}

}
