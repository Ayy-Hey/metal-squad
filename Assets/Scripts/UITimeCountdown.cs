using System;
using UnityEngine;

public class UITimeCountdown : MonoBehaviour
{
	private void LateUpdate()
	{
		if (!this.isReset || this.isDone)
		{
			return;
		}
		if (GameManager.Instance.StateManager.EState == EGamePlay.WAITING_BOSS)
		{
			base.gameObject.SetActive(false);
			this.isDone = true;
		}
	}

	public bool isReset;

	public Vector2 vt2NewPos;

	private bool isDone;

	public RectTransform thisTransform;
}
