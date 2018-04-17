using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : MonoBehaviour {
	private bool UsedExtraLife;

	// Use this for initialization
	void Start () {
		UsedExtraLife = false;
	}
	
	public bool UseExtraLife(){
		if (UsedExtraLife == false) {
			UsedExtraLife = true;
			return true;
		}
		
		return false;
	}
}
