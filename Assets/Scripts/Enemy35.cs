using System;
using System.Collections.Generic;
using MyDataLoader;
using Spine;
using UnityEngine;

public class Enemy35 : BaseEnemySpine
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map14;
	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		base.Hit(damage);
		this.SetHit();
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			GameManager.Instance.hudManager.combo.ShowCombo(this.lastWeapon);
			if (this.cacheEnemyData.gift)
			{
				GameManager.Instance.giftManager.Create(this.transform.position, this.cacheEnemyData.gift_value);
			}
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
			this.State = ECharactor.DIE;
			base.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		this.skeletonAnimation.state.Event -= this.HandleEvent;
		this.skeletonAnimation.state.Complete -= this.HandleComplete;
		try
		{
			GameManager.Instance.ListEnemy.Remove(this);
			EnemyManager.Instance.EnemyPool350.Store(this);
		}
		catch
		{
		}
		this.State = ECharactor.NONE;
		base.StopAllCoroutines();
	}

	public void OnUpdate()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.HORIZONTAL)
		{
			if (orientaltion == CameraController.Orientation.VERTICAL)
			{
				if (this.transform.position.y < CameraController.Instance.transform.position.y && !CameraController.Instance.IsInView(this.transform))
				{
					if (!object.ReferenceEquals(DataLoader.LevelDataCurrent, null) && this.isCreateWithJson)
					{
						DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy--;
					}
					this.State = ECharactor.DIE;
					base.gameObject.SetActive(false);
					return;
				}
			}
		}
		else if (this.transform.position.x < CameraController.Instance.transform.position.x && !CameraController.Instance.IsInView(this.transform))
		{
			if (!object.ReferenceEquals(DataLoader.LevelDataCurrent, null) && this.isCreateWithJson)
			{
				DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy--;
			}
			this.State = ECharactor.DIE;
			base.gameObject.SetActive(false);
			return;
		}
		Vector3 position = this.transform.position;
		TrackEntry current = this.skeletonAnimation.state.GetCurrent(0);
		ECharactor state = this.State;
		if (state != ECharactor.RUN)
		{
			if (state == ECharactor.IDLE)
			{
				position.y = CameraController.Instance.transform.position.y + 3f;
				this.transform.position = position;
				if (current == null || current.Animation != this.IdleAnim)
				{
					this.skeletonAnimation.state.SetAnimation(0, this.IdleAnim, true);
				}
				this.skeletonAnimation.loop = true;
				this.skeletonAnimation.AnimationName = this.idleAnim;
				this.TimeDelay += Time.deltaTime;
				if (this.TimeDelay >= 1f && !this.isAttack)
				{
					this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim, false);
					this.isAttack = true;
				}
			}
		}
		else
		{
			if (current == null || current.Animation != this.WalkAnim)
			{
				this.skeletonAnimation.state.SetAnimation(0, this.WalkAnim, true);
			}
			this.skeletonAnimation.loop = true;
			this.skeletonAnimation.AnimationName = this.walkAnim;
			position.y = CameraController.Instance.transform.position.y + 3f;
			this.transform.position = position;
			this.transform.Translate(Vector2.left * Time.deltaTime * this.Speed);
			if (this.Count > 0)
			{
				float num = Mathf.Abs(this.transform.position.x - (CameraController.Instance.transform.position.x + (float)(3 * this.Count - 6)));
				if (num <= 0.1f)
				{
					this.Count--;
					base.SetIdle();
					this.isAttack = false;
					this.TimeDelay = 0f;
				}
			}
		}
	}

	public void OnInitEnemy(int Levlel)
	{
		EnemyCharactor enemyCharactor = new EnemyCharactor();
		enemyCharactor.enemy = new Enemy[1];
		enemyCharactor.enemy[0] = new Enemy();
		enemyCharactor.enemy[0].Level = Levlel;
		enemyCharactor.enemy[0].HP = this.data.datas[Levlel].hp;
		enemyCharactor.enemy[0].Speed = this.data.datas[Levlel].speed;
		enemyCharactor.enemy[0].DamageLv2 = this.data.datas[Levlel].damage;
		enemyCharactor.enemy[0].Radius_Damage = 0.5f;
		enemyCharactor.enemy[0].Vision_Min = 5f;
		enemyCharactor.enemy[0].Vision_Max = this.data.datas[Levlel].maxVision;
		enemyCharactor.enemy[0].Time_Reload_AttackLv2 = this.data.datas[Levlel].timeReload;
		enemyCharactor.enemy[0].Speed_BulletLv2 = 8f;
		base.InitEnemy(enemyCharactor, 0);
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.skeletonAnimation.skeleton.FlipX = false;
		this.GunTipBone = this.skeletonAnimation.skeleton.FindBone("Gun_tip");
		base.SetRun();
		this.Count = 3;
		this.Step = 0;
		this.Speed = 1f;
		this.TimeDelay = 0f;
		this.isAttack = false;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		string name = e.Data.Name;
		if (name != null)
		{
			if (name == "Throw")
			{
				Vector3 pos = new Vector3(this.transform.position.x + this.GunTipBone.WorldX, this.transform.position.y + this.GunTipBone.WorldY);
				GameManager.Instance.bombManager.CreateBombV2(pos, this.cacheEnemy.DamageLv2);
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
			if (Enemy35._003C_003Ef__switch_0024map14 == null)
			{
				Enemy35._003C_003Ef__switch_0024map14 = new Dictionary<string, int>(7)
				{
					{
						"Attack01",
						0
					},
					{
						"Death01",
						1
					},
					{
						"Death02",
						1
					},
					{
						"Death03",
						1
					},
					{
						"Death04",
						1
					},
					{
						"Death05",
						1
					},
					{
						"Death06",
						1
					}
				};
			}
			int num;
			if (Enemy35._003C_003Ef__switch_0024map14.TryGetValue(text, out num))
			{
				if (num != 0)
				{
					if (num == 1)
					{
						this.skeletonAnimation.state.SetAnimation(0, this.IdleAnim, true);
						this.rigidbody2D.isKinematic = true;
					}
				}
				else
				{
					base.SetRun();
				}
			}
		}
	}

	[SerializeField]
	private DataEVL data;

	private int Count = 3;

	private int Step;

	private float Speed = 1f;

	private float TimeDelay;

	private bool isAttack;
}
