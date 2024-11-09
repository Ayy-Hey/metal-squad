using System;
using UnityEngine;

public class SkillBoss5_1 : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		IHealth component = collision.gameObject.GetComponent<IHealth>();
		if (component != null && this.AttackDamage != null)
		{
			ISkill component2 = collision.GetComponent<ISkill>();
			if (component2 != null && component2.IsInVisible())
			{
				return;
			}
			this.AttackDamage(component);
		}
		else
		{
			Log.Info("damage rambo");
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		IHealth component = collision.gameObject.GetComponent<IHealth>();
		if (component != null && this.AttackDamage != null)
		{
			ISkill component2 = collision.transform.GetComponent<ISkill>();
			if (component2 != null && component2.IsInVisible())
			{
				return;
			}
			this.AttackDamage(component);
		}
		else
		{
			Log.Info("damage rambo");
		}
	}

	public Action<IHealth> AttackDamage;

	public Rigidbody2D rigid;
}
