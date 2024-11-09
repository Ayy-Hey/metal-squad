using System;
using DG.Tweening;
using UnityEngine;

public class RotationHelper : MonoBehaviour
{
	private void Start()
	{
		base.transform.DORotate(new Vector3((float)this.rotation, 0f, 0f), this.duration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
	}

	public float duration;

	public int rotation;
}
