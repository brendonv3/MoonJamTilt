using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGameController : MonoBehaviour {

	GameObject _ball;
	GameObject ball{
		get{
			if (_ball == null)
				_ball = Instantiate(Resources.Load ("Prefabs/Ball") as GameObject);

			return _ball;
		}
	}

	public List<Level> allLevels;
	public List<SimpleTeleporter> allTeleporters;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < allTeleporters.Count; i++) 
		{
			allTeleporters[i].triggeredA += HandleATriggered;
			allTeleporters[i].triggeredB += HandleBTriggered;
		}

		for(int i=0; i<allLevels.Count; i++)
		{
			allLevels [i].FinishedLoading += HandleLevelFinishedLoading;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButton (0)) {
			for (int i = 0; i < allLevels.Count; i++)
				allLevels [i].endPoint.OnTriggerEnter (null);
		}
	}

	public void HandleATriggered(SimpleTeleporter teleporter)
	{
		ball.transform.position = teleporter.b.transform.position + Vector3.up;
	}

	public void HandleBTriggered(SimpleTeleporter teleporter)
	{
		ball.transform.position = teleporter.a.transform.position + Vector3.up;
	}

	public void HandleLevelFinishedLoading(Level level)
	{
		ball.transform.position = (level.spawnPoint != null ? level.spawnPoint.transform.position : level.root.transform.position) + new Vector3 (0f,.07f,0f);
	}
}
