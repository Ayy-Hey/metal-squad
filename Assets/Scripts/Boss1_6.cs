using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using PVPManager;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss1_6 : BaseBoss
{
	private void GetMode()
	{
		this._mode = GameMode.Instance.EMode;
	}

	private void Update()
	{
		if (!this.isInit || this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.PAUSE && this.skeletonAnimation.timeScale != 0f)
			{
				this.timeScale = this.skeletonAnimation.timeScale;
				this.skeletonAnimation.timeScale = 0f;
				this.PauseBullet(true);
			}
			return;
		}
		if (this.skeletonAnimation.timeScale == 0f)
		{
			this.skeletonAnimation.timeScale = this.timeScale;
			this.PauseBullet(false);
		}
		if (!this.Begin())
		{
			return;
		}
		this.UpdateBoss();
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
			if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				enemyCharactor.enemy[0].Damage *= 3f;
			}
			else
			{
				enemyCharactor.enemy[0].Damage *= num;
			}
			this.InitEnemy(enemyCharactor, 0);
			this.cacheEnemy.HP *= num;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.hpBong = Mathf.Round(this.cacheEnemy.HP / 70f);
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.animsFxDa = this.skeFxDa.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.GetMode();
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this._camTransform = CameraController.Instance.transform;
		this._ramboTransform = GameManager.Instance.player.transform;
		this.InitBullet();
		this.InitLaserBoss();
		this.InitBong();
		this.ActiveWarning(false);
		Behaviour col = this.handsBoss[0].col;
		bool enabled = false;
		this.handsBoss[1].col.enabled = enabled;
		col.enabled = enabled;
		this._changeState = false;
		this._state = Boss1_6.E_State.Dam;
		this.InitBoxBoss();
		this.posBegin = this.transform.position;
		this.posBegin.y = this.posBegin.y + 11.15f;
		this.PlaySound(Boss1_6.TitanSound.START, false);
		this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
		this.ShakeCam(false);
		if (GameMode.Instance.modePlay != GameMode.ModePlay.Boss_Mode && GameMode.Instance.modePlay != GameMode.ModePlay.PvpMode)
		{
			base.Invoke("FixCamera", 1f);
		}
		this.isInit = true;
	}

	private void FixCamera()
	{
		if (this.isFixCamera)
		{
			return;
		}
		this.isFixCamera = true;
		float zoomAmount = 8f - CameraController.Instance.mCamera.orthographicSize;
		ProCamera2D.Instance.Zoom(zoomAmount, 3f, EaseType.EaseInOut);
		ProCamera2D.Instance.OffsetY = -0.5f;
	}

	private void InitBoxBoss()
	{
		this.boxBoss.InitEnemy(this.cacheEnemy.HP);
		this.boxBoss.OnHit = new Action<float, EWeapon>(this.Hit);
		this.boxBoss.bodyCollider2D.enabled = false;
		this.boxBoss.AddHealthPointAction = this.AddHealthPointAction;
		GameManager.Instance.ListEnemy.Add(this.boxBoss);
	}

	private void SetDamageHand(float damage)
	{
		for (int i = 0; i < this.handsBoss.Length; i++)
		{
			this.handsBoss[i].SetDamage(damage);
		}
	}

	private void InitBullet()
	{
		if (this.listBullet != null)
		{
			this.poolingBullets = new ObjectPooling<BulletBoss1_6>(this.listBullet.Count, null, null);
			for (int i = 0; i < this.listBullet.Count; i++)
			{
				this.poolingBullets.Store(this.listBullet[i]);
			}
		}
	}

	private void InitBong()
	{
		if (this.listBong != null)
		{
			this.poolingBongs = new ObjectPooling<BongDinh>(this.listBong.Count, null, null);
			for (int i = 0; i < this.listBong.Count; i++)
			{
				this.poolingBongs.Store(this.listBong[i]);
			}
		}
	}

	private void InitLaserBoss()
	{
		this.laserBoss.speed = -6f;
		this.laserBoss.damage = this.GetDamage(Boss1_6.E_State.Laser1);
		this.laserBoss.damage /= 2f;
		this.laserBoss.ActionAttackToRambo = new Action(this.OnActionAttackToRambo);
	}

	private bool Begin()
	{
		if (this._isBegin)
		{
			return true;
		}
		this.ShakeCam(true);
		this.transform.position = Vector3.MoveTowards(this.transform.position, this.posBegin, Time.deltaTime * 2f);
		this.skeletonAnimation.skeleton.FlipX = false;
		if (this.transform.position == this.posBegin)
		{
			this._isBegin = true;
			this.meshRenderer.sortingLayerName = "Gameplay";
			this.meshRenderer.sortingOrder = 0;
			this.boxBoss.bodyCollider2D.enabled = true;
			this.PlayAnim(this._state, 1f, false);
		}
		return false;
	}

	private void UpdateBoss()
	{
		float deltaTime = Time.deltaTime;
		if (!this._changeState)
		{
			switch (this._state)
			{
			case Boss1_6.E_State.Dam:
			case Boss1_6.E_State.Dam_quet:
			case Boss1_6.E_State.Om:
				this.SetDamageHand(this.GetDamage(this._state));
				if (this._state == Boss1_6.E_State.Dam)
				{
					this.ActiveWarning(true);
				}
				break;
			}
			this._changeState = true;
		}
		else
		{
			Boss1_6.E_State state = this._state;
			if (state != Boss1_6.E_State.Laser1 && state != Boss1_6.E_State.Laser2)
			{
			}
		}
		this.UpdateBullet();
		if (this.laserBoss.isActive)
		{
			this.laserBoss.UpdateObject(deltaTime);
		}
		this.UpdateBong(deltaTime);
	}

	public void ChanState()
	{
		bool flag = this.boxBoss.HP > this.cacheEnemy.HP * 5f / 7f;
		bool flag2 = this.boxBoss.HP > this.cacheEnemy.HP * 3f / 7f;
		if (flag)
		{
			Boss1_6.E_State state = this._state;
			if (state != Boss1_6.E_State.Dam)
			{
				if (state != Boss1_6.E_State.Fire_1)
				{
					if (state != Boss1_6.E_State.Die)
					{
						this._state = Boss1_6.E_State.Dam;
					}
				}
				else
				{
					try
					{
						if (this.transform.position.x > this._ramboTransform.position.x)
						{
							this._state = Boss1_6.E_State.Laser2;
						}
						else
						{
							this._state = Boss1_6.E_State.Laser1;
						}
					}
					catch
					{
					}
				}
			}
			else
			{
				this._state = Boss1_6.E_State.Fire_1;
				this.ChangeFireType();
			}
		}
		else if (flag2)
		{
			Boss1_6.E_State state2 = this._state;
			switch (state2)
			{
			case Boss1_6.E_State.Dam:
				this._state = Boss1_6.E_State.Fire_1;
				this.ChangeFireType();
				break;
			case Boss1_6.E_State.Dam_quet:
				this._state = Boss1_6.E_State.Om;
				break;
			case Boss1_6.E_State.Om:
				try
				{
					if (this.transform.position.x > this._ramboTransform.position.x)
					{
						this._state = Boss1_6.E_State.Laser2;
					}
					else
					{
						this._state = Boss1_6.E_State.Laser1;
					}
				}
				catch
				{
				}
				break;
			default:
				if (state2 != Boss1_6.E_State.Die)
				{
					this._state = Boss1_6.E_State.Dam;
				}
				break;
			case Boss1_6.E_State.Fire_1:
				this._state = Boss1_6.E_State.Dam_quet;
				break;
			}
		}
		else
		{
			switch (this._state)
			{
			case Boss1_6.E_State.Dam:
				this._state = Boss1_6.E_State.Fire_1;
				this.ChangeFireType();
				goto IL_28E;
			case Boss1_6.E_State.Dam_quet:
				try
				{
					if (this.transform.position.x > this._ramboTransform.position.x)
					{
						this._state = Boss1_6.E_State.Laser2;
					}
					else
					{
						this._state = Boss1_6.E_State.Laser1;
					}
				}
				catch
				{
				}
				goto IL_28E;
			case Boss1_6.E_State.Om:
				this._state = Boss1_6.E_State.BongDuoi_1;
				goto IL_28E;
			case Boss1_6.E_State.Laser1:
			case Boss1_6.E_State.Laser2:
				this._state = Boss1_6.E_State.Om;
				goto IL_28E;
			case Boss1_6.E_State.Fire_1:
				this._state = Boss1_6.E_State.Dam_quet;
				goto IL_28E;
			case Boss1_6.E_State.Die:
				goto IL_28E;
			}
			this._state = Boss1_6.E_State.Dam;
		}
		IL_28E:
		this._changeState = false;
		this.PlayAnim(this._state, 1f, false);
	}

	private void ActiveWarning(bool enable)
	{
		this.objWarning[0].SetActive(enable);
		this.objWarning[1].SetActive(enable);
	}

	private void UpdateBullet()
	{
		for (int i = 0; i < this.listBullet.Count; i++)
		{
			if (this.listBullet[i].isInit)
			{
				this.listBullet[i].UpdateObject();
			}
		}
	}

	private void PauseBullet(bool pause)
	{
		for (int i = 0; i < this.listBullet.Count; i++)
		{
			if (this.listBullet[i].isInit)
			{
				if (pause)
				{
					this.listBullet[i].Pause();
				}
				else
				{
					this.listBullet[i].Resume();
				}
			}
		}
	}

	private void UpdateBong(float deltaTime)
	{
		for (int i = 0; i < this.listBong.Count; i++)
		{
			if (this.listBong[i].isInit)
			{
				this.listBong[i].UpdateObject(deltaTime);
			}
		}
	}

	private void PlayAnim(Boss1_6.E_State state, float timeScale = 1f, bool loop = false)
	{
		switch (state)
		{
		case Boss1_6.E_State.Dam_quet:
		case Boss1_6.E_State.Om:
		case Boss1_6.E_State.Laser1:
			this.skeFxDa.skeleton.FlipX = false;
			break;
		case Boss1_6.E_State.Laser2:
			this.skeFxDa.skeleton.FlipX = true;
			break;
		}
		this.skeletonAnimation.timeScale = timeScale;
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)state], loop);
	}

	private void ActiveHandsBoss(int id, bool active = true)
	{
		this.handsBoss[id].col.enabled = active;
	}

	private void Fire()
	{
		this.PlaySound(Boss1_6.TitanSound.FIRE, false);
		for (int i = 0; i < 7; i++)
		{
			float angle = 112.5f - 2.5f * (float)this._mode + (float)i * 22.5f + 2.5f * (float)this._fireCount * (float)this._mode;
			this.CreateBullet(angle);
		}
		this._fireCount++;
	}

	private void CreateBullet(float angle)
	{
		this._bullet = null;
		this._bullet = this.poolingBullets.New();
		if (this._bullet == null)
		{
			this._bullet = UnityEngine.Object.Instantiate<BulletBoss1_6>(this.listBullet[0]);
			this._bullet.gameObject.transform.parent = this.listBullet[0].gameObject.transform.parent;
			this._bullet.gameObject.transform.localScale = this.listBullet[0].gameObject.transform.localScale;
			this.listBullet.Add(this._bullet);
		}
		this._bullet.gameObject.SetActive(true);
		this._bullet.DisableAction = new Action<BulletBoss1_6>(this.OnActionDisableBullet);
		this._bullet.ActionAttackToRambo = new Action(this.OnActionAttackToRambo);
		this._bullet.InitObject();
		float damage = this.GetDamage(Boss1_6.E_State.Fire_1);
		float speed = this.GetSpeed(this._state) * (float)(this.fireType + 1);
		this._bullet.SetValue(EWeapon.NONE, 0, this.firePoint.position, Vector3.down, damage, speed, angle);
	}

	private void FireTargetRambo()
	{
		try
		{
			Quaternion quaternion = Quaternion.LookRotation(this.firePoint.position - this._ramboTransform.position, Vector3.forward);
			quaternion.x = (quaternion.y = 0f);
			this.CreateBullet(quaternion.eulerAngles.z);
			this._fireCount++;
		}
		catch
		{
		}
	}

	private void OnActionDisableBullet(BulletBoss1_6 bullet)
	{
		this.poolingBullets.Store(bullet);
	}

	private void ChangeFireType()
	{
		this.fireType = ((this.fireType != 0) ? 0 : 1);
		this._fireTime = (int)(((int)this._mode + (int)1) * ((int)this.fireType + (int)GameMode.Mode.HARD));
	}

	private void FireBong()
	{
		float num = 12f / (float)(this.numberBong - 1);
		this.bongTargetPoint.x = CameraController.Instance.transform.position.x - 6f + UnityEngine.Random.Range(-0.3f, 0.3f);
		this.bongTargetPoint.y = this.fireBongPoint.position.y + 3.5f;
		for (int i = 0; i < this.numberBong; i++)
		{
			this.CreatBong();
			this.bongTargetPoint.x = this.bongTargetPoint.x + num;
		}
	}

	private void CreatBong()
	{
		this.bong = null;
		this.bong = this.poolingBongs.New();
		if (this.bong == null)
		{
			this.bong = UnityEngine.Object.Instantiate<BongDinh>(this.listBong[0]);
			this.bong.gameObject.transform.parent = this.listBong[0].gameObject.transform.parent;
			this.bong.gameObject.transform.localScale = this.listBong[0].gameObject.transform.localScale;
			this.listBong.Add(this.bong);
		}
		this.bong.InitObject(this.hpBong, this.GetDamage(this._state), this.GetSpeed(this._state), this.fireBongPoint.position, this.bongTargetPoint, new Action<BongDinh>(this.BongHide));
	}

	private void BongHide(BongDinh obj)
	{
		this.poolingBongs.Store(obj);
	}

	private void OnActionAttackToRambo()
	{
	}

	private void PlaySound(Boss1_6.TitanSound sound, bool loop = false)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioSource.clip = this.audioClips[(int)sound];
			this.audioSource.loop = loop;
			this.audioSource.Play();
		}
	}

	private void StopSound()
	{
		this.audioSource.loop = false;
		this.audioSource.Stop();
	}

	private float GetDamage(Boss1_6.E_State state)
	{
		return Mathf.Round(this.ratioDamages[this.GetSkillIdOfState(state)] * this.cacheEnemy.Damage);
	}

	private int GetSkillIdOfState(Boss1_6.E_State state)
	{
		switch (state)
		{
		case Boss1_6.E_State.Dam:
			return 0;
		case Boss1_6.E_State.Dam_quet:
			return 2;
		case Boss1_6.E_State.Om:
			return 3;
		case Boss1_6.E_State.Laser1:
			return 1;
		case Boss1_6.E_State.Fire_1:
			return 4;
		case Boss1_6.E_State.BongDuoi_1:
			return 5;
		}
		return 0;
	}

	private float GetSpeed(Boss1_6.E_State state)
	{
		return Mathf.Round(this.ratioSpeed[this.GetSkillIdOfState(state)] * this.cacheEnemy.Speed);
	}

	private void ShakeCam(bool isBegin = false)
	{
		try
		{
			if (GameManager.Instance.StateManager.EState != EGamePlay.WIN)
			{
				if (isBegin)
				{
					CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
				}
				else
				{
					CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
				}
			}
		}
		catch
		{
		}
	}

	public void Hit(float damage, EWeapon eweapon)
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
		if (!this._crazy)
		{
			this._crazy = (this.boxBoss.HP < this.cacheEnemy.HP / 2f);
		}
		this.lastWeapon = eweapon;
		if (this.boxBoss.HP <= 0f && this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(0, 2, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			base.StopAllCoroutines();
			if (this.syncBossIronMechState != null)
			{
				this.syncBossIronMechState.SendRpc_Die();
			}
			this.Die();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[2].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[2].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Iron Mech has been unlocked in BossMode!");
			}
			base.StartCoroutine(this.EffectDie());
			this.meshRenderer.sortingLayerName = "forceground";
			Vector3 position = this.transform.position;
		}
		else
		{
			this.skeletonAnimation.state.SetAnimation(1, this.anims[12], false);
			GameManager.Instance.bossManager.ShowLineBloodBoss(this.boxBoss.HP, this.cacheEnemy.HP);
		}
	}

	public void Die()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		this.laserBoss.InActive();
		this.ActiveWarning(false);
		this.skeletonAnimation.skeleton.SetColor(Color.white);
		this.skeletonAnimation.state.ClearTracks();
		this.PlayAnim(Boss1_6.E_State.Die, 1f, false);
		this.ShakeCam(false);
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
	}

	private IEnumerator EffectDie()
	{
		this.ShakeCam(false);
		Vector3 pos = this.handsBoss[0].transform.position;
		Vector3 pos2 = this.handsBoss[1].transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.15f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one * 2f);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = this.boxBoss.transform.position;
		pos.y -= 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = (pos2 = this.boxBoss.transform.position);
		pos.x -= 2f;
		pos2.x += 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.15f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one * 2f);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = this.firePoint.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.5f);
		this.ShakeCam(false);
		pos = this.firePoint.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = this.firePoint.transform.position;
		pos.y -= 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.5f);
		this.ShakeCam(false);
		pos = this.boxBoss.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 3f);
		PlayerManagerStory.Instance.OnRunGameOver();
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = this.boxBoss.transform.position;
		pos.y -= 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = (pos2 = this.boxBoss.transform.position);
		pos.x -= 2f;
		pos2.x += 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.15f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one * 2f);
		yield return new WaitForSeconds(0.25f);
		this.ShakeCam(false);
		pos = this.firePoint.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.5f);
		this.ShakeCam(false);
		pos = this.firePoint.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one * 2f);
		yield return new WaitForSeconds(0.25f);
		yield break;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		string name = e.Data.Name;
		switch (name)
		{
		case "attack1":
			this.ActiveHandsBoss(0, true);
			this.ActiveHandsBoss(1, true);
			this.PlaySound(Boss1_6.TitanSound.HAND_DOWN, false);
			this.ActiveWarning(false);
			this.ShakeCam(false);
			this.ShakeCam(false);
			break;
		case "attack2":
			this.PlaySound(Boss1_6.TitanSound.LASER, false);
			if (this._state == Boss1_6.E_State.Laser1)
			{
				this.laserBoss.ActiveLaser(10, 3f, this.laserStartX, -this.laserStartX);
			}
			else
			{
				this.laserBoss.ActiveLaser(10, 3f, -this.laserStartX, this.laserStartX);
			}
			this.skeFxDa.timeScale = 0.75f;
			this.skeFxDa.state.SetAnimation(0, this.animsFxDa[0], false);
			break;
		case "attack3":
			this.ShakeCam(false);
			this.ShakeCam(false);
			this.ActiveHandsBoss(0, true);
			break;
		case "da3":
			this.skeFxDa.state.SetAnimation(0, this.animsFxDa[2], false);
			break;
		case "attack4":
			this.ShakeCam(false);
			this.ShakeCam(false);
			this.ActiveHandsBoss(0, true);
			this.ActiveHandsBoss(1, true);
			break;
		case "da4":
			this.skeFxDa.state.SetAnimation(0, this.animsFxDa[1], false);
			break;
		case "attack5":
		{
			int num2 = this.fireType;
			if (num2 != 1)
			{
				if (num2 != 0)
				{
				}
				this.Fire();
			}
			else
			{
				this.FireTargetRambo();
			}
			break;
		}
		case "attack6":
			this.FireBong();
			break;
		case "of":
			this.ActiveHandsBoss(0, false);
			this.ActiveHandsBoss(1, false);
			break;
		}
	}

	private void OnComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString();
		switch (text)
		{
		case "attack1":
			this.ActiveHandsBoss(0, false);
			this.ActiveHandsBoss(1, false);
			if (this._crazy)
			{
				this.ChanState();
			}
			else
			{
				this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
			}
			break;
		case "attack2-1":
		case "attack2-2":
			this.skeFxDa.timeScale = 1f;
			this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
			break;
		case "attack3":
			this.ActiveHandsBoss(0, false);
			if (this._crazy)
			{
				this.ChanState();
			}
			else
			{
				this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
			}
			break;
		case "attack4":
			this.ActiveHandsBoss(0, false);
			this.ActiveHandsBoss(1, false);
			if (this._crazy)
			{
				this.ChanState();
			}
			else
			{
				this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
			}
			break;
		case "attack5-1":
			this.PlayAnim(Boss1_6.E_State.Fire_2, 1f, false);
			break;
		case "attack5-2":
			if (this._fireCount < this._fireTime)
			{
				this.PlayAnim(Boss1_6.E_State.Fire_2, 1f, false);
			}
			else
			{
				this._fireCount = 0;
				this.PlayAnim(Boss1_6.E_State.Fire_3, 1f, false);
			}
			break;
		case "attack5-3":
			this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
			break;
		case "attack6-1":
			this.PlayAnim(Boss1_6.E_State.BongDuoi_2, 1f, false);
			break;
		case "attack6-2":
			this.PlayAnim(Boss1_6.E_State.BongDuoi_3, 1f, false);
			break;
		case "attack6-3":
			if (this._crazy)
			{
				this.ChanState();
			}
			else
			{
				this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
			}
			break;
		case "idle":
			if (!this.IsRemoteBoss)
			{
				if (!this._isBegin)
				{
					this.PlayAnim(Boss1_6.E_State.Idle, 1.5f, false);
				}
				else
				{
					if (this.syncBossIronMechState != null)
					{
						int randomRamboActorNumber = this.GetRandomRamboActorNumber();
						this.ChangeBossTarget(randomRamboActorNumber);
						this.syncBossIronMechState.SendRpcChanState(this._state, randomRamboActorNumber);
					}
					this.ChanState();
				}
			}
			break;
		case "die":
			if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				CoOpManager.Instance.EndOfGame(true);
			}
			break;
		}
	}

	public void ChangeBossTarget(int actorNumber)
	{
		try
		{
			this._ramboTransform = GameManager.Instance.ListRambo.Find((BaseCharactor rambo) => ((PlayerMain)rambo).actorNumber == actorNumber).transform;
		}
		catch
		{
		}
	}

	private int GetRandomRamboActorNumber()
	{
		PlayerMain playerMain;
		do
		{
			int index = UnityEngine.Random.Range(0, GameManager.Instance.ListRambo.Count);
			playerMain = (GameManager.Instance.ListRambo[index] as PlayerMain);
		}
		while (!(playerMain != null));
		return playerMain.actorNumber;
	}

	[Header("Spine Anim:___________________")]
	[Header("*******************************")]
	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[Header("Boss variable:__________________")]
	[SerializeField]
	private SkeletonAnimation skeFxDa;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip[] audioClips;

	[SerializeField]
	[Header("Dam-Laser-Dam_Quet-Om-Fire-BongDuoi")]
	private float[] ratioDamages;

	[SerializeField]
	private float[] ratioSpeed;

	[SerializeField]
	public BoxBoss1_3 boxBoss;

	[SerializeField]
	private HandBoss1_6[] handsBoss;

	[SerializeField]
	private LaserBoss1_6 laserBoss;

	[SerializeField]
	private float laserStartX;

	[SerializeField]
	private Transform firePoint;

	[SerializeField]
	private Transform fireBongPoint;

	[SerializeField]
	private List<BulletBoss1_6> listBullet;

	[SerializeField]
	private List<BongDinh> listBong;

	[SerializeField]
	private float hpBong;

	[SerializeField]
	private int numberBong;

	[SerializeField]
	private GameObject[] objWarning;

	private ObjectPooling<BulletBoss1_6> poolingBullets;

	private BulletBoss1_6 _bullet;

	private ObjectPooling<BongDinh> poolingBongs;

	private BongDinh bong;

	private Spine.Animation[] anims;

	private Spine.Animation[] animsFxDa;

	public Boss1_6.E_State _state;

	private bool _isBegin;

	public Transform _ramboTransform;

	private Transform _camTransform;

	private bool _dangerousState;

	private bool _changeState;

	private Vector3 posBegin;

	private int _fireTime;

	private int _fireCount;

	private int effDieCount;

	private Vector3 bongTargetPoint;

	private float timeScale = 1f;

	private int fireType = 1;

	private bool _crazy;

	private GameMode.Mode _mode;

	private float _coolDownPlayFxLaser;

	public bool IsRemoteBoss;

	public SyncBossIronMechState syncBossIronMechState;

	private bool isFixCamera;

	private int hit;

	public enum E_State
	{
		Dam,
		Dam_quet,
		Om,
		Laser1,
		Laser2,
		Fire_1,
		Fire_2,
		Fire_3,
		BongDuoi_1,
		BongDuoi_2,
		BongDuoi_3,
		Die,
		Hit,
		Idle
	}

	public enum TitanSound
	{
		START,
		HAND_DOWN,
		HAND_UP,
		FIRE,
		LASER,
		IDLE
	}
}
