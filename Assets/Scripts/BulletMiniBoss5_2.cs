using System;
using UnityEngine;

public class BulletMiniBoss5_2 : Bullet
{
	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.isReady = false;
			this.callback(this);
		}
		catch
		{
		}
	}

	protected void OnCollisionEnter2D(Collision2D coll)
	{
		GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
		base.gameObject.SetActive(false);
		if (!coll.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		ISkill component = coll.transform.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		IHealth component2 = coll.gameObject.GetComponent<IHealth>();
		if (component2 != null)
		{
			component2.AddHealthPoint(-this.Damage, EWeapon.NONE);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
		base.gameObject.SetActive(false);
		if (!collision.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		ISkill component = collision.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		IHealth component2 = collision.gameObject.GetComponent<IHealth>();
		if (component2 != null)
		{
			component2.AddHealthPoint(-this.Damage, EWeapon.NONE);
		}
	}

	public void Init(Vector3 pos, Vector2 Direction, float Damage, float Speed, Action<BulletMiniBoss5_2> callback = null)
	{
		this.InitObject();
		this.callback = callback;
		base.gameObject.SetActive(true);
		this.SetValue(EWeapon.NONE, 0, pos, Direction, Damage, Speed, 0f);
	}

	public override void SetValue(EWeapon weapon, int type, Vector3 pos, Vector2 Direction, float Damage, float Speed, float angle = 0f)
	{
		base.SetValue(weapon, type, pos, Direction, Damage, Speed, angle);
		Quaternion rotation = Quaternion.LookRotation(Direction, Vector3.forward);
		rotation.x = 0f;
		rotation.y = 0f;
		rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z + angle);
		this.transform.rotation = rotation;
		this.isReady = true;
		this.rigidbody2D.mass = 0.01f;
	}

	public override void UpdateObject()
	{
		if (!this.isReady)
		{
			return;
		}
		this.rigidbody2D.velocity = -this.transform.up * this.Speed;
	}

	private void OnBecameInvisible()
	{
		base.gameObject.SetActive(false);
	}

	public Action<BulletMiniBoss5_2> callback;
}
