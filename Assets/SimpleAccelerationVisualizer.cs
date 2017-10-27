using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAccelerationVisualizer : MonoBehaviour {

	public Transform offset;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		offset.rotation = Input.gyro.attitude;
	}
}
