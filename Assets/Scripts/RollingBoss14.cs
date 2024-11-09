using System;
using UnityEngine;

public class RollingBoss14 : MonoBehaviour
{
	public void Active(float damage)
	{
		this.box.enabled = true;
		this.damage = damage;
	}

	public void InActive()
	{
		this.box.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.transform.CompareTag("Rambo"))
		{
			IHealth component = coll.transform.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.damage, EWeapon.NONE);
			}
		}
	}

	[SerializeField]
	private Collider2D box;

	private float damage;
}
