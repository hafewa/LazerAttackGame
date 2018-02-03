using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

	public int m_iValue;
	public Vector3 speed;

	// Use this for initialization
	void Start () {
		speed = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		speed += (Physics.gravity * Time.deltaTime);

		gameObject.transform.position += speed * Time.deltaTime;

		if (transform.position.z < -10f)
			Destroy (this.gameObject);
	}

	public int GetValue(){
		return m_iValue;
	}
}
