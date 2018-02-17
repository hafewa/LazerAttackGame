using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour {
	public GameObject scrapMetal;
	public GameObject[] ores;
	public GameObject[] powerUps;
	public int rareDropRate;
	public int rareDropCount;
	public int powerUpDropRate;
	public int powerUpDropCount;
	public bool isBoss;
	public bool killed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	public void Kill(){
		if (killed)
			return;
		killed = true;
		//spawn a scrap metal
		Instantiate (scrapMetal, transform.position, new Quaternion(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90), 1));

		int v = Random.Range (0, 100);
		//10% chance of dropping a rare-ore
		if (!GameObject.Find ("Player"))
			return;
		
		int luck = GameObject.Find("Player").GetComponent<PlayerMovement>().GetLuckLevel();
		if (v < rareDropRate + (luck * 5) || isBoss) {
			int realCount = rareDropCount;
			if (isBoss)
				realCount += (luck / 100 * realCount);
			for (int i = 0; i < realCount; i++) {
				int x = Random.Range (0, ores.Length);
				
				float jmp = 0;
				if (i < rareDropCount / 2)
					jmp = i * Random.Range (0, 5000f);
				else
					jmp = i * Random.Range (0, -5000f);
				
				var rare = Instantiate (ores [x], transform.position, new Quaternion(0, 0, 0, 1));
				
				if (!isBoss)
					rare.GetComponent<Rigidbody> ().AddForce (new Vector3 (jmp, 0, 8000f));
				else
					rare.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range(-6000f, 6000f), 0, Random.Range(15000f, 20000f)));
			}
		}
	}
}
