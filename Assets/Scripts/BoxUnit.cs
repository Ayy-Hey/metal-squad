using System;
using MyDataLoader;

public class BoxUnit : BaseEnemy
{
	public void InitEnemy(float hp)
	{
		Enemy[] array = new Enemy[]
		{
			new Enemy()
		};
		array[0].HP = hp;
		this.HP = hp;
		this.cacheEnemy = array[0];
		this.State = ECharactor.IDLE;
		this.isInit = true;
	}

	public override void Hit(float damage)
	{
		this.OnHitDamage(this.Id, damage);
	}

	public int Id;

	public Action<int, float> OnHitDamage;

	public Action<EWeapon> OnHitWeapon;
}
