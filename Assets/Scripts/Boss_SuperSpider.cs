using System;
using System.Collections;
using System.Collections.Generic;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_SuperSpider : BaseBoss
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.InitBoss();
		yield break;
	}

	private void Update()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.PAUSE && !this._isPause)
			{
				this.PauseObject(true);
			}
			return;
		}
		if (this._isPause)
		{
			this.PauseObject(false);
		}
		if (this._state == Boss_SuperSpider.EState.Die)
		{
			return;
		}
		this.CachePosition();
		float deltaTime = Time.deltaTime;
		if (!this.fighting)
		{
			this.UpdateBeforeFighting(deltaTime);
		}
		else
		{
			this.UpdateObject(deltaTime);
		}
	}

	public override void Init()
	{
		base.Init();
		this.fighting = true;
	}

	private void InitBoss()
	{
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		this.bossData = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		float num = GameMode.Instance.GetMode();
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
		{
			num = 5f;
		}
		this.bossData.enemy[0].HP *= num;
		this.bossData.enemy[0].Damage *= num;
		this.bossData.enemy[0].DamageLv2 *= num;
		this._Level = (int)((GameMode.Instance.modePlay != GameMode.ModePlay.Boss_Mode) ? GameMode.Instance.MODE : GameMode.Instance.modeBossMode);
		this.animations = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.boxBoss.OnHit = new Action<float, EWeapon>(this.OnHit);
		this.boxBoss.bodyCollider2D.enabled = false;
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.listBongDinh[0].gameObject.transform.parent.parent = null;
		this.poolBongDinh = new ObjectPooling<BongDinh>(0, null, null);
		this.poolSkillLua = new ObjectPooling<SkillLuaBoss15>(this.listSkillLua.Count, null, null);
		for (int i = 0; i < this.listSkillLua.Count; i++)
		{
			this.poolSkillLua.Store(this.listSkillLua[i]);
		}
		this.hpNhenNho = Mathf.Round(this.bossData.enemy[0].HP / 20f);
		this._coolDownAttack = this.timeReloadAttackWhenFollowRambo;
		this.skillDuoi.Init();
		this._changeState = true;
		this._state = Boss_SuperSpider.EState.Idle;
		this.PlayAnim(0, true);
		this.isInit = true;
	}

	private void Begin()
	{
		if (this._isBegin)
		{
			return;
		}
		this._isBegin = true;
		this._changeState = false;
		this._state = Boss_SuperSpider.EState.XuatHien1;
		this.boxBoss.bodyCollider2D.enabled = true;
		this.boxBoss.cacheEnemy = new Enemy();
		this.boxBoss.cacheEnemy.HP = (this.boxBoss.HP = this.bossData.enemy[0].HP);
		GameManager.Instance.ListEnemy.Add(this.boxBoss);
	}

	private void UpdateBeforeFighting(float deltaTime)
	{
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			return;
		}
		if (!this._changeState)
		{
			this._changeState = true;
			bool loop = false;
			Boss_SuperSpider.EState state = this._state;
			switch (state)
			{
			case Boss_SuperSpider.EState.Attack_Duoi_1:
				break;
			case Boss_SuperSpider.EState.Attack_Duoi_2:
			{
				loop = true;
				this._posSkillDuoi = this._posRambo;
				RaycastHit2D raycastHit2D = Physics2D.Raycast(this._posSkillDuoi + Vector3.up, Vector2.down, 10f, this.groundMask);
				if (raycastHit2D.collider)
				{
					this._posSkillDuoi = raycastHit2D.point;
				}
				this.skillDuoi.Attack(this.GetDamage(), this._posSkillDuoi, delegate
				{
					this._changeState = false;
					this._state = Boss_SuperSpider.EState.Attack_Duoi_3;
				});
				break;
			}
			case Boss_SuperSpider.EState.Attack_Duoi_3:
				this._coolDownAttack = this.timeReloadAttackWhenFollowRambo;
				break;
			default:
				switch (state)
				{
				case Boss_SuperSpider.EState.Idle:
					loop = true;
					break;
				case Boss_SuperSpider.EState.Run:
					loop = true;
					break;
				}
				break;
			}
			this.PlayAnim(0, loop);
		}
		else
		{
			Boss_SuperSpider.EState state2 = this._state;
			switch (state2)
			{
			case Boss_SuperSpider.EState.Attack_Duoi_1:
				break;
			case Boss_SuperSpider.EState.Attack_Duoi_2:
				break;
			case Boss_SuperSpider.EState.Attack_Duoi_3:
				break;
			default:
				switch (state2)
				{
				case Boss_SuperSpider.EState.Idle:
					if (4f < this._posCam.x - this._pos.x)
					{
						this._changeState = false;
						this._state = Boss_SuperSpider.EState.Run;
					}
					else if (this._coolDownAttack <= 0f)
					{
						this._changeState = false;
						this._state = Boss_SuperSpider.EState.Attack_Duoi_1;
					}
					break;
				case Boss_SuperSpider.EState.Run:
					this._pos.x = this._pos.x + this.bossData.enemy[0].Speed * deltaTime * 2f;
					this.transform.position = this._pos;
					if (this._pos.x >= this._posCam.x)
					{
						this._changeState = false;
						this._state = Boss_SuperSpider.EState.Idle;
					}
					break;
				}
				break;
			}
		}
		if (this._coolDownAttack > 0f)
		{
			this._coolDownAttack -= deltaTime;
		}
	}

	private void CachePosition()
	{
		this._pos = this.transform.position;
		this._posRambo = GameManager.Instance.player.transform.position;
		this._posCam = CameraController.Instance.transform.position;
	}

	private void UpdateObject(float deltaTime)
	{
		this.Begin();
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		for (int i = 0; i < this.listBongDinh.Count; i++)
		{
			if (this.listBongDinh[i] && this.listBongDinh[i].isInit)
			{
				this.listBongDinh[i].UpdateObject(deltaTime);
			}
		}
	}

	private void StartState()
	{
		this._changeState = true;
		bool loop = false;
		switch (this._state)
		{
		case Boss_SuperSpider.EState.Attack_Duoi_2:
		{
			loop = true;
			this._posSkillDuoi = this._posRambo;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this._posSkillDuoi + Vector3.up, Vector2.down, 10f, this.groundMask);
			if (raycastHit2D.collider)
			{
				this._posSkillDuoi = raycastHit2D.point;
			}
			this.skillDuoi.Attack(this.GetDamage(), this._posSkillDuoi, delegate
			{
				this.ChangeState();
			});
			break;
		}
		case Boss_SuperSpider.EState.Attack4_2:
			loop = true;
			this._startPosEffGunL = Physics2D.Raycast(this.transGunL.position, this._pos - this.transGunL.position, 10f, this.groundMask).point;
			this._startPosEffGunR = Physics2D.Raycast(this.transGunR.position, this._pos - this.transGunR.position, 10f, this.groundMask).point;
			break;
		case Boss_SuperSpider.EState.Run:
		case Boss_SuperSpider.EState.Run2:
			loop = true;
			this.FindTargetMove();
			break;
		case Boss_SuperSpider.EState.XuatHien2:
			this._pos.x = this._posCam.x;
			this.transform.position = this._pos;
			break;
		}
		this.PlayAnim(0, loop);
	}

	private void PauseObject(bool pause)
	{
		this._isPause = pause;
		this.skeletonAnimation.timeScale = ((!pause) ? this.skeletonTimeScale : 0f);
	}

	private void UpdateState(float deltaTime)
	{
		Boss_SuperSpider.EState state = this._state;
		if (state != Boss_SuperSpider.EState.Run && state != Boss_SuperSpider.EState.Run2)
		{
			if (state == Boss_SuperSpider.EState.Attack4_2)
			{
				if (this._coolDownFile4 > 0f)
				{
					this._coolDownFile4 -= deltaTime;
				}
				if (this._coolDownFile4 <= 0f)
				{
					this._posEffGunL = this._startPosEffGunL + Vector3.left * (float)this._countFire4;
					this._posEffGunR = this._startPosEffGunR + Vector3.right * (float)this._countFire4;
					GameManager.Instance.fxManager.ShowEffect(3, this.transGunL.position, Vector3.one, true, true);
					this._hitGunL = Physics2D.Raycast(this.transGunL.position, this._posEffGunL - this.transGunL.position, 10f, this.ramboMask);
					if (this._hitGunL.transform)
					{
						ISkill component = this._hitGunL.transform.GetComponent<ISkill>();
						if (component == null || !component.IsInVisible())
						{
							this._hitGunL.transform.GetComponent<IHealth>().AddHealthPoint(-this.GetDamage(), EWeapon.NONE);
							GameManager.Instance.fxManager.ShowExplosion1(this._hitGunL.point);
						}
						else
						{
							GameManager.Instance.fxManager.ShowExplosion1(this._posEffGunL);
						}
					}
					else
					{
						GameManager.Instance.fxManager.ShowExplosion1(this._posEffGunL);
					}
					GameManager.Instance.fxManager.ShowEffect(3, this.transGunR.position, Vector3.one, true, true);
					this._hitGunR = Physics2D.Raycast(this.transGunR.position, this._posEffGunR - this.transGunR.position, 10f, this.ramboMask);
					if (this._hitGunR.transform)
					{
						this._hitGunR.transform.GetComponent<IHealth>().AddHealthPoint(-this.GetDamage(), EWeapon.NONE);
						GameManager.Instance.fxManager.ShowExplosion1(this._hitGunR.point);
					}
					else
					{
						GameManager.Instance.fxManager.ShowExplosion1(this._posEffGunR);
					}
					this._countFire4++;
					if (this._countFire4 >= 7)
					{
						this._countFire4 = 0;
						this.ChangeState();
					}
					else
					{
						this._coolDownFile4 = 0.2f;
					}
				}
			}
		}
		else
		{
			this.MoveToTarget(deltaTime);
		}
		for (int i = 0; i < this.listSkillLua.Count; i++)
		{
			if (this.listSkillLua[i] && this.listSkillLua[i].isInit)
			{
				this.listSkillLua[i].UpdateObject();
			}
		}
	}

	private void ChangeState()
	{
		if (!this._crazy)
		{
			this._crazy = (this.boxBoss.HP < this.boxBoss.cacheEnemy.HP * 3f / 5f);
		}
		this._changeState = false;
		switch (this._state)
		{
		case Boss_SuperSpider.EState.Attack_Duoi_1:
			this._state = Boss_SuperSpider.EState.Attack_Duoi_2;
			break;
		case Boss_SuperSpider.EState.Attack_Duoi_2:
			this._state = Boss_SuperSpider.EState.Attack_Duoi_3;
			break;
		case Boss_SuperSpider.EState.Attack_Duoi_3:
		case Boss_SuperSpider.EState.Attack_Nhen:
		case Boss_SuperSpider.EState.Attack1_3:
		case Boss_SuperSpider.EState.Attack4_3:
			this._oldAttackState = this._state;
			this.ChangeStateToIdleOrRun();
			break;
		case Boss_SuperSpider.EState.Attack1_1:
			this._state = Boss_SuperSpider.EState.Attack1_2;
			break;
		case Boss_SuperSpider.EState.Attack1_2:
			this._state = Boss_SuperSpider.EState.Attack1_3;
			break;
		case Boss_SuperSpider.EState.Attack4_1:
			this._state = Boss_SuperSpider.EState.Attack4_2;
			break;
		case Boss_SuperSpider.EState.Attack4_2:
			this._state = Boss_SuperSpider.EState.Attack4_3;
			break;
		case Boss_SuperSpider.EState.Idle:
		case Boss_SuperSpider.EState.Run:
		case Boss_SuperSpider.EState.Run2:
			switch (this._oldAttackState)
			{
			case Boss_SuperSpider.EState.Attack_Duoi_3:
				this._state = Boss_SuperSpider.EState.Attack1_1;
				break;
			case Boss_SuperSpider.EState.Attack_Nhen:
				this._state = Boss_SuperSpider.EState.Attack4_1;
				break;
			case Boss_SuperSpider.EState.Attack1_3:
				this._state = Boss_SuperSpider.EState.Attack_Nhen;
				break;
			case Boss_SuperSpider.EState.Attack4_3:
				this._state = Boss_SuperSpider.EState.Attack_Duoi_1;
				break;
			}
			break;
		case Boss_SuperSpider.EState.XuatHien1:
			this._state = Boss_SuperSpider.EState.XuatHien2;
			break;
		case Boss_SuperSpider.EState.XuatHien2:
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				this._state = Boss_SuperSpider.EState.Attack_Duoi_1;
				break;
			case 1:
				this._state = Boss_SuperSpider.EState.Attack_Nhen;
				break;
			case 2:
				this._state = Boss_SuperSpider.EState.Attack1_1;
				break;
			case 3:
				this._state = Boss_SuperSpider.EState.Attack4_1;
				break;
			}
			break;
		}
	}

	private void ChangeStateToIdleOrRun()
	{
		float num = this._pos.x - this._posRambo.x;
		if (Mathf.Abs(num) < 6f)
		{
			if (!this._crazy)
			{
				this._state = Boss_SuperSpider.EState.Idle;
			}
			else
			{
				switch (UnityEngine.Random.Range(0, 4))
				{
				case 0:
					this._state = Boss_SuperSpider.EState.Attack_Duoi_1;
					break;
				case 1:
					this._state = Boss_SuperSpider.EState.Attack_Nhen;
					break;
				case 2:
					this._state = Boss_SuperSpider.EState.Attack1_1;
					break;
				case 3:
					this._state = Boss_SuperSpider.EState.Attack4_1;
					break;
				}
			}
		}
		else
		{
			this._state = ((!this._crazy) ? ((num <= 0f) ? Boss_SuperSpider.EState.Run : Boss_SuperSpider.EState.Run2) : Boss_SuperSpider.EState.XuatHien1);
		}
	}

	private void FindTargetMove()
	{
		this._targetMove = ((this._pos.x <= this._posRambo.x) ? Mathf.Min(this._posRambo.x, this._posCam.x + 3f) : Mathf.Max(this._posRambo.x, this._posCam.x - 3f));
	}

	private void MoveToTarget(float deltaTime)
	{
		this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetMove, this.bossData.enemy[0].Speed * deltaTime);
		this.transform.position = this._pos;
		float num = Mathf.Abs(this._pos.x - this._posRambo.x);
		float num2 = Mathf.Abs(this._targetMove - this._pos.x);
		if (num2 < 0.1f || num <= 1f)
		{
			this.ChangeState();
		}
	}

	private void PlayAnim(Boss_SuperSpider.EState state, int track = 0, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(track, this.animations[(int)state], loop);
	}

	private void PlayAnim(int track = 0, bool loop = false)
	{
		try
		{
			this.skeletonAnimation.state.SetAnimation(track, this.animations[(int)this._state], loop);
		}
		catch
		{
		}
	}

	private void CreateLua(float speed)
	{
		SkillLuaBoss15 skillLuaBoss = this.poolSkillLua.New();
		if (!skillLuaBoss)
		{
			skillLuaBoss = UnityEngine.Object.Instantiate<SkillLuaBoss15>(this.listSkillLua[0]);
			skillLuaBoss.gameObject.transform.parent = this.listSkillLua[0].gameObject.transform.parent;
			this.listSkillLua.Add(skillLuaBoss);
		}
		skillLuaBoss.Run(this.GetDamage(), speed, this.luaTransform.position, new Action<SkillLuaBoss15>(this.HideLua));
	}

	private void HideLua(SkillLuaBoss15 obj)
	{
		this.poolSkillLua.Store(obj);
	}

	private void CreateNhen()
	{
		this._checkPointCreateNhen = this._posCam;
		int num = this._Level + 1;
		for (int i = 0; i <= num; i++)
		{
			this._checkPointCreateNhen.x = UnityEngine.Random.Range(this._posCam.x - 5f, this._posCam.x + 5f);
			this._hitPointCreateNhen = Physics2D.Raycast(this._checkPointCreateNhen, Vector2.down, 10f, this.groundMask);
			if (this._hitPointCreateNhen.collider)
			{
				EnemyManager.Instance.CreateMiniSpider(this.GetDamage(), this.hpNhenNho, this.GetSpeed(), this._hitPointCreateNhen.point);
			}
		}
	}

	private void CreateBongDinh()
	{
		int num = 4 + this._Level * 2;
		float num2 = CameraController.Instance.Size().x * 2f / (float)num;
		float num3 = CameraController.Instance.camPos.x - CameraController.Instance.Size().x + num2 / 2f;
		float hp = Mathf.Round(this.bossData.enemy[0].HP / 50f);
		Vector3 zero = Vector3.zero;
		zero.y = CameraController.Instance.Size().y + CameraController.Instance.camPos.y + 1f;
		for (int i = 0; i < num; i++)
		{
			zero.x = num3 + num2 * (float)i;
			BongDinh bongDinh = this.poolBongDinh.New();
			if (!bongDinh)
			{
				bongDinh = UnityEngine.Object.Instantiate<BongDinh>(this.listBongDinh[0]);
				this.listBongDinh.Add(bongDinh);
				bongDinh.gameObject.transform.parent = this.listBongDinh[0].gameObject.transform.parent;
			}
			bongDinh.InitObject(hp, this.bossData.enemy[0].Damage, this.bossData.enemy[0].Speed * 2f, zero, zero, delegate(BongDinh b)
			{
				this.poolBongDinh.Store(b);
			});
		}
	}

	private float GetSpeed()
	{
		switch (this._state)
		{
		case Boss_SuperSpider.EState.Attack_Nhen:
			return Mathf.Round(this.bossData.enemy[0].Speed * this.pSpeedNhenNho);
		case Boss_SuperSpider.EState.Attack1_1:
		case Boss_SuperSpider.EState.Attack1_2:
		case Boss_SuperSpider.EState.Attack1_3:
			return Mathf.Round(this.bossData.enemy[0].Speed * this.pSpeedLua);
		default:
			return this.bossData.enemy[0].Speed;
		}
	}

	private float GetDamage()
	{
		switch (this._state)
		{
		case Boss_SuperSpider.EState.Attack_Duoi_1:
		case Boss_SuperSpider.EState.Attack_Duoi_2:
		case Boss_SuperSpider.EState.Attack_Duoi_3:
			return Mathf.Round(this.bossData.enemy[0].Damage * this.pDamDuoi);
		case Boss_SuperSpider.EState.Attack_Nhen:
			return Mathf.Round(this.bossData.enemy[0].Damage * this.pDamNhenNho);
		case Boss_SuperSpider.EState.Attack1_1:
		case Boss_SuperSpider.EState.Attack1_2:
		case Boss_SuperSpider.EState.Attack1_3:
			return Mathf.Round(this.bossData.enemy[0].Damage * this.pDamLua);
		case Boss_SuperSpider.EState.Attack4_1:
		case Boss_SuperSpider.EState.Attack4_2:
		case Boss_SuperSpider.EState.Attack4_3:
			return Mathf.Round(this.bossData.enemy[0].Damage * this.pDamGun);
		default:
			return this.bossData.enemy[0].Damage;
		}
	}

	private void OnHit(float arg1, EWeapon arg2)
	{
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			this.hit++;
			if (this.hit >= 10)
			{
				this.hit = 0;
				GameManager.Instance.giftManager.CreateItemWeapon(this.transform.position);
			}
		}
		if (this.boxBoss.HP > 0f)
		{
			GameManager.Instance.bossManager.ShowLineBloodBoss(this.boxBoss.HP, this.boxBoss.cacheEnemy.HP);
			this.skeletonAnimation.state.SetAnimation(1, this.animations[11], false);
		}
		else
		{
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.boxBoss.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[5].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[5].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Super Spider has been unlocked in BossMode!");
			}
			base.StopAllCoroutines();
			this._state = Boss_SuperSpider.EState.Die;
			this.skeletonAnimation.timeScale = 1f;
			this.PlayAnim(this._state, 0, false);
			base.StartCoroutine(this.Die());
		}
	}

	private IEnumerator Die()
	{
		this._pos = this.luaTransform.position;
		this._pos.y = this._pos.y + 1f;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		this._pos = this.transGunL.position;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		this._pos = this.transGunR.position;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		this._pos = this.luaTransform.position;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		this._pos = this.luaTransform.position;
		this._pos.x = this._pos.x + 2f;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		this._pos = this.luaTransform.position;
		this._pos.x = this._pos.x - 2f;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.3f);
		this._pos = this.luaTransform.position;
		this._pos.y = this._pos.y - 1f;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		PlayerManagerStory.Instance.OnRunGameOver();
		yield break;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		Boss_SuperSpider.EState state = this._state;
		if (state != Boss_SuperSpider.EState.Attack1_2)
		{
			if (state == Boss_SuperSpider.EState.Attack_Nhen)
			{
				this.CreateNhen();
			}
		}
		else
		{
			this.CreateLua(this.GetSpeed());
			this.CreateLua(-this.GetSpeed());
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		if (this.fighting)
		{
			if (trackEntry.Animation.Name.Contains("hit"))
			{
				return;
			}
			switch (this._state)
			{
			case Boss_SuperSpider.EState.Attack_Duoi_1:
			case Boss_SuperSpider.EState.Attack_Duoi_3:
			case Boss_SuperSpider.EState.Attack_Nhen:
			case Boss_SuperSpider.EState.Attack1_1:
			case Boss_SuperSpider.EState.Attack1_3:
			case Boss_SuperSpider.EState.Attack4_1:
			case Boss_SuperSpider.EState.Attack4_3:
			case Boss_SuperSpider.EState.Idle:
			case Boss_SuperSpider.EState.XuatHien2:
				this.ChangeState();
				break;
			case Boss_SuperSpider.EState.Attack1_2:
				this._coutFire1++;
				if (this._coutFire1 > this._Level + 1)
				{
					this._coutFire1 = 0;
					this.ChangeState();
				}
				else
				{
					this.PlayAnim(0, false);
				}
				break;
			case Boss_SuperSpider.EState.XuatHien1:
				base.Invoke("ChangeState", 2f);
				this.CreateBongDinh();
				break;
			}
		}
		else
		{
			Boss_SuperSpider.EState state = this._state;
			if (state != Boss_SuperSpider.EState.Attack_Duoi_1)
			{
				if (state == Boss_SuperSpider.EState.Attack_Duoi_3)
				{
					this._changeState = false;
					this._state = ((this._posCam.x - this._pos.x < 4f) ? Boss_SuperSpider.EState.Idle : Boss_SuperSpider.EState.Run);
				}
			}
			else
			{
				this._changeState = false;
				this._state = Boss_SuperSpider.EState.Attack_Duoi_2;
			}
		}
	}

	internal bool fighting;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private float skeletonTimeScale = 1.3f;

	[SerializeField]
	private BoxBoss1_3 boxBoss;

	[SerializeField]
	private float pDamLua;

	[SerializeField]
	private float pDamDuoi;

	[SerializeField]
	private float pDamNhenNho;

	[SerializeField]
	private float pDamGun;

	[SerializeField]
	private float pSpeedLua;

	[SerializeField]
	private float pSpeedNhenNho;

	[SerializeField]
	private List<SkillLuaBoss15> listSkillLua;

	[SerializeField]
	private Transform luaTransform;

	[SerializeField]
	private List<BongDinh> listBongDinh;

	[SerializeField]
	private float hpNhenNho;

	[SerializeField]
	private SkillDuoiBoss15 skillDuoi;

	[SerializeField]
	private float timeReloadAttackWhenFollowRambo;

	[SerializeField]
	private LayerMask groundMask;

	[SerializeField]
	private LayerMask ramboMask;

	[SerializeField]
	private Transform transGunL;

	[SerializeField]
	private Transform transGunR;

	private int _Level;

	private Boss_SuperSpider.EState _state;

	private EnemyCharactor bossData;

	private Vector3 _pos;

	private bool _changeState;

	private Vector3 _posRambo;

	private Vector3 _posCam;

	private Vector3 _posSkillDuoi;

	private Vector3 _startPosEffGunL;

	private Vector3 _startPosEffGunR;

	private Vector3 _posEffGunL;

	private Vector3 _posEffGunR;

	private RaycastHit2D _hitGunL;

	private RaycastHit2D _hitGunR;

	private float _targetMove;

	private Boss_SuperSpider.EState _oldAttackState;

	private int _coutFire1;

	private Spine.Animation[] animations;

	private ObjectPooling<SkillLuaBoss15> poolSkillLua;

	private ObjectPooling<BongDinh> poolBongDinh;

	private int _countFire4;

	private float _coolDownFile4;

	private Vector2 _checkPointCreateNhen;

	private RaycastHit2D _hitPointCreateNhen;

	private bool _isPause;

	private bool _isBegin;

	private float _coolDownAttack;

	private bool _crazy;

	private int hit;

	private enum EState
	{
		Attack_Duoi_1,
		Attack_Duoi_2,
		Attack_Duoi_3,
		Attack_Nhen,
		Attack1_1,
		Attack1_2,
		Attack1_3,
		Attack4_1,
		Attack4_2,
		Attack4_3,
		Die,
		Hit,
		Idle,
		XuatHien1,
		Run,
		Run2,
		XuatHien2
	}
}
