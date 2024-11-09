using System;
using UnityEngine;

public class TrapLaserRotate : CachingMonoBehaviour
{
	private void Update()
	{
		this.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * this.Speed));
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			IHealth component = other.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.Damage, EWeapon.LASER);
			}
		}
	}

	[SerializeField]
	private float Speed = 2f;

	[SerializeField]
	private float Damage = 20f;
}
