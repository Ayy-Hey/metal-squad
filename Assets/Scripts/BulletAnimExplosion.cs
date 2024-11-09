using System;
using UnityEngine;

public class BulletAnimExplosion : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			IHealth component = other.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.bullet.Damage, EWeapon.BOMB);
			}
			base.gameObject.SetActive(false);
		}
	}

	public BulletAnim bullet;
}
