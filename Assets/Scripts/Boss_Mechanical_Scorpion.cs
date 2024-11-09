using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_Mechanical_Scorpion : BaseBoss
{
	private void Update()
	{
		if (!this.isInit || this._state == Boss_Mechanical_Scorpion.EState.Death || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.PAUSE && !this._pause)
			{
				this.Pause(true);
			}
			return;
		}
		if (this._pause)
		{
			this.Pause(false);
		}
		float deltaTime = Time.deltaTime;
		this.OnUpdate(deltaTime);
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
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this._boneAttack3 = this.skeletonAnimation.skeleton.FindBone(this.boneAttack3);
		this._boneBody = this.skeletonAnimation.skeleton.FindBone(this.boneBody);
		this._boneGun = this.skeletonAnimation.skeleton.FindBone(this.boneGun);
		this._boneSnowBall = this.skeletonAnimation.skeleton.FindBone(this.boneSnowBall);
		this.bodyCollider2D.enabled = false;
		this.skeletonAnimation.state.SetAnimation(0, this.anims[4], true);
		this.Flip(this.transform.position.x < GameManager.Instance.player.transform.position.x);
		this.isInit = true;
		try
		{
			ProCamera2D.Instance.OffsetX = 0f;
		}
		catch (Exception ex)
		{
		}
	}

	private void CallbackAttackCauTuyet()
	{
	}

	private void CallbackAttackLan()
	{
		this.ChangeState();
	}

	private void CallbackAttackKhe()
	{
	}

	private void OnUpdate(float deltaTime)
	{
		if (!this._isBegin)
		{
			this.transform.Translate((!this._flip) ? (-this.cacheEnemy.Speed * 2f * deltaTime) : (this.cacheEnemy.Speed * 2f * deltaTime), 0f, 0f);
			float num = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x);
			if (num < 6f && base.isInCamera)
			{
				this._isBegin = true;
				this._state = Boss_Mechanical_Scorpion.EState.Idle;
				this._changeState = false;
				this.bodyCollider2D.enabled = true;
				GameManager.Instance.ListEnemy.Add(this);
			}
			return;
		}
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		this._bodyPos = this._boneBody.GetWorldPosition(this.transform);
		this._bodyPos.y = this._bodyPos.y + 0.7f;
		this.bodyCollider2D.offset = this._bodyPos - this.transform.position;
		this.tfTarget[0].position = this._bodyPos;
		this.tfTarget[1].position = this._bodyPos - Vector3.left / 2f;
		this.tfTarget[2].position = this._bodyPos + Vector3.left / 2f;
	}

	private void StartState()
	{
		this._changeState = true;
		int trackIndex = 0;
		bool loop = false;
		switch (this._state)
		{
		case Boss_Mechanical_Scorpion.EState.Attack_CauTuyet:
			loop = true;
			this.Flip(this.transform.position.x < GameManager.Instance.player.transform.position.x);
			this._speedMove = 0f;
			this.skeletonAnimation.timeScale = 0.2f;
			this.attackBoxCauTuyet.Active(this.GetDamage(), true, new Action(this.CallbackAttackCauTuyet));
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Khe:
			this.PlaySound(this.audioKhe, 1f);
			this.Flip(this.transform.position.x < GameManager.Instance.player.transform.position.x);
			this._timeActiveKhe = 0.2f;
			if (this._isStun)
			{
				this.skeletonAnimation.timeScale = 1.3f;
			}
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_1:
			this.Flip(this.transform.position.x < GameManager.Instance.player.transform.position.x);
			this.PlaySound(this.audioLan1, 1f);
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_2:
			loop = true;
			this._speedMove = 0f;
			this.PlaySound(this.audioLan2, 1f);
			this.attackBoxLan.Active(this.GetDamage(), true, new Action(this.CallbackAttackLan));
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_3:
			this.PlaySound(this.audioLan3, 1f);
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Gun_1:
			trackIndex = 1;
			this._speedMove = this.cacheEnemy.Speed;
			this.Flip(this.transform.position.x < GameManager.Instance.player.transform.position.x);
			this.skeletonAnimation.state.SetEmptyAnimation(0, 0f);
			this.PlaySound(this.audioShot1, 1f);
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Gun_2:
			trackIndex = 1;
			loop = true;
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Gun_3:
			trackIndex = 1;
			this.PlaySound(this.audioShot1, 1f);
			break;
		case Boss_Mechanical_Scorpion.EState.Run:
			loop = true;
			this.skeletonAnimation.timeScale = 1.3f;
			this._targetMove = this.transform.position;
			if (!base.isInCamera)
			{
				this._isRunAndGun = true;
				this._targetMove.x = CameraController.Instance.camPos.x + ((this._targetMove.x >= CameraController.Instance.camPos.x) ? UnityEngine.Random.Range(2f, 4f) : (-UnityEngine.Random.Range(2f, 4f)));
			}
			else
			{
				float num = UnityEngine.Random.Range(3f, 5f);
				this._targetMove.x = this._targetMove.x + ((this._targetMove.x >= CameraController.Instance.transform.position.x) ? (-num) : num);
			}
			this._speedMove = this.cacheEnemy.Speed;
			this.Flip(this.transform.position.x < this._targetMove.x);
			break;
		case Boss_Mechanical_Scorpion.EState.Stun_2:
			loop = true;
			this.PlaySound(this.audioStun2, 1f);
			break;
		}
		this.skeletonAnimation.state.SetAnimation(trackIndex, this.anims[(int)this._state], loop);
	}

	private void UpdateState(float deltaTime)
	{
		Boss_Mechanical_Scorpion.EState state = this._state;
		switch (state)
		{
		case Boss_Mechanical_Scorpion.EState.Attack_CauTuyet:
			if (this._flip)
			{
				this.transform.Translate(this._speedMove * deltaTime, 0f, 0f);
			}
			else
			{
				this.transform.Translate(-this._speedMove * deltaTime, 0f, 0f);
			}
			if (this._speedMove < this.cacheEnemy.Speed * 3f)
			{
				this._speedMove += deltaTime * 10f;
			}
			this.attackBoxCauTuyet.transform.position = this._boneSnowBall.GetWorldPosition(this.transform);
			if (this.skeletonAnimation.timeScale < 1f)
			{
				this.skeletonAnimation.timeScale = Mathf.MoveTowards(this.skeletonAnimation.timeScale, 1f, deltaTime);
			}
			if (!base.isInCamera)
			{
				this.ChangeState();
			}
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Khe:
			if (this.attackBoxKhe.isAttack)
			{
				if (this._timeActiveKhe > 0f)
				{
					this._timeActiveKhe -= deltaTime;
				}
				else
				{
					this.attackBoxKhe.Deactive();
				}
			}
			break;
		default:
			if (state == Boss_Mechanical_Scorpion.EState.Run)
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetMove, this._speedMove * deltaTime);
				if ((Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x) < 1f && base.isInCamera) || this.transform.position == this._targetMove)
				{
					this.ChangeState();
				}
				if (this._isRunAndGun)
				{
					bool flag = Mathf.Abs(this.transform.position.x - CameraController.Instance.camPos.x) <= 6f;
					if (flag)
					{
						this.ChangeState();
					}
				}
			}
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_2:
			if (this._flip)
			{
				this.transform.Translate(this._speedMove * deltaTime, 0f, 0f);
			}
			else
			{
				this.transform.Translate(-this._speedMove * deltaTime, 0f, 0f);
			}
			if (this._speedMove < this.cacheEnemy.Speed * 3f)
			{
				this._speedMove += deltaTime * 10f;
			}
			this.attackBoxLan.transform.position = this._bodyPos;
			if (!base.isInCamera)
			{
				this.ChangeState();
			}
			break;
		}
	}

	private void ChangeState()
	{
		if (this._state == Boss_Mechanical_Scorpion.EState.Death)
		{
			return;
		}
		switch (this._state)
		{
		case Boss_Mechanical_Scorpion.EState.Attack_CauTuyet:
			this.attackBoxCauTuyet.Deactive();
			this.ChangeStateToRunOrIdle();
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Khe:
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_3:
			this.ChangeStateToRunOrIdle();
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_1:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_Lan_2;
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_2:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_Lan_3;
			this.attackBoxLan.Deactive();
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Gun_1:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_Gun_2;
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Gun_2:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_Gun_3;
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Gun_3:
			this.skeletonAnimation.state.SetEmptyAnimation(1, 0f);
			if (this._isRunAndGun)
			{
				this._isRunAndGun = false;
				this._state = Boss_Mechanical_Scorpion.EState.Run;
			}
			else
			{
				this.ChangeStateToRunOrIdle();
			}
			break;
		case Boss_Mechanical_Scorpion.EState.Idle:
		case Boss_Mechanical_Scorpion.EState.Stun_3:
			this.ChangeStateToAttack();
			break;
		case Boss_Mechanical_Scorpion.EState.Run:
			this.skeletonAnimation.timeScale = 1f;
			if (this._isRunAndGun)
			{
				this._state = ((UnityEngine.Random.Range(0, 4) >= 3) ? Boss_Mechanical_Scorpion.EState.Attack_Khe : Boss_Mechanical_Scorpion.EState.Attack_Gun_1);
			}
			else
			{
				this.ChangeStateToAttack();
			}
			break;
		case Boss_Mechanical_Scorpion.EState.Stun_1:
			this._state = Boss_Mechanical_Scorpion.EState.Stun_2;
			break;
		case Boss_Mechanical_Scorpion.EState.Stun_2:
			this._state = Boss_Mechanical_Scorpion.EState.Stun_3;
			break;
		}
		if (this.HP <= this.cacheEnemy.HP / 2f && !this._isStun)
		{
			this._isStun = true;
			this._state = Boss_Mechanical_Scorpion.EState.Stun_1;
		}
		this._changeState = false;
	}

	private void ChangeStateToRunOrIdle()
	{
		if (!base.isInCamera)
		{
			this._state = Boss_Mechanical_Scorpion.EState.Run;
			return;
		}
		bool flag = this.HP < this.cacheEnemy.HP * 3f / 5f;
		if (flag)
		{
			this.ChangeStateToAttack();
			return;
		}
		this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? Boss_Mechanical_Scorpion.EState.Idle : Boss_Mechanical_Scorpion.EState.Run);
	}

	private void ChangeStateToAttack()
	{
		float num = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x);
		if (num < 2f)
		{
			this._state = ((UnityEngine.Random.Range(0, 4) >= 3) ? Boss_Mechanical_Scorpion.EState.Attack_Khe : Boss_Mechanical_Scorpion.EState.Attack_Lan_1);
			return;
		}
		if (num < 4f)
		{
			this._state = ((UnityEngine.Random.Range(0, 4) >= 3) ? Boss_Mechanical_Scorpion.EState.Attack_Khe : Boss_Mechanical_Scorpion.EState.Attack_CauTuyet);
			return;
		}
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_CauTuyet;
			break;
		case 1:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_Lan_1;
			break;
		case 2:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_Khe;
			break;
		default:
			this._state = Boss_Mechanical_Scorpion.EState.Attack_Gun_1;
			break;
		}
	}

	private void Flip(bool flip)
	{
		this._flip = flip;
		this._scale = this.transform.localScale;
		this._scale.x = ((!flip) ? Mathf.Abs(this._scale.x) : (-Mathf.Abs(this._scale.x)));
		this.transform.localScale = this._scale;
	}

	private void Pause(bool v)
	{
		this._pause = v;
		if (v)
		{
			this._spineTimeScale = this.skeletonAnimation.timeScale;
			this.skeletonAnimation.timeScale = 0f;
		}
		else
		{
			this.skeletonAnimation.timeScale = this._spineTimeScale;
		}
	}

	private void PlaySound(AudioClip clip, float volume = 1f)
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(clip, volume);
		}
		catch
		{
		}
	}

	private float GetDamage()
	{
		float num = 1f;
		switch (this._state)
		{
		case Boss_Mechanical_Scorpion.EState.Attack_CauTuyet:
			num = 1f;
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Khe:
		case Boss_Mechanical_Scorpion.EState.Attack_Lan_2:
			num = 2f;
			break;
		case Boss_Mechanical_Scorpion.EState.Attack_Gun_2:
			num = 0.6f;
			break;
		}
		return Mathf.Round(this.cacheEnemy.Damage * num);
	}

	public override void Hit(float damage)
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
		if (this.HP <= 0f)
		{
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			base.StartCoroutine(this.Die());
			return;
		}
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		this.skeletonAnimation.state.SetAnimation(2, this.anims[10], false);
	}

	private IEnumerator Die()
	{
		yield return 0;
		this._state = Boss_Mechanical_Scorpion.EState.Death;
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this._state], false);
		GameManager.Instance.ListEnemy.Remove(this);
		GameManager.Instance.hudManager.HideControl();
		GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
		if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[8].CheckUnlock(GameMode.Mode.NORMAL))
		{
			ProfileManager.bossModeProfile.bossProfiles[8].SetUnlock(GameMode.Mode.NORMAL, true);
			UIShowInforManager.Instance.ShowUnlock("Mechanical Scorpion has been unlocked in BossMode!");
		}
		Vector3 fxPos = this._boneGun.GetWorldPosition(this.transform);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		fxPos = this._boneBody.GetWorldPosition(this.transform);
		fxPos.y += 1.5f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		fxPos = this.transform.position;
		fxPos.y += 1f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		PlayerManagerStory.Instance.OnRunGameOver();
		yield break;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string text = e.ToString();
		if (text != null)
		{
			if (!(text == "rung"))
			{
				if (!(text == "attack3"))
				{
					if (!(text == "attack4"))
					{
						if (!(text == "off"))
						{
							if (text == "on")
							{
								this._speedMove = this.cacheEnemy.Speed;
							}
						}
						else
						{
							this._speedMove = 0f;
						}
					}
					else
					{
						Vector3 worldPosition = this._boneGun.GetWorldPosition(this.transform);
						BulletEnemy bulletEnemy = GameManager.Instance.bulletManager.CreateBulletEnemy(7, (!this._flip) ? Vector2.left : Vector2.right, worldPosition, this.GetDamage(), this.cacheEnemy.Speed * 3f, 0f);
						bulletEnemy.spriteRenderer.flipX = false;
						bulletEnemy.useByBoss = true;
						this.PlaySound(this.audioShot2, 1f);
					}
				}
				else
				{
					this.attackBoxKhe.Active(this.GetDamage(), true, new Action(this.CallbackAttackKhe));
					this.attackBoxKhe.transform.position = this._boneAttack3.GetWorldPosition(this.transform);
				}
			}
			else
			{
				if (this._state == Boss_Mechanical_Scorpion.EState.Attack_Khe)
				{
					CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
					return;
				}
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
		}
		Boss_Mechanical_Scorpion.EState state = this._state;
		if (state == Boss_Mechanical_Scorpion.EState.Attack_CauTuyet)
		{
			this.PlaySound(this.audioCauTuyet, 1f);
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string text = trackEntry.ToString();
		switch (text)
		{
		case "attack_3":
			this.skeletonAnimation.timeScale = 1f;
			this.ChangeState();
			break;
		case "attack_2-1":
		case "attack_2-3":
		case "attack_4-1":
		case "attack_4-3":
		case "stun1":
		case "stun3":
		case "idle":
			this.ChangeState();
			break;
		case "attack_4-2":
			this._countFire++;
			if (this._countFire > (this._mode + 1) * 2)
			{
				this._countFire = 0;
				this.ChangeState();
			}
			break;
		case "stun2":
			this.ChangeState();
			break;
		case "attack_2-2":
			if (this._state == Boss_Mechanical_Scorpion.EState.Attack_Lan_2)
			{
				this.PlaySound(this.audioLan2, 1f);
			}
			break;
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneBody;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneGun;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneSnowBall;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneAttack3;

	[SerializeField]
	private AttackBox attackBoxCauTuyet;

	[SerializeField]
	private AttackBox attackBoxKhe;

	[SerializeField]
	private AttackBox attackBoxLan;

	[SerializeField]
	private AudioClip audioCauTuyet;

	[SerializeField]
	private AudioClip audioKhe;

	[SerializeField]
	private AudioClip audioLan1;

	[SerializeField]
	private AudioClip audioLan2;

	[SerializeField]
	private AudioClip audioLan3;

	[SerializeField]
	private AudioClip audioShot1;

	[SerializeField]
	private AudioClip audioShot2;

	[SerializeField]
	private AudioClip audioShot3;

	[SerializeField]
	private AudioClip audioStun2;

	[SerializeField]
	private AudioClip audioDie;

	private Boss_Mechanical_Scorpion.EState _state;

	private bool _pause;

	private Spine.Animation[] anims;

	private Bone _boneBody;

	private Bone _boneGun;

	private Bone _boneSnowBall;

	private Bone _boneAttack3;

	private bool _changeState;

	private int _mode;

	private int _countFire;

	private bool _flip;

	private Vector3 _scale;

	private float _speedMove;

	private Vector3 _targetMove;

	private float _timeActiveKhe;

	private bool _isBegin;

	private Vector3 _bodyPos;

	private bool _isStun;

	private float _spineTimeScale;

	private bool _isRunAndGun;

	private int hit;

	private enum EState
	{
		_1,
		Attack_CauTuyet,
		Attack_Khe,
		Attack_Lan_1,
		Attack_Lan_2,
		Attack_Lan_3,
		Attack_Gun_1,
		Attack_Gun_2,
		Attack_Gun_3,
		Death,
		Hit,
		Idle,
		Run,
		Stun_1,
		Stun_2,
		Stun_3
	}
}
