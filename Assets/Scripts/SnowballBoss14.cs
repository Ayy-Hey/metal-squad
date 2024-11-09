using System;
using UnityEngine;

public class SnowballBoss14 : CachingMonoBehaviour
{
	public void SnowBall(float speed, float damage, Vector3 pos, Action disable)
	{
		this.actionDisable = disable;
		this.velocity.x = speed;
		this.rotateZ = (float)((speed <= 0f) ? 1 : -1);
		this.damage = damage;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		base.Invoke("InActive", 5f);
	}

	public override void UpdateObject()
	{
		base.UpdateObject();
		this.rigidbody2D.velocity = this.velocity;
		this.transform.Rotate(0f, 0f, this.rotateZ);
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.transform.CompareTag("Rambo"))
		{
			IHealth component = coll.transform.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.damage, EWeapon.NONE);
			}
			coll.transform.GetComponent<BaseRambo>().DownSpeed();
			this.InActive();
		}
	}

	private void InActive()
	{
		base.gameObject.SetActive(false);
		if (this.actionDisable != null)
		{
			this.actionDisable();
		}
	}

	private void OnDisable()
	{
		base.CancelInvoke();
	}

	private Action actionDisable;

	private float damage;

	private Vector2 velocity;

	private float rotateZ;
}
