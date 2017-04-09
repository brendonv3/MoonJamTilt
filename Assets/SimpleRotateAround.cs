using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleRotateAround : MonoBehaviour {

	public Transform pivotPoint;
	public float time;
	public Vector3 deltaRotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Out()
	{
		DOTween.Kill (this.transform);
	}

	public void In()
	{
	}
}
