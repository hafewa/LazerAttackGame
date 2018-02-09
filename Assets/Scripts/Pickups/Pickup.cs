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

	// Use this for initialization
	protected virtual void Start () {
		speed = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		speed += (Physics.gravity * Time.deltaTime);

		gameObject.transform.position += speed * Time.deltaTime;

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
