using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	GameObject _ball;
	GameObject ball{
		get{
			if (_ball == null)
				_ball = Instantiate(Resources.Load ("Prefabs/Ball") as GameObject);

			return _ball;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//	Summon and unsummon a scene.

}
