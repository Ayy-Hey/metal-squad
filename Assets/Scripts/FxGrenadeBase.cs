using System;
using UnityEngine;

public class FxGrenadeBase : MonoBehaviour
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
		if (this.timeHide > 0.7f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private float timeHide;

	private bool isInit;

	private const float TIME_HIDE = 0.7f;

	[SerializeField]
	private Animator anim;

	public bool isPreview;
}
