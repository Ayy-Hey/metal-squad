using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Enemy33 : BaseHumanLv2
{
	private void OnDisable()
	{
		this.skeletonAnimation.state.Event -= this.HandleEvent;
		this.skeletonAnimation.state.Complete -= this.HandleComplete;
		this.skeletonAnimation.skeleton.SetColor(new Color(1f, 1f, 1f, 1f));
		this.boxCollider.enabled = false;
		this.isInit = false;
		try
		{
			GameManager.Instance.ListEnemy.Remove(this);
			EnemyManager.Instance.EnemyPool330.Store(this);
		}
		catch
		{
		}
		base.StopAllCoroutines();
	}

	public void OnInitEnemy(int Level, Vector2 pos)
	{
		EnemyCharactor enemyCharactor = new EnemyCharactor();
		enemyCharactor.enemy = new Enemy[1];
		enemyCharactor.enemy[0] = new Enemy();
		enemyCharactor.enemy[0].Level = Level;
		enemyCharactor.enemy[0].HP = this.data.datas[Level].hp;
		enemyCharactor.enemy[0].Speed = this.data.datas[Level].speed;
		enemyCharactor.enemy[0].DamageLv2 = this.data.datas[Level].damage;
		enemyCharactor.enemy[0].Radius_Damage = 0.5f;
		enemyCharactor.enemy[0].Vision_Min = 5f;
		enemyCharactor.enemy[0].Vision_Max = this.data.datas[Level].maxVision;
		enemyCharactor.enemy[0].Time_Reload_AttackLv2 = this.data.datas[Level].timeReload;
		enemyCharactor.enemy[0].Speed_BulletLv2 = 8f;
		base.OnInit(enemyCharactor, 0, pos);
		this.TargetCurrent = base.GetTarget();
		this.GunTipBone = this.skeletonAnimation.skeleton.FindBone("Gun_tip01");
		this.GunTipBone2 = this.skeletonAnimation.skeleton.FindBone("Gun_tip02");
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.cacheEnemyData.ismove = false;
		this.isInit = true;
	}

	public void OnUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		this.circleColliderHead.offset = new Vector2((float)((!this.skeletonAnimation.skeleton.FlipX) ? -1 : 1) * this.CircleHeadCache.x, this.CircleHeadCache.y);
		base.OnUpdateAI();
	}

	protected override void OnShoot()
	{
		base.OnShoot();
		base.SetAttack1();
	}

	protected override void ChangeLevel(BaseEnemy.ELevel eLevel)
	{
	}

	private void Shoot(Vector2 pos)
	{
		BulletEnemy bulletEnemy = GameManager.Instance.bulletManager.CreateBulletEnemy(11, (!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left, pos, this.cacheEnemy.DamageLv2 * this.DAMAGE_SCALE, this.cacheEnemy.Speed_BulletLv2, 0f);
		bulletEnemy.spriteRenderer.flipX = false;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		Vector2 pos = new Vector2(this.transform.position.x + this.GunTipBone.WorldX, this.transform.position.y + this.GunTipBone.WorldY);
		string name = e.Data.Name;
		if (name != null)
		{
			if (name == "shoot")
			{
				this.Shoot(pos);
			}
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		this.skeletonAnimation.state.ClearTrack(entry.TrackIndex);
		string text = entry.ToString().Split(new char[]
		{
			'_'
		})[0];
		if (text != null)
		{
			if (text == "Death01" || text == "Death02" || text == "Death03" || text == "Death04" || text == "Death05" || text == "Death06")
			{
				this.skeletonAnimation.state.SetEmptyAnimations(0f);
				this.boxCollider.enabled = false;
				if (this.circleColliderHead != null)
				{
					this.circleColliderHead.enabled = false;
				}
				this.rigidbody2D.isKinematic = true;
				base.gameObject.SetActive(false);
			}
		}
	}

	private Vector2 CircleHeadCache = new Vector2(0.52f, 1.37f);

	[SerializeField]
	private Collider2D circleColliderHead;

	[SerializeField]
	private DataEVL data;
}
