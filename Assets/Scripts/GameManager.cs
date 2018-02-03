using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public enum GAME_STATE{
		MENU = 0,
		PLAYING,
		DEATH,
	}

	private GAME_STATE m_eState;

	//score management
	private int m_iCurrentGameScore;
	private int m_iOverallPlayerScore;

	public Text m_uitScoreText;

	// Use this for initialization
	void Start () {
		//get details
		m_iOverallPlayerScore = PlayerPrefs.GetInt ("points", 0);
		//Debug.Log ("stored score:" + m_iOverallPlayerScore);
		m_iCurrentGameScore = 0;
		//Physics.gravity = new Vector3 (0, 0, -200f);
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().name == "Game")
			m_uitScoreText.text = m_iCurrentGameScore + "";
		else if (SceneManager.GetActiveScene ().name == "Shop")
			m_uitScoreText.text = m_iOverallPlayerScore + "";
	}

	public void PlayerDead(){
		//end the game
		m_iOverallPlayerScore += m_iCurrentGameScore;
		m_iCurrentGameScore = 0;
		PlayerPrefs.SetInt ("points", m_iOverallPlayerScore);

		SceneManager.LoadScene (0);
	}

	public void AddGameScore(int s){
		m_iCurrentGameScore += s;
	}
}
