using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour {
	public GameObject scrapMetal;
	public GameObject[] ores;
	public int rareDropRate;
	public int rareDropCount;
	public bool isBoss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	public void Kill(){
		//spawn a scrap metal
		Instantiate (scrapMetal, transform.position, new Quaternion(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90), 1));

		int v = Random.Range (0, 100);
		//10% chance of dropping a rare-ore
		if (v < rareDropRate || isBoss) {
			for (int i = 0; i < rareDropCount; i++) {
				int x = Random.Range (0, ores.Length);

				float jmp = 0;
				if (i < rareDropCount / 2)
					jmp = i * 2000f;
				else
					jmp = i * -2000f;
				float upForce = Random.Range (8000, 12000);
				float bossUpForce = Random.Range (12000, 20000);
				var rare = Instantiate (ores [x], transform.position, new Quaternion(0, 0, 0, 1));

				if(!isBoss)
					rare.GetComponent<Rigidbody> ().AddForce (new Vector3 (jmp, 0, upForce));
				else
					rare.GetComponent<Rigidbody> ().AddForce (new Vector3 (jmp, 0, bossUpForce));
			}
		}
	}
}
