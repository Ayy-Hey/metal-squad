using System;
using UnityEngine;

public class GrenadeIce : BaseGrenade
{
	public override void OnInit(PlayerMain _player, bool FlipX, bool hasDamage = true)
	{
		this.player = _player;
		this.isInit = true;
		this.rigidbody2D.isKinematic = false;
		this.Radius = ProfileManager.grenadesProfile[3].GetOption(EGrenadeOption.Damage_Range);
		if (hasDamage)
		{
			this.Damaged = ProfileManager.grenadesProfile[3].GetOption(EGrenadeOption.Damage);
			this.Damaged -= this.Damaged * GameManager.Instance.RatePower;
		}
		else
		{
			this.Damaged = 0f;
		}
		int force_Bomb = this.player._PlayerData.ramboProfile.grenades.Force_Bomb;
		float d = (float)force_Bomb * this.rigidbody2D.mass;
		this.rigidbody2D.AddForce(new Vector2((!FlipX) ? 0.7f : -0.7f, 0.8f) * d);
		this.weaponCurrent = EWeapon.GRENADE_CHEMICAL;
		base.SetSpine(ProfileManager.grenadesProfile[3].LevelUpGrade);
		this.OnTrigerEnter = (Action)Delegate.Combine(this.OnTrigerEnter, new Action(delegate()
		{
			GameManager.Instance.fxManager.CreateGrenadeIce(this.transform.position);
		}));
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Instance.bombManager.PoolBaseGrenade[1].Store(this);
		Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, this.Radius, this.layerMask);
		for (int i = 0; i < array.Length; i++)
		{
			IIce component = array[i].GetComponent<IIce>();
			if (component != null)
			{
				component.Hit(-this.Damaged, EWeapon.GRENADE_ICE);
			}
			else
			{
				BaseEnemy component2 = array[i].GetComponent<BaseEnemy>();
				if (component2 != null && component2.isInCamera)
				{
					component2.AddHealthPoint(-this.Damaged, this.weaponCurrent);
				}
			}
		}
	}
}
