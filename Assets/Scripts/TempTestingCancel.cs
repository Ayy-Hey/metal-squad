using System;
using UnityEngine;

public class TempTestingCancel : MonoBehaviour
{
	private void Start()
	{
		this.tween = LeanTween.move(base.gameObject, base.transform.position + Vector3.one * 3f, (float)UnityEngine.Random.Range(2, 2)).setRepeat(-1).setLoopClamp();
	}

	public void Update()
	{
		if (this.tween != null)
		{
			this.isTweening = LeanTween.isTweening(base.gameObject);
			if (this.tweenOverride)
			{
				LeanTween.cancel(base.gameObject);
			}
		}
	}

	public bool isTweening;

	public bool tweenOverride;

	private LTDescr tween;
}
