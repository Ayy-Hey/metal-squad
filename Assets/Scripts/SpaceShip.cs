using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SpaceShip : BaseEnemy
{
	private void OnValidate()
	{
		if (!this.skeletonAnimation)
		{
			this.skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
			this.skeletonAnimation.gameObject.AddComponent<SpriteVisible>();
		}
		if (!this.data)
		{
			this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			GameManager.Instance.ListEnemy.Remove(this);
			this.callbackDie(this);
		}
		catch
		{
		}
	}

	private void InitSpine()
	{
		if (this._isInitSpine)
		{
			return;
		}
		this._isInitSpine = true;
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this._boneOrigin = this.skeletonAnimation.skeleton.FindBone(this.boneOrigin);
		this._boneGunDirector = this.skeletonAnimation.skeleton.FindBone(this.boneGunDirector);
		this._boneGun0 = this.skeletonAnimation.skeleton.FindBone(this.boneGun0);
		this._boneGun1 = this.skeletonAnimation.skeleton.FindBone(this.boneGun1);
		this._boneBulletLaser = this.skeletonAnimation.skeleton.FindBone(this.boneBulletLaser);
		this._boneBulletFlash = this.skeletonAnimation.skeleton.FindBone(this.boneBulletFlash);
		this._boneFxFire = this.skeletonAnimation.skeleton.FindBone("target-tay-o");
		this._boneBlood = this.skeletonAnimation.skeleton.FindBone(this.boneBlood);
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
	}

	public void Init(Action<SpaceShip> callbackDie, bool isMiniBoss = true)
	{
		this._isMiniBoss = isMiniBoss;
		this.callbackDie = callbackDie;
		if (this.cacheEnemy == null)
		{
			this.cacheEnemy = new Enemy();
		}
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		float mode = GameMode.Instance.GetMode();
		this.cacheEnemy.HP = (this.HP = this.data.datas[this.cacheEnemyData.level].hp * mode);
		this.cacheEnemy.Speed = this.data.datas[this.cacheEnemyData.level].speed;
		this.cacheEnemy.Damage = this.data.datas[this.cacheEnemyData.level].damage * mode;
		this.cacheEnemy.Time_Reload_Attack = this.data.datas[this.cacheEnemyData.level].timeReload;
		this._mode = (int)GameMode.Instance.EMode;
		this.TimeCounterUpgradeEnemy = (float)((this._mode != 2) ? (this._mode * 15 + 15) : 0);
		this._timeReloadAttack = 0.5f;
		this._coolDownHide = 8f;
		this.tfFxFire.gameObject.SetActive(false);
		base.gameObject.SetActive(true);
		this.bodyCollider2D.enabled = true;
		this.transform.localScale = ((!isMiniBoss) ? (Vector3.one * 0.6f) : Vector3.one);
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		this.InitSpine();
		this.skeletonAnimation.skeleton.FlipX = (this.cacheEnemyData.pos_x > CameraController.Instance.camPos.x);
		this._targetPos = this.cacheEnemyData.Vt2;
		this.transform.position = this._targetPos;
		this.GetStartPos();
		this._state = SpaceShip.EState.Start_1;
		this._changeState = false;
		this.isInit = true;
	}

	internal void Pause(bool pause)
	{
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.TimeCounterUpgradeEnemy > 0f)
		{
			this.TimeCounterUpgradeEnemy -= deltaTime;
			if (this.TimeCounterUpgradeEnemy <= 0f)
			{
				this._mode += ((this._mode >= 2) ? 0 : 1);
				this.TimeCounterUpgradeEnemy = (float)((this._mode >= 2) ? 0 : (this._mode * 15 + 15));
			}
		}
		this.UpdateOriginAndBox();
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
	}

	private void StartState()
	{
		this._changeState = true;
		switch (this._state)
		{
		case SpaceShip.EState.Aim_MachineGun:
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > GameManager.Instance.player.transform.position.x);
			this._targetPos = this._boneGun0.GetWorldPosition(this.transform);
			this._targetLock = GameManager.Instance.player.tfOrigin.position;
			this._boneGunDirector.SetPositionSkeletonSpace(this._targetPos - this.transform.position);
			this.PlayAnim(2, false);
			break;
		case SpaceShip.EState.Attack_Laser:
		case SpaceShip.EState.Attack_Flash:
			this._oldAttack = this._state;
			this._isAttack = false;
			this._attackDone = false;
			this._oldPosY = this.transform.position.y;
			if (GameManager.Instance.player.IsGround)
			{
				this._targetLock = GameManager.Instance.player.transform.position;
			}
			else
			{
				RaycastHit2D raycastHit2D = Physics2D.Raycast(this.transform.position, Vector2.down, 10f, this.maskGround);
				if (raycastHit2D.collider)
				{
					this._targetLock = raycastHit2D.point;
				}
				else
				{
					this._targetPos = CameraController.Instance.camPos;
					this._targetPos.z = 0f;
				}
			}
			this._targetLock.y = Mathf.Max(this._targetLock.y, CameraController.Instance.camPos.y - 2f);
			this.skeletonAnimation.skeleton.FlipX = (this._targetLock.x < this.transform.position.x);
			break;
		case SpaceShip.EState.Attack_MachineGun:
			this._oldAttack = this._state;
			this.PlayAnim(1, false);
			break;
		case SpaceShip.EState.Attack_Bomb:
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > CameraController.Instance.camPos.x);
			this._oldAttack = this._state;
			this._timeReloadBomb = 0f;
			this.PlayAnim(1, false);
			break;
		case SpaceShip.EState.Die:
			this.PlayAnim(0, false);
			this.tfFxFire.position = this._boneFxFire.GetWorldPosition(this.transform);
			this.tfFxFire.gameObject.SetActive(true);
			break;
		case SpaceShip.EState.Idle:
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > CameraController.Instance.camPos.x);
			this.PlayAnim(0, true);
			break;
		case SpaceShip.EState.Start_1:
			this._targetPos = this.transform.position;
			this.GetStartPos();
			this.skeletonAnimation.skeleton.FlipX = (this._targetPos.x > CameraController.Instance.camPos.x);
			this.PlayAnim(0, false);
			break;
		case SpaceShip.EState.Start_2:
			this.PlayAnim(0, false);
			this.transform.position = this._targetPos;
			break;
		case SpaceShip.EState.Run:
		{
			this._targetLock = this.transform.position;
			float num = UnityEngine.Random.Range(1f, CameraController.Instance.Size().x);
			this._targetLock.x = ((this._targetLock.x >= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - num) : (CameraController.Instance.camPos.x + num));
			this._targetLock.y = UnityEngine.Random.Range(CameraController.Instance.camPos.y, CameraController.Instance.camPos.y + 2.5f);
			this.skeletonAnimation.skeleton.FlipX = (this._targetLock.x < this.transform.position.x);
			break;
		}
		}
	}

	private void UpdateState(float deltaTime)
	{
		switch (this._state)
		{
		case SpaceShip.EState.Aim_MachineGun:
		{
			this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLock, ref this._veloMove, 0.3f);
			this._boneGunDirector.SetPositionSkeletonSpace(this._targetPos - this.transform.position);
			bool flag = Vector3.Distance(this._targetPos, this._targetLock) < 0.1f;
			if (flag)
			{
				this.ChangeState();
			}
			break;
		}
		case SpaceShip.EState.Attack_Laser:
		case SpaceShip.EState.Attack_Flash:
			if (!this._isAttack)
			{
				this._targetPos = this.transform.position;
				this._targetPos.y = Mathf.SmoothDamp(this._targetPos.y, this._targetLock.y, ref this._veloMove.y, 0.5f);
				this.transform.position = this._targetPos;
				if (Mathf.Abs(this._targetPos.y - this._targetLock.y) < 0.1f)
				{
					this._isAttack = true;
					this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > GameManager.Instance.player.transform.position.x);
					this.PlayAnim(1, false);
				}
			}
			if (this._attackDone)
			{
				this._targetPos = this.transform.position;
				this._targetPos.y = Mathf.SmoothDamp(this._targetPos.y, this._oldPosY, ref this._veloMove.y, 0.5f);
				this.transform.position = this._targetPos;
				if (Mathf.Abs(this._targetPos.y - this._oldPosY) < 0.1f)
				{
					this._attackDone = false;
					this.ChangeState();
				}
			}
			break;
		case SpaceShip.EState.Attack_MachineGun:
			this._targetLock = GameManager.Instance.player.tfOrigin.position;
			this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLock, ref this._veloMove, 0.2f);
			this._boneGunDirector.SetPositionSkeletonSpace(this._targetPos - this.transform.position);
			break;
		case SpaceShip.EState.Attack_Bomb:
		{
			this._targetPos = this.transform.position;
			this._targetPos.x = this._targetPos.x + ((!this.skeletonAnimation.skeleton.FlipX) ? (this.cacheEnemy.Speed * 2f * deltaTime) : (-this.cacheEnemy.Speed * 2f * deltaTime));
			this.transform.position = this._targetPos;
			bool flag = Mathf.Abs(this._targetPos.x - CameraController.Instance.camPos.x) > CameraController.Instance.Size().x - 1f;
			if (!base.isInCamera)
			{
				this.skeletonAnimation.state.SetEmptyAnimations(0f);
				this.ChangeState();
				return;
			}
			this._timeReloadBomb -= deltaTime;
			if (this._timeReloadBomb <= 0f)
			{
				this._timeReloadBomb = this.timeReloadBomb - this.timeReloadBomb * (float)this._mode / 3f;
				float damage = Mathf.Round(this.cacheEnemy.Damage * 1.2f);
				GameManager.Instance.bombManager.CreateBombCicle1(damage, 1f, this.transform.position, (!this._isMiniBoss) ? 0.6f : 1f);
			}
			break;
		}
		case SpaceShip.EState.Die:
			this.transform.Translate(0f, -2f * deltaTime, 0f);
			this.tfFxFire.position = this._boneFxFire.GetWorldPosition(this.transform);
			if (this.transform.position.y < CameraController.Instance.camPos.y - 3f)
			{
				GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.tfOrigin.position, Vector3.one);
				base.gameObject.SetActive(false);
			}
			break;
		case SpaceShip.EState.Idle:
		{
			this._timeReloadAttack -= deltaTime;
			bool flag = this._timeReloadAttack <= 0f || !base.isInCamera;
			if (flag)
			{
				this.ChangeState();
				this._timeReloadAttack = this.cacheEnemy.Time_Reload_Attack - this.cacheEnemy.Time_Reload_Attack * (float)this._mode * 0.25f;
			}
			break;
		}
		case SpaceShip.EState.Start_1:
			this._delayStart1 += deltaTime;
			if (this._delayStart1 > 0.2f)
			{
				this._delayStart1 = 0f;
				this.ChangeState();
			}
			break;
		case SpaceShip.EState.Run:
		{
			this._targetPos = this.transform.position;
			this._targetPos = Vector3.SmoothDamp(this._targetPos, this._targetLock, ref this._veloMove, 1f);
			this.transform.position = this._targetPos;
			bool flag = Vector3.Distance(this._targetPos, this._targetLock) < 0.1f;
			if (flag)
			{
				this.ChangeState();
			}
			break;
		}
		}
	}

	private void UpdateOriginAndBox()
	{
		this._originPos = this._boneOrigin.GetWorldPosition(this.transform);
		this.tfTarget[0].position = this._originPos;
		this._originPos.x = this._originPos.x - ((!this._isMiniBoss) ? 0.5f : 0.8f);
		this.tfTarget[1].position = this._originPos;
		this._originPos.x = this._originPos.x + ((!this._isMiniBoss) ? 1f : 1.8f);
		this.tfTarget[2].position = this._originPos;
		this.bodyCollider2D.offset = this.tfTarget[0].localPosition;
		if (this.attackFlash.isAttack)
		{
			this.attackFlash.transform.position = this._boneBulletFlash.GetWorldPosition(this.transform);
		}
		Vector3 worldPosition = this._boneBlood.GetWorldPosition(this.transform);
		worldPosition.y += 0.5f;
		this.lineBloodEnemy.transform.position = worldPosition;
	}

	private void PlayAnim(int track, bool loop)
	{
		this.PlayAnim(track, loop, this._state);
	}

	private void PlayAnim(int track, bool loop, SpaceShip.EState state)
	{
		this.skeletonAnimation.state.SetAnimation(track, this.anims[(int)state], loop);
	}

	private void ChangeState()
	{
		if (!base.isInCamera && this._state != SpaceShip.EState.Start_1)
		{
			this._state = SpaceShip.EState.Start_1;
			this._changeState = false;
			return;
		}
		switch (this._state)
		{
		case SpaceShip.EState.Aim_MachineGun:
			this._state = SpaceShip.EState.Attack_MachineGun;
			break;
		case SpaceShip.EState.Attack_Laser:
		case SpaceShip.EState.Attack_MachineGun:
		case SpaceShip.EState.Attack_Flash:
		case SpaceShip.EState.Attack_Bomb:
		{
			int num = UnityEngine.Random.Range(0, 5);
			if (num < this._mode)
			{
				this.ChangeStateToAttack();
			}
			else
			{
				this._state = ((num >= 4) ? SpaceShip.EState.Idle : SpaceShip.EState.Run);
			}
			break;
		}
		case SpaceShip.EState.Idle:
		case SpaceShip.EState.Run:
			this.ChangeStateToAttack();
			break;
		case SpaceShip.EState.Start_1:
			this._state = SpaceShip.EState.Start_2;
			break;
		case SpaceShip.EState.Start_2:
			this.PlayAnim(0, true, SpaceShip.EState.Idle);
			this.ChangeStateToAttack();
			break;
		}
		this._changeState = false;
	}

	private void ChangeStateToAttack()
	{
		switch (this._oldAttack)
		{
		case SpaceShip.EState.Attack_Laser:
			this._state = SpaceShip.EState.Attack_Bomb;
			return;
		case SpaceShip.EState.Attack_MachineGun:
			this._state = SpaceShip.EState.Attack_Laser;
			return;
		case SpaceShip.EState.Attack_Bomb:
			this._state = SpaceShip.EState.Attack_Flash;
			return;
		}
		this._state = SpaceShip.EState.Aim_MachineGun;
	}

	private void GetStartPos()
	{
		this._targetPos.x = ((this._targetPos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - CameraController.Instance.Size().x + 2f) : (CameraController.Instance.camPos.x + CameraController.Instance.Size().x - 2f));
		this._targetPos.y = UnityEngine.Random.Range(CameraController.Instance.camPos.y, CameraController.Instance.camPos.y + 2.5f);
	}

	public override void Hit(float damage)
	{
		base.Hit(damage);
		if (this.HP <= 0f)
		{
			this.Die();
			return;
		}
		this.skeletonAnimation.state.SetAnimation(3, this.anims[6], false);
	}

	private void Die()
	{
		this._state = SpaceShip.EState.Die;
		this.bodyCollider2D.enabled = false;
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		base.CalculatorToDie(true, false);
		this._changeState = false;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string text = e.ToString();
		if (text != null)
		{
			if (!(text == "attack-1"))
			{
				if (!(text == "attack-one"))
				{
					if (!(text == "attack-off"))
					{
						if (text == "attack-2")
						{
							float damage = Mathf.Round(this.cacheEnemy.Damage * 1.2f);
							this.attackFlash.gameObject.transform.position = this._boneBulletFlash.GetWorldPosition(this.transform);
							this.attackFlash.Active(damage, true, null);
						}
					}
					else
					{
						this.attackLaser.Deactive();
					}
				}
				else
				{
					this.attackLaser.gameObject.transform.position = this._boneBulletLaser.GetWorldPosition(this.transform);
					float damage = Mathf.Round(this.cacheEnemy.Damage * 1.5f);
					this.attackLaser.Active(damage, true, null);
				}
			}
			else
			{
				Vector3 worldPosition = this._boneGun0.GetWorldPosition(this.transform);
				Vector3 v = worldPosition - this._boneGun1.GetWorldPosition(this.transform);
				GameManager.Instance.bulletManager.CreateBulletEnemy(8, v, worldPosition, this.cacheEnemy.Damage, this.cacheEnemy.Speed * 2f, 0f).spriteRenderer.flipX = false;
			}
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "attack-3"))
			{
				if (!(text == "attack-1"))
				{
					if (!(text == "attack-2"))
					{
						if (!(text == "attack"))
						{
							if (text == "start-2")
							{
								this.ChangeState();
							}
						}
						else
						{
							this._attackDone = true;
						}
					}
					else
					{
						this.attackFlash.Deactive();
						this._attackDone = true;
					}
				}
				else
				{
					this._countBullet++;
					bool flag = this._countBullet >= this._mode * 2 + 2;
					if (flag)
					{
						this._countBullet = 0;
						this.skeletonAnimation.state.SetEmptyAnimation(2, 0.2f);
						this.ChangeState();
					}
					else
					{
						this.PlayAnim(1, false);
					}
				}
			}
			else if (this._state == SpaceShip.EState.Attack_Bomb)
			{
				this.PlayAnim(1, false);
			}
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneOrigin;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneGunDirector;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneGun0;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneGun1;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneBulletLaser;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneBulletFlash;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneBlood;

	[SerializeField]
	private LayerMask maskGround;

	[SerializeField]
	private AttackBox attackLaser;

	[SerializeField]
	private AttackBox attackFlash;

	[SerializeField]
	private Transform tfFxFire;

	[SerializeField]
	private float timeReloadBomb = 0.5f;

	private bool _isMiniBoss;

	private Action<SpaceShip> callbackDie;

	private SpaceShip.EState _state;

	private SpaceShip.EState _oldAttack;

	private bool _changeState;

	private Spine.Animation[] anims;

	private Bone _boneOrigin;

	private Bone _boneGunDirector;

	private Bone _boneGun0;

	private Bone _boneGun1;

	private Bone _boneBulletLaser;

	private Bone _boneBulletFlash;

	private Bone _boneFxFire;

	private Bone _boneBlood;

	private bool _isInitSpine;

	private int _mode;

	private Vector3 _targetPos;

	private Vector3 _targetLock;

	private float _oldPosY;

	private Vector3 _veloMove;

	private float _timeReloadBomb;

	private bool _isAttack;

	private bool _attackDone;

	private float _coolDownHide;

	private int _countBullet;

	private float _timeReloadAttack;

	private Vector3 _originPos;

	private float _delayStart1;

	private enum EState
	{
		Aim_MachineGun,
		Attack_Laser,
		Attack_MachineGun,
		Attack_Flash,
		Attack_Bomb,
		Die,
		Hit,
		Hit2,
		Idle,
		Start_1,
		Start_2,
		Run
	}
}
