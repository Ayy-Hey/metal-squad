using System;
using UnityEngine;

public class Traps : MonoBehaviour
{
	private void Start()
	{
		this.countdowm = Time.timeSinceLevelLoad;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Rambo") && Time.timeSinceLevelLoad - this.countdowm >= 0.5f)
		{
			BaseRambo component = other.GetComponent<BaseRambo>();
			if (component != null)
			{
				component.DownSpeed(0.3f);
			}
			IHealth component2 = other.GetComponent<IHealth>();
			if (component2 != null)
			{
				component2.AddHealthPoint(-50f, EWeapon.NONE);
			}
			this.countdowm = Time.timeSinceLevelLoad;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			BaseRambo component = other.GetComponent<BaseRambo>();
			if (component != null)
			{
				component.NormalSpeed();
			}
		}
	}

	private float countdowm;
}
