using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	private Vector3 origPos;

	public float m_iDamage;
	public float m_fSpeed;

	public bool canRotate;
	public float rotSpeed;

	// Use this for initialization
	void Start () {
		if (m_fSpeed <= 0)
			m_fSpeed = 0.1f;
		origPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, transform.position + new Vector3(0f, 0f, 1f), m_fSpeed);
		if (canRotate)
			transform.RotateAround (transform.position, new Vector3 (0, 0, 1), rotSpeed * Time.deltaTime);
		
		float dist = Vector3.Distance (origPos, transform.position);

		if (dist > 50f || dist < -50f) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Enemy") {
			other.GetComponent<EnemyHealth>().Hurt(m_iDamage);
			Destroy (this.gameObject);
		}
	}
}
