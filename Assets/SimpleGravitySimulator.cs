using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimpleGravitySimulator : MonoBehaviour {

	public Transform root;
	public List<Rigidbody> allRigidBodies = new List<Rigidbody>();

	public Transform directionNode;
	public Transform compass;

	public float power = 0f;

	// Use this for initialization
	void Start () {
		allRigidBodies = root.GetComponentsInChildren<Rigidbody>().ToList();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 getGravity = Input.acceleration;//compass.InverseTransformPoint(directionNode.position);

		for(int i=0; i<allRigidBodies.Count; i++)
			allRigidBodies[i].AddForce(getGravity * power);
	}
}
