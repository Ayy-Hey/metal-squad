using System;
using System.Collections;
using System.Collections.Generic;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss3_2 : BaseBoss
{
	public Transform[] arrPos { get; set; }

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		base.Hit(damage);
		Color color = new Color(1f, 1f, 1f, 1f);
		Color color2 = new Color(1f, 0.5f, 0f, 1f);
		this.SetHit();
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP * GameMode.Instance.GetMode());
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(2, 3, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			if (this.lastWeapon == EWeapon.LASER)
			{
				GameManager.Instance.mMission.isLaserBoss = true;
				if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL)
				{
					GameManager.Instance.mMission.StartCheck();
				}
			}
			GameManager.Instance.hudManager.HideControl();
			PlayerManagerStory.Instance.OnRunGameOver();
			this.isInit = false;
			this.SetDie();
			this.skeletonAnimation.AnimationState.SetEmptyAnimations(0f);
			this.skeletonAnimation.AnimationState.SetAnimation(0, "death", false);
			base.StartCoroutine(this.EffectDie());
			this.rigidbody2D.isKinematic = true;
			this.State = ECharactor.DIE;
		}
	}

	private void InitModeEnemy()
	{
		float mode = GameMode.Instance.GetMode();
		this.HP *= mode;
		this.damage1 = this.cacheEnemy.Damage * mode;
		this.damage2 = this.cacheEnemy.DamageLv2 * mode;
	}

	public void InitEnemy(int level, EnemyCharactor enemy)
	{
		base.InitEnemy(enemy, level);
		this.cacheEnemy = enemy.enemy[level];
		this.HP = this.cacheEnemy.HP;
		this.InitModeEnemy();
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		this.arrPos = GameManager.Instance.bossManager.Points_Boss3_2;
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.skeletonAnimation.skeleton.FlipX = true;
		this.step = -1;
		this.Guntip1_01 = this.skeletonAnimation.skeleton.FindBone("Gun_tip04");
		this.Guntip1_02 = this.skeletonAnimation.skeleton.FindBone("bone10");
		this.Guntip2_01 = this.skeletonAnimation.skeleton.FindBone("Gun_tip03");
		this.Guntip2_02 = this.skeletonAnimation.skeleton.FindBone("bone11");
		this.Guntip3_01 = this.skeletonAnimation.skeleton.FindBone("Gun_tip02");
		this.Guntip3_02 = this.skeletonAnimation.skeleton.FindBone("bone12");
		this.Guntip4_01 = this.skeletonAnimation.skeleton.FindBone("Gun_tip01");
		this.Guntip4_02 = this.skeletonAnimation.skeleton.FindBone("bone13");
		this.isInit = true;
		this.boxCollider.enabled = false;
		this.timeCreateBee = Time.timeSinceLevelLoad;
		if (this.meshRenderer != null)
		{
			this.meshRenderer.sortingOrder = 30;
		}
		this.timeReload = (this.coolDown = 4f);
		this.listShip = new List<BaseEnemy>();
	}

	public void Begin()
	{
		float num = Mathf.Abs(this.transform.position.x - this.arrPos[0].position.x);
		if (num < 0.1f)
		{
			this.SetIdle();
			this.isFirt = false;
			return;
		}
		this.SetRun();
		this.rigidbody2D.MovePosition(this.rigidbody2D.position + Vector2.right * Time.deltaTime);
	}

	private void ShootRocket(Vector2 pos)
	{
		RocketEnemy rocketEnemy = GameManager.Instance.bulletManager.CreateRocketEnemy(pos);
		Vector2 position = GameManager.Instance.player.GetPosition();
		position.x = Mathf.Max(pos.x + (float)((!this.skeletonAnimation.skeleton.FlipX) ? 1 : -1), GameManager.Instance.player.GetPosition().x);
		position = GameManager.Instance.player.GetPosition();
		rocketEnemy.SetFire(0, position, this.damage1, (GameManager.Instance.player.GetPosition().x >= pos.x) ? 0 : 340);
	}

	private void MoveNextStep(float d0)
	{
		if (d0 < 0.1f)
		{
			this.SetIdle();
			this.isFirt = false;
			return;
		}
		this.rigidbody2D.MovePosition(this.rigidbody2D.position + Vector2.right * Time.deltaTime * 1.5f);
	}

	private void FixedUpdate()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState == EGamePlay.PAUSE || GameManager.Instance.StateManager.EState == EGamePlay.PREVIEW)
		{
			return;
		}
		if (this.isFirt)
		{
			this.Begin();
			return;
		}
		float d = Mathf.Abs(this.transform.position.x - this.arrPos[this.step].position.x);
		float num = Mathf.Abs(-2f - this.rigidbody2D.position.y);
		float num2 = Mathf.Abs(-4.3f - this.rigidbody2D.position.y);
		if (this.coolDown > 0f)
		{
			this.coolDown -= Time.fixedDeltaTime;
		}
		ECharactor state = this.State;
		if (state != ECharactor.IDLE)
		{
			if (state == ECharactor.RUN)
			{
				this.timeRun += Time.deltaTime;
				if (this.timeRun < 3f)
				{
					return;
				}
				this.skeletonAnimation.AnimationName = this.walkAnim;
				this.skeletonAnimation.loop = true;
				if (num2 > 0.1f)
				{
					this.rigidbody2D.position = Vector2.MoveTowards(this.rigidbody2D.position, new Vector2(this.rigidbody2D.position.x, -4.3f), Time.deltaTime * 0.5f);
				}
				else
				{
					this.boxCollider.enabled = false;
					this.MoveNextStep(d);
				}
				bool flag = this.coolDown <= 0f && this.listShip.Count < 2;
				if (flag)
				{
					for (int i = this.listShip.Count; i < 2; i++)
					{
						this.CreateShip();
					}
					this.coolDown = this.timeReload;
				}
			}
		}
		else
		{
			this.timeIdle += Time.deltaTime;
			if (this.timeIdle >= 1f && num > 0.1f)
			{
				this.rigidbody2D.position = Vector2.MoveTowards(this.rigidbody2D.position, new Vector2(this.rigidbody2D.position.x, -2f), Time.deltaTime * 0.5f);
			}
			if (this.timeIdle >= 2f)
			{
				this.skeletonAnimation.AnimationName = this.idleAnim;
				this.skeletonAnimation.loop = true;
				if (this.transform.position.x < GameManager.Instance.player.GetPosition().x && !base.isInCamera)
				{
					this.SetRun();
					return;
				}
				if (Time.timeSinceLevelLoad - this.timeAttack >= 5f && base.isInCamera)
				{
					this.StepAttack++;
					this.timeAttack = Time.timeSinceLevelLoad;
					this.skeletonAnimation.state.SetAnimation(1, this.attackAnim, false);
				}
			}
		}
	}

	private void CreateShip()
	{
		MiniBoss5_2 miniBoss5_ = EnemyManager.Instance.CreateMiniShip();
		if (miniBoss5_.cacheEnemyData == null)
		{
			miniBoss5_.cacheEnemyData = new EnemyDataInfo();
		}
		miniBoss5_.cacheEnemyData.level = 9;
		miniBoss5_.cacheEnemyData.ismove = true;
		miniBoss5_.cacheEnemyData.type = 105;
		miniBoss5_.cacheEnemyData.pos_y = CameraController.Instance.camPos.y + UnityEngine.Random.Range(0f, 2f);
		miniBoss5_.cacheEnemyData.pos_x = CameraController.Instance.camPos.x + ((UnityEngine.Random.Range(0, 2) != 1) ? (-CameraController.Instance.Size().x - 2f) : (CameraController.Instance.Size().x + 2f));
		miniBoss5_.InitEnemy(false, delegate(MiniBoss5_2 _ship)
		{
			EnemyManager.Instance.PoolMiniShip.Store(_ship);
			this.listShip.Remove(_ship);
		});
		GameManager.Instance.ListEnemy.Add(miniBoss5_);
		this.listShip.Add(miniBoss5_);
	}

	public new void SetIdle()
	{
		if (this.State == ECharactor.IDLE)
		{
			return;
		}
		base.SetIdle();
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
		this.timeIdle = 0f;
		this.StepAttack = 0;
		this.timeAttack = Time.timeSinceLevelLoad;
		this.boxCollider.enabled = true;
	}

	public new void SetRun()
	{
		if (this.State == ECharactor.RUN || this.State == ECharactor.DIE)
		{
			return;
		}
		base.SetRun();
		try
		{
			GameManager.Instance.ListEnemy.Remove(this);
		}
		catch
		{
		}
		this.timeRun = 0f;
		this.step++;
		this.maxAttack = UnityEngine.Random.Range(1, 3);
		if (this.step > this.arrPos.Length - 1)
		{
			this.step = this.arrPos.Length - 1;
		}
		else
		{
			CameraController.Instance.NewBoundaryRight(Mathf.Max(20f, this.arrPos[this.step].position.x + CameraController.Instance.Size().x), true);
		}
	}

	private IEnumerator EffectDie()
	{
		Vector3 pos = this.transform.position;
		Vector3 pos2 = pos;
		Vector3 pos3 = pos;
		pos2.x -= 2f;
		pos3.x += 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos3, Vector3.one);
		yield return new WaitForSeconds(0.5f);
		pos = this.transform.position;
		pos2 = pos;
		pos3 = pos;
		pos2.x -= 2f;
		pos3.x += 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos3, Vector3.one);
		yield return new WaitForSeconds(0.5f);
		pos = this.transform.position;
		pos2 = pos;
		pos3 = pos;
		pos2.x -= 2f;
		pos3.x += 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos3, Vector3.one);
		yield break;
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
			if (!(name == "shut1"))
			{
				if (!(name == "shut2"))
				{
					if (!(name == "shot01"))
					{
					}
				}
				else
				{
					Vector2 pos = new Vector2(this.Guntip1_01.WorldX + this.transform.position.x, this.Guntip1_01.WorldY + this.transform.position.y);
					Vector2 pos2 = new Vector2(this.Guntip3_01.WorldX + this.transform.position.x, this.Guntip3_01.WorldY + this.transform.position.y);
					this.ShootRocket(pos);
					this.ShootRocket(pos2);
				}
			}
			else
			{
				Vector2 pos3 = new Vector2(this.Guntip2_01.WorldX + this.transform.position.x, this.Guntip2_01.WorldY + this.transform.position.y);
				Vector2 pos4 = new Vector2(this.Guntip4_01.WorldX + this.transform.position.x, this.Guntip4_01.WorldY + this.transform.position.y);
				this.ShootRocket(pos3);
				this.ShootRocket(pos4);
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
		string text = entry.ToString();
		if (text != null)
		{
			if (text == "attack")
			{
				if (this.StepAttack >= this.maxAttack)
				{
					this.SetRun();
					this.StepAttack = 0;
				}
			}
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string idleAnim;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string attackAnim;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string walkAnim;

	private bool isFirt = true;

	private float timeIdle;

	private float timeRun;

	private int StepAttack;

	private float timeAttack;

	private int step;

	private int maxAttack;

	private float timeCreateBee;

	private Bone Guntip1_01;

	private Bone Guntip1_02;

	private Bone Guntip2_01;

	private Bone Guntip2_02;

	private Bone Guntip3_01;

	private Bone Guntip3_02;

	private Bone Guntip4_01;

	private Bone Guntip4_02;

	private List<BaseEnemy> listShip;

	private float timeReload;

	private float coolDown;

	private float damage1;

	private float damage2;
}
