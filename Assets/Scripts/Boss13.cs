using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss13 : BaseBoss
{
	private void Update()
	{
		if (!this.isInit || this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this.Begin())
		{
			return;
		}
		this.OnUpdate();
	}

	private void Test()
	{
		this.UpdateRocket();
		this.CachePosition();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			this._bossVelocity = Vector2.zero;
			this._bossVelocity.y = -Physics2D.gravity.y * this.rigidbody2D.gravityScale * 0.65f;
			this._bossVelocity.x = (this._bossPosition.x - this._ramboPosition.x) * Physics2D.gravity.y * this.rigidbody2D.gravityScale / this._bossVelocity.y / 2f;
			this.rigidbody2D.velocity = this._bossVelocity;
			this._changeState = false;
			this.PlayAnim(Boss13.E_StateBoss13.Jump, 0, false);
		}
		if (!this._changeState && this.rigidbody2D.velocity.y <= 0f)
		{
			this.PlayAnim(Boss13.E_StateBoss13.Down, 0, false);
			this._changeState = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
	}

	public override void Init()
	{
		base.Init();
		this.InitEnemy();
		try
		{
			ProCamera2D.Instance.OffsetX = 0f;
		}
		catch (Exception ex)
		{
		}
	}

	public void InitEnemy()
	{
		try
		{
			float num = GameMode.Instance.GetMode();
			string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
			string text2 = ProfileManager.DataEncryption.decrypt2(text);
			EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
			{
				num = 5f;
			}
			enemyCharactor.enemy[0].Damage *= num;
			enemyCharactor.enemy[0].Speed *= ((num > 2f) ? 2f : num);
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
		this.bodyCollider2D.enabled = false;
		this.colliderHandSkill.enabled = false;
		GameManager.Instance.ListEnemy.Add(this);
		this.skeletonAnimation.AnimationState.Event += this.OnEvent;
		this.skeletonAnimation.AnimationState.Complete += this.OnComplete;
		this.skeletonAnimation.skeleton.FlipX = false;
		this.InitParameter(1f);
		if (GameManager.Instance)
		{
			this._ramboTrans = GameManager.Instance.player.transform;
		}
		else
		{
			this._ramboTrans = GameObject.FindGameObjectWithTag("Rambo").transform;
		}
		this._mainLegParticle = this.legParticle.main;
		this._state = Boss13.E_StateBoss13.Song_Am;
		this._bossVelocity = new Vector2(-this._bossSpeed, 0f);
		this.PlayAnim(Boss13.E_StateBoss13.Tien, 0, true);
		this.isInit = true;
	}

	private void InitParameter(float ratio = 1f)
	{
		this._bossDamage = this.cacheEnemy.Damage * ratio;
		this._bossSpeed = this.cacheEnemy.Speed * ratio;
		this._skillSpeed = this.ratioSkillSpeed * this._bossSpeed;
		this.phunDoc.damage = this.GetDamage(Boss13.E_SkillBoss.Phun);
		this.songAm.SetDamage(this.GetDamage(Boss13.E_SkillBoss.SongAm));
	}

	private void InitRocketBoss()
	{
		this.listRocketBosss[0].gameObject.transform.parent.parent = null;
		try
		{
			this._pollRocketBoss = new ObjectPooling<RocketBoss13>(this.listRocketBosss.Count, null, null);
			for (int i = 0; i < this.listRocketBosss.Count; i++)
			{
				this._pollRocketBoss.Store(this.listRocketBosss[i]);
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
			this.InitRocketBoss();
			return true;
		}
		return false;
	}

	private void OnUpdate()
	{
		this.CachePosition();
		if (!this._changeState)
		{
			this._changeState = true;
			this._loopAnim = false;
			switch (this._state)
			{
			case Boss13.E_StateBoss13.Jump:
				this._bossVelocity = Vector2.zero;
				this._bossVelocity.y = -Physics2D.gravity.y * this.rigidbody2D.gravityScale * 0.65f;
				this._bossVelocity.x = (this._bossPosition.x - this._ramboPosition.x) * Physics2D.gravity.y * this.rigidbody2D.gravityScale / this._bossVelocity.y / 2f;
				this.rigidbody2D.velocity = this._bossVelocity;
				break;
			case Boss13.E_StateBoss13.Tien:
				this._bossVelocity = Vector2.zero;
				this._bossVelocity.x = ((!this.skeletonAnimation.skeleton.FlipX) ? (-this._bossSpeed) : this._bossSpeed);
				this._bossVelocity.y = this.rigidbody2D.velocity.y;
				this._loopAnim = true;
				break;
			case Boss13.E_StateBoss13.Lui:
				this._bossVelocity = Vector2.zero;
				this._bossVelocity.x = ((!this.skeletonAnimation.skeleton.FlipX) ? this._bossSpeed : (-this._bossSpeed));
				this._bossVelocity.y = this.rigidbody2D.velocity.y;
				this._loopAnim = true;
				this.TimeMoveBack = Time.timeSinceLevelLoad;
				break;
			}
			this.PlayAnim(this._state, 0, this._loopAnim);
		}
		else
		{
			switch (this._state)
			{
			case Boss13.E_StateBoss13.Down:
				if (this.rigidbody2D.velocity.y >= 0f)
				{
					this.rigidbody2D.velocity = Vector2.zero;
					this.ShakeCamera(4, 0.25f);
					for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
					{
						float num = Mathf.Abs(this._bossPosition.x - GameManager.Instance.ListRambo[i].transform.position.x);
						if (num <= 2f && this._bossPosition.y >= GameManager.Instance.ListRambo[i].transform.position.y)
						{
							GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this.GetDamage(Boss13.E_SkillBoss.Dam), EWeapon.NONE);
							this._attackToRambo = true;
						}
					}
					this._mainLegParticle.startLifetimeMultiplier = 2f;
					this.legParticle.Play();
					this.ChangeState();
				}
				break;
			case Boss13.E_StateBoss13.Jump:
				if (this.rigidbody2D.velocity.y <= 0f)
				{
					this.ChangeState();
				}
				break;
			case Boss13.E_StateBoss13.Tien:
				this.BossMove();
				break;
			case Boss13.E_StateBoss13.Lui:
				this.BossMove();
				break;
			}
		}
		this.UpdateRocket();
	}

	private void UpdateRocket()
	{
		for (int i = 0; i < this.listRocketBosss.Count; i++)
		{
			if (this.listRocketBosss[i].isInited)
			{
				this.listRocketBosss[i].UpdateObject();
			}
		}
	}

	private void CachePosition()
	{
		this._bossPosition = this.transform.position;
		this._ramboPosition = this._ramboTrans.position;
	}

	private void OnLateUpdate()
	{
	}

	private void BossMove()
	{
		this.rigidbody2D.velocity = this._bossVelocity;
		bool flag = this.GetDistanceToRambo() <= 2f || Time.timeSinceLevelLoad - this.TimeMoveBack > 4f;
		if (flag)
		{
			this.ChangeState();
			this.TimeMoveBack = Time.timeSinceLevelLoad;
		}
	}

	private void ChangeState()
	{
		Boss13.E_StateBoss13 state = this._state;
		if (state != Boss13.E_StateBoss13.Jump)
		{
			if (!base.isInCamera)
			{
				this._state = Boss13.E_StateBoss13.Jump;
			}
			else
			{
				bool flag = !this._dangerState && this._state != Boss13.E_StateBoss13.Tien && this._state != Boss13.E_StateBoss13.Lui && this._state != Boss13.E_StateBoss13.Idle && UnityEngine.Random.Range(0, 2) == 1;
				if (flag)
				{
					this._state = Boss13.E_StateBoss13.Idle;
				}
				else
				{
					bool flag2 = Mathf.Abs(this._bossPosition.x - this._ramboPosition.x) > 3f && ((!this._dangerState) ? (UnityEngine.Random.Range(0, 2) == 1) : (UnityEngine.Random.Range(0, 3) == 1));
					if (flag2)
					{
						this._state = ((this.skeletonAnimation.skeleton.FlipX != this._ramboPosition.x > this._bossPosition.x) ? (this._state = Boss13.E_StateBoss13.Lui) : (this._state = Boss13.E_StateBoss13.Tien));
					}
					else
					{
						switch ((!this._dangerState) ? UnityEngine.Random.Range(0, 4) : UnityEngine.Random.Range(0, 6))
						{
						case 0:
							this._state = Boss13.E_StateBoss13.Jump;
							break;
						case 1:
							this._state = Boss13.E_StateBoss13.Phun;
							break;
						case 2:
							this._state = Boss13.E_StateBoss13.Nen;
							break;
						case 3:
							this._state = Boss13.E_StateBoss13.Song_Am;
							break;
						default:
							this._state = Boss13.E_StateBoss13.Rocket_Fire;
							break;
						}
					}
				}
			}
		}
		else
		{
			this._state = Boss13.E_StateBoss13.Down;
		}
		if (this._state != Boss13.E_StateBoss13.Lui)
		{
			this.skeletonAnimation.skeleton.FlipX = (this._ramboPosition.x > this._bossPosition.x);
		}
		this._attackToRambo = false;
		this._changeState = false;
	}

	private float GetDamage(Boss13.E_SkillBoss skill)
	{
		return Mathf.Round(this._bossDamage * this.ratioDamageOfSkills[(int)skill]);
	}

	private float GetDistanceToRambo()
	{
		return Mathf.Abs(this._bossPosition.x - this._ramboPosition.x);
	}

	private float GetDistanceToCam()
	{
		return Mathf.Abs(this._bossPosition.x - CameraController.Instance.camPos.x);
	}

	private void PlayAnim(Boss13.E_StateBoss13 anim, int trackIndex, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(trackIndex, this.anims[(int)anim], loop);
	}

	private void CreatRocket()
	{
		for (int i = 0; i < this.numberRocketFire; i++)
		{
			this._rocket = null;
			this._rocket = this._pollRocketBoss.New();
			if (this._rocket == null)
			{
				this._rocket = UnityEngine.Object.Instantiate<RocketBoss13>(this.listRocketBosss[0]);
				this._rocket.gameObject.transform.parent = this.listRocketBosss[0].gameObject.transform.parent;
				this.listRocketBosss.Add(this._rocket);
			}
			this._rocket.gameObject.SetActive(true);
			this._rocket.transform.position = this.tranformRocketPoints[i * 2].position;
			this._direction = this.tranformRocketPoints[i * 2 + 1].position - this.tranformRocketPoints[i * 2].position;
			this._rocket.InitObject(this.GetDamage(Boss13.E_SkillBoss.Rocket), this._skillSpeed, (float)i * 0.2f + 0.3f, this.tranformRocketPoints[i * 2].position, this._direction, this._ramboTrans, new Action<RocketBoss13>(this.PoolRocket));
		}
	}

	private void PoolRocket(RocketBoss13 rocket)
	{
		this._pollRocketBoss.Store(rocket);
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			this.hit++;
			if (this.hit >= 10)
			{
				this.hit = 0;
				GameManager.Instance.giftManager.CreateItemWeapon(this.transform.position);
			}
		}
		if (!this._dangerState && this.HP <= this.cacheEnemy.HP / 2f)
		{
			this._dangerState = true;
		}
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(3, 0, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			base.StopAllCoroutines();
			this.Die();
			GameManager.Instance.ListEnemy.Remove(this);
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			PlayerManagerStory.Instance.OnRunGameOver();
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[12].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[12].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Tenguka has been unlocked in BossMode!");
			}
		}
	}

	public void ShakeCamera(int numberOfShake, float distance)
	{
		if (GameManager.Instance != null && GameManager.Instance.StateManager.EState != EGamePlay.WIN)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
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
		this.PlayAnim(Boss13.E_StateBoss13.Death, 0, false);
		base.StartCoroutine(this.EffectDie());
	}

	private IEnumerator EffectDie()
	{
		this.ShakeCamera(4, 0.25f);
		Vector3 pos = this.tfOrigin.position;
		Vector3 pos2 = pos;
		pos.y += 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCamera(4, 0.25f);
		pos.x += 1.5f;
		pos.y -= 1f;
		pos2.x -= 1.2f;
		pos2.y -= 1f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCamera(4, 0.25f);
		pos.x -= 2f;
		pos2.x += 2f;
		pos2.y -= 0.9f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield break;
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		switch (this._state)
		{
		case Boss13.E_StateBoss13.Song_Am:
			this.ChangeState();
			break;
		case Boss13.E_StateBoss13.Phun:
			this.ChangeState();
			break;
		case Boss13.E_StateBoss13.Nen:
			this.ChangeState();
			break;
		case Boss13.E_StateBoss13.Rocket_Fire:
			this.ChangeState();
			break;
		case Boss13.E_StateBoss13.Idle:
			this.ChangeState();
			break;
		}
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string name = e.Data.Name;
		switch (name)
		{
		case "attack01":
			this.colliderHandSkill.enabled = true;
			break;
		case "attack01_end":
			this.colliderHandSkill.enabled = false;
			break;
		case "attack02":
			this.phunDoc.Active();
			break;
		case "attack02_end":
			this.phunDoc.Off();
			break;
		case "attack03":
			this.ShakeCamera(2, 0.2f);
			for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
			{
				float num2 = Vector3.Distance(this.handLTrans.position, GameManager.Instance.ListRambo[i].transform.position);
				if (num2 <= 2f && this.handLTrans.position.x >= GameManager.Instance.ListRambo[i].transform.position.y)
				{
					GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this.GetDamage(Boss13.E_SkillBoss.Nen), EWeapon.NONE);
					this._attackToRambo = true;
				}
			}
			break;
		case "attack04":
			this.CreatRocket();
			break;
		case "walk":
			this._mainLegParticle.startLifetimeMultiplier = 1f;
			this.legParticle.Play();
			this.ShakeCamera(2, 0.2f);
			break;
		}
	}

	[SerializeField]
	[Header("*******************************")]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private string[] anims;

	[SerializeField]
	private float[] ratioDamageOfSkills;

	[SerializeField]
	private float ratioSkillSpeed;

	[SerializeField]
	private Collider2D colliderHandSkill;

	[SerializeField]
	private Transform handLTrans;

	[SerializeField]
	private HandBoss1_6 songAm;

	[SerializeField]
	private SkillPhunDocBoss13 phunDoc;

	[SerializeField]
	private ParticleSystem legParticle;

	[SerializeField]
	private int numberRocketFire;

	[SerializeField]
	private List<RocketBoss13> listRocketBosss;

	[SerializeField]
	private Transform[] tranformRocketPoints;

	private Boss13.E_StateBoss13 _state;

	private float _deltaTime;

	private Transform _ramboTrans;

	private Vector2 _bossVelocity;

	private Vector3 _bossPosition;

	private Vector3 _ramboPosition;

	private ObjectPooling<RocketBoss13> _pollRocketBoss;

	private float _bossSpeed;

	private float _bossDamage;

	private float _skillSpeed;

	private bool _isBegin;

	private bool _changeState;

	private bool _loopAnim;

	private bool _attackToRambo;

	private bool _dangerState;

	private ParticleSystem.MainModule _mainLegParticle;

	private float TimeMoveBack;

	private RocketBoss13 _rocket;

	private Vector3 _direction;

	private int hit;

	private enum E_StateBoss13
	{
		Song_Am,
		Phun,
		Nen,
		Rocket_Fire,
		Death,
		Down,
		Hit,
		Idle,
		Jump,
		Tien,
		Lui
	}

	private enum E_SkillBoss
	{
		SongAm,
		Dam,
		Phun,
		Nen,
		Rocket
	}
}
