using System;
using UnityEngine;

public class GrenadeBasic : BaseGrenade
{
	public override void OnInit(PlayerMain _player, bool FlipX, bool hasDamage = true)
	{
		this.player = _player;
		this.isInit = true;
		this.rigidbody2D.isKinematic = false;
		this.Radius = ProfileManager.grenadesProfile[0].GetOption(EGrenadeOption.Damage_Range);
		if (hasDamage)
		{
			this.Damaged = ProfileManager.grenadesProfile[0].GetOption(EGrenadeOption.Damage);
			this.Damaged -= this.Damaged * GameManager.Instance.RatePower;
		}
		else
		{
			this.Damaged = 0f;
		}
		int force_Bomb = this.player._PlayerData.ramboProfile.grenades.Force_Bomb;
		float d = (float)force_Bomb * this.rigidbody2D.mass;
		this.rigidbody2D.AddForce(new Vector2((!FlipX) ? 0.7f : -0.7f, 0.8f) * d);
		this.weaponCurrent = EWeapon.GRENADE_M61;
		base.SetSpine(ProfileManager.grenadesProfile[0].LevelUpGrade);
		this.OnTrigerEnter = (Action)Delegate.Combine(this.OnTrigerEnter, new Action(delegate()
		{
			GameManager.Instance.fxManager.CreateGrenadeBasic(this.transform.position).transform.localScale = new Vector3(3f, 3f, 3f);
		}));
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Instance.bombManager.PoolBaseGrenade[0].Store(this);
		Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, this.Radius, this.layerMask);
		for (int i = 0; i < array.Length; i++)
		{
			BaseEnemy component = array[i].GetComponent<BaseEnemy>();
			if (component != null && component.isInCamera)
			{
				component.AddHealthPoint(-this.Damaged, this.weaponCurrent);
			}
		}
	}
}
