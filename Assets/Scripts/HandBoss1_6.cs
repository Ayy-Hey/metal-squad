using System;
using UnityEngine;

public class HandBoss1_6 : MonoBehaviour
{
	public void SetDamage(float damage)
	{
		this.damage = damage;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		IHealth component = collision.gameObject.GetComponent<IHealth>();
		if (component != null)
		{
			ISkill component2 = collision.GetComponent<ISkill>();
			if (component2 != null && component2.IsInVisible())
			{
				return;
			}
			component.AddHealthPoint(-this.damage, EWeapon.NONE);
		}
		else
		{
			Log.Info("damage rambo");
		}
	}

	public Collider2D col;

	public Collider2D col2;

	private float damage;
}
