using System;
using UnityEngine;

public class Trap1 : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			this.timeDamage = 0f;
			IHealth component = other.GetComponent<IHealth>();
			if (!object.ReferenceEquals(component, null))
			{
				component.AddHealthPoint(-this.DAMAGE, EWeapon.NONE);
			}
			ISpeed component2 = other.GetComponent<ISpeed>();
			if (!object.ReferenceEquals(component2, null))
			{
				component2.DownSpeed(0.3f);
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.CompareTag("Rambo"))
		{
			return;
		}
		this.timeDamage += Time.deltaTime;
		if (this.timeDamage > 1f)
		{
			this.timeDamage = 0f;
			IHealth component = other.GetComponent<IHealth>();
			if (!object.ReferenceEquals(component, null))
			{
				component.AddHealthPoint(-this.DAMAGE, EWeapon.NONE);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Rambo"))
		{
			return;
		}
		ISpeed component = other.GetComponent<ISpeed>();
		if (!object.ReferenceEquals(component, null))
		{
			component.NormalSpeed();
		}
	}

	[SerializeField]
	private float DAMAGE;

	private float timeDamage;
}
