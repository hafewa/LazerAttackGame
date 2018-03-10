using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoost : MonoBehaviour {
	public float boost;
	// Use this for initialization
	void Start () {
		if (boost <= 0)
			boost = 1.5f;
	}
	
	public float BoostDamage(float dmg){
		return dmg *= boost;
	}
}
