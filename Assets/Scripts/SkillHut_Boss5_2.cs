using System;
using UnityEngine;

public class SkillHut_Boss5_2 : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (!coll.CompareTag("Rambo"))
		{
			return;
		}
		ISkill component = coll.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		IMoving component2 = coll.GetComponent<IMoving>();
		if (component2 == null)
		{
			return;
		}
		component2.Freeze(this.boss52.transform.position, 1f, this.boss52.timeSlowWhenHut, this.boss52.minusSpeedAffterHut);
		IHealth component3 = coll.GetComponent<IHealth>();
		if (component3 != null)
		{
			component3.AddHealthPoint(-this.boss52.damageHut, EWeapon.NONE);
		}
	}

	[SerializeField]
	private Boss5_2 boss52;
}
