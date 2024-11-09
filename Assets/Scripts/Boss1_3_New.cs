using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss1_3_New : BaseBoss
{
	private void GetMode()
	{
		this._mode = GameMode.Instance.EMode;
	}

	private void Update()
	{
		if (!this.isInit || this.State == ECharactor.DIE || (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW))
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.PAUSE && this.skeletonAnimation.timeScale != 0f)
			{
				this.timeScale = this.skeletonAnimation.timeScale;
				this.skeletonAnimation.timeScale = 0f;
			}
			return;
		}
		if (this.skeletonAnimation.timeScale == 0f)
		{
			this.skeletonAnimation.timeScale = this.timeScale;
		}
		if (!this.Begin())
		{
			return;
		}
		this.UpdateBoss();
	}

	public void SkipPreviewBoss()
	{
		this._position = this.transform.position;
		this._camPosition = CameraController.Instance.transform.position;
		float num = this._position.x - this._camPosition.x;
		if (num > 12f)
		{
			this._position.x = this._camPosition.x + 12f;
			this.transform.position = this._position;
		}
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
		if (PreGameOver.Instance)
		{
			PreGameOver.Instance.skipPreviewEvent = new Action(this.SkipPreviewBoss);
		}
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
			this.InitEnemy(enemyCharactor, 0);
			this.cacheEnemy.HP *= num;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.animations = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.GetMode();
		this._dinoGun = 2;
		this.InitBullets();
		this.InitRockets();
		this.InitMiniLaser();
		this.FlipBoss(false);
		base.isInCamera = true;
		this.dinoDop.SetDamage(this.GetDamage(Boss1_3_New.EState.Dop));
		this.dinoFlame.damage = this.GetDamage(Boss1_3_New.EState.PhunLua);
		this.dinoFlame.Off();
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this._camTrans = CameraController.Instance.transform;
		this._ramboTrans = GameManager.Instance.player.transform;
		this.maxDe = ((this._mode != GameMode.Mode.SUPER_HARD) ? this.maxDe : 6);
		this.posBegin = new Vector3(this._camTrans.position.x + 5f, this.transform.position.y, this.transform.position.z);
		this.PlaySound(Boss1_3_New.DinoSounds.START, false);
		this._state = Boss1_3_New.EState.PhunLua;
		this.PlayAnim(Boss1_3_New.EState.Run, true, 1f);
		this._isBegin = false;
		this.isInit = true;
	}

	public void SetBegin()
	{
		SpriteVisible component = base.GetComponent<SpriteVisible>();
		if (component != null)
		{
			SpriteVisible spriteVisible = component;
			spriteVisible.OnVisible = (Action)Delegate.Combine(spriteVisible.OnVisible, new Action(delegate()
			{
				base.StartCoroutine(this.DelayBegin());
			}));
		}
	}

	private IEnumerator DelayBegin()
	{
		yield return new WaitForSeconds(1.5f);
		if (!this._isBegin)
		{
			this._isBegin = true;
			this.InitBoxBoss();
			this._changeState = false;
		}
		yield break;
	}

	private void InitBullets()
	{
		if (this.listBulletBoss != null)
		{
			this.listBulletBoss[0].gameObject.transform.parent.parent = null;
			this.poolingBulletsBoss = new ObjectPooling<BulletLuu>(this.listBulletBoss.Count, null, null);
			for (int i = 0; i < this.listBulletBoss.Count; i++)
			{
				this.poolingBulletsBoss.Store(this.listBulletBoss[i]);
			}
		}
	}

	private void InitRockets()
	{
		if (this.listRocketBoss != null)
		{
			this.listRocketBoss[0].gameObject.transform.parent.parent = null;
			this.poolingRocketBoss = new ObjectPooling<RocketBoss1_3>(this.listRocketBoss.Count, null, null);
			for (int i = 0; i < this.listRocketBoss.Count; i++)
			{
				this.poolingRocketBoss.Store(this.listRocketBoss[i]);
			}
		}
	}

	private void InitMiniLaser()
	{
		if (this.listMiniLaser != null)
		{
			this.listMiniLaser[0].gameObject.transform.parent.parent = null;
			this.poolingMiniLaser = new ObjectPooling<MiniLaserBoss1_3>(this.listMiniLaser.Count, null, null);
			for (int i = 0; i < this.listMiniLaser.Count; i++)
			{
				this.poolingMiniLaser.Store(this.listMiniLaser[i]);
			}
		}
	}

	private void InitBoxBoss()
	{
		if (this.boxBoss != null)
		{
			this.boxBoss[0].bodyCollider2D.enabled = true;
			this.boxBoss[0].InitEnemy(this.cacheEnemy.HP);
			this.boxBoss[0].OnHit = new Action<float, EWeapon>(this.Hit);
			this.boxBoss[0].bodyCollider2D.enabled = true;
			this.boxBoss[0].tfOrigin = null;
			this.boxBoss[0].tag = "Boss";
			GameManager.Instance.ListEnemy.Add(this.boxBoss[0]);
			this.boxBoss[1].bodyCollider2D.enabled = true;
			this.boxBoss[1].InitEnemy(this.cacheEnemy.HP);
			this.boxBoss[1].OnHit = new Action<float, EWeapon>(this.Hit);
			this.boxBoss[1].bodyCollider2D.enabled = true;
			this.boxBoss[1].tfOrigin = null;
			GameManager.Instance.ListEnemy.Add(this.boxBoss[1]);
		}
	}

	private bool Begin()
	{
		if (this._isBegin)
		{
			return true;
		}
		this.transform.Translate(-this.cacheEnemy.Speed * Time.deltaTime, 0f, 0f);
		bool flag = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x) < 5f && base.isInCamera;
		if (flag)
		{
			this._isBegin = true;
			this.InitBoxBoss();
			this._changeState = false;
		}
		return false;
	}

	private void UpdateBoss()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.CachePos();
		float deltaTime = Time.deltaTime;
		if (!this._changeState)
		{
			this._changeState = true;
			switch (this._state)
			{
			case Boss1_3_New.EState.PhunLua:
				this.FlipBoss(this._position.x < this._ramboPosition.x);
				this.PlayAnim(this._state, false, 1f);
				this.PlaySound(Boss1_3_New.DinoSounds.FLAME, false);
				break;
			case Boss1_3_New.EState.Dop:
				this._targetMove.y = this._position.y;
				this._targetMove.x = this._camPosition.x + (float)((this._ramboPosition.x <= this._camPosition.x) ? -12 : 12);
				this.FlipBoss(this._position.x < this._targetMove.x);
				this._isGoDop = false;
				this.PlayAnim(Boss1_3_New.EState.Run, true, 2f);
				break;
			case Boss1_3_New.EState.Ban_Luu:
				this.FlipBoss(this._position.x < this._ramboPosition.x);
				if (this._dinoGun == 2)
				{
					this.PlayAnim(this._state, 0);
				}
				else
				{
					this.PlayAnim(Boss1_3_New.EState.Gun2, 1);
					this.PlaySound(Boss1_3_New.DinoSounds.CHANGE_GUN, false);
				}
				break;
			case Boss1_3_New.EState.Ban_Rocket:
				this.FlipBoss(this._position.x < this._ramboPosition.x);
				this._idRocket = 0;
				if (this._dinoGun == 1)
				{
					this.PlayAnim(this._state, 0);
				}
				else
				{
					this.PlayAnim(Boss1_3_New.EState.Gun1, 1);
					this.PlaySound(Boss1_3_New.DinoSounds.CHANGE_GUN, false);
				}
				break;
			case Boss1_3_New.EState.Tha_De1:
				this.FlipBoss(this._position.x < this._ramboPosition.x);
				this.PlayAnim(this._state, false, 1f);
				break;
			case Boss1_3_New.EState.Idle:
				this.PlayAnim(this._state, false, 1f);
				break;
			case Boss1_3_New.EState.Walk:
				this.FindTargetMove();
				this.FlipBoss(this._position.x < this._targetMove.x);
				this.PlayAnim(this._state, true, 1f);
				break;
			}
		}
		else
		{
			switch (this._state)
			{
			case Boss1_3_New.EState.Dop:
				if (this._position != this._targetMove)
				{
					this._position = Vector3.MoveTowards(this._position, this._targetMove, this.cacheEnemy.Speed * 4f * deltaTime);
					this.transform.position = this._position;
					if (!this._isGoDop)
					{
						if (this._position == this._targetMove)
						{
							this._isGoDop = true;
							this._targetMove.x = this._ramboPosition.x + (float)((this._ramboPosition.x <= this._position.x) ? 3 : -3);
							this._targetMove.x = Mathf.Max(this._targetMove.x, this._camPosition.x - 5f);
							this._targetMove.x = Mathf.Min(this._targetMove.x, this._camPosition.x + 5f);
							this.FlipBoss(this._position.x < this._ramboPosition.x);
						}
					}
					else
					{
						float num = base.Distance(this._position.x, this._ramboPosition.x);
						if (num < 3f || this._position == this._targetMove)
						{
							this.PlayAnim(this._state, false, 1f);
							this.dinoDop.col.enabled = true;
							this._targetMove = this._position;
						}
					}
				}
				break;
			case Boss1_3_New.EState.Walk:
				this._position = Vector3.MoveTowards(this._position, this._targetMove, this.cacheEnemy.Speed * deltaTime);
				this.transform.position = this._position;
				if (this._position == this._targetMove)
				{
					this.ChangeState();
				}
				break;
			}
		}
		this.UpdateBullet(deltaTime);
		this.UpdateRocket(deltaTime);
		this.UpdateDe(deltaTime);
	}

	private void CachePos()
	{
		this._position = this.transform.position;
		this._camPosition = this._camTrans.position;
		this._ramboPosition = this._ramboTrans.position;
	}

	private void UpdateBullet(float deltaTime)
	{
		for (int i = 0; i < this.listBulletBoss.Count; i++)
		{
			if (this.listBulletBoss[i].isInit)
			{
				this.listBulletBoss[i].UpdateObject(deltaTime);
			}
		}
	}

	private void UpdateRocket(float deltaTime)
	{
		for (int i = 0; i < this.listRocketBoss.Count; i++)
		{
			if (this.listRocketBoss[i].isInit)
			{
				this.listRocketBoss[i].UpdateObject(deltaTime);
			}
		}
	}

	private void UpdateDe(float deltaTime)
	{
		for (int i = 0; i < this.listMiniLaser.Count; i++)
		{
			if (this.listMiniLaser[i].isInit)
			{
				this.listMiniLaser[i].UpdateObject(deltaTime);
			}
		}
	}

	private void ChangeState()
	{
		float num = base.Distance(this._position.x, this._ramboPosition.x);
		switch (this._state)
		{
		case Boss1_3_New.EState.PhunLua:
		case Boss1_3_New.EState.Dop:
		case Boss1_3_New.EState.Ban_Luu:
		case Boss1_3_New.EState.Ban_Rocket:
		case Boss1_3_New.EState.Tha_De1:
			this._state = ((num < 6f) ? Boss1_3_New.EState.Idle : Boss1_3_New.EState.Walk);
			break;
		case Boss1_3_New.EState.Idle:
		case Boss1_3_New.EState.Walk:
		{
			bool flag = this.HP > 2f * this.cacheEnemy.HP / 3f;
			if (flag)
			{
				this._state = ((num >= 3.5f) ? ((num >= 6f) ? Boss1_3_New.EState.PhunLua : Boss1_3_New.EState.Ban_Luu) : Boss1_3_New.EState.Ban_Rocket);
			}
			else if (this._countAttack >= 3)
			{
				this._state = Boss1_3_New.EState.Tha_De1;
				this._countAttack = 0;
			}
			else if (this._countAttack == 1)
			{
				this._state = Boss1_3_New.EState.Dop;
				this._countAttack++;
			}
			else
			{
				this._state = ((num >= 3.5f) ? ((num >= 6f) ? Boss1_3_New.EState.PhunLua : Boss1_3_New.EState.Ban_Luu) : Boss1_3_New.EState.Ban_Rocket);
				this._countAttack++;
			}
			break;
		}
		}
		this._changeState = false;
	}

	private void FlipBoss(bool flip)
	{
		this._flip = flip;
		Vector3 localScale = this.transform.localScale;
		localScale.x = ((!flip) ? Mathf.Abs(localScale.x) : (-Mathf.Abs(localScale.x)));
		this.transform.localScale = localScale;
	}

	private void PlayAnim(Boss1_3_New.EState state, bool loop = false, float speedAnim = 1f)
	{
		this.skeletonAnimation.timeScale = speedAnim;
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.animations[(int)state], loop);
	}

	private void PlayAnim(Boss1_3_New.EState state, int order)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(order, this.animations[(int)state], false);
	}

	private void FindTargetMove()
	{
		this._targetMove.y = this._position.y;
		this._targetMove.x = this._ramboPosition.x + (float)((this._position.x >= this._ramboPosition.x) ? 2 : -2);
	}

	private IEnumerator CreatBullet()
	{
		float lineTime = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.PlaySound(Boss1_3_New.DinoSounds.FIRE, false);
			this._bullet = null;
			this._bullet = this.poolingBulletsBoss.New();
			if (this._bullet == null)
			{
				this._bullet = UnityEngine.Object.Instantiate<BulletLuu>(this.listBulletBoss[0], this.gunPoints[i + 4].position, Quaternion.identity);
				this._bullet.gameObject.transform.parent = this.listBulletBoss[0].gameObject.transform.parent;
				this.listBulletBoss.Add(this._bullet);
			}
			Vector3 director = this.gunPoints[i + 4].position - this.gunPoints[i].position;
			this._bullet.InitObject(this.GetDamage(Boss1_3_New.EState.Ban_Luu), this.GetSpeed(Boss1_3_New.EState.Ban_Luu), lineTime, this.gunPoints[i + 4].position, director, new Action<BulletLuu>(this.OnHideBullet));
			lineTime += 0.05f;
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	private void OnHideBullet(BulletLuu bullet)
	{
		this.poolingBulletsBoss.Store(bullet);
	}

	private void CreatRocket()
	{
		this.PlaySound(Boss1_3_New.DinoSounds.ROCKET, false);
		this._rocket = null;
		this._rocket = this.poolingRocketBoss.New();
		if (this._rocket == null)
		{
			this._rocket = UnityEngine.Object.Instantiate<RocketBoss1_3>(this.listRocketBoss[0]);
			this._rocket.gameObject.transform.parent = this.listRocketBoss[0].gameObject.transform.parent;
			this.listRocketBoss.Add(this._rocket);
		}
		this._rocketPos = this.rocketPoints[this._idRocket].position;
		this._rocketDirection = this.rocketPoints[this._idRocket + 2].position - this.rocketPoints[this._idRocket].position;
		this._rocket.InitObject(this.GetDamage(Boss1_3_New.EState.Ban_Rocket), this.GetSpeed(Boss1_3_New.EState.Ban_Rocket), 1f, this._rocketPos, this._rocketDirection, this._ramboTrans, new Action<RocketBoss1_3>(this.OnActionInActiveRocket));
		this._idRocket++;
	}

	private void OnActionInActiveRocket(RocketBoss1_3 rocket)
	{
		this.poolingRocketBoss.Store(rocket);
		this.ShakeCam(false);
	}

	private IEnumerator ThaDe()
	{
		Vector2 laserDirection = Vector2.zero;
		laserDirection.y = -1f;
		Vector3 targetMove = Vector3.zero;
		float delta = 12.6f / (float)this.maxDe;
		for (int i = 0; i < this.maxDe; i++)
		{
			this.miniLaser = null;
			this.miniLaser = this.poolingMiniLaser.New();
			if (this.miniLaser == null)
			{
				this.miniLaser = UnityEngine.Object.Instantiate<MiniLaserBoss1_3>(this.listMiniLaser[0]);
				this.miniLaser.gameObject.transform.parent = this.listMiniLaser[0].gameObject.transform.parent;
				this.listMiniLaser.Add(this.miniLaser);
			}
			float min = this._camPosition.x - 6f + (float)i * delta;
			targetMove.x = UnityEngine.Random.Range(min, min + delta);
			targetMove.y = this._camPosition.y + 3.2f;
			laserDirection.y = -1f;
			laserDirection.x = ((UnityEngine.Random.Range(0, 2) != 1) ? ((targetMove.x <= this._camPosition.x) ? UnityEngine.Random.Range(0f, 1f) : UnityEngine.Random.Range(-1f, 0f)) : 0f);
			this.miniLaser.InitObjec(this.GetDamage(this._state), 0.5f + (float)i * 0.1f, 5f, laserDirection, this.miniLaserPoint.position, (!this.skeletonAnimation.skeleton.FlipX) ? Vector3.left : Vector3.right, targetMove, this.GetSpeed(this._state), new Action<MiniLaserBoss1_3>(this.HideMiniLaser));
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	private void HideMiniLaser(MiniLaserBoss1_3 obj)
	{
		this.poolingMiniLaser.Store(obj);
	}

	private void PlaySound(Boss1_3_New.DinoSounds sound, bool loop = false)
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.audioClip[(int)sound], 0.5f);
		}
		catch
		{
		}
	}

	private float GetDamage(Boss1_3_New.EState skill)
	{
		float a = Mathf.Round(this.ratioDamages[(int)skill] * this.cacheEnemy.Damage);
		return Mathf.Max(a, 1f);
	}

	private float GetSpeed(Boss1_3_New.EState skill)
	{
		float a = this.ratioSpeed[(int)skill] * this.cacheEnemy.Speed;
		return Mathf.Max(a, 1f);
	}

	private void ShakeCam(bool isBegin = false)
	{
		try
		{
			if (GameManager.Instance.StateManager.EState != EGamePlay.WIN)
			{
				if (!isBegin)
				{
					CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
				}
				else
				{
					CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
				}
			}
		}
		catch
		{
		}
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			base.StopAllCoroutines();
			this.Die();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			base.StartCoroutine(this.EffectDie());
			return;
		}
		this.skeletonAnimation.state.SetAnimation(2, this.animations[10], false);
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
	}

	public void Hit(float damage, EWeapon eweapon)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.HP += damage;
		this.lastWeapon = eweapon;
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			this.hit++;
			if (this.hit >= 10)
			{
				this.hit = 0;
				GameManager.Instance.giftManager.CreateItemWeapon(this.transform.position);
			}
		}
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			base.StopAllCoroutines();
			this.Die();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			base.StartCoroutine(this.EffectDie());
			return;
		}
		this.skeletonAnimation.state.SetAnimation(2, this.animations[10], false);
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
	}

	private void Die()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		this.dinoFlame.particle.Stop();
		this.skeletonAnimation.AnimationState.SetEmptyAnimations(0f);
		this.PlayAnim(Boss1_3_New.EState.Death, false, 1f);
		this.skeletonAnimation.timeScale = 0.2f;
		this.ShakeCam(false);
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
	}

	private IEnumerator EffectDie()
	{
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		Vector3 pos = this.dinoFlame.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.1f);
		this.ShakeCam(false);
		pos = this.tfTarget[0].position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.1f);
		this.ShakeCam(false);
		pos = this.tfTarget[1].position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.1f);
		this.ShakeCam(false);
		pos = this.tfTarget[2].position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		pos = this.transform.position;
		pos.y += 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 1.5f);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = this.dinoFlame.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.5f);
		this.ShakeCam(false);
		pos = this.boxBoss[1].transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		yield return new WaitForSeconds(0.5f);
		this.skeletonAnimation.timeScale = 1f;
		base.StartCoroutine(this.WaitMissionCompleted());
		PlayerManagerStory.Instance.OnRunGameOver();
		DailyQuestManager.Instance.MissionBoss(0, 1, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
		{
			if (isCompleted)
			{
				UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
			}
		});
		yield break;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "attack1"))
			{
				if (!(name == "of"))
				{
					if (!(name == "attack3"))
					{
						if (!(name == "attack4"))
						{
							if (!(name == "attack2"))
							{
								if (name == "run")
								{
									this.ShakeCam(false);
									this.PlaySound(Boss1_3_New.DinoSounds.WALK, false);
								}
							}
							else
							{
								this.dinoDop.col.enabled = true;
							}
						}
						else
						{
							this.CreatRocket();
						}
					}
					else
					{
						this._countAttack3++;
						if (this._countAttack3 <= 1)
						{
							base.StartCoroutine(this.CreatBullet());
						}
						else
						{
							this._countAttack3 = 0;
						}
					}
				}
				else
				{
					this.dinoFlame.Off();
				}
			}
			else
			{
				this.dinoFlame.Active();
			}
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString();
		switch (text)
		{
		case "1":
			this._dinoGun = 1;
			this.PlayAnim(this._state, 0);
			break;
		case "2":
			this._dinoGun = 2;
			this.PlayAnim(this._state, 0);
			break;
		case "attack5-1":
			base.StartCoroutine(this.ThaDe());
			this.PlayAnim(Boss1_3_New.EState.Tha_De2, false, 1f);
			break;
		case "attack5-2":
			this.PlayAnim(Boss1_3_New.EState.Tha_De3, false, 1f);
			break;
		case "attack2":
			this.dinoDop.col.enabled = false;
			this.ChangeState();
			break;
		case "attack1":
		case "attack4":
		case "attack3":
		case "attack5-3":
		case "idle":
			this.ChangeState();
			break;
		}
	}

	private IEnumerator WaitMissionCompleted()
	{
		if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[1].CheckUnlock(GameMode.Mode.NORMAL))
		{
			ProfileManager.bossModeProfile.bossProfiles[1].SetUnlock(GameMode.Mode.NORMAL, true);
			yield return new WaitForSeconds(1f);
			UIShowInforManager.Instance.ShowUnlock("BossMode is unlocked!");
			yield return new WaitForSeconds(0.5f);
			UIShowInforManager.Instance.ShowUnlock("T-Rex has been unlocked in BossMode!");
		}
		yield break;
	}

	[SerializeField]
	[Header("Spine Anim:___________________")]
	[Header("*******************************")]
	private SkeletonAnimation skeletonAnimation;

	private Spine.Animation[] animations;

	[Header("Boss variable:__________________")]
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip[] audioClip;

	[SerializeField]
	private BoxBoss1_3[] boxBoss;

	[SerializeField]
	private DinoFlame dinoFlame;

	[SerializeField]
	private HandBoss1_6 dinoDop;

	[SerializeField]
	private float[] ratioDamages;

	[SerializeField]
	private float[] ratioSpeed;

	[SerializeField]
	private List<BulletLuu> listBulletBoss;

	[SerializeField]
	private List<RocketBoss1_3> listRocketBoss;

	[SerializeField]
	private List<MiniLaserBoss1_3> listMiniLaser;

	[SerializeField]
	private int maxDe;

	[SerializeField]
	private Transform[] gunPoints;

	[SerializeField]
	private Transform[] rocketPoints;

	[SerializeField]
	private Transform miniLaserPoint;

	private ObjectPooling<BulletLuu> poolingBulletsBoss;

	private ObjectPooling<RocketBoss1_3> poolingRocketBoss;

	private ObjectPooling<MiniLaserBoss1_3> poolingMiniLaser;

	private int _model;

	private bool _isBegin;

	private Boss1_3_New.EState _state;

	private bool _changeState;

	private int _dinoGun;

	private int _countAttack;

	private int _countRocket;

	private BulletLuu _bullet;

	private RocketBoss1_3 _rocket;

	private Transform _ramboTrans;

	private Transform _camTrans;

	private Vector3 _position;

	private Vector3 _ramboPosition;

	private Vector3 _camPosition;

	private Vector3 _targetMove;

	private Vector3 _rocketPos;

	private Vector3 _rocketDirection;

	private int effDieCount;

	private bool _isGoDop;

	private MiniLaserBoss1_3 miniLaser;

	private Vector3 posBegin;

	private float timePingPongColorRambo;

	private bool hitRambo;

	private float timeScale;

	private Vector2 offset;

	private GameMode.Mode _mode;

	private int _countAttack3;

	private int _idRocket;

	private bool _flip;

	private int hit;

	private enum EState
	{
		Gun1,
		Gun2,
		PhunLua,
		Dop,
		Ban_Luu,
		Ban_Rocket,
		Tha_De1,
		Tha_De2,
		Tha_De3,
		Death,
		Hit,
		Idle,
		Run,
		Walk
	}

	public enum DinoSounds
	{
		START,
		WALK,
		FLAME,
		FIRE,
		ROCKET,
		CHANGE_GUN
	}
}
