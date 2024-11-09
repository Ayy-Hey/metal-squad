using System;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Player;
using Prime31;
using PVPManager;
using Spine.Unity;
using UnityEngine;

public class PlayerMain : BaseRambo, IHealth
{
	public bool IsInit
	{
		get
		{
			return this.isInit;
		}
		set
		{
			this.isInit = value;
			if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer)
			{
				PvP_LocalPlayer.Instance.IsInit = this.isInit;
			}
		}
	}

	public void OnInit(int IDChar, int actorNumber = 0, int NPC_Level = 0, int Gun1_Level = 0)
	{
		base.gameObject.SetActive(true);
		GameManager.Instance.ListRambo.Add(this);
		this.IDChar = IDChar;
		this.actorNumber = actorNumber;
		this._controller.onControllerCollidedEvent += this.onControllerCollider;
		this._controller.onTriggerEnterEvent += this.onTriggerEnterEvent;
		this._controller.onTriggerExitEvent += this.onTriggerExitEvent;
		if (!this.isNPC)
		{
			this.transform.position = new Vector3(0f, 999f);
		}
		this._PlayerSpine = new PlayerSpine(this, false);
		this._PlayerInput = new PlayerInput(this, this);
		this._PlayerData = new PlayerData(this, ProfileManager.settingProfile.IDChar, NPC_Level);
		this._PlayerBooster = new PlayerBooster();
		this._PlayerJetpack = new PlayerJetpack();
		this._PlayerSkill = new PlayerSkill(this, this);
		this.OffsetBoxCurrent = this.boxMain.offset;
		this.SizeBoxCurrent = this.boxMain.size;
		if (this.effHit == null)
		{
			this.effHit = (UnityEngine.Object.Instantiate(Resources.Load("Popup/Blood", typeof(PlayerHit)), GameManager.Instance.mCanvas.transform) as PlayerHit);
		}
		if (this.isNPC)
		{
			this.LoadOnlineProfile(this.NPC_IDGun1, this.NPC_IDKnife, this.NPC_IDGrenades, true, Gun1_Level, 0);
		}
		else
		{
			this.LoadProfile();
		}
		this.IsInit = true;
	}

	public void OnInitOnline(int _IDChar, int _IDGUN1, int _IDKnife, int _IDGrenades, int _actorNumber = 0, int rankUpgrade = 0)
	{
		this.IsRemotePlayer = true;
		base.gameObject.SetActive(true);
		this.isBatTu = true;
		GameManager.Instance.ListRambo.Add(this);
		this.IDChar = _IDChar;
		this.actorNumber = _actorNumber;
		this._controller.onControllerCollidedEvent += this.onControllerCollider;
		this._controller.onTriggerEnterEvent += this.onTriggerEnterEvent;
		this._controller.onTriggerExitEvent += this.onTriggerExitEvent;
		this._PlayerSpine = new PlayerSpine(this, true);
		this._PlayerInput = new PlayerInput(this, this);
		this._PlayerData = new PlayerData(this, this.IDChar, 0);
		this._PlayerBooster = new PlayerBooster();
		this._PlayerJetpack = new PlayerJetpack();
		this._PlayerSkill = new PlayerSkill(this, this);
		this.OffsetBoxCurrent = this.boxMain.offset;
		this.SizeBoxCurrent = this.boxMain.size;
		if (this.effHit == null)
		{
			this.effHit = (UnityEngine.Object.Instantiate(Resources.Load("Popup/Blood", typeof(PlayerHit)), GameManager.Instance.mCanvas.transform) as PlayerHit);
		}
		this.LoadOnlineProfile(_IDGUN1, _IDKnife, _IDGrenades, false, 0, rankUpgrade);
		this.IsInit = true;
	}

	public void OnInitNPC(int _IDChar, int _IDGUN1, int _IDKnife, int _IDGrenades, int NPC_Level, int Gun1_Level)
	{
		this.isNPC = true;
		this.NPC_IDGun1 = _IDGUN1;
		this.NPC_IDKnife = _IDKnife;
		this.NPC_IDGrenades = _IDGrenades;
		this.isFirtTouchGround = true;
		this.OnInit(_IDChar, 0, NPC_Level, Gun1_Level);
		GameManager.Instance.NPC = base.gameObject;
		this.npcFakeControl = base.gameObject.GetComponent<NPC_FakeControl>();
		ProCamera2D.Instance.AddCameraTarget(this.transform, 1f, 1f, 0f, default(Vector2));
		GameManager.Instance.isRescue = true;
	}

	private void Update()
	{
		if (this.IsRemotePlayer)
		{
			return;
		}
		if (!this.isFirtTouchGround && this.IsInit && this._PlayerSpine.targetAnimation == this.GunCurrent.GetAnimJump(1) && PlayerManagerStory.Instance.typeBegin == PlayerManagerStory.TypeBegin.JUMP)
		{
			this._velocity.y = this._velocity.y + this._PlayerData.gravity * Time.deltaTime * 0.7f;
			this._controller.move(this._velocity * Time.deltaTime);
			this._velocity = this._controller.velocity;
		}
		if (!this.IsInit || (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && GameManager.Instance.StateManager.EState != EGamePlay.RUNNING && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW) || this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		this.objKnifeSword.OnUpdate(deltaTime);
		this.objKnife.OnUpdate(deltaTime);
		if (this._PlayerSpine != null)
		{
			this._PlayerSpine.OnUpdate(deltaTime);
		}
		if (this._PlayerInput != null && !this.isNPC)
		{
			this._PlayerInput.OnUpdate(deltaTime);
		}
		if (this.isNPC)
		{
			this.npcFakeControl.OnUpdate();
		}
		if (this._PlayerSkill != null)
		{
			this._PlayerSkill.OnUpdate(deltaTime);
		}
		this._PlayerJetpack.OnUpdate(deltaTime);
		this.GunCurrent.WeaponCurrent.OnUpdate(deltaTime);
		if (!this.isAutoRun)
		{
			this.OnMovement(this.EMovement);
		}
		this.OnUpdateTarget(deltaTime);
		if (Time.timeSinceLevelLoad - this.time_protected > 5f && this.IsProteced)
		{
			this.IsProteced = false;
		}
	}

	public void OnMovement(BaseCharactor.EMovementBasic EMovement)
	{
		if (this._controller.isGrounded)
		{
			this._velocity.y = 0f;
		}
		bool flag = (!this.hasTarget && !this.isLockDirection) || !this._PlayerInput.IsShooting;
		bool flag2 = false;
		if (EMovement == BaseCharactor.EMovementBasic.Sit)
		{
			Vector2 offsetBoxCurrent = this.OffsetBoxCurrent;
			Vector2 sizeBoxCurrent = this.SizeBoxCurrent;
			offsetBoxCurrent.y = this.OffsetBoxCurrent.y * 0.75f;
			sizeBoxCurrent.y = this.SizeBoxCurrent.y * 0.75f;
			this.boxMain.offset = offsetBoxCurrent;
			this.boxMain.size = sizeBoxCurrent;
		}
		else
		{
			this.boxMain.offset = this.OffsetBoxCurrent;
			this.boxMain.size = this.SizeBoxCurrent;
		}
		switch (EMovement)
		{
		case BaseCharactor.EMovementBasic.Left:
			this.normalizedHorizontalSpeed = -1f;
			if (flag && !this._PlayerSpine.FlipX)
			{
				this._PlayerSpine.FlipX = true;
			}
			if (this._controller.isGrounded && !this._PlayerInput.isPressJump)
			{
				flag2 = (!flag && !this._PlayerSpine.FlipX);
				this._PlayerSpine.targetAnimation = this.GunCurrent.GetAnimRun(flag2);
			}
			goto IL_2D5;
		case BaseCharactor.EMovementBasic.Right:
			this.normalizedHorizontalSpeed = 1f;
			if (flag && this._PlayerSpine.FlipX)
			{
				this._PlayerSpine.FlipX = false;
			}
			if (this._controller.isGrounded && !this._PlayerInput.isPressJump)
			{
				flag2 = (!flag && this._PlayerSpine.FlipX);
				this._PlayerSpine.targetAnimation = this.GunCurrent.GetAnimRun(flag2);
			}
			goto IL_2D5;
		case BaseCharactor.EMovementBasic.Sit:
			this.normalizedHorizontalSpeed = 0f;
			if (this._controller.isGrounded && !this._PlayerInput.isPressJump)
			{
				flag2 = true;
				this._velocity.y = this._velocity.y * 3f;
				this._controller.ignoreOneWayPlatformsThisFrame = true;
				this._PlayerSpine.targetAnimation = this.GunCurrent.GetAnimIdle();
			}
			goto IL_2D5;
		case BaseCharactor.EMovementBasic.FREEZE:
			this._PlayerInput.OnRemoveInput(false);
			this.normalizedHorizontalSpeed = 0f;
			this._PlayerSpine.targetAnimation = this.GunCurrent.GetAnimFreeze();
			goto IL_2D5;
		}
		flag2 = false;
		if (this._controller.isGrounded && !this._PlayerInput.isPressJump)
		{
			this.normalizedHorizontalSpeed = 0f;
			this._PlayerSpine.targetAnimation = this.GunCurrent.GetAnimIdle();
		}
		IL_2D5:
		bool flag3 = false;
		if (this._controller.isGrounded && this._PlayerInput.isPressJump)
		{
			this._PlayerSpine.RemoveAim();
			if (!GameManager.Instance.isJetpackMode)
			{
				flag2 = false;
				this._velocity.y = Mathf.Sqrt(2f * this._PlayerData.jumpHeight * -this._PlayerData.gravity);
			}
			this._PlayerSpine.targetAnimation = this.GunCurrent.GetAnimJump(1);
			flag3 = true;
		}
		if (this._PlayerSpine.previousTargetAnimation != this._PlayerSpine.targetAnimation)
		{
			if (!GameManager.Instance.isJetpackMode)
			{
				if (flag3)
				{
					this.skeletonAnimation.state.ClearTrack(0);
					this.skeletonAnimation.AnimationState.SetAnimation(0, this.GunCurrent.GetAnimJump(0), false);
					this.skeletonAnimation.AnimationState.AddAnimation(0, this.GunCurrent.GetAnimJump(1), true, 0f);
				}
				else if (this._PlayerSpine.previousTargetAnimation == this.GunCurrent.GetAnimJump(0) || this._PlayerSpine.previousTargetAnimation == this.GunCurrent.GetAnimJump(1))
				{
					GameManager.Instance.fxManager.OnShowFxJump(this.transform.position);
					this.skeletonAnimation.state.ClearTrack(0);
					this.skeletonAnimation.AnimationState.SetAnimation(0, this.GunCurrent.GetAnimJump(2), false);
					this.skeletonAnimation.AnimationState.AddAnimation(0, this._PlayerSpine.targetAnimation, true, 0f);
				}
				else
				{
					this.skeletonAnimation.AnimationState.SetAnimation(0, this._PlayerSpine.targetAnimation, true);
				}
			}
			else
			{
				this.skeletonAnimation.AnimationState.SetAnimation(0, this._PlayerSpine.targetAnimation, true);
			}
		}
		this._PlayerSpine.previousTargetAnimation = this._PlayerSpine.targetAnimation;
		float groundDamping = this._PlayerData.groundDamping;
		float num = ((!flag2) ? this._PlayerData.runSpeedNormal : this._PlayerData.runSpeedSlow) * this._PlayerBooster.speedBooster;
		this._velocity.x = Mathf.Lerp(this._velocity.x, this.normalizedHorizontalSpeed * num, Time.deltaTime * groundDamping) * base.GetSpeedScale();
		if (GameManager.Instance.isJetpackMode && this._PlayerInput.isPressJump && this._PlayerJetpack.isReadyFly)
		{
			this._velocity.y = Mathf.Lerp(this._velocity.y, 1f * num, Time.deltaTime * groundDamping) * base.GetSpeedScale() * this._PlayerBooster.speedBooster;
		}
		else
		{
			this._velocity.y = this._velocity.y + this._PlayerData.gravity * Time.deltaTime;
		}
		this._controller.move(this._velocity * Time.deltaTime);
		this._velocity = this._controller.velocity;
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			Vector3 position = this.transform.position;
			float min = CameraController.Instance.LeftCamera();
			position.x = Mathf.Clamp(position.x, min, CameraController.Instance.RightCamera());
			position.y = Mathf.Clamp(position.y, CameraController.Instance.BottomCamera() - 4f, CameraController.Instance.TopCamera());
			this.transform.position = position;
		}
	}

	public Vector2 GetTargetFromDirection(Vector2 direction)
	{
		Vector2 result = Vector2.zero;
		Vector2 originGun = this.GunCurrent.WeaponCurrent.GetOriginGun();
		direction.Normalize();
		RaycastHit2D raycastHit2D = Physics2D.Raycast(originGun, direction, 1000f, this.layerTarget);
		if (raycastHit2D.collider != null)
		{
			result = raycastHit2D.point;
		}
		return result;
	}

	public void OnCreateGrenade()
	{
		Vector3 vector = Vector3.zero;
		if (!this._controller.isGrounded)
		{
			vector = this.tfOrigin.position;
		}
		else
		{
			vector = this.GunCurrent.GetHandPos();
		}
		if (!this.IsRemotePlayer && this.syncRamboState != null)
		{
			switch (this._PlayerData.IDGrenades)
			{
			case 0:
				this.syncRamboState.SendRpc_ThrowGrendeBasic(vector, this._PlayerSpine.FlipX);
				break;
			case 1:
				this.syncRamboState.SendRpc_ThrowGrendeIce(vector, this._PlayerSpine.FlipX);
				break;
			case 2:
				this.syncRamboState.SendRpc_ThrowGrendeFire(vector, this._PlayerSpine.FlipX);
				break;
			case 3:
				this.syncRamboState.SendRpc_ThrowGrendeSmoke(vector, this._PlayerSpine.FlipX);
				break;
			}
		}
		switch (this._PlayerData.IDGrenades)
		{
		case 0:
			GameManager.Instance.bombManager.ThrowGrendeBasic(this, vector, this._PlayerSpine.FlipX, true);
			break;
		case 1:
			GameManager.Instance.bombManager.ThrowGrendeIce(this, vector, this._PlayerSpine.FlipX, true);
			break;
		case 2:
			GameManager.Instance.bombManager.ThrowGrendeFire(this, vector, this._PlayerSpine.FlipX, true);
			break;
		case 3:
			GameManager.Instance.bombManager.ThrowGrendeSmoke(this, vector, this._PlayerSpine.FlipX, true);
			break;
		}
		ProfileManager.grenadesProfile[this._PlayerData.IDGrenades].TotalBomb--;
		PopupManager.Instance.SaveReward((Item)PopupManager.Instance.ConvertToIndexItem(ItemConvert.Bomb, this._PlayerData.IDGrenades), -1, "Use", null);
		ControlManager.Instance.BombValueChange();
	}

	public void OnUpdateTarget(float deltaTime)
	{
		if (!this.IsInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this._PlayerInput.IsShooting)
		{
			return;
		}
		if (this._PlayerData != null)
		{
			this._PlayerData.IfNotEnoughBullet();
		}
		Vector2 vector = this.GetTarget();
		float num = Vector2.Distance(vector, this.tfOrigin.position);
		if (GameManager.Instance.ListEnemy.Count <= 0 || num < 0.5f || vector.x >= 3.40282347E+38f || vector.y >= 3.40282347E+38f || this.TargetCache == null || this.TargetCache.GetState() == ECharactor.DIE || !this.TargetCache.isInCamera || !this.TargetCache.gameObject.activeSelf)
		{
			this.hasTarget = false;
			vector = this.GetTargetFromDirection(this._PlayerSpine.FlipX ? Vector2.left : Vector2.right);
		}
		else
		{
			this.hasTarget = true;
			if (!this.isLockDirection)
			{
				this._PlayerSpine.FlipX = (vector.x < this.transform.position.x);
			}
			GameManager.Instance.tfAutoTarget.position = vector;
		}
		if (!this.isLockDirection)
		{
			this.tfBone.position = Vector3.SmoothDamp(this.tfBone.position, vector, ref this.vt3, 0.1f);
		}
		this.DirectionJoystick = vector - (Vector2)this.skeletonAnimation.transform.position;
	}

	public bool CheckKnife()
	{
		if (!this._controller.isGrounded)
		{
			return false;
		}
		RaycastHit2D[] array = Physics2D.RaycastAll(this.tfOrigin.position, (!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left, 0.499f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].collider)
			{
				if (array[i].collider.gameObject.activeSelf)
				{
					if (array[i].collider.gameObject.CompareTag("Enemy"))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void LoadProfile()
	{
		this._PlayerData.LoadProfile();
		GameManager.Instance.bulletManager.LoadAndCreateBullet(this);
		this._PlayerBooster.LoadProfile();
		this._PlayerSpine.OnInit();
	}

	public void LoadOnlineProfile(int IDGUN1, int IDKnife, int IDGrenades, bool useOptionLevel = false, int Gun1_Level = 0, int rankUpgrade = 0)
	{
		this._PlayerData.LoadOnlineProfile(IDGUN1, IDKnife, IDGrenades, useOptionLevel, Gun1_Level, rankUpgrade);
		this._PlayerSpine.OnInit();
	}

	public float PercentHP()
	{
		return base.HPCurrent / this._PlayerData.ramboProfile.HP;
	}

	private void CollisionWithGulft()
	{
		if (this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		this.AddHealthPoint(-300f, EWeapon.NONE);
		Vector3 position = Vector3.zero;
		position.y = CameraController.Instance.TopCamera();
		float num = float.MaxValue;
		int num2 = -1;
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.HORIZONTAL)
		{
			if (orientaltion == CameraController.Orientation.VERTICAL)
			{
				List<Vector2> list = new List<Vector2>();
				for (int i = 0; i < GameManager.Instance.listRevival.Count; i++)
				{
					float num3 = Mathf.Abs(GameManager.Instance.listRevival[i].position.y - CameraController.Instance.transform.position.y);
					if (num3 < 3f)
					{
						list.Add(GameManager.Instance.listRevival[i].position);
					}
				}
				int a = 0;
				float num4 = float.MaxValue;
				for (int j = 0; j < list.Count; j++)
				{
					float num5 = Mathf.Abs(list[j].y - base.GetPosition().y);
					if (num5 < num4)
					{
						num4 = num5;
						a = j;
					}
				}
				position = list[Mathf.Min(a, list.Count - 1)];
			}
		}
		else
		{
			try
			{
				for (int k = 0; k < GameManager.Instance.listRevival.Count; k++)
				{
					if (CameraController.Instance.IsInView(GameManager.Instance.listRevival[k]))
					{
						float num6 = Mathf.Abs(GameManager.Instance.listRevival[k].position.x - this.transform.position.x);
						if (num6 <= num && GameManager.Instance.listRevival[k].gameObject.activeSelf)
						{
							num = num6;
							position = GameManager.Instance.listRevival[k].position;
							num2 = k;
						}
					}
				}
				if (GameMode.Instance.modePlay != GameMode.ModePlay.Boss_Mode && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL && GameMode.Instance.modePlay != GameMode.ModePlay.CoOpMode)
				{
					float checkpoint_pos_x = DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].checkpoint_pos_x;
					if (position.x >= checkpoint_pos_x || (!GameManager.Instance.listRevival[num2].gameObject.activeSelf && num2 != -1))
					{
						num = float.MaxValue;
						for (int l = 0; l < GameManager.Instance.listRevival.Count; l++)
						{
							if (GameManager.Instance.listRevival[l].position.x >= CameraController.Instance.LeftCamera() && GameManager.Instance.listRevival[l].position.x <= CameraController.Instance.RightCamera())
							{
								float num7 = Mathf.Abs(GameManager.Instance.listRevival[l].position.x - this.transform.position.x);
								if (num7 <= num)
								{
									num = num7;
									position = GameManager.Instance.listRevival[l].position;
								}
							}
						}
					}
				}
			}
			catch
			{
				position = this.transform.position;
				position.y = 0f;
			}
		}
		this.transform.position = position;
	}

	private Vector2 GetTarget()
	{
		Vector2 target = new Vector2(float.MaxValue, float.MaxValue);
		float num = float.MaxValue;
		for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
		{
			BaseEnemy baseEnemy = GameManager.Instance.ListEnemy[i];
			if (baseEnemy.isInCamera && baseEnemy.HP > 0f && baseEnemy.gameObject.activeSelf)
			{
				this.TargetCache = baseEnemy;
				Vector2 a = this.tfOrigin.position;
				Vector2 b = baseEnemy.Origin();
				float num2 = Vector2.Distance(a, b);
				if (num2 < num)
				{
					num = num2;
					target = baseEnemy.GetTarget();
				}
			}
		}
		return target;
	}

	private void onControllerCollider(RaycastHit2D hit)
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			this.LastPosIsGrounded = this.transform.position;
		}
	}

	private void onTriggerEnterEvent(Collider2D col)
	{
		if (col.CompareTag("Gulf"))
		{
			this.CollisionWithGulft();
		}
		if (col.CompareTag("gold"))
		{
			GameManager.Instance.fxManager.OnShowFxEarnGold(this.transform.position);
		}
		if (col.CompareTag("Ground"))
		{
			if (this._PlayerInput.IsShooting)
			{
				this._PlayerSpine.SetAim();
			}
			if (!this.isFirtTouchGround && GameManager.Instance.StateManager.EState == EGamePlay.BEGIN && PlayerManagerStory.Instance.typeBegin == PlayerManagerStory.TypeBegin.JUMP)
			{
				this.isFirtTouchGround = true;
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
				SingletonGame<AudioController>.Instance.PlaySound_P(this._audioGoGo, 1f);
				if (!object.ReferenceEquals(EnemyManager.Instance.enemy_helicopter, null))
				{
					EnemyManager.Instance.enemy_helicopter.gameObject.SetActive(true);
					EnemyManager.Instance.enemy_helicopter.Init();
				}
				GameManager.Instance.skillManager.Init();
				GameManager.Instance.StateManager.SetGameRuning();
				float zoomAmount = 3.6f - CameraController.Instance.mCamera.orthographicSize;
				if (GameMode.Instance.modePlay != GameMode.ModePlay.Boss_Mode && GameMode.Instance.modePlay != GameMode.ModePlay.PvpMode && GameMode.Instance.modePlay != GameMode.ModePlay.CoOpMode)
				{
					ProCamera2D.Instance.Zoom(zoomAmount, 2f, EaseType.EaseInOut);
				}
				if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
				{
					GameManager.Instance.TutorialManager.OnReady();
				}
			}
		}
	}

	private void onTriggerExitEvent(Collider2D col)
	{
	}

	public void OnRevive(float ratehp)
	{
		this.skeletonAnimation.skeleton.A = 0f;
		Vector2 v = this.LastPosIsGrounded;
		float num = float.MaxValue;
		foreach (Transform transform in GameManager.Instance.listRevival)
		{
			if (!(transform == null))
			{
				try
				{
					bool flag = CameraController.Instance.IsInView(transform);
					float num2 = Vector3.Distance(transform.position, this.transform.position);
					if (num2 < num && flag)
					{
						num = num2;
						v = transform.position;
					}
				}
				catch
				{
				}
			}
		}
		this.transform.position = v;
		base.HPCurrent = 0f;
		base.HPCurrent = ratehp * this._PlayerData.ramboProfile.HP;
		if (GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode || (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode && !this.IsRemotePlayer))
		{
			PvP_LocalPlayer.Instance.HP = base.HPCurrent;
		}
		this.EMovement = BaseCharactor.EMovementBasic.Release;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		this.effHoiSinh.OnPlay(delegate
		{
			this.skeletonAnimation.skeleton.A = 1f;
			this._PlayerSpine.targetAnimation = this.GunCurrent.GetAnimIdle();
			this.skeletonAnimation.state.SetAnimation(0, this._PlayerSpine.targetAnimation, true);
			this._PlayerSpine.previousTargetAnimation = this._PlayerSpine.targetAnimation;
			this.effShield.OnPlay();
			this.IsProteced = true;
			this.time_protected = Time.timeSinceLevelLoad;
			foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(base.GetPosition(), 3f, this.layerAttackOnlyEnemy))
			{
				if (collider2D != null)
				{
					try
					{
						collider2D.GetComponent<BaseEnemy>().AddHealthPoint(-500f, EWeapon.NONE);
					}
					catch (Exception ex)
					{
					}
				}
			}
		});
		EventDispatcher.PostEvent<MyMessage>("HpValueChange", null);
	}

	private void OnReviveTutorial()
	{
		this.OnRevive(1f);
		EventDispatcher.PostEvent<MyMessage>("HpValueChange", null);
	}

	public bool isBatTu { get; set; }

	public void AddHealthPoint(float hp, EWeapon lastWeapon)
	{
		if (SplashScreen._isBuildMarketing || this.isBatTu || this.IsRemotePlayer)
		{
			return;
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING || this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		float num = 0f;
		if ((hp < 0f && this.IsProteced) || SplashScreen._isBuildMarketing)
		{
			return;
		}
		if (hp <= 0f)
		{
			if (this.effHit != null)
			{
				this.effHit.OnShow();
			}
			num = hp + hp * GameManager.Instance.RatePower;
			if (this._PlayerBooster.IsArmor)
			{
				hp += 0.3f * Mathf.Abs(hp);
			}
		}
		else
		{
			this.effHoiMau.OnPlay();
		}
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			hp = -5f;
			num = 0f;
		}
		base.HPCurrent += hp + num;
		base.HPCurrent = Mathf.Clamp(base.HPCurrent, 0f, this._PlayerData.ramboProfile.HP);
		if (GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode || (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode && !this.IsRemotePlayer))
		{
			PvP_LocalPlayer.Instance.HP = base.HPCurrent;
		}
		if (base.HPCurrent <= 0f)
		{
			this.GunCurrent.WeaponCurrent.OnRelease();
			if (GameManager.Instance.isJetpackMode)
			{
				JetpackManager.Instance.ObjFly.SetActive(false);
			}
			base.HPCurrent = 0f;
			this._PlayerSpine.OnDie();
			this.EMovement = BaseCharactor.EMovementBasic.DIE;
			SingletonGame<AudioController>.Instance.PlaySound_P(this._audioDead, 1f);
			if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
			{
				EventDispatcher.PostEvent<MyMessage>("HpValueChange", null);
				base.Invoke("OnReviveTutorial", 1f);
				return;
			}
			if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
			{
				EventDispatcher.PostEvent("LostGame");
			}
		}
		EventDispatcher.PostEvent<MyMessage>("HpValueChange", null);
	}

	public List<GunPlayer> Guns;

	public GunPlayer GunCurrent;

	public PlayerSpine _PlayerSpine;

	public PlayerInput _PlayerInput;

	public PlayerData _PlayerData;

	public PlayerBooster _PlayerBooster;

	public PlayerJetpack _PlayerJetpack;

	public PlayerSkill _PlayerSkill;

	public CharacterController2D _controller;

	private RaycastHit2D _lastControllerColliderHit;

	private float normalizedHorizontalSpeed;

	private Vector3 _velocity;

	public Transform tfBone;

	public Vector2 DirectionJoystick;

	private bool isInit;

	private bool isFirtTouchGround;

	public bool isLockDirection;

	private BaseEnemy TargetCache;

	private bool hasTarget;

	public LayerMask layerTarget;

	public LayerMask layerTargetSkill3;

	public LayerMask layerAttackOnlyEnemy;

	public bool IsProteced;

	public bool isAutoRun;

	private float time_protected;

	[SpineSlot("", "", false, true, false)]
	public string[] SlotJetpack;

	[SpineAttachment(true, false, false, "", "", "", true, false)]
	public string[] AttachmentJetpack;

	[SpineSlot("", "", false, true, false)]
	public string SlotBag;

	[SpineAttachment(true, false, false, "", "", "", true, false)]
	public string AttachmentBag;

	public KnifeSword objKnifeSword;

	public Knife objKnife;

	public BoxCollider2D boxMain;

	private Vector2 OffsetBoxCurrent;

	private Vector2 SizeBoxCurrent;

	private Vector2 LastPosIsGrounded;

	public AudioClip _audioGoGo;

	public AudioClip _audioDead;

	public AudioClip _audioVictory;

	public PlayerHoiSinh effHoiSinh;

	public PlayerShield effShield;

	public PlayAnMau effHoiMau;

	private PlayerHit effHit;

	public bool isGunDefault;

	public bool IsRemotePlayer;

	public SyncRamboState syncRamboState;

	public int actorNumber;

	public bool isNPC;

	private NPC_FakeControl npcFakeControl;

	public int IDChar;

	private int NPC_IDGun1;

	private int NPC_IDKnife;

	private int NPC_IDGrenades;

	private Vector3 vt3 = default(Vector3);
}
