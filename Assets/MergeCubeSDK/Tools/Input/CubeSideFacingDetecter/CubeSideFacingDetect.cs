using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CubeSideFacingDetect : MonoBehaviour {
	static CubeSideFacingDetect s_ins;
	void Awake(){
		s_ins = this;

		for (int i = 0; i < scenes.Length; i++)
			scenes [i].SetActive (false);
	}
	public static CubeSideFacingDetect ins {
		get { return s_ins; }
	}

	// 0-front   1-back    2-top    3-bottom    4-left    5-right
	public Transform [] sides;
	public GameObject [] scenes;

	bool isActive = true;
	int lastNearestIndex = -1;

	public bool DetectionEnabled{
		get{
			return isActive;
		}
		set{
			isActive = value;
		}		
	}
	// Update is called once per frame
	void Update () {
		if (isActive) {
			Vector3 upPos = transform.position + Vector3.up;
			int nearestIndex = -1;
			float dis = 1000f;
			for (int i = 0; i < sides.Length; i++) {
				if (dis > Vector3.Distance (sides [i].position, upPos)) {
					dis = Vector3.Distance (sides [i].position, upPos);
					nearestIndex = i;					
				}
			}
			TriggerEvent (nearestIndex);
		}
	}

	void TriggerEvent(int nearestIndex){
		Debug.LogWarning ("nearestIndex = "+nearestIndex);
		if (nearestIndex != lastNearestIndex) {
			if (lastNearestIndex >= 0) {
				scenes [lastNearestIndex].SetActive (false);
			}
			lastNearestIndex = nearestIndex;
			Debug.LogWarning ("New Near = "+lastNearestIndex);
			scenes [lastNearestIndex].SetActive (true);
		}
	}
}
