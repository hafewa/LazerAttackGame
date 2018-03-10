using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour {
	public Text HighScoreValue;
	public Text GamesPlayedValue;
	public Text LongestGameValue;
	public Text MostWavesValue;
	public Text MostBossesDefeatedValue;

	// Use this for initialization
	void Start () {
		PlayerPrefsManager ppm = PlayerPrefsManager.Get ();
		HighScoreValue.text = ppm.GetHighScore().ToString ();
		GamesPlayedValue.text = ppm.GetGamesPlayed ().ToString ();
		LongestGameValue.text = ppm.GetLongestGame() + " seconds";
		MostWavesValue.text = ppm.GetHighestWaves ().ToString ();
		MostBossesDefeatedValue.text = ppm.GetMostBossesDeafeted ().ToString ();
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
