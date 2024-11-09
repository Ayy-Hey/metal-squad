using System;
using UnityEngine;

public class GrenadesAttack : MonoBehaviour
{
	protected void OnTriggerEnter2D(Collider2D coll)
	{
		IHealth component;
		if (coll.CompareTag("Explosive"))
		{
			component = coll.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.bombScript.Damage, EWeapon.BOMB);
			}
			return;
		}
		if (this.bombScript.CompareTag("BombPlayer") && coll.CompareTag("Rambo"))
		{
			return;
		}
		if (this.bombScript.CompareTag("BombEnemy") && !coll.CompareTag("Rambo"))
		{
			return;
		}
		component = coll.GetComponent<IHealth>();
		if (component != null)
		{
			component.AddHealthPoint(-this.bombScript.Damage, EWeapon.BOMB);
		}
	}

	public Bomb bombScript;
}
