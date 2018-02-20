using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousRotateAround : MonoBehaviour {
	public Transform point;
	public float RotationSpeed = 0f;

	// Update is called once per frame
	void Update () {
		transform.RotateAround (point.position, new Vector3 (0, 1, 1), RotationSpeed * Time.deltaTime);
	}
}
