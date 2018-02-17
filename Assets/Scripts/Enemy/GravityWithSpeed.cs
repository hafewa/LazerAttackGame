using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWithSpeed : MonoBehaviour {
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

		transform.position += ((Physics.gravity) * Time.deltaTime);

		if (canRotate)
			transform.RotateAround (transform.position, new Vector3 (-1, 0, 1), rotSpeed * Time.deltaTime);
	}
}
