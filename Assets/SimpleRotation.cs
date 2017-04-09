using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleRotation : MonoBehaviour {

	Quaternion cachedRotation;
	public Vector3 deltaRotation;
	public float time;

	// Use this for initialization
	void Start () {

		cachedRotation = this.transform.localRotation;

		Out ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Out(){
		DOTween.Kill (this.transform);
		this.transform.DOLocalRotate (cachedRotation.eulerAngles + deltaRotation, time).SetEase(DG.Tweening.Ease.InOutQuad).OnComplete(()=>In());
	}

	public void In(){
		DOTween.Kill (this.transform);
		this.transform.DOLocalRotate (cachedRotation.eulerAngles, time).SetEase(DG.Tweening.Ease.InOutQuad).OnComplete(()=>Out());
	}
}
