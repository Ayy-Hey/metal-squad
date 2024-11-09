using System;
using MyDataLoader;

public class BoxHit : BaseEnemy
{
	public void InitEnemy(float hp)
	{
		Enemy[] array = new Enemy[]
		{
			new Enemy()
		};
		array[0].HP = hp;
		this.HP = hp;
		EnemyCharactor enemyCharactor = new EnemyCharactor();
		enemyCharactor.enemy = array;
		this.cacheEnemy = array[0];
	}

	public override void Hit(float damage)
	{
		this.OnHitDamage(damage);
	}

	public Action<EWeapon> OnHitWeapon;

	public Action<float> OnHitDamage;
}
