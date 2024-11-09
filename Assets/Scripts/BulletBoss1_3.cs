using System;
using UnityEngine;

public class BulletBoss1_3 : Bullet
{
	public override void SetValue(EWeapon weapon, int type, Vector3 pos, Vector2 Direction, float Damage, float Speed, float angle = 0f)
	{
		base.SetValue(weapon, type, pos, Direction, Damage, Speed, 0f);
		this.transform.rotation = Quaternion.identity;
		this.Damage = Damage;
		this.transform.position = pos;
		Quaternion rotation = Quaternion.LookRotation(Direction.normalized, Vector3.forward);
		rotation.x = 0f;
		rotation.y = 0f;
		this.transform.rotation = rotation;
		this.isReady = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			IHealth component = collision.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.Damage, EWeapon.NONE);
			}
		}
		GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
		base.gameObject.SetActive(false);
	}

	protected void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.transform.CompareTag("Rambo"))
		{
			IHealth component = coll.gameObject.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.Damage, EWeapon.NONE);
			}
		}
		GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
		base.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		if (this.DisableAction != null)
		{
			this.DisableAction(this);
		}
		this.isInit = false;
		this.transform.localScale = Vector3.one;
	}

	public Action<BulletBoss1_3> DisableAction;
}
