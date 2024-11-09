using System;
using UnityEngine;

public class FxIce : MonoBehaviour
{
	public void OnInit()
	{
		this.isInit = true;
		this.anim.Play(0);
		this.timeHide = 0f;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.timeHide += deltaTime;
		if (this.timeHide > 0.4f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		try
		{
			GameManager.Instance.fxManager.PoolFxIce.Store(this);
		}
		catch
		{
		}
	}

	private float timeHide;

	private bool isInit;

	private const float TIME_HIDE = 0.4f;

	[SerializeField]
	private Animator anim;
}
