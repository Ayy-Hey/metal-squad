using System;
using System.Collections;
using System.Collections.Generic;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss4_1 : BaseBoss
{
	private void FixedUpdate()
	{
		this.UpdateObject();
	}

	private IEnumerator EffectDie()
	{
		Vector3 pos = this.transform.position;
		Vector3 pos2 = pos;
		Vector3 pos3 = pos;
		pos2.x -= 1f;
		pos3.x += 2f;
		pos2.y += (float)UnityEngine.Random.Range(-2, 2);
		pos3.y += (float)UnityEngine.Random.Range(-2, 2);
		GameManager.Instance.fxManager.ShowEffect(5, pos2, Vector3.one, true, true);
		GameManager.Instance.fxManager.ShowEffect(5, pos3, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		pos = this.transform.position;
		pos2 = pos;
		pos3 = pos;
		pos2.x -= 2f;
		pos3.x += 1f;
		pos2.y += (float)UnityEngine.Random.Range(-2, 2);
		pos3.y += (float)UnityEngine.Random.Range(-2, 2);
		GameManager.Instance.fxManager.ShowEffect(5, pos, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		pos = this.transform.position;
		pos2 = pos;
		pos3 = pos;
		pos2.x -= 2f;
		pos3.x += 2f;
		pos2.y += (float)UnityEngine.Random.Range(-2, 2);
		pos3.y += (float)UnityEngine.Random.Range(-2, 2);
		GameManager.Instance.fxManager.ShowEffect(5, pos2, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		base.StartCoroutine(this.EffectDie());
		yield break;
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		base.Hit(damage);
		this.timePingPongColor = 0f;
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP * GameMode.Instance.GetMode());
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(3, 2, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			PlayerManagerStory.Instance.OnRunGameOver();
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[13].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[13].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Sun Ray has been unlocked in BossMode!");
			}
			this.isInit = false;
			this.SetDie();
			this.rigidbody2D.isKinematic = true;
			base.StartCoroutine(this.EffectDie());
			this.State = ECharactor.DIE;
			for (int i = 0; i < this.ListBullet.Count; i++)
			{
				if (this.ListBullet[i] != null && this.ListBullet[i].gameObject.activeSelf)
				{
					this.ListBullet[i].gameObject.SetActive(false);
				}
			}
		}
	}

	private void InitModeEnemy()
	{
		float mode = GameMode.Instance.GetMode();
		this.HP *= mode;
		this.damage1 = this.cacheEnemy.Damage * mode;
		this.damage2 = this.cacheEnemy.DamageLv2 * mode;
	}

	public override void Init()
	{
		base.Init();
		this.InitEnemy(5, DataLoader.boss);
	}

	public void InitEnemy(int level, EnemyCharactor enemy)
	{
		base.InitEnemy(enemy, level);
		if (this.AttackAnim2 == null)
		{
			this.AttackAnim2 = this.skeletonAnimation.skeleton.Data.FindAnimation(this.attackAnim2);
		}
		if (this.AttackAnim3 == null)
		{
			this.AttackAnim3 = this.skeletonAnimation.skeleton.Data.FindAnimation(this.attackAnim3);
		}
		this.AttackAnim = this.skeletonAnimation.skeleton.Data.FindAnimation(this.attackAnim);
		this.IdleAnim = this.skeletonAnimation.skeleton.Data.FindAnimation(this.idleAnim);
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.skeletonAnimation.state.Start += this.StartEvent;
		this.bones = new Bone[8];
		for (int i = 1; i <= 8; i++)
		{
			this.bones[i - 1] = this.skeletonAnimation.skeleton.FindBone("guntip02_0" + i);
		}
		this.boneCenter = this.skeletonAnimation.skeleton.FindBone("guntip03");
		this.bonesBomb = new Bone[2];
		for (int j = 0; j < 2; j++)
		{
			this.bonesBomb[j] = this.skeletonAnimation.skeleton.FindBone("bone1" + j);
		}
		this.HP = enemy.enemy[level].HP;
		this.InitModeEnemy();
		GameManager.Instance.ListEnemy.Add(this);
		this.BulletPool = new ObjectPooling<BulletBoss4_1>(15, null, null);
		this.ListBullet[0].gameObject.transform.parent.parent = null;
		for (int k = 0; k < this.ListBullet.Count; k++)
		{
			this.BulletPool.Store(this.ListBullet[k]);
		}
		this.isInit = true;
	}

	public override void UpdateObject()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		for (int i = 0; i < this.ListBullet.Count; i++)
		{
			if (this.ListBullet[i] != null && this.ListBullet[i].gameObject.activeSelf)
			{
				this.ListBullet[i].UpdateObject();
			}
		}
		this.FirstMove();
		base.PingPongColor();
		ECharactor state = this.State;
		if (state != ECharactor.IDLE)
		{
			if (state != ECharactor.RUN)
			{
				if (state != ECharactor.ATTACK_3)
				{
				}
			}
			else
			{
				this.DoMove();
				if (!this.isAttackOther)
				{
					bool flag = UnityEngine.Random.Range(0, 2) == 1;
					if (flag)
					{
						this.SetAttack1();
					}
					else
					{
						this.SetAttack3();
					}
					this.isAttackOther = true;
				}
			}
		}
		else
		{
			if (Time.timeSinceLevelLoad - this.TimeIdle >= this.time_idle && !this.isAttack)
			{
				this.SetRun();
			}
			this.DoAttack();
		}
	}

	private void FirstMove()
	{
		if (this.isFirst)
		{
			return;
		}
		Vector2 position = CameraController.Instance.Position;
		position.y = this.rigidbody2D.position.y;
		this.rigidbody2D.MovePosition(Vector2.Lerp(this.rigidbody2D.position, position, Time.deltaTime * this.cacheEnemy.Speed));
		float num = Vector2.Distance(this.rigidbody2D.position, position);
		if (num <= 0.1f)
		{
			this.isFirst = true;
			this.isCenter = true;
			this.circleCollider.enabled = true;
			this.SetIdle();
		}
	}

	private void DoAttack()
	{
		if (Time.timeSinceLevelLoad - this.TimeAttack >= this.time_attack)
		{
			this.TimeAttack = Time.timeSinceLevelLoad;
			this.SetAttack2();
		}
	}

	private void DoMove()
	{
		Vector2 position = CameraController.Instance.Position;
		position.x = position.x - CameraController.Instance.Size().x + 2f;
		position.y = this.rigidbody2D.position.y;
		Vector2 position2 = CameraController.Instance.Position;
		position2.x = position2.x + CameraController.Instance.Size().x - 2f;
		position2.y = this.rigidbody2D.position.y;
		this.rigidbody2D.MovePosition(Vector2.Lerp(this.rigidbody2D.position, (!this.isMoveLeft) ? position2 : position, Time.deltaTime * this.cacheEnemy.Speed));
		float num = Vector2.Distance(this.rigidbody2D.position, position);
		if (num <= 0.1f)
		{
			this.isRight = false;
			this.isCenter = false;
			this.SetIdle();
		}
	}

	private void SetAttack1()
	{
		this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim, false);
	}

	private void SetAttack3()
	{
		this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim, false);
	}

	private void SetAttack2()
	{
		this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim2, false);
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.velocity = Vector2.zero;
		}
		this.isAttack = true;
	}

	private new void SetIdle()
	{
		if (this.State == ECharactor.IDLE)
		{
			return;
		}
		this.State = ECharactor.IDLE;
		this.TimeIdle = Time.timeSinceLevelLoad;
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.velocity = Vector2.zero;
		}
		this.skeletonAnimation.state.SetAnimation(0, this.IdleAnim, true);
		this.isAttackOther = false;
	}

	private new void SetRun()
	{
		if (this.State == ECharactor.RUN)
		{
			return;
		}
		this.isMoveLeft = (UnityEngine.Random.Range(0, 1) == 0);
		this.State = ECharactor.RUN;
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.velocity = Vector2.zero;
		}
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
			if (!(name == "shoot02"))
			{
				if (name == "shoot01")
				{
					for (int i = 0; i < this.bonesBomb.Length; i++)
					{
						Vector2 v = new Vector2(this.bonesBomb[i].WorldX + this.transform.position.x, this.bonesBomb[i].WorldY + this.transform.position.y);
						GameManager.Instance.bombManager.CreateBombBoss4_1(v);
					}
				}
			}
			else
			{
				for (int j = 0; j < 8; j++)
				{
					Vector2 direction = new Vector2(this.bones[j].WorldX - this.boneCenter.WorldX, this.bones[j].WorldY - this.boneCenter.WorldY);
					Vector2 pos = new Vector2(this.bones[j].WorldX + this.transform.position.x, this.bones[j].WorldY + this.transform.position.y);
					this.CreateBullet(pos, direction);
				}
			}
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		if (entry.TrackIndex > 0)
		{
			this.skeletonAnimation.state.ClearTrack(entry.TrackIndex);
		}
		string text = entry.ToString();
		if (text != null)
		{
			if (!(text == "attack02"))
			{
				if (text == "attack03")
				{
					this.SetRun();
				}
			}
			else
			{
				this.isAttack = false;
				if (!base.isInCamera)
				{
					this.SetRun();
				}
			}
		}
	}

	private void StartEvent(TrackEntry entry)
	{
	}

	private void CreateBullet(Vector2 pos, Vector2 direction)
	{
		BulletBoss4_1 bulletBoss4_ = this.BulletPool.New();
		if (bulletBoss4_ == null)
		{
			bulletBoss4_ = UnityEngine.Object.Instantiate<Transform>(this.ListBullet[0].transform).GetComponent<BulletBoss4_1>();
			bulletBoss4_.transform.parent = this.ListBullet[0].transform.parent;
			this.ListBullet.Add(bulletBoss4_);
		}
		bulletBoss4_.gameObject.SetActive(true);
		bulletBoss4_.Shoot(pos, direction, this.cacheEnemy.Damage, this.cacheEnemy.Speed_Bullet, delegate(BulletBoss4_1 b)
		{
			this.BulletPool.Store(b);
		});
	}

	private float time_attack = 7f;

	private float time_idle = 15f;

	public Vector2 Target = Vector2.zero;

	private int Step;

	private float TimeSleep;

	private float TimeAttack;

	private float TimeIdle;

	private bool isFirst;

	private bool isAttack;

	private bool isAttackOther;

	private bool isRight;

	private bool isCenter;

	public LayerMask layerMask;

	[SerializeField]
	[Header("Spine")]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	public string attackAnim;

	public Spine.Animation AttackAnim;

	[SpineAnimation("", "", true, false)]
	public string idleAnim;

	public Spine.Animation IdleAnim;

	[SpineAnimation("", "", true, false)]
	public string attackAnim2;

	public Spine.Animation AttackAnim2;

	[SpineAnimation("", "", true, false)]
	public string attackAnim3;

	public Spine.Animation AttackAnim3;

	private Bone[] bones;

	private Bone boneCenter;

	private Bone[] bonesBomb;

	public CircleCollider2D circleCollider;

	[Header("Bullet")]
	public List<BulletBoss4_1> ListBullet;

	public ObjectPooling<BulletBoss4_1> BulletPool;

	private bool isMoveLeft;

	private float damage1;

	private float damage2;
}
