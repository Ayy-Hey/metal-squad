using System;
using UnityEngine;

public class PlayerJetpack
{
	public PlayerJetpack()
	{
		this.Power = 0.3f;
		this.isInit = true;
		int num = (int)(this.Power * 10f);
		if (GameManager.Instance.isJetpackMode)
		{
			for (int i = 0; i < 10; i++)
			{
				JetpackManager.Instance.ListLinePower[i].gameObject.SetActive(num >= i + 1);
			}
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit || !GameManager.Instance.isJetpackMode || this.Power <= 0f || GameManager.Instance.player._controller.isGrounded)
		{
			return;
		}
		this.Power -= deltaTime * 0.1f;
		this.Power = Mathf.Clamp01(this.Power);
		int num = (int)(this.Power * 10f);
		for (int i = 0; i < 10; i++)
		{
			JetpackManager.Instance.ListLinePower[i].gameObject.SetActive(num >= i + 1);
		}
	}

	public void AddPower()
	{
		this.Power += 0.3f;
		int num = (int)(this.Power * 10f);
		for (int i = 0; i < 10; i++)
		{
			JetpackManager.Instance.ListLinePower[i].gameObject.SetActive(num >= i + 1);
		}
	}

	public bool isReadyFly
	{
		get
		{
			return this.Power > 0f && GameManager.Instance.isJetpackMode;
		}
	}

	private float Power;

	private bool isInit;
}
