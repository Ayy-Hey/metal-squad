using System;
using MyDataLoader;
using UnityEngine;

public class KhienBoss1_2 : BaseEnemy
{
	public void InitObject(Vector3 position, bool flip = false)
	{
		if (this.cacheEnemy == null)
		{
			EnemyCharactor enemyCharactor = new EnemyCharactor();
			enemyCharactor.enemy = new Enemy[1];
			this.cacheEnemy = new Enemy();
			this.InitEnemy(enemyCharactor, 0);
		}
		this.cacheEnemy.HP = (this.HP = 10000f);
		GameManager.Instance.ListEnemy.Add(this);
		base.gameObject.SetActive(true);
		this.transform.position = position;
		this.euler.z = (float)((!flip) ? 0 : 180);
		this.transform.eulerAngles = this.euler;
		this.scale.x = (this.scale.y = (this.scale.z = 0.1f));
		this.transform.localScale = this.scale;
		this.active = true;
		this.isInit = true;
	}

	public void UpdateObject(float deltaTime)
	{
		if (this.isInit)
		{
			if (this.active)
			{
				if (this.scale.x < 1f)
				{
					this.Scale(1f, deltaTime);
				}
			}
			else if (this.scale.x > 0.1f)
			{
				this.Scale(0.1f, deltaTime);
			}
			else
			{
				this.isInit = false;
				base.gameObject.SetActive(false);
				GameManager.Instance.ListEnemy.Remove(this);
			}
		}
	}

	private void Scale(float scaleTo, float deltaTime)
	{
		this.scale = this.transform.localScale;
		this.scale.y = (this.scale.z = (this.scale.x = Mathf.MoveTowards(this.scale.x, scaleTo, deltaTime * 2f)));
		this.transform.localScale = this.scale;
	}

	public void Off()
	{
		this.active = false;
	}

	private Vector3 euler = Vector3.zero;

	private Vector3 scale = Vector3.zero;

	private bool active;
}
