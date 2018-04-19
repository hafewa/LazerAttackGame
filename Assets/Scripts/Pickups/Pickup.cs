using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
	public Vector3 speed;
	public enum PICKUPTYPE
	{
		TREASURE = 0,
		POWERUP
	}
	public PICKUPTYPE m_ptType;
	public int m_iValue;
	public bool used = false;
	// Use this for initialization
	protected virtual void Start () {
		speed = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		speed += (Physics.gravity * Time.deltaTime);
		Vector3 tmpSpeed = speed;
		if (WaveSpawner.Get () != null) {
			if (WaveSpawner.Get ().IsInSpeedMode ())
				tmpSpeed.z *= 4f;
		}
		gameObject.transform.position += tmpSpeed * Time.deltaTime;

		if (transform.position.z < -10f)
			Destroy (this.gameObject);
	}

	public int GetValue(){
		return m_iValue;
	}

	public PICKUPTYPE GetPickupType(){
		return m_ptType;
	}
}
