﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadronMenuController : MonoBehaviour {
	public Transform rightBuddyPos;
	public Transform leftBuddyPos;
	public GameObject leftBuddy;
	public GameObject rightBuddy;
	public Transform currentItemSpawnPoint;

	public ShopCrate.Reward[] possibleBuddies;
	private List<ShopCrate.Reward> buddiesOwned;

	private int currSquadIndex;
	private GameObject currSquadItem;
	public GameObject m_goNoBuddiesOwned;

	// Use this for initialization
	void Start () {
		string buddyList = PlayerPrefs.GetString ("buddys");
		buddiesOwned = new List<ShopCrate.Reward>();
		foreach (ShopCrate.Reward r in possibleBuddies) {
			buddiesOwned.Add (r);
		}

		string right = PlayerPrefs.GetString ("RightBuddy", "");
		string left = PlayerPrefs.GetString ("LeftBuddy", "");

		int buddyCount = 0;
		if (left != "" || right != "") {
			foreach (ShopCrate.Reward r in buddiesOwned) {
				if (buddyCount >= 2)
					break;
				
				if (r.codename == right) {
					rightBuddy = Instantiate (r.obj, rightBuddyPos);
					rightBuddy.transform.position = rightBuddyPos.transform.position;
					buddyCount++;
				} else if (r.codename == left) {
					leftBuddy = Instantiate (r.obj, leftBuddyPos);
					leftBuddy.transform.position = leftBuddyPos.transform.position;

					buddyCount++;
				}
			}
		}

		if (buddiesOwned.Count > 0) {
			currSquadItem = Instantiate (buddiesOwned [0].obj, currentItemSpawnPoint);
			currSquadItem.transform.position = currentItemSpawnPoint.transform.position;
			m_goNoBuddiesOwned.SetActive (false);
		} else {
			m_goNoBuddiesOwned.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		currentItemSpawnPoint.transform.RotateAround (currentItemSpawnPoint.transform.position, new Vector3 (0, 1, 1), 45f * Time.deltaTime);
	}

	public void IncOrDecShopItem(int i){
		currSquadIndex += i;

		if (currSquadIndex < 0)
			currSquadIndex = buddiesOwned.Count - 1;
		else if (currSquadIndex > buddiesOwned.Count - 1)
			currSquadIndex = 0;

		Destroy (currSquadItem);
		currSquadItem = Instantiate (buddiesOwned [currSquadIndex].obj, currentItemSpawnPoint);
		currSquadItem.transform.position = currentItemSpawnPoint.transform.position;
	}

	public void AddCurrentItemToLeftBuddy(){
//		if(buddiesOwned[currSquadIndex].codename == PlayerPrefs.GetString("LeftBuddy" || buddiesOwned[currSquadIndex].codename == PlayerPrefs.GetString("RightBuddy")
//			return;
		Destroy(leftBuddy);
		leftBuddy = Instantiate (buddiesOwned [currSquadIndex].obj, leftBuddyPos);
		leftBuddy.transform.position = leftBuddyPos.transform.position;
		PlayerPrefs.SetString ("LeftBuddy", buddiesOwned [currSquadIndex].codename);
	}

	public void AddCurrentItemToRightBuddy(){
//		if(buddiesOwned[currSquadIndex].codename == PlayerPrefs.GetString("LeftBuddy" || buddiesOwned[currSquadIndex].codename == PlayerPrefs.GetString("RightBuddy")
//			return;
		Destroy(rightBuddy);
		rightBuddy = Instantiate (buddiesOwned [currSquadIndex].obj, rightBuddyPos);
		rightBuddy.transform.position = rightBuddyPos.transform.position;
		PlayerPrefs.SetString ("RightBuddy", buddiesOwned [currSquadIndex].codename);
	}
}
