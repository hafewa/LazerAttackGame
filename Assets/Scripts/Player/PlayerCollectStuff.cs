using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectStuff : MonoBehaviour {
	List<GameObject> m_arrPowerups;

	private int m_iDmgLevel;
	private int m_iSubDmgLevel;

	// Use this for initialization
	void Start () {
		//clear of all items, just in case
		if (m_arrPowerups.Count > 0)
			for (int i = 0; i < m_arrPowerups.Count; i++)
				m_arrPowerups.Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncSubDamage(int i){
		m_iSubDmgLevel++;

		if (m_iSubDmgLevel > 3) {
			m_iSubDmgLevel = 0;
			m_iDmgLevel++;
		}
	}

	public void AddBoost(GameObject go){
		m_arrPowerups.Add (go);
	}

	void OnTriggerEnter(Collider other){
		//Debug.Log (other.tag);
	}
}
