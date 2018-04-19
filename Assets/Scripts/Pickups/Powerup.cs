using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : Pickup {
	public POWERUPTYPE m_eType;

	public enum POWERUPTYPE
	{
		WEAPON = 0,
		MAGNET,
		LUCK,
		SPEED
	}

	// Use this for initialization
	protected override void Start () {
		m_ptType = PICKUPTYPE.POWERUP;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

	}
}
