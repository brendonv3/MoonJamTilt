using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleTeleporter : MonoBehaviour {

	public TeleporterNode a;
	public TeleporterNode b;

	public Action<SimpleTeleporter> triggeredA;
	public Action<SimpleTeleporter> triggeredB;

	// Use this for initialization
	void Start () {
		a.triggered = HandleATriggered;
		b.triggered = HandleBTriggered;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleATriggered(){
		Debug.Log ("atriggeredS");
		if (triggeredA != null)
			triggeredA (this);
	}

	public void HandleBTriggered(){
		Debug.Log ("bTriggered");
		if (triggeredB != null)
			triggeredB (this);
	}
}
