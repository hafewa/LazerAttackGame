using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
	public float speedMultiplier;
	public bool canRotate;
	public float rotSpeed;

	// Use this for initialization
	void Start () {
		if (speedMultiplier <= 0)
			speedMultiplier = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < -10f)
			Destroy (this.gameObject);
		
		transform.position += (transform.forward * speedMultiplier) * Time.deltaTime;

		if (canRotate)
			transform.RotateAround (transform.position, new Vector3 (0, 0, 1), rotSpeed * Time.deltaTime);
	}
}
