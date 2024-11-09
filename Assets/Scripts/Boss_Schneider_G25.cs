using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_Schneider_G25 : BaseBoss
{
	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (!this._paused && GameManager.Instance.StateManager.EState == EGamePlay.PAUSE)
			{
				this.Pause(true);
			}
			return;
		}
		if (this._paused)
		{
			this.Pause(false);
		}
		this.UpdateBoss(deltaTime);
	}

	private void Pause(bool v)
	{
		this._paused = v;
		if (v)
		{
			this.skeletonAnimation.timeScale = 0f;
		}
		else
		{
			this.skeletonAnimation.timeScale = 1f;
		}
	}

	private void Flip(bool flip)
	{
		this.flip = flip;
		Vector3 localScale = this.transform.localScale;
		localScale.x = ((!flip) ? Mathf.Abs(localScale.x) : (-Mathf.Abs(localScale.x)));
		this.transform.localScale = localScale;
	}

	private void FindBone()
	{
		this._laserBone = new Bone[2];
		this._mainGunBone = new Bone[2];
		this._machineGunBone = new Bone[2];
		this._rocketBone = new Bone[6];
		for (int i = 0; i < 6; i++)
		{
			if (i < 2)
			{
				this._laserBone[i] = this.skeletonAnimation.skeleton.FindBone(this.laserBone[i]);
				this._mainGunBone[i] = this.skeletonAnimation.skeleton.FindBone(this.mainGunBone[i]);
				this._machineGunBone[i] = this.skeletonAnimation.skeleton.FindBone(this.machineGunBone[i]);
			}
			this._rocketBone[i] = this.skeletonAnimation.skeleton.FindBone(this.rocketBone[i]);
		}
	}

	public override void Init()
	{
		base.Init();
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		this.cacheEnemy = enemyCharactor.enemy[0];
		float num = GameMode.Instance.GetMode();
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
		{
			num = 5f;
		}
		this.cacheEnemy.HP *= num;
		this.cacheEnemy.Damage *= num;
		this.HP = this.cacheEnemy.HP;
		this._mode = (int)GameMode.Instance.EMode;
		base.gameObject.SetActive(true);
		GameManager.Instance.ListEnemy.Add(this);
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.InitLaser();
		this.InitBulletBi();
		this.InitMachine();
		this.FindBone();
		this._targetRunX = CameraController.Instance.transform.position.x + 3f;
		this._state = Boss_Schneider_G25.EState.Run;
		this.PlayAnim(0, true);
		this.flip = false;
		this.isInit = true;
		try
		{
			ProCamera2D.Instance.OffsetX = 0f;
		}
		catch (Exception ex)
		{
		}
	}

	private void InitLaser()
	{
		this.laser.SetTexture(0);
		this.laser.SetMainTextureTilling(new Vector2(10f, 1f));
		this.laser.speedChangeOffset = 2.5f;
		this.laser.SetColor(Color.white);
		this.effPreLaserFire.gameObject.SetActive(false);
	}

	private void InitBulletBi()
	{
		if (!object.ReferenceEquals(this.listBi, null))
		{
			this.listBi[0].gameObject.transform.parent.parent = null;
			this.poolBulletBi = new ObjectPooling<BulletBi>(this.listBi.Count, null, null);
			for (int i = 0; i < this.listBi.Count; i++)
			{
				this.poolBulletBi.Store(this.listBi[i]);
			}
		}
	}

	private void InitMachine()
	{
		this.timeReloadMachineGun -= (float)this._mode * this.timeReloadMachineGun / 4f;
	}

	private void UpdateBoss(float deltaTime)
	{
		if (!this._isBegin)
		{
			this._pos = this.transform.position;
			this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetRunX, this.cacheEnemy.Speed * deltaTime);
			this.transform.position = this._pos;
			float num = Mathf.Abs(this._pos.x - GameManager.Instance.player.transform.position.x);
			if (this._pos.x == this._targetRunX || num < 5f)
			{
				this._state = Boss_Schneider_G25.EState.Attack_MainGun;
				this._changeState = false;
				this._isBegin = true;
				this.StartState();
			}
			return;
		}
		if (this._changeState)
		{
			this.UpdateState(deltaTime);
		}
		for (int i = 0; i < this.listBi.Count; i++)
		{
			if (this.listBi[i] && this.listBi[i].isInit)
			{
				this.listBi[i].OnUpdate(deltaTime);
			}
		}
	}

	private void ChangeState()
	{
		if (!this._crazy)
		{
			this._crazy = (this.HP <= this.cacheEnemy.HP * 3f / 5f);
		}
		this._changeState = false;
		Boss_Schneider_G25.EState state = this._state;
		if (state != Boss_Schneider_G25.EState.Aim_Laser)
		{
			if (state != Boss_Schneider_G25.EState.Aim_MachineGun)
			{
				if (state != Boss_Schneider_G25.EState.Idle)
				{
					if (state != Boss_Schneider_G25.EState.Run)
					{
						this._state = ((!base.isInCamera || this._crazy) ? Boss_Schneider_G25.EState.Run : Boss_Schneider_G25.EState.Idle);
					}
					else
					{
						this.ChangeStateToAttack();
					}
				}
				else
				{
					bool flag = UnityEngine.Random.Range(0, 3) == 1;
					if (!base.isInCamera || flag)
					{
						this._state = Boss_Schneider_G25.EState.Run;
					}
					else
					{
						this.ChangeStateToAttack();
					}
				}
			}
			else
			{
				this._state = Boss_Schneider_G25.EState.Attack_MachineGun;
			}
		}
		else
		{
			this._state = Boss_Schneider_G25.EState.Attack_Laser;
		}
		this.StartState();
	}

	private void ChangeStateToAttack()
	{
		this.CachePos();
		this.GetDistanceRambo();
		SingletonGame<AudioController>.Instance.StopSound();
		if (this._distanceRambo > 3f)
		{
			switch (this._oldAttack)
			{
			case Boss_Schneider_G25.EState.Attack_Laser:
			case Boss_Schneider_G25.EState.Attack_MachineGun:
				this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? Boss_Schneider_G25.EState.Attack_Rocket : Boss_Schneider_G25.EState.Attack_MainGun);
				break;
			case Boss_Schneider_G25.EState.Attack_MainGun:
				this._state = Boss_Schneider_G25.EState.Aim_MachineGun;
				break;
			case Boss_Schneider_G25.EState.Attack_Rocket:
				this._state = Boss_Schneider_G25.EState.Aim_Laser;
				break;
			}
		}
		else
		{
			switch (this._oldAttack)
			{
			case Boss_Schneider_G25.EState.Attack_Laser:
			case Boss_Schneider_G25.EState.Attack_MachineGun:
				this._state = Boss_Schneider_G25.EState.Attack_Rocket;
				break;
			case Boss_Schneider_G25.EState.Attack_MainGun:
			case Boss_Schneider_G25.EState.Attack_Rocket:
				this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? Boss_Schneider_G25.EState.Aim_Laser : Boss_Schneider_G25.EState.Aim_MachineGun);
				break;
			}
		}
	}

	private void StartState()
	{
		bool loop = false;
		int track = 0;
		this.CachePos();
		switch (this._state)
		{
		case Boss_Schneider_G25.EState.Aim_Laser:
			track = 2;
			this.PlaySound();
			this.Flip(this._pos.x < this._ramPos.x);
			this._targetLaserPos = this._laserBone[0].GetWorldPosition(this.transform);
			this._targetLaserPos.x = this._targetLaserPos.x + ((!this.flip) ? -0.5f : 0.5f);
			this.targetLaser.position = this._targetLaserPos;
			this._targetLaserPos.y = this._targetLaserPos.y - 1f;
			this.effPreLaserFire.gameObject.SetActive(true);
			break;
		case Boss_Schneider_G25.EState.Aim_MachineGun:
			track = 2;
			this.PlaySound();
			this.Flip(this._pos.x < this._ramPos.x);
			this.targetMachineGun.position = this._machineGunBone[0].GetWorldPosition(this.transform);
			break;
		case Boss_Schneider_G25.EState.Attack_Laser:
			loop = true;
			this.PlaySound();
			this._oldAttack = this._state;
			this._laserSize = 0.35f;
			this._targetLaserPos.y = this._targetLaserPos.y + 1f;
			this._coolDownFireLaser = 1f;
			this.laser.SetSize(this._laserSize);
			this.laser.ActiveEff();
			this.effPreLaserFire.gameObject.SetActive(false);
			break;
		case Boss_Schneider_G25.EState.Attack_MachineGun:
			loop = true;
			this._oldAttack = this._state;
			this._coolDownTimeMachineGun = this.timeFireMachineGun;
			this._coolDownReloadMachineGun = 0f;
			break;
		case Boss_Schneider_G25.EState.Attack_MainGun:
			this.Flip(this._pos.x < this._ramPos.x);
			this._oldAttack = this._state;
			this._countFireMain = 0;
			break;
		case Boss_Schneider_G25.EState.Attack_Rocket:
			this.Flip(this._pos.x < this._ramPos.x);
			this._oldAttack = this._state;
			this._countFireRocket = 0;
			this._rocketId = 0;
			break;
		case Boss_Schneider_G25.EState.Run:
		{
			loop = true;
			try
			{
				SingletonGame<AudioController>.Instance.PlayLoopSound(this.audioClips[(int)this._state], 1f, 1f);
			}
			catch
			{
			}
			this.GetDistanceRambo();
			int num = UnityEngine.Random.Range(3, 5);
			this._targetRunX = ((this._distanceRambo <= 6f) ? ((this._pos.x <= this._camPos.x) ? (this._pos.x + (float)num) : (this._pos.x - (float)num)) : this._ramPos.x);
			this._targetRunX = Mathf.Min(this._camPos.x + 6f, Mathf.Max(this._camPos.x - 6f, this._targetRunX));
			this.Flip(this._pos.x < this._targetRunX);
			break;
		}
		}
		this.PlayAnim(track, loop);
		this._changeState = true;
	}

	private void UpdateState(float deltaTime)
	{
		if (this._state == Boss_Schneider_G25.EState.Die)
		{
			return;
		}
		switch (this._state)
		{
		case Boss_Schneider_G25.EState.Aim_Laser:
		{
			this.targetLaser.position = Vector3.SmoothDamp(this.targetLaser.position, this._targetLaserPos, ref this._veloMoveTargetLaser, 0.3f);
			this.effPreLaserFire.position = this._laserBone[0].GetWorldPosition(this.transform);
			float num = Vector3.Distance(this.targetLaser.position, this._targetLaserPos);
			if (num < 0.05f)
			{
				this.ChangeState();
			}
			break;
		}
		case Boss_Schneider_G25.EState.Aim_MachineGun:
		{
			this.targetMachineGun.position = Vector3.SmoothDamp(this.targetMachineGun.position, this._ramPos, ref this._veloMoveMachineGun, 0.3f);
			float num2 = Vector3.Distance(this.targetMachineGun.position, this._ramPos);
			if (num2 < 0.1f)
			{
				this.ChangeState();
			}
			break;
		}
		case Boss_Schneider_G25.EState.Attack_Laser:
		{
			this.targetLaser.position = Vector3.SmoothDamp(this.targetLaser.position, this._targetLaserPos, ref this._veloMoveTargetLaser, 0.75f);
			if (this._coolDownFireLaser > 0.21f)
			{
				this._laserSize = Mathf.MoveTowards(this._laserSize, 0.75f, deltaTime * 2f);
			}
			else
			{
				this._laserSize = Mathf.MoveTowards(this._laserSize, 0.35f, deltaTime * 2f);
			}
			this.laser.SetSize(this._laserSize);
			this._coolDownFireLaser -= deltaTime;
			Vector3 worldPosition = this._laserBone[0].GetWorldPosition(this.transform);
			Vector3 normalized = (worldPosition - this._laserBone[1].GetWorldPosition(this.transform)).normalized;
			Vector3 endPos = worldPosition + normalized * 46.4f;
			this.laser.OnShow(deltaTime, worldPosition, endPos);
			this._laserHit = Physics2D.Raycast(worldPosition, normalized, 50f, this.ramboMask);
			if (this._laserHit)
			{
				try
				{
					ISkill component = this._laserHit.transform.GetComponent<ISkill>();
					if (component == null || !component.IsInVisible())
					{
						this._laserHit.transform.GetComponent<IHealth>().AddHealthPoint(-this.GetDamage(), EWeapon.NONE);
					}
				}
				catch
				{
				}
			}
			float num3 = Vector3.Distance(this.targetLaser.position, this._targetLaserPos);
			if (num3 < 0.05f)
			{
				this.laser.DisableEff();
				this.laser.Off();
				this.SetEmptyAnim(2, 0.1f);
				this.ChangeState();
			}
			break;
		}
		case Boss_Schneider_G25.EState.Attack_MachineGun:
			if (this._coolDownTimeMachineGun > 0f)
			{
				this.CachePos();
				this._coolDownTimeMachineGun -= deltaTime;
				this.targetMachineGun.position = Vector3.SmoothDamp(this.targetMachineGun.position, this._ramPos, ref this._veloMoveMachineGun, 0.3f);
				if (this._coolDownReloadMachineGun <= 0f)
				{
					this._coolDownReloadMachineGun = this.timeReloadMachineGun;
					this.FireMachine();
				}
				else
				{
					this._coolDownReloadMachineGun -= deltaTime;
				}
			}
			else
			{
				this.SetEmptyAnim(2, 0.1f);
				this.ChangeState();
			}
			break;
		case Boss_Schneider_G25.EState.Run:
		{
			this.CachePos();
			float maxDelta = this.GetSpeed() * deltaTime;
			this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetRunX, maxDelta);
			this.transform.position = this._pos;
			if (this._pos.x == this._targetRunX)
			{
				this.ChangeState();
			}
			break;
		}
		}
	}

	private void PlayAnim(int track, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(track, this.anims[(int)this._state], loop);
	}

	private void SetEmptyAnim(int track, float time)
	{
		this.skeletonAnimation.state.SetEmptyAnimation(track, time);
	}

	private void FireMainGun()
	{
		this.PlaySound();
		BulletBi bulletBi = this.poolBulletBi.New();
		if (!bulletBi)
		{
			bulletBi = UnityEngine.Object.Instantiate<BulletBi>(this.listBi[0]);
			this.listBi.Add(bulletBi);
			bulletBi.gameObject.transform.parent = this.listBi[0].gameObject.transform.parent;
		}
		float maxVeloY = UnityEngine.Random.Range(5f, 7f);
		Vector3 worldPosition = this._mainGunBone[0].GetWorldPosition(this.transform);
		bool isMoveLeft = worldPosition.x < this._mainGunBone[1].GetWorldPosition(this.transform).x;
		bulletBi.Init(this.GetDamage(), this.GetSpeed(), 3f, maxVeloY, isMoveLeft, worldPosition, new Action<BulletBi>(this.HideBi));
	}

	private void HideBi(BulletBi obj)
	{
		this.poolBulletBi.Store(obj);
	}

	private void FireMachine()
	{
		this.PlaySound();
		Vector3 worldPosition = this._machineGunBone[0].GetWorldPosition(this.transform);
		Vector3 v = worldPosition - this._machineGunBone[1].GetWorldPosition(this.transform);
		BulletEnemy bulletEnemy = GameManager.Instance.bulletManager.CreateBulletEnemy(8, v, worldPosition, this.GetDamage(), this.GetSpeed(), 0f);
		bulletEnemy.spriteRenderer.flipX = false;
		bulletEnemy.useByBoss = true;
	}

	private void FireRocket()
	{
		this.PlaySound();
		Vector3 worldPosition = this._rocketBone[this._rocketId].GetWorldPosition(this.transform);
		Vector3 direction = worldPosition - this._rocketBone[this._rocketId + 3].GetWorldPosition(this.transform);
		GameManager.Instance.bulletManager.CreateRocketBossType1(this.GetDamage(), this.GetSpeed(), direction, worldPosition, GameManager.Instance.player.tfOrigin, 1f);
		this._rocketId++;
	}

	private float GetDamage()
	{
		float f = 50f;
		switch (this._state)
		{
		case Boss_Schneider_G25.EState.Attack_Laser:
			f = this.cacheEnemy.Damage * this.rateDamageLaser;
			break;
		case Boss_Schneider_G25.EState.Attack_MachineGun:
			f = this.cacheEnemy.Damage * this.rateDamageMachineGun;
			break;
		case Boss_Schneider_G25.EState.Attack_MainGun:
			f = this.cacheEnemy.Damage * this.rateDamageMainGun;
			break;
		case Boss_Schneider_G25.EState.Attack_Rocket:
			f = this.cacheEnemy.Damage * this.rateDamageRocket;
			break;
		}
		return Mathf.Round(f);
	}

	private float GetSpeed()
	{
		float result = 2f;
		Boss_Schneider_G25.EState state = this._state;
		if (state != Boss_Schneider_G25.EState.Attack_MachineGun)
		{
			if (state != Boss_Schneider_G25.EState.Attack_MainGun)
			{
				if (state == Boss_Schneider_G25.EState.Attack_Rocket)
				{
					result = this.cacheEnemy.Speed * this.rateSpeedRocket;
				}
			}
			else
			{
				result = this.cacheEnemy.Speed * this.rateSpeedMainGun;
			}
		}
		else
		{
			result = this.cacheEnemy.Speed * this.rateSpeedMachineGun;
		}
		return result;
	}

	private void CachePos()
	{
		this._pos = this.transform.position;
		this._camPos = CameraController.Instance.transform.position;
		this._ramPos = GameManager.Instance.player.tfOrigin.transform.position;
	}

	private void GetDistanceRambo()
	{
		this._distanceRambo = Mathf.Abs(this._pos.x - this._ramPos.x);
	}

	private void PlaySound()
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.audioClips[(int)this._state], 1f);
		}
		catch
		{
		}
	}

	public override void Hit(float damage)
	{
		if (this.HP > 0f)
		{
			GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
			this.skeletonAnimation.state.SetAnimation(1, this.anims[7], false);
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
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		this._state = Boss_Schneider_G25.EState.Die;
		this.PlayAnim(3, false);
		GameManager.Instance.hudManager.HideControl();
		if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[16].CheckUnlock(GameMode.Mode.NORMAL))
		{
			ProfileManager.bossModeProfile.bossProfiles[16].SetUnlock(GameMode.Mode.NORMAL, true);
			UIShowInforManager.Instance.ShowUnlock("Schneider G25 has been unlocked in BossMode!");
		}
		GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
		this.Die();
	}

	private void Die()
	{
		base.StartCoroutine(this.EffDie());
	}

	private IEnumerator EffDie()
	{
		yield return new WaitForSeconds(0.25f);
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.tfTarget[1].position, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.tfTarget[2].position, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.tfTarget[0].position, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		this._pos = this.transform.position;
		this._pos.x = this._pos.x - 2f;
		this._pos.y = this._pos.y + 2f;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		this._pos.x = this._pos.x + 4f;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this._pos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.tfTarget[0].position, Vector3.one);
		PlayerManagerStory.Instance.OnRunGameOver();
		yield break;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "attack2"))
			{
				if (!(name == "attack3"))
				{
					if (name == "no")
					{
						CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
						GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.tfTarget[0].position, Vector3.one);
					}
				}
				else
				{
					this.FireRocket();
				}
			}
			else
			{
				this.FireMainGun();
			}
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		if (object.ReferenceEquals(trackEntry.Animation.Name, "hit"))
		{
			return;
		}
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "attack2"))
			{
				if (!(text == "attack3"))
				{
					if (text == "idel")
					{
						this.ChangeState();
					}
				}
				else
				{
					this._countFireRocket++;
					if (this._countFireRocket > this._mode)
					{
						this.ChangeState();
						return;
					}
					this._rocketId = 0;
					this.PlayAnim(0, false);
				}
			}
			else
			{
				this._countFireMain++;
				if (this._countFireMain > this._mode)
				{
					this.ChangeState();
					return;
				}
				this.PlayAnim(0, false);
			}
		}
	}

	[HideInInspector]
	public bool flip;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private Transform targetLaser;

	[SerializeField]
	private Transform targetMachineGun;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string[] laserBone;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string[] mainGunBone;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string[] machineGunBone;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string[] rocketBone;

	[SerializeField]
	private LaserHandMade laser;

	[SerializeField]
	private LayerMask ramboMask;

	[SerializeField]
	private Transform effPreLaserFire;

	[SerializeField]
	private float timeFireMachineGun;

	[SerializeField]
	private float timeReloadMachineGun;

	[SerializeField]
	private List<BulletBi> listBi;

	[SerializeField]
	private float rateDamageMainGun;

	[SerializeField]
	private float rateDamageLaser;

	[SerializeField]
	private float rateDamageMachineGun;

	[SerializeField]
	private float rateDamageRocket;

	[SerializeField]
	private float rateSpeedMainGun;

	[SerializeField]
	private float rateSpeedMachineGun;

	[SerializeField]
	private float rateSpeedRocket;

	[SerializeField]
	private AudioClip[] audioClips;

	private bool _isBegin;

	private int _mode;

	private Spine.Animation[] anims;

	private Boss_Schneider_G25.EState _state;

	private Boss_Schneider_G25.EState _oldAttack;

	private bool _paused;

	private bool _changeState;

	private Bone[] _laserBone;

	private Bone[] _mainGunBone;

	private Bone[] _machineGunBone;

	private Bone[] _rocketBone;

	private float _distanceRambo;

	private float _targetRunX;

	private Vector3 _pos;

	private Vector3 _camPos;

	private Vector3 _ramPos;

	private int _countFireMain;

	private int _countFireRocket;

	private int _rocketId;

	private Vector3 _veloMoveTargetLaser;

	private Vector3 _targetLaserPos;

	private float _lockTargetLaserY;

	private float _coolDownFireLaser;

	private float _laserSize;

	private RaycastHit2D _laserHit;

	private float _coolDownTimeMachineGun;

	private float _coolDownReloadMachineGun;

	private Vector3 _veloMoveMachineGun;

	private ObjectPooling<BulletBi> poolBulletBi;

	private bool _crazy;

	private int hit;

	private enum EState
	{
		Aim_Laser,
		Aim_MachineGun,
		Attack_Laser,
		Attack_MachineGun,
		Attack_MainGun,
		Attack_Rocket,
		Die,
		Hit,
		Idle,
		Run
	}
}
