using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {
	private static PlayerPrefsManager instance;
	public int TotalScore { get { return PlayerPrefs.GetInt ("points", 0); } }
	public string CurrentAssignedShip { get { return PlayerPrefs.GetString ("ShipName", "SpaceShip1"); } }
	private int GamesPlayed { get { return PlayerPrefs.GetInt ("GamesPlayed", 0); } }
	private int HighScore { get { return PlayerPrefs.GetInt ("PlayerHighScore", 0); } }
	private int BossesDefeatedRecord { get { return PlayerPrefs.GetInt ("BossesDefeated", 0); } }
	private int WavesDefeatedRecord { get { return PlayerPrefs.GetInt ("WavesDefeated", 0); } }
	private int LongestGameTimeRecord { get { return PlayerPrefs.GetInt ("LongestGameInSeconds", 0); } }

	void Awake(){
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public static PlayerPrefsManager Get(){
		return instance;
	}

	//add some points to the players total score (total game, not singular run)
	public void AddScore(int points){
		PlayerPrefs.SetInt ("points", TotalScore + points);
	}

	public int GetHighScore(){
		return HighScore;
	}

	//returns the current ships level
	public int GetShipLevel(string shipName){
		return PlayerPrefs.GetInt (shipName + ":PlayerLevel", 0);
	}

	//level up the current ship
	public void IncrementShipLevel(string shipName){
		PlayerPrefs.SetInt(shipName + ":PlayerLevel", PlayerPrefs.GetInt(shipName + ":PlayerLevel", 0) + 1);
	}

	//only use this in particular situations (i.e. testing/error occurred and shiplevel < 0)
	public void ResetShipLevel(string shipName){
		PlayerPrefs.SetInt (shipName + ":PlayerLevel", 0);
	}

	//unlock ship by name
	public void UnlockShip(string shipName){
		PlayerPrefs.SetString (shipName + ":Unlocked", "true");
	}

	//assign a current ship
	public void SetCurrentShip(string shipName){
		PlayerPrefs.SetString ("ShipName", shipName);
	}

	//is the ship the person buying locked?
	public bool IsShipLocked(string shipName){
		return PlayerPrefs.GetString (shipName + ":Unlocked", "") == "";
	}

	public int GetGamesPlayed(){
		return GamesPlayed;
	}

	public void IncGamesPlayed(){
		PlayerPrefs.SetInt ("GamesPlayed", GamesPlayed + 1);
	}

	public bool IsNewHighScore(int s){
		return s > HighScore;
	}

	public void SetNewHighscoreIfHigher(int s){
		if (s > HighScore) 
		{
			PlayerPrefs.SetInt ("PlayerHighScore", s);
		}
	}

	public void SetNewHighBosses(int b){
		if (b > BossesDefeatedRecord) {
			PlayerPrefs.SetInt ("BossesDefeated", b);
		}
	}

	public int GetMostBossesDeafeted(){
		return PlayerPrefs.GetInt("BossesDefeated", 0);
	}

	public void SetNewHighWaves(int w){
		if (w > WavesDefeatedRecord) {
			PlayerPrefs.SetInt ("WavesDefeated", w);
		}
	}

	public int GetHighestWaves(){
		return PlayerPrefs.GetInt("WavesDefeated", 0);
	}

	public void SetNewLongestGame(int s){
		if (s > LongestGameTimeRecord) {
			PlayerPrefs.SetInt ("LongestGameInSeconds", s);
		}
	}

	public int GetLongestGame(){
		return LongestGameTimeRecord;
	}
}
