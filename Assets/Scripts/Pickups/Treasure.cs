using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Pickup {

	// Use this for initialization
	protected override void Start () {
		m_ptType = PICKUPTYPE.TREASURE;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
}
