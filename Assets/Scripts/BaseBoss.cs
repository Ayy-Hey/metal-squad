using System;
using UnityEngine;

public class BaseBoss : BaseEnemy
{
	public virtual void Init()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	public virtual void OnUpdateBoss(float deltaTime)
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (!this.isPaused)
			{
				this.OnPause(true);
			}
			return;
		}
		if (this.isPaused)
		{
			this.OnPause(false);
		}
		if (!this.isChangeState)
		{
			this.OnStartState();
		}
		else
		{
			this.OnUpdateState(deltaTime);
		}
	}

	protected virtual void OnStartState()
	{
		this.isChangeState = true;
	}

	protected virtual void OnUpdateState(float deltaTime)
	{
	}

	protected virtual void OnChangeState()
	{
		this.isChangeState = false;
	}

	protected virtual void OnPause(bool v)
	{
		this.isPaused = v;
	}

	public override void Hit(float damage)
	{
		base.Hit(damage);
		if (this.HP < 0f)
		{
			GameManager.Instance.ListEnemy.Remove(this);
			GameManager.Instance.hudManager.HideControl();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			this.OnDie();
			return;
		}
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		this.OnHit();
	}

	protected virtual void OnHit()
	{
	}

	protected virtual void OnDie()
	{
		this.isInit = false;
	}

	public EBoss boss;

	protected bool isChangeState;

	protected bool isPaused;

	[SerializeField]
	protected TextAsset Data_Encrypt;

	[SerializeField]
	protected TextAsset Data_Decrypt;
}
