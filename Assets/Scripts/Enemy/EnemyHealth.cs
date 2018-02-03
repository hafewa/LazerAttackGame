using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour 
{
	void Start()
	{

	}

	void Update()
	{

	}

	public float GetHealth(){
		if(this.gameObject.GetComponent<BasicEnemy> () != null)
			return this.gameObject.GetComponent<BasicEnemy> ().GetHealth ();
		else if(this.gameObject.GetComponent<BasicBoss> () != null)
			return this.gameObject.GetComponent<BasicBoss> ().GetHealth ();

		return 0;
	}

	public void Hurt(float dmg){
		if(this.gameObject.GetComponent<BasicEnemy> () != null)
			this.gameObject.GetComponent<BasicEnemy> ().Hurt (dmg);
		else if(this.gameObject.GetComponent<BasicBoss> ())
			this.gameObject.GetComponent<BasicBoss> ().Hurt (dmg);
	}
}
