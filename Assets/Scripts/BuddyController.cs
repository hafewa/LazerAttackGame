using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyController : MonoBehaviour {
	private float m_fBulletTimer;
	public float m_fBulletDelay;

	public GameObject m_goBullet;

	// Use this for initialization
	void Start () {
		if(m_fBulletDelay <= 0)
			m_fBulletDelay = 1f;
		m_fBulletTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
//		if (m_fBulletTimer > m_fBulletDelay) {
//			Instantiate(m_goBullet, transform.position, m_goBullet.transform.localRotation);
//			m_fBulletTimer = 0f;
//		}
//
//		m_fBulletTimer += Time.deltaTime;
	}
}
