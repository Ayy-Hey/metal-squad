using System;
using System.Collections;
using System.Collections.Generic;
using MyDataLoader;
using Newtonsoft.Json;
using PVPManager;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_Ultron : BaseBoss
{
	private void OnDisable()
	{
		try
		{
			LeanTween.cancel(base.gameObject);
		}
		catch
		{
		}
	}

	private void Update()
	{
		if (this.isInit && GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			if (this._pause)
			{
				this.Pause(false);
			}
			float deltaTime = Time.deltaTime;
			this.OnUpdate(deltaTime);
			return;
		}
		if (this._state == Boss_Ultron.EState.Die)
		{
			return;
		}
		if (!this._pause && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW)
		{
			this.Pause(true);
		}
	}

	private void Pause(bool v)
	{
		this._pause = v;
		if (v)
		{
			this._timeScale = this.skeletonAnimation.timeScale;
			this.skeletonAnimation.timeScale = 0f;
		}
		else
		{
			this.skeletonAnimation.timeScale = this._timeScale;
		}
	}

	public override void Init()
	{
		base.Init();
		this._ramboTransform = GameManager.Instance.player.transform;
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		this.cacheEnemy = enemyCharactor.enemy[0];
		float num = GameMode.Instance.GetMode();
		Behaviour behaviour = this.bodyBox;
		bool enabled = false;
		this.headerBox.enabled = enabled;
		behaviour.enabled = enabled;
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer)
		{
			num = 5f;
			GameManager.Instance.ListEnemy.Add(this);
			Behaviour behaviour2 = this.bodyBox;
			enabled = true;
			this.headerBox.enabled = enabled;
			behaviour2.enabled = enabled;
		}
		this.cacheEnemy.HP *= num;
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
		{
			this.cacheEnemy.Damage *= 2f;
		}
		else
		{
			this.cacheEnemy.Damage *= num;
		}
		this.HP = this.cacheEnemy.HP;
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			this._mode = 1;
			this.HP *= 0.6f;
			this.cacheEnemy.HP *= 0.6f;
		}
		else
		{
			this._mode = (int)GameMode.Instance.EMode;
		}
		this._enemyCount = 0;
		this._timeReloadEnemy = 10f;
		this._coolDownTimeEnemy = 10f;
		base.gameObject.SetActive(true);
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this._anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this._listSkyStone = new List<SkyStone>();
		this._poolSkyStone = new ObjectPooling<SkyStone>(0, null, null);
		this._listPosSky = new List<Vector2>();
		this.GetBone();
		this._coutSoundStart = 0;
		this._state = Boss_Ultron.EState.Start;
		this.PlayAnim(0, false);
		this._changeState = true;
		this.isInit = true;
	}

	private void GetBone()
	{
		this._boneHead = this.skeletonAnimation.skeleton.FindBone(this.boneHead);
		this._boneHand = this.skeletonAnimation.skeleton.FindBone(this.boneHand);
		this._boneBody = this.skeletonAnimation.skeleton.FindBone(this.boneBody);
		this._boneBulletAttack1 = this.skeletonAnimation.skeleton.FindBone(this.boneBulletAttack1);
		this._boneBulletAttack5 = this.skeletonAnimation.skeleton.FindBone(this.boneBulletAttack5);
		this._boneGun0 = this.skeletonAnimation.skeleton.FindBone(this.boneGun0);
		this._boneGun1 = this.skeletonAnimation.skeleton.FindBone(this.boneGun1);
		this._listBoneRocket = new Bone[this.listBoneRocket.Length];
		for (int i = 0; i < this.listBoneRocket.Length; i++)
		{
			this._listBoneRocket[i] = this.skeletonAnimation.skeleton.FindBone(this.listBoneRocket[i]);
		}
	}

	private void OnUpdate(float deltaTime)
	{
		this.UpdateCollider();
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		this.UpdateSkyStone();
		this.CreateEnemySp(deltaTime);
	}

	private void CreateEnemySp(float deltaTime)
	{
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL || GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			return;
		}
		if (this._coolDownTimeEnemy > 0f)
		{
			this._coolDownTimeEnemy -= deltaTime;
		}
		bool flag = this._coolDownTimeEnemy <= 0f && this._enemyCount < 2;
		if (flag)
		{
			this._enemyCount++;
			this._coolDownTimeEnemy = this._timeReloadEnemy;
			SpaceShip spaceShip = EnemyManager.Instance.CreateSpaceShip();
			if (spaceShip.cacheEnemyData == null)
			{
				spaceShip.cacheEnemyData = new EnemyDataInfo();
			}
			spaceShip.cacheEnemyData.level = 8;
			spaceShip.cacheEnemyData.ismove = true;
			spaceShip.cacheEnemyData.type = 104;
			spaceShip.cacheEnemyData.pos_y = CameraController.Instance.camPos.y;
			spaceShip.cacheEnemyData.pos_x = CameraController.Instance.camPos.x + ((UnityEngine.Random.Range(0, 2) != 1) ? (-(CameraController.Instance.Size().x + 2f)) : (CameraController.Instance.Size().x + 2f));
			spaceShip.Init(delegate(SpaceShip s)
			{
				this._enemyCount--;
				EnemyManager.Instance.PoolSpaceShip.Store(s);
			}, false);
			GameManager.Instance.ListEnemy.Add(spaceShip);
		}
	}

	private void UpdateCollider()
	{
		this._pos = this.transform.position;
		Transform transform = this.tfTarget[0].transform;
		Vector2 vector = this._boneHead.GetWorldPosition(this.transform) - this._pos;
		this.headerBox.offset = vector;
		transform.localPosition = vector;
	}

	private void UpdateState(float deltaTime)
	{
		switch (this._state)
		{
		case Boss_Ultron.EState.Attack_1:
			if (this.attackBoxBullet1.isAttack)
			{
				this.attackBoxBullet1.transform.position = this._boneBulletAttack1.GetWorldPosition(this.transform);
				if (this._coolDownAttack1 > 0f)
				{
					this._coolDownAttack1 -= deltaTime;
					if (this._coolDownAttack1 <= 0f)
					{
						this.attackBoxBullet1.Deactive();
						this.DestroysSkyStone();
					}
				}
			}
			break;
		case Boss_Ultron.EState.Attack_2:
			if (this.attackBoxHand.isAttack)
			{
				this.attackBoxHand.transform.position = this._boneHand.GetWorldPosition(this.transform);
				if (this._coolDownAttack2 > 0f)
				{
					this._coolDownAttack2 -= deltaTime;
				}
				if (this._coolDownAttack2 <= 0f)
				{
					this.attackBoxHand.Deactive();
					this.meshRenderer.sortingLayerID = SortingLayer.NameToID("forceground");
					this.meshRenderer.sortingOrder = 8;
					if (!this.IsRemoteBoss)
					{
						if (this.syncBossUltronState != null)
						{
							this.syncBossUltronState.SendRpcCreateSkyStone(this._skyStonePos);
						}
						this.CreateSkyStone().Init(this.cacheEnemy.Damage, this._skyStonePos, delegate(SkyStone skt)
						{
							this._poolSkyStone.Store(skt);
						});
					}
				}
			}
			break;
		case Boss_Ultron.EState.Attack_4:
			if (this._isAttack4Fire)
			{
				this._coolDownReloadAttack4 -= deltaTime;
				if (this._coolDownReloadAttack4 <= 0f)
				{
					this._coolDownReloadAttack4 = this.coolDownReloadAttack4;
					Vector3 worldPosition = this._boneGun0.GetWorldPosition(this.transform);
					Vector3 direction = worldPosition - this._boneGun1.GetWorldPosition(this.transform);
					GameManager.Instance.bulletManager.CreateBulletLightning(this.cacheEnemy.Damage / 2f, this.cacheEnemy.Speed * 2f, worldPosition, direction);
				}
			}
			break;
		case Boss_Ultron.EState.Attack_5:
			if (this.attackBoxBullet5.isAttack)
			{
				this.attackBoxBullet5.transform.position = this._boneBulletAttack5.GetWorldPosition(this.transform);
				if (this._coolDownAttack5 > 0f)
				{
					this._coolDownAttack5 -= deltaTime;
					if (this._coolDownAttack5 <= 0f)
					{
						this.attackBoxBullet5.Deactive();
					}
				}
			}
			break;
		case Boss_Ultron.EState.Attack_Pow:
			if (this._coolDownAttackPow > 0f)
			{
				this._coolDownAttackPow -= deltaTime;
				if (this._coolDownAttackPow <= 0f)
				{
					for (int i = 0; i < this._listPosSky.Count; i++)
					{
						Vector2 pos = this._listPosSky[i];
						pos.y -= 0.7f;
						if (!this.IsRemoteBoss)
						{
							if (this.syncBossUltronState != null)
							{
								this.syncBossUltronState.SendRpcCreateSkyStone(pos);
							}
							this.CreateSkyStone().Init(this.cacheEnemy.Damage, pos, delegate(SkyStone skt)
							{
								this._poolSkyStone.Store(skt);
							});
						}
					}
				}
			}
			break;
		}
	}

	private void StartState()
	{
		this._changeState = true;
		int track = 0;
		bool loop = false;
		switch (this._state)
		{
		case Boss_Ultron.EState.Attack_1:
			this._coolDownAttack1 = 0.2f;
			break;
		case Boss_Ultron.EState.Attack_2:
			this._attacId = 0;
			this._coolDownAttack2 = 1f;
			break;
		case Boss_Ultron.EState.Attack_4:
			this._attacId = 1;
			this._coolDownReloadAttack4 = 0f;
			break;
		case Boss_Ultron.EState.Attack_5:
			this._attacId = 2;
			this._coolDownAttack5 = 0.5f;
			break;
		case Boss_Ultron.EState.Attack_3_1:
			this._attacId = 3;
			break;
		case Boss_Ultron.EState.Attack_3_2:
			loop = true;
			this._rocketId = 0;
			break;
		case Boss_Ultron.EState.Attack_Pow:
			this._attacId = 4;
			this._useBomb = true;
			if (GameMode.Instance.EMode != GameMode.Mode.TUTORIAL && this._upPow)
			{
				this.skeletonAnimation.timeScale = 1.5f;
			}
			this.DestroysSkyStone();
			break;
		case Boss_Ultron.EState.Weakly_2:
			loop = true;
			break;
		}
		this.PlayAnim(track, loop);
		this.PlaySound(this._state);
	}

	public void ChangeState()
	{
		if (!this._upPow && this.HP < this.cacheEnemy.HP * 0.6f)
		{
			this._upPow = true;
			this.cacheEnemy.Damage *= 1.25f;
			this.cacheEnemy.Speed *= 1.25f;
			this.coolDownReloadAttack4 /= 1.5f;
			this._state = Boss_Ultron.EState.Weakly_1;
			this._changeState = false;
			return;
		}
		switch (this._state)
		{
		case Boss_Ultron.EState.Attack_1:
		case Boss_Ultron.EState.Attack_4:
		case Boss_Ultron.EState.Attack_5:
		case Boss_Ultron.EState.Attack_3_3:
		{
			int num = UnityEngine.Random.Range(0, 6);
			if (!((!this._upPow) ? (num < 3) : (num < 5)))
			{
				this._state = Boss_Ultron.EState.Idle;
			}
			else
			{
				switch (this._attacId)
				{
				case 0:
					this._state = Boss_Ultron.EState.Attack_4;
					break;
				case 1:
					this._state = Boss_Ultron.EState.Attack_5;
					break;
				case 2:
					this._state = Boss_Ultron.EState.Attack_3_1;
					break;
				case 3:
					this._state = Boss_Ultron.EState.Attack_Pow;
					break;
				case 4:
					this._state = Boss_Ultron.EState.Attack_2;
					break;
				}
			}
			break;
		}
		case Boss_Ultron.EState.Attack_2:
			this._state = Boss_Ultron.EState.Attack_1;
			break;
		case Boss_Ultron.EState.Attack_3_1:
			this._state = Boss_Ultron.EState.Attack_3_2;
			break;
		case Boss_Ultron.EState.Attack_3_2:
			this._state = Boss_Ultron.EState.Attack_3_3;
			break;
		case Boss_Ultron.EState.Attack_Pow:
		case Boss_Ultron.EState.Idle:
			switch (this._attacId)
			{
			case 0:
				this._state = Boss_Ultron.EState.Attack_4;
				break;
			case 1:
				this._state = Boss_Ultron.EState.Attack_5;
				break;
			case 2:
				this._state = Boss_Ultron.EState.Attack_3_1;
				break;
			case 3:
				this._state = Boss_Ultron.EState.Attack_Pow;
				break;
			case 4:
				this._state = Boss_Ultron.EState.Attack_2;
				break;
			}
			break;
		case Boss_Ultron.EState.Start:
		{
			this._state = Boss_Ultron.EState.Attack_4;
			Behaviour behaviour = this.bodyBox;
			bool enabled = true;
			this.headerBox.enabled = enabled;
			behaviour.enabled = enabled;
			GameManager.Instance.ListEnemy.Add(this);
			break;
		}
		case Boss_Ultron.EState.Weakly_1:
			this._state = Boss_Ultron.EState.Weakly_2;
			break;
		case Boss_Ultron.EState.Weakly_2:
			this._state = Boss_Ultron.EState.Weakly_3;
			break;
		case Boss_Ultron.EState.Weakly_3:
			this._state = Boss_Ultron.EState.Attack_Pow;
			break;
		}
		this._changeState = false;
	}

	private void PlayAnim(int track, bool loop)
	{
		this.skeletonAnimation.state.SetAnimation(track, this._anims[(int)this._state], loop);
	}

	private void PlaySound(Boss_Ultron.EState state)
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.clips[(int)this._state], 1f);
		}
		catch
		{
		}
	}

	private void CreateRocket()
	{
		float timeDelayTarget = UnityEngine.Random.Range(0.5f, 1f);
		Vector3 worldPosition = this._listBoneRocket[this._rocketId].GetWorldPosition(this.transform);
		Vector3 direction = worldPosition - this._listBoneRocket[this._rocketId + 2].GetWorldPosition(this.transform);
		try
		{
			GameManager.Instance.bulletManager.CreateRocketBossType1(this.cacheEnemy.Damage, this.cacheEnemy.Speed, direction, worldPosition, this._ramboTransform, timeDelayTarget).transform.localScale = Vector3.one * 1.5f;
		}
		catch
		{
		}
		this._rocketId = ((this._rocketId != 0) ? 0 : 1);
	}

	public SkyStone CreateSkyStone()
	{
		SkyStone skyStone = this._poolSkyStone.New();
		if (!skyStone)
		{
			skyStone = UnityEngine.Object.Instantiate<SkyStone>(this.skyStone);
			this._listSkyStone.Add(skyStone);
			skyStone.gameObject.transform.parent = this.tfParenSkyStone;
		}
		return skyStone;
	}

	public void CreateRemoteSkyStone(Vector2 pos)
	{
		this.CreateSkyStone().Init(this.cacheEnemy.Damage, pos, delegate(SkyStone skt)
		{
			this._poolSkyStone.Store(skt);
		});
	}

	private void UpdateSkyStone()
	{
		for (int i = 0; i < this._listSkyStone.Count; i++)
		{
			if (this._listSkyStone[i] && this._listSkyStone[i].isInit)
			{
				this._listSkyStone[i].OnUpdate();
			}
		}
	}

	private void DestroysSkyStone()
	{
		for (int i = 0; i < this._listSkyStone.Count; i++)
		{
			if (this._listSkyStone[i] && this._listSkyStone[i].isInit)
			{
				this._listSkyStone[i].End();
			}
		}
	}

	public override void Hit(float damage)
	{
		if (this.HP >= 0f)
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
			{
				this.hit++;
				if (this.hit >= 10)
				{
					this.hit = 0;
					GameManager.Instance.giftManager.CreateItemWeapon(CameraController.Instance.Position);
				}
			}
			this.skeletonAnimation.state.SetAnimation(1, this._anims[11], false);
			GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
			return;
		}
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			if (this.OnDead != null)
			{
				this.OnDead();
			}
			this.PlayAnim(0, false);
			GameManager.Instance.ListEnemy.Remove(this);
			GameManager.Instance.hudManager.HideControl();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			base.StartCoroutine(this.Die());
			return;
		}
		PlayerManagerStory.Instance.OnRunGameOver();
		if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[17].CheckUnlock(GameMode.Mode.NORMAL))
		{
			ProfileManager.bossModeProfile.bossProfiles[17].SetUnlock(GameMode.Mode.NORMAL, true);
			UIShowInforManager.Instance.ShowUnlock("Ultron has been unlocked in BossMode!");
		}
		this.PlayAnim(0, false);
		Behaviour behaviour = this.headerBox;
		bool enabled = false;
		this.bodyBox.enabled = enabled;
		behaviour.enabled = enabled;
		GameManager.Instance.ListEnemy.Remove(this);
		GameManager.Instance.hudManager.HideControl();
		GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
		if (this.syncBossUltronState != null)
		{
			this.syncBossUltronState.SendRpc_Die();
		}
		base.StartCoroutine(this.Die());
	}

	public IEnumerator Die()
	{
		if (this._state != Boss_Ultron.EState.Die)
		{
			this._state = Boss_Ultron.EState.Die;
			GameManager.Instance.fxManager.ShowFxNo01(this._boneBody.GetWorldPosition(this.transform), 1f);
			yield return new WaitForSeconds(0.25f);
			GameManager.Instance.fxManager.ShowFxNo01(this._boneHead.GetWorldPosition(this.transform), 1f);
			yield return new WaitForSeconds(0.25f);
			GameManager.Instance.fxManager.ShowFxNo01(this._boneHand.GetWorldPosition(this.transform), 1f);
			yield return new WaitForSeconds(0.25f);
			GameManager.Instance.fxManager.ShowFxNo01(this._listBoneRocket[2].GetWorldPosition(this.transform), 1f);
			yield return new WaitForSeconds(0.25f);
			GameManager.Instance.fxManager.ShowFxNo01(this._listBoneRocket[3].GetWorldPosition(this.transform), 1f);
			yield return new WaitForSeconds(0.25f);
			GameManager.Instance.fxManager.ShowFxNo01(this._boneBody.GetWorldPosition(this.transform), 1f);
		}
		yield break;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		switch (this._state)
		{
		case Boss_Ultron.EState.Attack_1:
			if (!this.attackBoxBullet1.isAttack)
			{
				this.attackBoxBullet1.Active(this.cacheEnemy.Damage * 2f, true, null);
				this.attackBoxBullet1.transform.position = this._boneBulletAttack1.GetWorldPosition(this.transform);
			}
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			break;
		case Boss_Ultron.EState.Attack_2:
			if (!this.attackBoxHand.isAttack)
			{
				this.attackBoxHand.Active(this.cacheEnemy.Damage, true, null);
				this.attackBoxHand.transform.position = this._boneHand.GetWorldPosition(this.transform);
				this.meshRenderer.sortingLayerID = SortingLayer.NameToID("Gameplay");
				this.meshRenderer.sortingOrder = 50;
				this._skyStonePos = this.attackBoxHand.transform.position;
				this._skyStonePos.y = this._skyStonePos.y - 1.5f;
				this._skyStonePos.x = UnityEngine.Random.Range(this._skyStonePos.x - 6f, this._skyStonePos.x - 2f);
				RaycastHit2D raycastHit2D = Physics2D.Raycast(this._skyStonePos + Vector2.up, Vector2.down, 5f, this.maskGround);
				if (raycastHit2D.collider)
				{
					GameManager.Instance.fxManager.ShowFxWarning(1f, raycastHit2D.point, Fx_Warning.CameraLock.None, 0, null);
				}
			}
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			break;
		case Boss_Ultron.EState.Attack_4:
			if (e.Data.Name.Contains("start"))
			{
				this._isAttack4Fire = true;
			}
			if (e.Data.Name.Contains("end"))
			{
				this._isAttack4Fire = false;
			}
			CameraController.Instance.Shake(CameraController.ShakeType.ShakeEnemy);
			break;
		case Boss_Ultron.EState.Attack_5:
			this.attackBoxBullet5.Active(this.cacheEnemy.Damage * 1.5f, true, null);
			this.attackBoxBullet5.transform.position = this._boneBulletAttack5.GetWorldPosition(this.transform);
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			break;
		case Boss_Ultron.EState.Attack_3_1:
			CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
			break;
		case Boss_Ultron.EState.Attack_3_2:
			if (e.Data.Name.Contains("attack"))
			{
				this.CreateRocket();
			}
			if (e.Data.Name.Contains("dung"))
			{
				CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
			}
			break;
		case Boss_Ultron.EState.Attack_Pow:
			if (this._useBomb)
			{
				this._useBomb = false;
				this._coolDownAttackPow = 1f;
				int num = this._mode + 2;
				float max = this._pos.x - 4f;
				float min = CameraController.Instance.camPos.x - CameraController.Instance.Size().x + 0.5f;
				this._listPosSky.Clear();
				for (int i = 0; i < num; i++)
				{
					Vector3 camPos = CameraController.Instance.camPos;
					camPos.y = CameraController.Instance.camPos.y + CameraController.Instance.Size().y;
					camPos.x = UnityEngine.Random.Range(min, max);
					RaycastHit2D raycastHit2D2 = Physics2D.Raycast(camPos, Vector2.down, 15f, this.maskGround);
					if (raycastHit2D2.collider)
					{
						camPos.y = raycastHit2D2.point.y;
						UnityEngine.Debug.Log(raycastHit2D2.point);
						GameManager.Instance.fxManager.ShowFxWarning(1f, raycastHit2D2.point, Fx_Warning.CameraLock.None, 0, null);
						this._listPosSky.Add(camPos);
					}
				}
			}
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			break;
		case Boss_Ultron.EState.Start:
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			this._coutSoundStart++;
			if (this._coutSoundStart < 3)
			{
				this.PlaySound(this._state);
			}
			break;
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string name = trackEntry.Animation.Name;
		switch (name)
		{
		case "attack-1":
		case "attack-2":
		case "attack-4":
		case "attack-5":
		case "attack-3-1":
		case "attack-3-3":
		case "attack_pow":
		case "idle":
		case "start":
		case "weakly-1":
		case "weakly-3":
		case "weakly-2":
			if (!this.IsRemoteBoss)
			{
				int randomRamboActorNumber = this.GetRandomRamboActorNumber();
				this.ChangeBossTarget(randomRamboActorNumber);
				this.ChangeState();
				if (this.syncBossUltronState != null)
				{
					this.syncBossUltronState.SendRpcChanState(this._state, randomRamboActorNumber);
				}
			}
			break;
		case "attack-3-2":
			this._countAttackRocket++;
			if (this._countAttackRocket > this._mode + 1)
			{
				this._countAttackRocket = 0;
				if (!this.IsRemoteBoss)
				{
					int randomRamboActorNumber2 = this.GetRandomRamboActorNumber();
					this.ChangeBossTarget(randomRamboActorNumber2);
					this.ChangeState();
					if (this.syncBossUltronState != null)
					{
						this.syncBossUltronState.SendRpcChanState(this._state, randomRamboActorNumber2);
					}
				}
			}
			else
			{
				this.PlaySound(this._state);
			}
			break;
		case "die":
			LeanTween.moveLocalY(base.gameObject, CameraController.Instance.transform.position.y - 20f, 3f);
			this.skeletonAnimation.state.SetAnimation(0, this._anims[10], true);
			this.PlaySound(Boss_Ultron.EState.Die_1);
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

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneHead;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneBody;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneBulletAttack1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneBulletAttack5;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneGun0;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneGun1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneHand;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string[] listBoneRocket;

	[SerializeField]
	public Collider2D headerBox;

	[SerializeField]
	public Collider2D bodyBox;

	[SerializeField]
	private AttackBox attackBoxBullet1;

	[SerializeField]
	private AttackBox attackBoxBullet5;

	[SerializeField]
	private AttackBox attackBoxHand;

	[SerializeField]
	private SkyStone skyStone;

	[SerializeField]
	private Transform tfParenSkyStone;

	[SerializeField]
	private LayerMask maskGround;

	[SerializeField]
	private float coolDownReloadAttack4;

	[SerializeField]
	private int totalWeakly = 2;

	[SerializeField]
	private AudioClip[] clips;

	public Boss_Ultron.EState _state;

	public bool _changeState;

	private int _mode;

	private Spine.Animation[] _anims;

	private Bone _boneHead;

	private Bone _boneBody;

	private Bone _boneBulletAttack1;

	private Bone _boneBulletAttack5;

	private Bone _boneGun0;

	private Bone _boneGun1;

	private Bone _boneHand;

	private Bone[] _listBoneRocket;

	private Vector3 _pos;

	private Vector2 _skyStonePos;

	private List<SkyStone> _listSkyStone;

	private ObjectPooling<SkyStone> _poolSkyStone;

	private SkyStone _skyStone;

	private bool _upPow;

	private int _attacId;

	private bool _pause;

	private float _timeScale;

	private int _countAttackRocket;

	private bool _isAttack4Fire;

	private int _rocketId;

	private float _coolDownReloadAttack4;

	private float _coolDownAttack1;

	private float _coolDownAttack5;

	private int _countWeakly;

	private float _coolDownAttack2;

	private int _coutSoundStart;

	private bool _useBomb;

	private int _enemyCount;

	private float _timeReloadEnemy;

	private float _coolDownTimeEnemy;

	private float _coolDownAttackPow;

	private List<Vector2> _listPosSky;

	public Transform _ramboTransform;

	public bool IsRemoteBoss;

	public SyncBossUltronState syncBossUltronState;

	public Action OnDead;

	private int hit;

	public enum EState
	{
		Attack_1,
		Attack_2,
		Attack_3,
		Attack_4,
		Attack_5,
		Attack_3_1,
		Attack_3_2,
		Attack_3_3,
		Attack_Pow,
		Die,
		Die_1,
		Hit,
		Idle,
		Start,
		Weakly_1,
		Weakly_2,
		Weakly_3
	}
}
