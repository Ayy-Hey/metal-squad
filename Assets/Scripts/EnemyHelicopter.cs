using System;
using System.Collections;
using MyDataLoader;
using UnityEngine;

public class EnemyHelicopter : BaseEnemy
{
	public override void Hit(float damage)
	{
		base.Hit(damage);
		this.SetHit();
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP * GameMode.Instance.GetMode());
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.mMission.isDestroyCH_47 = true;
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
			{
				GameManager.Instance.mMission.StartCheck();
			}
			this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
			this.SetDie();
			this.rigidbody2D.isKinematic = false;
			this.rigidbody2D.gravityScale = 3f;
		}
	}

	public void Init()
	{
		this.meshRenderer.enabled = true;
		Enemy[] array = new Enemy[]
		{
			new Enemy()
		};
		this.HP = (array[0].HP = this.TOTAL_HP);
		this.InitEnemy(new EnemyCharactor
		{
			enemy = array
		}, 0);
		this.isInit = true;
		this.timeCreateEnemy = 0f;
		this.SetFlip(true);
		this.step = 0;
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP * GameMode.Instance.GetMode());
		this.isRunning = true;
		try
		{
			if (!GameManager.Instance.ListEnemy.Contains(this))
			{
				GameManager.Instance.ListEnemy.Add(this);
			}
		}
		catch
		{
		}
	}

	public void OnUpdate()
	{
		if (!this.isInit || this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this.isRunning)
		{
			this.TimeRestart += Time.deltaTime;
			if (this.TimeRestart >= 10f)
			{
				this.Restart();
			}
			return;
		}
		this.timeCreateEnemy += Time.deltaTime;
		Vector3 position = this.transform.position;
		position.y = CameraController.Instance.Position.y + CameraController.Instance.Size().y - 1f;
		this.transform.position = position;
		int num = this.step;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					this.transform.Translate(Vector3.left * Time.deltaTime * this.Speed);
					if (this.timeCreateEnemy >= 1f)
					{
						this.timeCreateEnemy = 0f;
						RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, 1000f, this.layerMask);
						if (hit)
						{
							float num2 = UnityEngine.Random.Range(0f, 1f);
							if (num2 < 0.3f)
							{
								this.CreateEnemy();
							}
							else if (num2 >= 0.3f && num2 < 0.6f)
							{
								this.CreateEnemy2();
							}
							else
							{
								this.CreateEnemy3();
							}
							this.CountEnemy++;
						}
					}
				}
			}
			else
			{
				if (this.timeCreateEnemy >= 1f)
				{
					this.timeCreateEnemy = 0f;
					RaycastHit2D hit2 = Physics2D.Raycast(this.transform.position, Vector2.down, 1000f, this.layerMask);
					if (hit2)
					{
						this.CreateEnemy();
						this.CountEnemy++;
					}
					else
					{
						this.step = 2;
					}
				}
				if (this.CountEnemy >= 3)
				{
					this.step = 2;
				}
			}
		}
		else
		{
			this.transform.Translate(Vector3.left * Time.deltaTime * this.Speed);
			float num3 = Mathf.Abs(this.transform.position.x - CameraController.Instance.Position.x);
			if (num3 <= 5f)
			{
				this.step = 1;
				this.timeCreateEnemy = 0f;
			}
		}
		if (this.transform.position.x < CameraController.Instance.Position.x - CameraController.Instance.Size().x - 4f)
		{
			this.isRunning = false;
			this.TimeRestart = 0f;
		}
	}

	private void CreateEnemy()
	{
		EnemySniper enemy = EnemyManager.Instance.CreateSniper();
		EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
		enemyDataInfo.ismove = true;
		enemyDataInfo.pos_x = this.transform.position.x;
		enemyDataInfo.pos_y = this.transform.position.y;
		enemyDataInfo.type = 0;
		enemy.Init(enemyDataInfo, delegate
		{
			EnemyManager.Instance.PoolSniper.Store(enemy);
		});
		EnemySniper enemy2 = enemy;
		enemy2.OnEnemyDeaded = (Action)Delegate.Combine(enemy2.OnEnemyDeaded, new Action(delegate()
		{
		}));
		enemy.cacheEnemy.Vision_Min = 3f;
		enemy.cacheEnemy.Vision_Max = 9f;
		GameManager.Instance.ListEnemy.Add(enemy);
		enemy.SetParachuter(0.5f);
	}

	private void CreateEnemy2()
	{
		EnemyGrenade enemy = EnemyManager.Instance.CreateEnemyGrenade();
		EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
		enemyDataInfo.ismove = true;
		enemyDataInfo.type = 2;
		enemyDataInfo.pos_x = this.transform.position.x;
		enemyDataInfo.pos_y = this.transform.position.y;
		enemy.Init(enemyDataInfo, delegate
		{
			EnemyManager.Instance.PoolEnemyGrenade.Store(enemy);
		});
		enemy.cacheEnemy.Vision_Min = 3f;
		enemy.cacheEnemy.Vision_Max = 9f;
		GameManager.Instance.ListEnemy.Add(enemy);
		enemy.SetParachuter(0.5f);
	}

	private void CreateEnemy3()
	{
		EnemyFire enemy = EnemyManager.Instance.CreateEnemyFire();
		EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
		enemyDataInfo.ismove = true;
		enemyDataInfo.type = 3;
		enemyDataInfo.pos_x = this.transform.position.x;
		enemyDataInfo.pos_y = this.transform.position.y;
		enemy.Init(enemyDataInfo, delegate
		{
			EnemyManager.Instance.PoolEnemyfire.Store(enemy);
		});
		enemy.cacheEnemy.Vision_Min = 1f;
		enemy.cacheEnemy.Vision_Max = 3f;
		GameManager.Instance.ListEnemy.Add(enemy);
		enemy.SetParachuter(0.5f);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Ground"))
		{
			GameManager.Instance.fxManager.ShowEffect(0, this.transform.position, Vector3.one, true, true);
			base.gameObject.SetActive(false);
		}
	}

	private IEnumerator HIDE()
	{
		this.rigidbody2D.velocity = Vector2.zero;
		this.rigidbody2D.gravityScale = 0f;
		this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
		yield return new WaitForSeconds(1f);
		base.gameObject.SetActive(false);
		yield break;
	}

	private void Restart()
	{
		this.CountEnemy = 0;
		this.timeCreateEnemy = 0f;
		Vector2 position = CameraController.Instance.Position;
		position.y += 2f;
		position.x += 10f;
		this.transform.position = position;
		this.isRunning = true;
		this.step = 0;
	}

	[SerializeField]
	private float Speed;

	[SerializeField]
	private float TOTAL_HP;

	private float timeCreateEnemy;

	[SerializeField]
	private LayerMask layerMask;

	private int step;

	private bool isRunning;

	private int CountEnemy;

	private float TimeRestart;
}
