using System;
using UnityEngine;

public class SkillSet_Boss5_2 : MonoBehaviour
{
	protected void OnTriggerEnter2D(Collider2D coll)
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
		IHealth component2 = coll.gameObject.GetComponent<IHealth>();
		if (component2 != null && this.OnSkillSet != null)
		{
			this.OnSkillSet(component2);
		}
	}

	public Action<IHealth> OnSkillSet;
}
