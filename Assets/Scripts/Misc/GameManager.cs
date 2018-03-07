using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour {
	public enum GAME_STATE{
		PLAYING = 0,
		DEATH,
	}

	private GAME_STATE m_eState;

	//score management
	private int m_iCurrentGameScore;
	private int m_iOverallPlayerScore;

	public Text m_uitScoreText;
	public GameObject m_goEndGameContainer;
	public Text m_uiEndScoreText;
	public GameObject highScoreGo;

	public AudioClip gameMusic;
	public AudioClip gameOverMusic;

	// Use this for initialization
	void Start () {
		//get details
		m_iOverallPlayerScore = PlayerPrefsManager.Get().TotalScore;
		//Debug.Log ("stored score:" + m_iOverallPlayerScore);
		m_iCurrentGameScore = 0;
		m_uitScoreText.text = m_iCurrentGameScore + "";
		//Physics.gravity = new Vector3 (0, 0, -200f);
		m_eState = GAME_STATE.PLAYING;
		m_uitScoreText.enabled = true;

		//show menu
		m_goEndGameContainer.SetActive(false);

		if(Advertisement.isSupported){
			#if UNITY_ANDROID
				Advertisement.Initialize ("1706898", true);
			#elif UNITY_IOS
				Advertisement.Initialize("1706897", true);
			#endif
		}

		PlayerPrefsManager.Get ().IncGamesPlayed ();

		AudioManager.Get ().PlayMusicLoop (gameMusic);
	}
	
	// Update is called once per frame
	void Update () {
		switch (m_eState) {
		case GAME_STATE.PLAYING:
			if (SceneManager.GetActiveScene ().name == "Game")
				m_uitScoreText.text = m_iCurrentGameScore + "";
//			else if (SceneManager.GetActiveScene ().name == "Shop")
//				m_uitScoreText.text = m_iOverallPlayerScore + "";
			break;
		case GAME_STATE.DEATH:
			
			break;
		}
	}

	public void PlayerDead(){
		//end the game
		m_uiEndScoreText.text = m_iCurrentGameScore + "";
		PlayerPrefsManager.Get ().AddScore (m_iCurrentGameScore);
		highScoreGo.SetActive(PlayerPrefsManager.Get().IsNewHighScore(m_iCurrentGameScore));
		PlayerPrefsManager.Get ().SetNewHighscoreIfHigher (m_iCurrentGameScore);
		m_iCurrentGameScore = 0;
		m_eState = GAME_STATE.DEATH;
		m_uitScoreText.enabled = false;

		PlayerPrefsManager.Get ().SetNewLongestGame ((int)Time.timeSinceLevelLoad);
		PlayerPrefsManager.Get ().SetNewHighWaves (WaveSpawner.Get ().wavesDefeated);
		PlayerPrefsManager.Get ().SetNewHighBosses (WaveSpawner.Get ().bossesDefeatedTracker.Count);
		//show menu
		m_goEndGameContainer.SetActive(true);
		AudioManager.Get ().PlayMusicLoop (gameOverMusic);
	}

	public void AddGameScore(int s){
		m_iCurrentGameScore += s;
	}
}
