using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour {

	public ParticleSystem particle;

	public void OnTriggerEnter(Collider col)
	{
		Debug.Log ("HitEndPoint");
		particle.Play();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
