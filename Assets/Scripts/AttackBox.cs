using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : CachingMonoBehaviour
{
	private void OnValidate()
	{
		if (!this.collider)
		{
			this.collider = base.GetComponent<Collider2D>();
		}
		this.collider.isTrigger = true;
	}

	public void AddTargetTag(string tag)
	{
		if (this.listTargetTag == null)
		{
			this.listTargetTag = new List<string>();
		}
		this.listTargetTag.Add(tag);
	}

	public void Active(float damage, bool oneHit = true, Action callbackAttack = null)
	{
		this.callbackAttack = callbackAttack;
		this._damage = damage;
		this.isOneHit = oneHit;
		base.gameObject.SetActive(true);
		this.collider.enabled = true;
		this.isAttack = true;
	}

	public void Deactive()
	{
		this.isAttack = false;
		this.collider.enabled = false;
		base.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!this.isOneHit)
		{
			return;
		}
		if (this.listTargetTag.Contains(collision.tag))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component == null || !component.IsInVisible())
				{
					float num = this._damage * ((!this.flowMode) ? 1f : GameMode.Instance.GetMode());
					collision.GetComponent<IHealth>().AddHealthPoint(-num, this.weapon);
					if (this.callbackAttack != null)
					{
						this.callbackAttack();
					}
				}
			}
			catch
			{
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (this.isOneHit)
		{
			return;
		}
		if (this.listTargetTag.Contains(collision.tag))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component == null || !component.IsInVisible())
				{
					float num = this._damage * ((!this.flowMode) ? 1f : GameMode.Instance.GetMode());
					GameManager.Instance.player.AddHealthPoint(-num, this.weapon);
					if (this.callbackAttack != null)
					{
						this.callbackAttack();
					}
				}
			}
			catch
			{
			}
		}
	}

	public Collider2D collider;

	public bool isOneHit = true;

	public List<string> listTargetTag;

	public EWeapon weapon;

	public float _damage;

	public bool flowMode;

	[HideInInspector]
	public bool isAttack;

	private Action callbackAttack;
}
