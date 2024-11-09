using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_HeroicMech : BaseBoss
{
	private void GetMode()
	{
		this._mode = GameMode.Instance.EMode;
	}

	private void Update()
	{
		if (!this.isInit || this.State == ECharactor.DIE || (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW))
		{
			if (!this.isInit && base.isInCamera)
			{
				this.InitEnemy();
			}
			if (GameManager.Instance.StateManager.EState == EGamePlay.PAUSE && this.skeletonAnimation.timeScale != 0f)
			{
				this._timeScale = this.skeletonAnimation.timeScale;
				this.skeletonAnimation.timeScale = 0f;
			}
			return;
		}
		if (this.skeletonAnimation.timeScale == 0f)
		{
			this.skeletonAnimation.timeScale = this._timeScale;
		}
		this.UpdateObject(Time.deltaTime);
	}

	public void SkipPreviewBoss()
	{
		this._pos = this.transform.position;
		this._posCam = CameraController.Instance.transform.position;
		float num = this._pos.x - this._posCam.x;
		if (num > 10f)
		{
			this._pos.x = this._posCam.x + 10f;
			this.transform.position = this._pos;
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
			this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.animations = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.GetMode();
		this.bodyCollider2D.enabled = false;
		this.meshRenderer.sortingOrder = 0;
		base.gameObject.SetActive(true);
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.GetBone();
		this.ResetCountDownTimeBodyLine();
		this._targetMove.x = CameraController.Instance.transform.position.x + 6f;
		this._targetMove.y = this.transform.position.y;
		this._gunType = 1;
		this._flip = false;
		this._isWalk = true;
		this._isGoLeft = true;
		this.PlayAnim(0, Boss_HeroicMech.EState.Walk, false, 1f);
		this.isInit = true;
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && GameManager.Instance.Level == global::ELevel.LEVEL_15)
		{
			return;
		}
	}

	private void GetBone()
	{
		this.boneGun1_1 = this.skeletonAnimation.skeleton.FindBone("Gun-tip-1");
		this.boneGun1_2 = this.skeletonAnimation.skeleton.FindBone("Gun-tip-2");
		this.boneGun2_1 = this.skeletonAnimation.skeleton.FindBone("Gun-tip-3");
		this.boneGun2_2 = this.skeletonAnimation.skeleton.FindBone("Gun-tip-4");
		this.boneRocket1 = this.skeletonAnimation.skeleton.FindBone("Gun-tip-5");
		this.boneRocket2 = this.skeletonAnimation.skeleton.FindBone("Gun-tip-6");
	}

	public void UpdateObject(float deltaTime)
	{
		this.CachePos();
		if (!this.Begin(deltaTime))
		{
			return;
		}
		if (!this._changeState)
		{
			this._changeState = true;
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		if (this._countDownTimeBodyLine > 0f)
		{
			this._countDownTimeBodyLine -= deltaTime;
			if (this._countDownTimeBodyLine <= 0f)
			{
				this.ActiveBodyLine(true);
			}
		}
		if (this.bodyLine.isActive)
		{
			this.bodyLine.OnUpdate(deltaTime);
		}
		if (this.laser.isInit)
		{
			this.laser.UpdateObject(deltaTime);
		}
	}

	private void CachePos()
	{
		this._pos = this.transform.position;
		this._posCam = CameraController.Instance.transform.position;
		this._posRambo = GameManager.Instance.player.tfOrigin.position;
	}

	private bool Begin(float deltaTime)
	{
		if (this._isBegin)
		{
			return true;
		}
		this.transform.Translate(-this.cacheEnemy.Speed * deltaTime, 0f, 0f);
		bool flag = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x) < 5f;
		if (flag)
		{
			GameManager.Instance.ListEnemy.Add(this);
			this._isWalk = false;
			this._isBegin = true;
			this.bodyCollider2D.enabled = true;
			this._state = Boss_HeroicMech.EState.Fire;
			this._changeState = false;
		}
		return false;
	}

	private void StartState()
	{
		Boss_HeroicMech.EState state = this._state;
		if (state != Boss_HeroicMech.EState.Fire && state != Boss_HeroicMech.EState.Rocket)
		{
			switch (state)
			{
			case Boss_HeroicMech.EState.Ngoi1:
			case Boss_HeroicMech.EState.Walk:
				this._isWalk = true;
				this.FindTargetMove(true);
				if (this._targetMove.x < this._pos.x)
				{
					if (this._scale.x < 0f)
					{
						this.PlayAnim(0, Boss_HeroicMech.EState.Walk_Back, false, 1f);
					}
					else
					{
						this.PlayAnim(0, Boss_HeroicMech.EState.Walk, false, 1f);
					}
				}
				else if (this._scale.x < 0f)
				{
					this.PlayAnim(0, Boss_HeroicMech.EState.Walk, true, 1f);
				}
				else
				{
					this.PlayAnim(0, Boss_HeroicMech.EState.Walk_Back, true, 1f);
				}
				break;
			}
		}
		else
		{
			this.FlipBoss(this._pos.x < this._posRambo.x);
			Boss_HeroicMech.EState aimState = (this._state != Boss_HeroicMech.EState.Rocket) ? Boss_HeroicMech.EState.Aim_Fire : Boss_HeroicMech.EState.Aim_Rocket;
			this._posAim = this._posRambo;
			this._posAim.x = ((Mathf.Abs(this._posAim.x - this._pos.x) >= 2f) ? this._posAim.x : ((this.transform.localScale.x >= 0f) ? (this._pos.x - 2f) : (this._pos.x + 2f)));
			this.PlayAnimAim(aimState, this._state, this._posAim);
		}
	}

	private void UpdateState(float deltaTime)
	{
		Boss_HeroicMech.EState state = this._state;
		if (state != Boss_HeroicMech.EState.Fire && state != Boss_HeroicMech.EState.Rocket)
		{
			switch (state)
			{
			case Boss_HeroicMech.EState.Ngoi1:
				if (this._pos.x == this._targetMove.x)
				{
					if (this._isWalk)
					{
						this._isWalk = false;
						this.FlipBoss(this._pos.x < this._posRambo.x);
						this.PlayAnim(0, this._state, false, 1f);
					}
				}
				else
				{
					this.MoveToTarget(deltaTime);
				}
				break;
			case Boss_HeroicMech.EState.Walk:
				this.MoveToTarget(deltaTime);
				if (this._pos.x == this._targetMove.x)
				{
					this._isWalk = false;
					this.ChangeState();
				}
				break;
			}
		}
	}

	private void FlipBoss(bool flip)
	{
		this._flip = flip;
		this._scale = this.transform.localScale;
		this._scale.x = ((!flip) ? Mathf.Abs(this._scale.x) : (-Mathf.Abs(this._scale.x)));
		this.transform.localScale = this._scale;
	}

	private void PlayAnim(int track, Boss_HeroicMech.EState state, bool loop = false, float speed = 1f)
	{
		this.skeletonAnimation.timeScale = speed;
		this.skeletonAnimation.AnimationState.SetAnimation(track, this.animations[(int)state], loop);
	}

	private void PlayAnimAim(Boss_HeroicMech.EState aimState, Boss_HeroicMech.EState attackState, Vector3 targetPos)
	{
		this.skeletonAnimation.state.SetEmptyAnimation(0, 0.3f);
		this.PlayAnim(2, aimState, false, 1f);
		base.StartCoroutine(this.MoveAim(targetPos, delegate
		{
			this.PlayAnim(1, attackState, false, 1f);
		}));
	}

	private IEnumerator MoveAim(Vector3 target, Action done)
	{
		yield return this.waitRun;
		float distance = Vector3.Distance(this.objTargetFire.transform.position, target);
		Vector3 velo = Vector3.zero;
		while (distance > 0.1f)
		{
			this.objTargetFire.transform.position = Vector3.SmoothDamp(this.objTargetFire.transform.position, target, ref velo, 0.2f);
			distance = Vector3.Distance(this.objTargetFire.transform.position, target);
			yield return 0;
		}
		try
		{
			done();
		}
		catch
		{
		}
		yield break;
	}

	private void ChangeState()
	{
		Boss_HeroicMech.EState state = this._state;
		if (state != Boss_HeroicMech.EState.Fire && state != Boss_HeroicMech.EState.Rocket)
		{
			switch (state)
			{
			case Boss_HeroicMech.EState.Ngoi1:
				this._state = Boss_HeroicMech.EState.Walk;
				break;
			case Boss_HeroicMech.EState.Walk:
				this._state = ((this._gunType != 1) ? Boss_HeroicMech.EState.Fire : Boss_HeroicMech.EState.Rocket);
				break;
			}
		}
		else
		{
			this._state = ((!this._isBodyLine) ? Boss_HeroicMech.EState.Walk : Boss_HeroicMech.EState.Ngoi1);
		}
		this._changeState = false;
	}

	private void FindTargetMove(bool isWalk = true)
	{
		this._targetMove.y = this._pos.y;
		if (isWalk)
		{
			this._targetMove.x = this._targetMove.x + (float)((!this._isGoLeft) ? 2 : -2);
			this._targetMove.x = Mathf.Max(this._targetMove.x, this._posCam.x - 6f);
			this._targetMove.x = Mathf.Min(this._targetMove.x, this._posCam.x + 6f);
			this._isGoLeft = (this._targetMove.x != this._posCam.x - 6f && this._isGoLeft);
			this._isGoLeft = (this._targetMove.x == this._posCam.x + 6f || this._isGoLeft);
		}
		else
		{
			this._targetMove.x = (float)((this._posCam.x + this._pos.x >= this._posCam.x) ? 6 : -6);
		}
	}

	private void MoveToTarget(float deltaTime)
	{
		this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetMove.x, this.cacheEnemy.Speed * deltaTime);
		this.transform.position = this._pos;
	}

	private void CreatBullet(int num)
	{
		Vector3 v;
		Vector3 worldPosition;
		if (num == 0)
		{
			v = this.boneGun1_1.GetWorldPosition(this.transform) - this.boneGun1_2.GetWorldPosition(this.transform);
			worldPosition = this.boneGun1_1.GetWorldPosition(this.transform);
		}
		else
		{
			v = this.boneGun2_1.GetWorldPosition(this.transform) - this.boneGun2_2.GetWorldPosition(this.transform);
			worldPosition = this.boneGun2_1.GetWorldPosition(this.transform);
		}
		BulletEnemy bulletEnemy = GameManager.Instance.bulletManager.CreateBulletEnemy(7, v, worldPosition, this.GetDamage(Boss_HeroicMech.EState.Fire), this.GetSpeed(Boss_HeroicMech.EState.Fire), 0f);
		bulletEnemy.spriteRenderer.flipX = false;
		bulletEnemy.useByBoss = true;
	}

	private void CreatRocket()
	{
		Vector3 v = this.boneRocket1.GetWorldPosition(this.transform) - this.boneRocket2.GetWorldPosition(this.transform);
		GameManager.Instance.bulletManager.CreateBulletAnim(ETypeBullet.BOSS_1_2, this.boneRocket1.GetWorldPosition(this.transform), v, this.GetDamage(Boss_HeroicMech.EState.Rocket), this.GetSpeed(Boss_HeroicMech.EState.Rocket));
	}

	private void ActiveBodyLine(bool oneShot = true)
	{
		this._isBodyLine = true;
		if (this.bodyLine.hideAction == null)
		{
			this.bodyLine.hideAction = new Action<bool>(this.DeactiveBodyLine);
		}
		float num = Mathf.Round(this.cacheEnemy.Damage / 50f);
		num = Mathf.Max(1f, num);
		num = ((this._mode != GameMode.Mode.NORMAL) ? num : 0.5f);
		float activeTime = (!oneShot) ? 0f : this.timeActiveBodyLine;
		this.bodyLine.Active(num, activeTime, oneShot);
	}

	private void DeactiveBodyLine(bool oneShot)
	{
		if (oneShot)
		{
			this._isBodyLine = false;
			this.ResetCountDownTimeBodyLine();
		}
	}

	private void ResetCountDownTimeBodyLine()
	{
		GameMode.Mode mode = this._mode;
		if (mode != GameMode.Mode.NORMAL)
		{
			if (mode != GameMode.Mode.HARD)
			{
				if (mode == GameMode.Mode.SUPER_HARD)
				{
					this._countDownTimeBodyLine = 3.5f;
				}
			}
			else
			{
				this._countDownTimeBodyLine = 4f;
			}
		}
		else
		{
			this._countDownTimeBodyLine = 5f;
		}
	}

	private void ActiveLaser()
	{
		Vector2 direction = (!this._flip) ? Vector2.left : Vector2.right;
		this.laser.InitObject(this.cacheEnemy.Damage * 4f, this.laserTimeActive, this.laserTimeDelay, direction, new Action(this.DeactiveLaser));
	}

	private void DeactiveLaser()
	{
		this._isLaserFire = false;
		this.PlayAnim(0, Boss_HeroicMech.EState.Ngoi3, false, 1f);
	}

	private float GetSpeed(Boss_HeroicMech.EState state)
	{
		return this.cacheEnemy.Speed * this.ratioSpeed[(int)state];
	}

	private float GetDamage(Boss_HeroicMech.EState state)
	{
		float f = this.cacheEnemy.Damage * this.ratioDamage[(int)state];
		return Mathf.Round(f);
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW))
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
		this.skeletonAnimation.state.SetAnimation(3, this.animations[9], false);
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[0].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[0].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Heroic Mech has been unlocked in BossMode!");
			}
			DailyQuestManager.Instance.MissionBoss(0, 0, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
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
			if (this.isBoss)
			{
				GameManager.Instance.hudManager.HideControl();
			}
			base.StartCoroutine(this.EffectDie());
		}
	}

	private void Die()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.bodyLine.Deactive();
		this.laser.gameObject.SetActive(false);
		this.State = ECharactor.DIE;
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		this.PlayAnim(3, Boss_HeroicMech.EState.Die, false, 1f);
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
	}

	private IEnumerator EffectDie()
	{
		Vector3 pos = this.tfOrigin.position;
		pos.y -= 1.5f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
		yield return new WaitForSeconds(0.2f);
		pos = this.boneGun1_2.GetWorldPosition(this.transform);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
		yield return new WaitForSeconds(0.2f);
		pos = this.tfOrigin.position;
		pos.y += 1f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
		yield return new WaitForSeconds(0.2f);
		pos = this.tfOrigin.position;
		pos.x += 1f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
		yield return new WaitForSeconds(0.2f);
		pos = this.boneGun1_1.GetWorldPosition(this.transform);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
		CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
		if (this.isBoss)
		{
			PlayerManagerStory.Instance.OnRunGameOver();
		}
		yield break;
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		try
		{
			string name = trackEntry.Animation.Name;
			switch (name)
			{
			case "attack1":
			{
				bool flag = this._fireCount > (int)this._mode;
				if (flag)
				{
					this._fireCount = 0;
					this.skeletonAnimation.state.SetEmptyAnimation(2, 0.3f);
					this.ChangeState();
				}
				else
				{
					this._changeState = false;
				}
				break;
			}
			case "attack2":
			{
				bool flag2 = this._rocketCount > (int)this._mode;
				if (flag2)
				{
					this._rocketCount = 0;
					this.skeletonAnimation.state.SetEmptyAnimation(2, 0.3f);
					this.ChangeState();
				}
				else
				{
					this._changeState = false;
				}
				break;
			}
			case "ngoi1":
				this.PlayAnim(0, Boss_HeroicMech.EState.Ngoi2, false, 1f);
				this._countDownTimeBodyLine = 0f;
				this.ActiveBodyLine(false);
				break;
			case "ngoi2":
				this.ActiveLaser();
				break;
			case "ngoi3":
				this.bodyLine.Deactive();
				this.ResetCountDownTimeBodyLine();
				this.ChangeState();
				break;
			case "walk":
			case "walk2":
				if (this._isWalk)
				{
					this.skeletonAnimation.AnimationState.SetAnimation(0, trackEntry.Animation.Name, false);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "attack1-1"))
			{
				if (!(name == "attack1-2"))
				{
					if (!(name == "attack2"))
					{
						if (name == "no")
						{
							Vector3 vector=Vector3.zero;
							this.tfOrigin.position = new Vector3(this.tfOrigin.position.x, vector.y - 1f, this.tfOrigin.position.z);
							GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.tfOrigin.position, Vector3.one * 1.4f);
							CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
						}
					}
					else
					{
						this._gunType = 2;
						this._rocketCount++;
						this.CreatRocket();
					}
				}
				else
				{
					this.CreatBullet(1);
				}
			}
			else
			{
				this._gunType = 1;
				this._fireCount++;
				this.CreatBullet(0);
			}
		}
	}

	[Header("Data boss:")]
	[SerializeField]
	private bool isBoss = true;

	[SerializeField]
	[Header("Spine:")]
	private SkeletonAnimation skeletonAnimation;

	private Spine.Animation[] animations;

	[SerializeField]
	private float[] ratioSpeed;

	[SerializeField]
	private float[] ratioDamage;

	[SerializeField]
	private GameObject objTargetFire;

	[SerializeField]
	private BodyLine bodyLine;

	[SerializeField]
	private float timeActiveBodyLine = 1.5f;

	[SerializeField]
	private Laser_HeroicMech laser;

	[SerializeField]
	private float laserTimeDelay;

	[SerializeField]
	private float laserTimeActive;

	private GameMode.Mode _mode;

	private Boss_HeroicMech.EState _state;

	private float _timeScale;

	private bool _isBegin;

	private bool _changeState;

	private int _gunType;

	private Vector3 _pos;

	private Vector3 _posCam;

	private Vector3 _posRambo;

	private Vector3 _targetMove = Vector3.zero;

	private bool _flip;

	private Vector3 _scale;

	private int _attackCount;

	private bool _isBodyLine;

	private bool _isLaserFire;

	private bool _isGoLeft;

	private bool _isWalk;

	private int _fireCount;

	private int _rocketCount;

	private float _countDownTimeBodyLine;

	private Bone boneGun1_1;

	private Bone boneGun1_2;

	private Bone boneGun2_1;

	private Bone boneGun2_2;

	private Bone boneRocket1;

	private Bone boneRocket2;

	private WaitUntil waitRun = new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);

	private Vector3 _posAim;

	private int hit;

	private enum EState
	{
		Aim_Fire,
		Aim_Rocket,
		Fire,
		Rocket,
		Attack3,
		Attack4_1,
		Attack4_2,
		Attack4_3,
		Die,
		Hit,
		Idle,
		Ngoi1,
		Ngoi2,
		Ngoi3,
		Walk,
		Walk_Back
	}
}
