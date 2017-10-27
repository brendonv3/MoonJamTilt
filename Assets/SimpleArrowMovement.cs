using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleArrowMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.position = transform.position + Vector3.right  + Vector3.up;
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.position = transform.position - Vector3.right  + Vector3.up;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			transform.position = transform.position + Vector3.forward + Vector3.up;
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			transform.position = transform.position - Vector3.forward + Vector3.up;
		}
	}
}
