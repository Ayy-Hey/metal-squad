using System;
using MyDataLoader;
using UnityEngine;

public class BoxBossMaraDevil : BaseEnemy
{
	public void InitEnemy(float hp)
	{
		Enemy[] enemy = new Enemy[]
		{
			new Enemy()
		};
		this.InitEnemy(new EnemyCharactor
		{
			enemy = enemy
		}, 0);
		this.cacheEnemy.HP = hp;
		this.HP = hp;
		base.isInCamera = true;
		this.State = ECharactor.IDLE;
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.gameObject.SetActive(true);
			this.lineBloodEnemy.Reset();
		}
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
		}
		this.isInit = true;
	}

	public override void Hit(float damage)
	{
		this.OnHit(damage, this.lastWeapon);
	}

	public Action<float, EWeapon> OnHit;
}
