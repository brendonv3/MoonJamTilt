using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGameController : MonoBehaviour {

	public SimpleTeleporter teleporter;
	public Transform ball;

	// Use this for initialization
	void Start () {
		teleporter.triggeredA += HandleATriggered;
		teleporter.triggeredB += HandleBTriggered;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleATriggered()
	{
		Debug.Log ("a");
		ball.position = teleporter.b.transform.position + Vector3.up;
	}

	public void HandleBTriggered()
	{
		Debug.Log ("b");
		ball.position = teleporter.a.transform.position + Vector3.up;
	}
}
