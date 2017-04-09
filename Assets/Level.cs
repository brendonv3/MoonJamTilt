using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Level : MonoBehaviour {

	public Transform spawnPoint;
	public Transform root;
	float hiddenScale=.001f;
	public Action<Level> FinishedLoading;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show(){
		DOTween.Kill (root);
		root.DOScale (1f, .3f).SetEase(DG.Tweening.Ease.InOutQuint).OnComplete(()=>ShowOnComplete());
	}

	public void Hide(){
		DOTween.Kill (root);
		root.DOScale (.001f, .3f).SetEase(DG.Tweening.Ease.InOutQuint);
	}

	public void ShowOnComplete()
	{
		if (FinishedLoading != null)
			FinishedLoading (this);
	}

}
