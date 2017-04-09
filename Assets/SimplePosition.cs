using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimplePosition : MonoBehaviour {


	public Vector3 deltaPosition;
	public float time;
	Vector3 cachedPosition;

	void Start () {
		//	CAche start posiiton
		cachedPosition = this.transform.localPosition;

		//	Start tween
		Debug.Log("start");

		Out ();
	}

	void Out(){
		DOTween.Kill (this.transform);
		this.transform.DOLocalMove ((cachedPosition + deltaPosition), time).SetEase(DG.Tweening.Ease.InOutQuad).OnComplete(()=>In());
	}

	void In(){
		DOTween.Kill (this.transform);
		this.transform.DOLocalMove ((cachedPosition), time).SetEase(DG.Tweening.Ease.InOutQuad).OnComplete(()=>Out());
	}
}
