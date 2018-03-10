using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBaseMove : MonoBehaviour {
	float speed = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		speed += Time.deltaTime * 2;
		transform.position = Vector3.MoveTowards (transform.position, transform.position + new Vector3 (0, 0, -1), speed * Time.deltaTime);

		if (transform.position.z < -50f)
			Destroy (this.gameObject);
	}
}
