using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCrate : MonoBehaviour {
	public int value;
	[System.Serializable]
	public class Reward
	{
		public string name;			//to show to player
		public string codename;		//for use in code
		public string description;
		public GameObject obj;
		public RARITY rarity;
		public int tradeablePointsValue;
		public REWARD_TYPE type;
	}
	[System.Serializable]
	public enum RARITY
	{
		COMMON = 0,
		RARE,
		LEGENDARY
	}
	[System.Serializable]
	public enum REWARD_TYPE{
		POINTS = 0,
		BUDDY
	}
	public List<Reward> m_lRewards;
	public float rareRate;		//buddys
	public float legendaryRate;	//spot the cow

	// Use this for initialization
	void Start () {
		if (value <= 0)
			value = 250;//set so it's never free
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int HowManyRewards(){
		return m_lRewards.Count;
	}

	public Reward GetRewardByIndex(int ind){
		return m_lRewards [ind];
	}

	public Reward GetRandomReward(){
		//rarity is based on % chance
		int rndm = Random.Range (0, 100);
		RARITY r = RARITY.COMMON;
		//check rarity
		if (rndm > legendaryRate) {
			r = RARITY.LEGENDARY;
		} else if (rndm > rareRate) {
			r = RARITY.RARE;
		} else {//common is always 0, guaranteed to get at least a common item
			//do nothin'
		}

		//get list of available rewards
		List<Reward> rwds = new List<Reward>();
		for (int i = 0; i < m_lRewards.Count; i++) {
			if (m_lRewards [i].rarity == r)
				rwds.Add (m_lRewards[i]);
		}

		//get random one of available rewards
		if (rwds.Count == 1)
			return rwds [0];
		
		int rndm2 = Random.Range (0, rwds.Count);

		return rwds [rndm2];
	}

	public string ExecuteRewardFunc(Reward reward){
		if (reward.type == REWARD_TYPE.POINTS) {
			PlayerPrefs.SetInt ("points", PlayerPrefsManager.Get().TotalScore + reward.tradeablePointsValue);
			return "\nYou got " + reward.tradeablePointsValue + " points";
		} else if (reward.type == REWARD_TYPE.BUDDY) {
			string buddysPrefsValue = PlayerPrefs.GetString ("buddys", "");

			buddysPrefsValue += "," + reward.codename;
			return "\n" + reward.name + " has been added to your buddys. Check the squadron menu to have a look.";
		}

		return "";
	}
}
