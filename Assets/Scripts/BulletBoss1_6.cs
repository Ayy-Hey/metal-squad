using System;
using UnityEngine;

public class BulletBoss1_6 : Bullet
{
	public override void SetValue(EWeapon weapon, int type, Vector3 pos, Vector2 Direction, float Damage, float Speed, float angle = 0f)
	{
		base.SetValue(weapon, type, pos, Direction, Damage, Speed, 0f);
		this.transform.rotation = Quaternion.identity;
		this.Damage = Damage;
		this.transform.position = pos;
		Quaternion quaternion = Quaternion.LookRotation(Direction.normalized, Vector3.forward);
		quaternion.x = 0f;
		quaternion.y = 0f;
		this.transform.eulerAngles = new Vector3(0f, 0f, quaternion.eulerAngles.z + angle);
		this.isReady = true;
		this.lateSpeed = Speed * 0.5f;
	}

	public override void UpdateObject()
	{
		base.UpdateObject();
		this.Speed = Mathf.SmoothDamp(this.Speed, this.lateSpeed, ref this.smoothVelo, 2f);
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		IHealth component = coll.GetComponent<IHealth>();
		if (component != null)
		{
			ISkill component2 = coll.GetComponent<ISkill>();
			if (component2 != null && component2.IsInVisible())
			{
				return;
			}
			component.AddHealthPoint(-this.Damage, EWeapon.NONE);
			if (this.ActionAttackToRambo != null)
			{
				this.ActionAttackToRambo();
			}
		}
		GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
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

	public Action<BulletBoss1_6> DisableAction;

	public Action ActionAttackToRambo;

	private float lateSpeed;

	private float smoothVelo;
}
