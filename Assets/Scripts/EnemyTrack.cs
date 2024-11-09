using System;
using System.Collections;
using MyDataLoader;
using Spine;
using UnityEngine;

public class EnemyTrack : BaseEnemySpine
{
	public override void Hit(float damage)
	{
		if (this.isDead)
		{
			return;
		}
		base.Hit(damage);
		this.SetHit();
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			base.StartCoroutine(this.ShowEffectDie());
			this.objLayer10.gameObject.SetActive(false);
			this.objColliderGround.SetActive(false);
			this.box.enabled = false;
			this.SetDie();
		}
	}

	public void Init()
	{
		this.isDead = false;
		this.meshRenderer.enabled = true;
		Enemy[] array = new Enemy[]
		{
			new Enemy()
		};
		array[0].HP = (float)this.TOTAL_HP;
		this.InitEnemy(new EnemyCharactor
		{
			enemy = array
		});
		this.isInit = true;
		this.step = 0;
		base.SetIdle();
		this.skeletonAnimation.state.SetAnimation(0, this.idleAnim, false);
		this.countEnemy = this.TOTAL_ENEMY;
		this.objColliderGround.SetActive(true);
		this.objLayer10.gameObject.SetActive(true);
		this.box.enabled = true;
		if (this.meshRenderer != null)
		{
			this.meshRenderer.sortingOrder = 0;
		}
	}

	public void InitEnemy(EnemyCharactor enemy)
	{
		base.InitEnemy(enemy, 0);
		this.skeletonAnimation.skeleton.FlipX = false;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.skeletonAnimation.state.Event += this.HandleEvent;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		if (e.Data.Name.Equals("idle"))
		{
			this.meshRenderer.enabled = false;
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
			if (text == "Death01" || text == "Death02")
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	public void OnUpdate()
	{
		if (!this.isInit || this.State != ECharactor.IDLE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING || this.HP <= 0f)
		{
			return;
		}
		int num = this.step;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					this.objColliderGround.SetActive(false);
					this.objLayer10.gameObject.SetActive(false);
					this.box.enabled = false;
					this.isDead = true;
					this.transform.Translate(Vector3.right * Time.deltaTime * 6f);
					if (this.transform.position.x > CameraController.Instance.transform.position.x + 12f)
					{
						base.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				this.timeMoveOut += Time.deltaTime;
				if (this.timeMoveOut >= 3f)
				{
					this.step = 2;
				}
			}
		}
		else
		{
			if (this.countEnemy <= 0)
			{
				try
				{
					this.objLayer10.gameObject.SetActive(false);
					this.skeletonAnimation.state.SetAnimation(0, this.walkAnim, true);
					this.step = 1;
					this.objColliderGround.SetActive(false);
					this.objLayer10.gameObject.SetActive(false);
					this.timeMoveOut = 0f;
				}
				catch
				{
					UnityEngine.Debug.Log("ex");
				}
				finally
				{
					this.step = 1;
				}
				return;
			}
			this.timeCreateEnemy += Time.deltaTime;
			if (this.timeCreateEnemy >= this.TimeCreate && this.mEnemy <= 3)
			{
				this.TimeCreate = UnityEngine.Random.Range(1f, 3f);
				int num2 = UnityEngine.Random.Range(1, 3);
				for (int i = 0; i < num2; i++)
				{
					this.CreateEnemy();
					this.countEnemy--;
				}
				this.timeCreateEnemy = 0f;
			}
		}
	}

	private void CreateEnemy()
	{
		this.mEnemy++;
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					EnemyKnife enemy3 = EnemyManager.Instance.CreateEnemyKnife();
					EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
					enemyDataInfo.pos_x = this.tfPosCreateEnemy.position.x;
					enemyDataInfo.pos_y = this.tfPosCreateEnemy.position.y;
					enemyDataInfo.ismove = true;
					enemyDataInfo.type = 7;
					enemy3.Init(enemyDataInfo, delegate
					{
						EnemyManager.Instance.PoolEnemyKnife.Store(enemy3);
					});
					EnemyKnife enemy4 = enemy3;
					enemy4.OnEnemyDeaded = (Action)Delegate.Combine(enemy4.OnEnemyDeaded, new Action(delegate()
					{
						this.mEnemy--;
					}));
					enemy3.cacheEnemy.Vision_Min = 0.5f;
					enemy3.cacheEnemy.Vision_Max = 1.25f;
					bool gift = UnityEngine.Random.Range(0f, 1f) >= 8f;
					enemy3.cacheEnemyData.gift = gift;
					enemy3.cacheEnemyData.gift_value = 0;
					GameManager.Instance.ListEnemy.Add(enemy3);
					enemy3.isTrucker = true;
				}
			}
			else
			{
				EnemyGrenade enemy2 = EnemyManager.Instance.CreateEnemyGrenade();
				EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
				enemyDataInfo.ismove = true;
				enemyDataInfo.type = 2;
				enemyDataInfo.pos_x = this.tfPosCreateEnemy.position.x;
				enemyDataInfo.pos_y = this.tfPosCreateEnemy.position.y;
				enemy2.Init(enemyDataInfo, delegate
				{
					EnemyManager.Instance.PoolEnemyGrenade.Store(enemy2);
				});
				EnemyGrenade enemy5 = enemy2;
				enemy5.OnEnemyDeaded = (Action)Delegate.Combine(enemy5.OnEnemyDeaded, new Action(delegate()
				{
					this.mEnemy--;
				}));
				enemy2.cacheEnemy.Vision_Min = 3f;
				enemy2.cacheEnemy.Vision_Max = 9f;
				bool gift2 = UnityEngine.Random.Range(0f, 1f) >= 8f;
				enemy2.cacheEnemyData.gift = gift2;
				enemy2.cacheEnemyData.gift_value = 0;
				GameManager.Instance.ListEnemy.Add(enemy2);
				enemy2.isTrucker = true;
			}
		}
		else
		{
			EnemyAkm enemy = EnemyManager.Instance.CreateEnemyAkm();
			EnemyDataInfo enemyDataInfo2 = new EnemyDataInfo();
			enemyDataInfo2.ismove = true;
			enemyDataInfo2.pos_x = this.tfPosCreateEnemy.position.x;
			enemyDataInfo2.pos_y = this.tfPosCreateEnemy.position.y;
			enemyDataInfo2.type = 1;
			bool gift3 = UnityEngine.Random.Range(0f, 1f) >= 8f;
			enemyDataInfo2.gift = gift3;
			enemyDataInfo2.gift_value = 0;
			enemy.Init(enemyDataInfo2, delegate
			{
				EnemyManager.Instance.PoolAkm.Store(enemy);
			});
			EnemyAkm enemy6 = enemy;
			enemy6.OnEnemyDeaded = (Action)Delegate.Combine(enemy6.OnEnemyDeaded, new Action(delegate()
			{
				this.mEnemy--;
			}));
			enemy.cacheEnemy.Vision_Min = 3f;
			enemy.cacheEnemy.Vision_Max = 9f;
			GameManager.Instance.ListEnemy.Add(enemy);
			enemy.isTrucker = true;
		}
	}

	private void OnDisable()
	{
		try
		{
			if (!object.ReferenceEquals(DataLoader.LevelDataCurrent, null) && DataLoader.LevelDataCurrent.isReaded)
			{
				DataLoader.LevelDataCurrent.points[this.CheckPoint].totalEnemy--;
			}
			GameManager.Instance.TotalEnemyKilled++;
			EnemyManager.Instance.PoolTrack.Store(this);
		}
		catch
		{
		}
	}

	private IEnumerator ShowEffectDie()
	{
		GameManager.Instance.fxManager.ShowEffect(0, this.transform.position, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		Vector3 newpos = this.transform.position;
		newpos.x -= UnityEngine.Random.Range(0.5f, 1.5f);
		GameManager.Instance.fxManager.ShowEffect(0, newpos, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		Vector3 newpos2 = this.transform.position;
		newpos2.x += UnityEngine.Random.Range(0.5f, 1.5f);
		GameManager.Instance.fxManager.ShowEffect(5, newpos2, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		Vector3 newpos3 = this.transform.position;
		newpos3.x -= UnityEngine.Random.Range(0.5f, 1.5f);
		GameManager.Instance.fxManager.ShowEffect(0, newpos3, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		Vector3 newpos4 = this.transform.position;
		newpos4.x += UnityEngine.Random.Range(0.5f, 1.5f);
		GameManager.Instance.fxManager.ShowEffect(5, newpos4, Vector3.one, true, true);
		yield break;
	}

	private int TOTAL_ENEMY = 10;

	[SerializeField]
	private int TOTAL_HP = 300;

	[NonSerialized]
	public int step;

	private int countEnemy;

	private float timeCreateEnemy;

	private float TimeCreate = 1f;

	private float timeMoveOut;

	[SerializeField]
	private Transform tfPosCreateEnemy;

	[SerializeField]
	private GameObject objColliderGround;

	[SerializeField]
	private SpriteRenderer objLayer10;

	[SerializeField]
	private BoxCollider2D box;

	private bool isDead;

	private int mEnemy;
}
