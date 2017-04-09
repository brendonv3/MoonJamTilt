using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level : MonoBehaviour {

	public Transform root;
	float hiddenScale=.001f;

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

	public void Show(){
		DOTween.Kill (root);
		root.DOScale (1f, .3f).SetEase(DG.Tweening.Ease.InOutQuint).OnComplete(()=>ShowOnComplete());
		//root.transform.DOScale ();
	}

	public void Hide(){
		Destroy (ball);
		DOTween.Kill (root);
		root.DOScale (.001f, .3f).SetEase(DG.Tweening.Ease.InOutQuint);
	}

	public void ShowOnComplete()
	{
		ball.transform.position = root.transform.position + new Vector3 (0f,.1f,0f);
	}

}
