using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleScale : MonoBehaviour {

	Vector3 cachedScale;
	public float deltaScale;
	public float time;

	// Use this for initialization
	void Start () {
		cachedScale = this.transform.localScale;
		Out ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Out()
	{
		DOTween.Kill (this.transform);
		this.transform.DOScale (cachedScale * deltaScale, time).SetEase(DG.Tweening.Ease.InOutQuad).OnComplete(()=>In());
	}

	public void In()
	{
		DOTween.Kill (this.transform);
		this.transform.DOScale (cachedScale, time).SetEase(DG.Tweening.Ease.InOutQuad).OnComplete(()=>Out());
	}
}
