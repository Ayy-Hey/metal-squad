using System;
using UnityEngine;

public class DalanAttack : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			IHealth component = other.GetComponent<IHealth>();
			if (component != null && !this.isAttack)
			{
				component.AddHealthPoint(-this.dalan.Damaged, EWeapon.DALAN);
				this.isAttack = true;
			}
		}
		if (other.CompareTag("Gulf"))
		{
			this.dalan.gameObject.SetActive(false);
			this.isAttack = false;
		}
	}

	[SerializeField]
	private Dalan dalan;

	private bool isAttack;
}
