using System;
using System.Collections;
using System.Collections.Generic;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss14 : BaseBoss
{
	private void OnValidate()
	{
		if (this.skeletonAnimation != null)
		{
			Spine.Animation[] items = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
			this.anims = new string[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				this.anims[i] = items[i].Name;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("setup Skeleton Animation!");
		}
		try
		{
			this.bossData = Resources.Load<TextAsset>("Charactor/Boss/" + base.GetType().ToString());
		}
		catch (Exception ex)
		{
			MonoBehaviour.print(ex.Message);
		}
	}

	private void Update()
	{
		if (!this.isInit || this.State == ECharactor.DIE || (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW))
		{
			return;
		}
		if (!this.Begin())
		{
			return;
		}
		this.UpdateState();
	}

	public override void Init()
	{
		base.Init();
		this.InitEnemy();
	}

	public void InitEnemy()
	{
		try
		{
			float mode = GameMode.Instance.GetMode();
			EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>(this.bossData.text);
			enemyCharactor.enemy[0].Damage *= mode;
			enemyCharactor.enemy[0].Speed *= ((mode > 2f) ? 2f : mode);
			this.InitEnemy(enemyCharactor, 0);
			this.cacheEnemy.HP = this.HP;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.State = ECharactor.RUN;
		this._isBegin = false;
		this._changeState = false;
		this.skeletonAnimation.AnimationState.Event += this.OnEvent;
		this.skeletonAnimation.AnimationState.Complete += this.OnComplete;
		this.skeletonAnimation.skeleton.FlipX = false;
		this._ramboTrans = GameManager.Instance.player.transform;
		GameManager.Instance.ListEnemy.Add(this);
		this._damageBoss = this.cacheEnemy.Damage;
		this._speedBoss = this.cacheEnemy.Speed;
		this._bossVelocity = new Vector2(-this._speedBoss, 0f);
		this._state = Boss14.E_StateBoss14.Fire;
		this._changeState = false;
		this._runFire = true;
		this.PlayAnim(Boss14.E_StateBoss14.Walk, true);
		this.isInit = true;
	}

	private void InitBullet()
	{
		try
		{
			this.listBullets[0].gameObject.transform.parent.parent = null;
			this._poolingBullets = new ObjectPooling<BulletBoss1_3>(this.listBullets.Count, null, null);
			for (int i = 0; i < this.listBullets.Count; i++)
			{
				this._poolingBullets.Store(this.listBullets[i]);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.Message);
		}
	}

	private bool Begin()
	{
		if (this._isBegin)
		{
			return true;
		}
		this._bossVelocity.y = this.rigidbody2D.velocity.y;
		this.rigidbody2D.velocity = this._bossVelocity;
		if (this.transform.position.x <= CameraController.Instance.Position.x + CameraController.Instance.Size().x - 2f)
		{
			this._isBegin = true;
			this.bodyCollider2D.enabled = true;
			this.InitBullet();
			return true;
		}
		return false;
	}

	private void UpdateState()
	{
		this.CachePosition();
		if (!this._changeState)
		{
			switch (this._state)
			{
			case Boss14.E_StateBoss14.Fire:
				this._countSkill++;
				break;
			case Boss14.E_StateBoss14.Punch:
				this._countSkill++;
				break;
			case Boss14.E_StateBoss14.Snowballs:
				this._countSkill++;
				break;
			case Boss14.E_StateBoss14.Rolling:
				this._countSkill++;
				this.RollingSetting();
				break;
			case Boss14.E_StateBoss14.Idle:
				this._loopState = true;
				break;
			case Boss14.E_StateBoss14.Walk:
				this.MoveSetting();
				this._loopState = true;
				break;
			}
			this._changeState = true;
			this.PlayAnim(this._state, this._loopState);
			this.skeletonAnimation.skeleton.FlipX = (this._ramboPosition.x > this._bossPosition.x);
		}
		else
		{
			switch (this._state)
			{
			case Boss14.E_StateBoss14.Snowballs:
				if (this.snowBallBoss14.gameObject.activeSelf)
				{
					this.snowBallBoss14.UpdateObject();
				}
				break;
			case Boss14.E_StateBoss14.Rolling:
				this.OnRolling();
				break;
			case Boss14.E_StateBoss14.Walk:
				this.BossMove();
				break;
			}
		}
		this.UpdateBullet();
	}

	private void CachePosition()
	{
		this._bossPosition = this.transform.position;
		this._ramboPosition = this._ramboTrans.position;
		this._camPosition = CameraController.Instance.Position;
	}

	private void RollingSetting()
	{
		this.rigidbody2D.constraints = RigidbodyConstraints2D.None;
		this._bossVelocity.x = this.GetSpeed(Boss14.E_SkillBoss14.Rolling) * (float)((!this.skeletonAnimation.skeleton.FlipX) ? -1 : 1);
		this.rollingBoss14.Active(this.GetDamage(Boss14.E_SkillBoss14.Rolling));
		this.rollingCollider.enabled = true;
		this.boxCollider.enabled = false;
		this.bodyCollider2D.enabled = false;
		this.boxChanBoss.enabled = false;
	}

	private void OnRolling()
	{
		this._bossVelocity.y = this.rigidbody2D.velocity.y;
		this.rigidbody2D.velocity = this._bossVelocity;
		if (this.GetDistanceToTarget(this._camPosition) > 5f)
		{
			this.rollingCollider.enabled = false;
			this.boxChanBoss.enabled = true;
			this.boxCollider.enabled = true;
			this.bodyCollider2D.enabled = true;
			this.rollingBoss14.InActive();
			this.transform.rotation = Quaternion.identity;
			this.ChangeState();
			this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	private void UpdateBullet()
	{
		for (int i = 0; i < this.listBullets.Count; i++)
		{
			if (this.listBullets[i].gameObject.activeSelf)
			{
				this.listBullets[i].UpdateObject();
			}
		}
	}

	private void MoveSetting()
	{
		if (this._runFire)
		{
			this._targetMove = this._bossPosition;
			this._targetMove.x = this._targetMove.x + (float)((!this.skeletonAnimation.skeleton.FlipX) ? -2 : 2);
		}
		else if (this._fleeRambo)
		{
			this.skeletonAnimation.skeleton.FlipX = (this._ramboPosition.x < this._camPosition.x);
		}
		this._bossVelocity.x = ((!this.skeletonAnimation.skeleton.FlipX) ? (-this._speedBoss) : this._speedBoss);
	}

	private void BossMove()
	{
		this.rigidbody2D.velocity = this._bossVelocity;
		if (this._runFire)
		{
			if (this.GetDistanceToTarget(this._targetMove) <= 0.1f)
			{
				this.ChangeState();
			}
		}
		else if (this._fleeRambo)
		{
			if (this.GetDistanceToTarget(this._ramboPosition) >= 2.5f || this.GetDistanceToTarget(this._camPosition) >= CameraController.Instance.Size().x)
			{
				this.ChangeState();
			}
		}
		else if (this.GetDistanceToTarget(this._ramboPosition) <= 2.5f)
		{
			this.ChangeState();
		}
	}

	private void ChangeState()
	{
		this._loopState = false;
		this._changeState = false;
		if (this._countSkill >= this.maxSkills)
		{
			this._state = Boss14.E_StateBoss14.Idle;
			this._countSkill = 0;
			return;
		}
		float distanceToTarget = this.GetDistanceToTarget(this._ramboPosition);
		switch (this._state)
		{
		case Boss14.E_StateBoss14.Fire:
			if (distanceToTarget > 5f)
			{
				this._state = Boss14.E_StateBoss14.Walk;
				this._runFire = true;
			}
			else
			{
				this._state = Boss14.E_StateBoss14.Punch;
			}
			break;
		case Boss14.E_StateBoss14.Punch:
			if (this._dangerState)
			{
				if (distanceToTarget < 4f)
				{
					this._state = Boss14.E_StateBoss14.Rolling;
				}
				else
				{
					this._state = Boss14.E_StateBoss14.Snowballs;
				}
			}
			else if (distanceToTarget > 5f)
			{
				this._state = Boss14.E_StateBoss14.Walk;
				this._fleeRambo = true;
			}
			else
			{
				this._state = Boss14.E_StateBoss14.Fire;
			}
			break;
		case Boss14.E_StateBoss14.Snowballs:
			if (this.snowBallBoss14.gameObject.activeSelf)
			{
				this.snowBallBoss14.gameObject.SetActive(false);
			}
			if (this._dangerState)
			{
				if (distanceToTarget < 3f)
				{
					this._state = Boss14.E_StateBoss14.Rolling;
				}
				else
				{
					this._state = Boss14.E_StateBoss14.Fire;
				}
			}
			else if (distanceToTarget < 3f)
			{
				this._state = Boss14.E_StateBoss14.Punch;
			}
			else
			{
				this._state = Boss14.E_StateBoss14.Fire;
			}
			break;
		case Boss14.E_StateBoss14.Rolling:
			if (distanceToTarget < 3f)
			{
				this._state = Boss14.E_StateBoss14.Punch;
			}
			else
			{
				this._state = Boss14.E_StateBoss14.Fire;
			}
			break;
		case Boss14.E_StateBoss14.Idle:
			if (distanceToTarget > 3f)
			{
				this._state = Boss14.E_StateBoss14.Fire;
			}
			else
			{
				this._state = Boss14.E_StateBoss14.Punch;
			}
			break;
		case Boss14.E_StateBoss14.Walk:
			if (this._runFire)
			{
				this._state = Boss14.E_StateBoss14.Fire;
				this._runFire = false;
			}
			else if (distanceToTarget < 3f)
			{
				this._state = Boss14.E_StateBoss14.Punch;
			}
			else
			{
				this._state = Boss14.E_StateBoss14.Snowballs;
			}
			break;
		}
	}

	private void PlayAnim(Boss14.E_StateBoss14 state, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)state], loop);
	}

	private float GetDamage(Boss14.E_SkillBoss14 skill)
	{
		return this._damageBoss * this.ratioDamageOfSkills[(int)skill];
	}

	private float GetSpeed(Boss14.E_SkillBoss14 skill)
	{
		return this._speedBoss * this.ratioSpeedOfSkills[(int)skill];
	}

	private float GetDistanceToTarget(Vector3 target)
	{
		return Mathf.Abs(this._bossPosition.x - target.x);
	}

	public void ShakeCamera(int numberOfShake, float distance)
	{
		if (GameManager.Instance != null && GameManager.Instance.StateManager.EState != EGamePlay.WIN)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
		}
	}

	private void CreateBullet()
	{
		for (int i = 0; i < 2; i++)
		{
			this._bullet = null;
			this._bullet = this._poolingBullets.New();
			if (this._bullet == null)
			{
				this._bullet = UnityEngine.Object.Instantiate<BulletBoss1_3>(this.listBullets[0]);
				this._bullet.gameObject.transform.parent = this.listBullets[0].gameObject.transform.parent;
				this.listBullets.Add(this._bullet);
			}
			this._bullet.gameObject.transform.position = this.transGunPoints[i * 2].position;
			this._bullet.gameObject.SetActive(true);
			this._bullet.InitObject();
			this._bullet.DisableAction = new Action<BulletBoss1_3>(this.OnDisableBullet);
			this._bullet.SetValue(EWeapon.NONE, 0, this.transGunPoints[i * 2].position, this.transGunPoints[i * 2 + 1].position - this.transGunPoints[i * 2].position, this.GetDamage(Boss14.E_SkillBoss14.Fire), this.GetSpeed(Boss14.E_SkillBoss14.Fire), 0f);
		}
	}

	private void OnDisableBullet(BulletBoss1_3 obj)
	{
		this._poolingBullets.Store(obj);
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this._dangerState && this.HP <= this.cacheEnemy.HP / 2f)
		{
			this._dangerState = true;
			this.maxIdle--;
			this.maxSkills++;
		}
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(2, 1, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			this.Die();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			PlayerManagerStory.Instance.OnRunGameOver();
			GameManager.Instance.ListEnemy.Remove(this);
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[8].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[8].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Mechanical Scorpion has been unlocked in BossMode!");
			}
		}
	}

	private void Die()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		this.skeletonAnimation.skeleton.SetColor(Color.white);
		this.skeletonAnimation.state.ClearTracks();
		this.PlayAnim(Boss14.E_StateBoss14.Death, false);
		this.ShakeCamera(4, 0.25f);
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
		base.StartCoroutine(this.EffectDie());
	}

	private IEnumerator EffectDie()
	{
		this.ShakeCamera(4, 0.25f);
		Vector3 pos = this.transform.position;
		Vector3 pos2 = this.transform.position;
		GameManager.Instance.fxManager.ShowEffect(5, pos, Vector3.one, true, true);
		GameManager.Instance.fxManager.ShowEffect(5, pos2, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		this.ShakeCamera(4, 0.25f);
		pos2 = this.transform.position;
		pos.x -= 0.62f;
		pos.y -= 0.8f;
		pos2.x += 1.2f;
		pos2.y += 0.6f;
		GameManager.Instance.fxManager.ShowEffect(5, pos, Vector3.one, true, true);
		GameManager.Instance.fxManager.ShowEffect(5, pos2, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		this.ShakeCamera(4, 0.25f);
		pos.x += 1f;
		pos.y += 1.2f;
		pos2.x -= 2f;
		pos2.y -= 0.9f;
		GameManager.Instance.fxManager.ShowEffect(5, pos, Vector3.one, true, true);
		GameManager.Instance.fxManager.ShowEffect(5, pos2, Vector3.one, true, true);
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		switch (this._state)
		{
		case Boss14.E_StateBoss14.Fire:
			this.ChangeState();
			break;
		case Boss14.E_StateBoss14.Punch:
			this.ChangeState();
			break;
		case Boss14.E_StateBoss14.Snowballs:
			this.ChangeState();
			break;
		case Boss14.E_StateBoss14.Idle:
			this._countIdle++;
			if (this._countIdle >= this.maxIdle)
			{
				this.ChangeState();
				this._countIdle = 0;
			}
			break;
		}
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "attack01"))
			{
				if (!(name == "attack02"))
				{
					if (!(name == "attack03"))
					{
						if (!(name == "attack04"))
						{
							if (name == "walk")
							{
								this.ShakeCamera(2, 0.2f);
							}
						}
					}
					else
					{
						float speed = this.GetSpeed(Boss14.E_SkillBoss14.Snowballs) * (float)((!this.skeletonAnimation.skeleton.FlipX) ? -1 : 1);
						float damage = this.GetDamage(Boss14.E_SkillBoss14.Snowballs);
						this.snowBallBoss14.SnowBall(speed, damage, this.transSnowPoint.position, new Action(this.ChangeState));
					}
				}
				else
				{
					this.ShakeCamera(2, 0.2f);
					if (GameManager.Instance)
					{
						for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
						{
							Vector3 position = this.transPunch.position;
							Vector3 position2 = GameManager.Instance.ListRambo[i].transform.position;
							float num = Mathf.Abs(position.x - position2.x);
							if (num < 2f && position.y >= position2.y)
							{
								GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this.GetDamage(Boss14.E_SkillBoss14.Punch), EWeapon.NONE);
							}
						}
					}
				}
			}
			else
			{
				base.Invoke("CreateBullet", 0.3f);
			}
		}
	}

	[SerializeField]
	[Header("*******************************")]
	private TextAsset bossData;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private Collider2D boxChanBoss;

	[SerializeField]
	private string[] anims;

	[SerializeField]
	private float[] ratioDamageOfSkills;

	[SerializeField]
	private float[] ratioSpeedOfSkills;

	[SerializeField]
	private int maxSkills;

	[SerializeField]
	private int maxIdle;

	[SerializeField]
	private Transform transSnowPoint;

	[SerializeField]
	private SnowballBoss14 snowBallBoss14;

	[SerializeField]
	private RollingBoss14 rollingBoss14;

	[SerializeField]
	private Collider2D rollingCollider;

	[SerializeField]
	private Transform transPunch;

	[SerializeField]
	private List<BulletBoss1_3> listBullets;

	[SerializeField]
	private Transform[] transGunPoints;

	private ObjectPooling<BulletBoss1_3> _poolingBullets;

	private bool _isBegin;

	private Boss14.E_StateBoss14 _state;

	private bool _changeState;

	private bool _loopState;

	private Transform _ramboTrans;

	private Vector2 _bossVelocity;

	private Vector3 _bossPosition;

	private Vector3 _ramboPosition;

	private Vector3 _camPosition;

	private Vector3 _targetMove;

	private float _deltaTime;

	private int _countIdle;

	private int _countSkill;

	private bool _runFire;

	private float _damageBoss;

	private float _speedBoss;

	private bool _dangerState;

	private bool _fleeRambo;

	private BulletBoss1_3 _bullet;

	private Vector3 _direction;

	private enum E_StateBoss14
	{
		Fire,
		Punch,
		Snowballs,
		Rolling,
		Death,
		Hit,
		Idle,
		Walk
	}

	private enum E_SkillBoss14
	{
		Fire,
		Punch,
		Snowballs,
		Rolling
	}
}
