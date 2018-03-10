using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePoints: MonoBehaviour {

	public float boost;
	// Use this for initialization
	void Start () {
		if (boost <= 0)
			boost = 1.5f;
	}
	
	public float BoostPoints(float pts){
		return pts *= boost;
	}
}
