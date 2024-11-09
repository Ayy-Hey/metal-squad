using System;
using System.Collections;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class ChoSoi : BaseEnemy
{
	private void OnValidate()
	{
		if (!this.data)
		{
			this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
		}
	}

	private void OnDisable()
	{
		this.isInit = false;
		if (this.DieCallback != null)
		{
			this.DieCallback();
		}
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (base.isInCamera && this._autoActive < 1f)
			{
				this._autoActive += deltaTime;
				if (this._autoActive >= 1f)
				{
					this.Init(12, delegate
					{
						CameraController.Instance.NewCheckpoint(true, 15f);
					});
					Log.Info("exception___box active error");
				}
			}
			if (!this._pause)
			{
				this.Pause(true);
			}
			return;
		}
		if (this._pause)
		{
			this.Pause(false);
		}
		this.OnUpdate(deltaTime);
		if (this.HP <= 0f)
		{
			this._checkStuck += deltaTime;
			if (this._checkStuck >= 5f)
			{
				UnityEngine.Debug.Log("---------------------------------------cho soi clear stuck----------------------------------");
				base.gameObject.SetActive(false);
			}
		}
	}

	public void Init(int level = 0, Action dieCallback = null)
	{
		this.DieCallback = dieCallback;
		float mode = GameMode.Instance.GetMode();
		this._level = Mathf.Clamp(level, 0, this.data.datas.Length - 1);
		this.cacheEnemy = new Enemy();
		this.cacheEnemy.HP = (this.HP = this.data.datas[this._level].hp * mode);
		this._damage = this.data.datas[this._level].damage * mode;
		base.gameObject.SetActive(true);
		this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		this._gunBone1 = this.skeletonAnimation.skeleton.FindBone(this.gunBone1);
		this._gunBone2 = this.skeletonAnimation.skeleton.FindBone(this.gunBone2);
		this._boneBoxHead = this.skeletonAnimation.skeleton.FindBone(this.boneBoxHead);
		this._boneBoxBody = this.skeletonAnimation.skeleton.FindBone(this.boneBoxBody);
		this._boneAttackLua = this.skeletonAnimation.skeleton.FindBone(this.guntipAttackLua);
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		GameManager.Instance.ListEnemy.Add(this);
		this._autoActive = 1f;
		this._state = ChoSoi.EState.Idle;
		this._changeState = false;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (this._state == ChoSoi.EState.Die)
		{
			return;
		}
		this.UpdateBox();
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
	}

	private void UpdateBox()
	{
		this._pos = this.transform.position;
		this.tfOrigin.position = this._boneBoxHead.GetWorldPosition(this.transform);
		this._offset = this.tfOrigin.position - this._pos;
		this._offset.x = ((!this._flip) ? this._offset.x : (-this._offset.x));
		this.bodyCollider2D.offset = this._offset;
		this._offset = this._boneBoxBody.GetWorldPosition(this.transform) - this._pos;
		this._offset.x = ((!this._flip) ? this._offset.x : (-this._offset.x));
		this.boxCollider.offset = this._offset;
	}

	private void StartState()
	{
		int track = 0;
		bool loop = false;
		float x = GameManager.Instance.player.transform.position.x;
		float x2 = CameraController.Instance.transform.position.x;
		float x3 = this.transform.position.x;
		if (this._state != ChoSoi.EState.Attack_Sung)
		{
			this.Flip(x3 < x);
		}
		switch (this._state)
		{
		case ChoSoi.EState.Aim_Sung:
			this.PlaySound();
			track = 2;
			this.skeletonAnimation.state.SetAnimation(0, this.anims[0], false);
			this._targetGunPoint = GameManager.Instance.player.tfOrigin.position;
			break;
		case ChoSoi.EState.Attack_Sung:
			track = 1;
			break;
		case ChoSoi.EState.Attack_KhongLo:
			this.PlaySound();
			break;
		case ChoSoi.EState.Attack_Vo:
			this._isActiveVo = false;
			this.PlaySound();
			break;
		case ChoSoi.EState.Idle:
			loop = !this._isBegin;
			break;
		case ChoSoi.EState.Run:
		{
			loop = true;
			float num = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x);
			if (num > 5f)
			{
				this._targetMove = x;
			}
			else
			{
				float num2 = UnityEngine.Random.Range(2f, 5f);
				this._targetMove = x2 + ((x2 <= x3) ? (-num2) : num2);
			}
			this.Flip(x3 < this._targetMove);
			break;
		}
		}
		this.PlayAnim(track, loop);
		this._changeState = true;
	}

	private void UpdateState(float deltaTime)
	{
		switch (this._state)
		{
		case ChoSoi.EState.Aim_Sung:
		{
			this.targetGun.position = Vector3.SmoothDamp(this.targetGun.position, this._targetGunPoint, ref this._veloMoveTargetGun, 0.2f);
			float num = Vector3.Distance(this.targetGun.position, this._targetGunPoint);
			if (num < 0.05f)
			{
				this.ChangeState();
			}
			break;
		}
		case ChoSoi.EState.Attack_Lua:
			if (this.attackBoxLua.gameObject.activeSelf)
			{
				this.attackBoxLua.transform.position = this._boneAttackLua.GetWorldPosition(this.transform);
			}
			break;
		case ChoSoi.EState.Attack_Vo:
			if (this._isActiveVo)
			{
				this.attackBoxVo.transform.position = this._boneAttackLua.GetWorldPosition(this.transform);
				float x = this.data.datas[this._level].speed * deltaTime * (float)((!this._flip) ? -1 : 1);
				this.transform.Translate(x, 0f, 0f);
			}
			break;
		case ChoSoi.EState.Idle:
			if (!this._isBegin)
			{
				float num2 = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x);
				if (num2 <= this.data.datas[this._level].maxVision)
				{
					this._isBegin = true;
					this._state = ChoSoi.EState.Attack_KhongLo;
					this._changeState = false;
				}
			}
			break;
		case ChoSoi.EState.Run:
			this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetMove, this.GetSpeed() * deltaTime);
			this.transform.position = this._pos;
			if (this._pos.x == this._targetMove)
			{
				this.ChangeState();
			}
			break;
		}
	}

	private void ChangeState()
	{
		switch (this._state)
		{
		case ChoSoi.EState.Aim_Sung:
			this._state = ChoSoi.EState.Attack_Sung;
			break;
		case ChoSoi.EState.Attack_Sung:
		case ChoSoi.EState.Attack_Lua:
		case ChoSoi.EState.Attack_Vo:
			this.RunOrIdle();
			break;
		case ChoSoi.EState.Attack_KhongLo:
			this._state = ChoSoi.EState.Aim_Sung;
			break;
		case ChoSoi.EState.Idle:
		case ChoSoi.EState.Run:
			this.ChangeAttack();
			break;
		}
		if (!this._isCcrazy && this.HP < this.cacheEnemy.HP / 2f && this.HP > 0f)
		{
			this._isCcrazy = true;
			this._state = ChoSoi.EState.Attack_KhongLo;
			UnityEngine.Debug.Log("dkm________________________" + this.HP);
		}
		this._changeState = false;
	}

	private void ChangeAttack()
	{
		float num = Mathf.Abs(this.transform.position.x - GameManager.Instance.player.transform.position.x);
		if (num <= 4f)
		{
			this._state = ((UnityEngine.Random.Range(0, 3) != 1) ? ChoSoi.EState.Attack_Lua : ChoSoi.EState.Aim_Sung);
		}
		else
		{
			this._state = ((UnityEngine.Random.Range(0, 3) != 1) ? ChoSoi.EState.Attack_Vo : ChoSoi.EState.Aim_Sung);
		}
	}

	private void RunOrIdle()
	{
		this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? ChoSoi.EState.Idle : ChoSoi.EState.Run);
	}

	private void Flip(bool flip)
	{
		this._flip = flip;
		Vector3 localScale = this.transform.localScale;
		localScale.x = ((!flip) ? Mathf.Abs(localScale.x) : (-Mathf.Abs(localScale.x)));
		this.transform.localScale = localScale;
	}

	private void PlayAnim(int track = 0, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(track, this.anims[(int)this._state], loop);
	}

	private float GetDamage()
	{
		float num = this._damage;
		ChoSoi.EState state = this._state;
		if (state == ChoSoi.EState.Attack_Lua || state == ChoSoi.EState.Attack_Vo)
		{
			num = Mathf.Round(num * 1.2f);
		}
		return num;
	}

	private float GetSpeed()
	{
		float num = this.data.datas[this._level].speed;
		ChoSoi.EState state = this._state;
		if (state == ChoSoi.EState.Attack_Sung)
		{
			num *= 1.2f;
		}
		return num;
	}

	private void Pause(bool pause)
	{
		this._pause = pause;
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
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
			this.skeletonAnimation.state.SetAnimation(3, this.anims[6], false);
			GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		}
		else
		{
			if (this._state == ChoSoi.EState.Die)
			{
				return;
			}
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.ListEnemy.Remove(this);
			this.boxCollider.enabled = false;
			this.bodyCollider2D.enabled = false;
			this.skeletonAnimation.state.SetEmptyAnimations(0f);
			this._state = ChoSoi.EState.Die;
			this.PlayAnim(2, false);
			base.StartCoroutine(this.Die());
		}
	}

	private IEnumerator Die()
	{
		Vector3 effPos = this._gunBone2.GetWorldPosition(this.transform);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, effPos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		effPos = this._boneBoxHead.GetWorldPosition(this.transform);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, effPos, Vector3.one);
		yield return new WaitForSeconds(0.2f);
		effPos = this._boneBoxBody.GetWorldPosition(this.transform);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, effPos, Vector3.one);
		CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
		GameManager.Instance.hudManager.LineBlood.gameObject.SetActive(false);
		yield break;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		switch (this._state)
		{
		case ChoSoi.EState.Attack_Sung:
		{
			Vector3 worldPosition = this._gunBone1.GetWorldPosition(this.transform);
			Vector3 v = worldPosition - this._gunBone2.GetWorldPosition(this.transform);
			this.PlaySound();
			if (this._isCcrazy)
			{
				BulletEnemy bulletEnemy = GameManager.Instance.bulletManager.CreateBulletEnemy(8, v, worldPosition, this.GetDamage(), this.GetSpeed(), 0f);
				bulletEnemy.spriteRenderer.flipX = false;
				bulletEnemy.useByBoss = true;
			}
			else
			{
				BulletEnemy bulletEnemy2 = GameManager.Instance.bulletManager.CreateBulletEnemy(7, v, worldPosition, this.GetDamage(), this.GetSpeed(), 0f);
				bulletEnemy2.spriteRenderer.flipX = false;
				bulletEnemy2.useByBoss = true;
			}
			break;
		}
		case ChoSoi.EState.Attack_Lua:
			this.attackBoxLua.Active(this.GetDamage(), true, null);
			this.particleAttackLua.Play();
			this.PlaySound();
			break;
		case ChoSoi.EState.Attack_KhongLo:
			if (this._isCcrazy)
			{
				this.transform.localScale = this.transform.localScale * 1.2f;
				this.skeletonAnimation.skeleton.SetColor(new Color(1f, 0.75f, 0.75f));
			}
			break;
		case ChoSoi.EState.Attack_Vo:
		{
			string name = e.Data.Name;
			if (name != null)
			{
				if (!(name == "nhay1"))
				{
					if (name == "nhay2")
					{
						this._isActiveVo = false;
						this.attackBoxVo.Deactive();
					}
				}
				else
				{
					this._isActiveVo = true;
					this.attackBoxVo.Active(this.GetDamage(), true, null);
				}
			}
			break;
		}
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string name = trackEntry.Animation.Name;
		if (name != null)
		{
			if (!(name == "attack-2-lua"))
			{
				if (!(name == "attack-3-pow"))
				{
					if (!(name == "attack-4-cao") && !(name == "idel"))
					{
						if (!(name == "attack-1-sung"))
						{
							if (name == "die")
							{
								base.StopAllCoroutines();
								base.gameObject.SetActive(false);
							}
						}
						else
						{
							this.skeletonAnimation.state.SetEmptyAnimations(0f);
							this.ChangeState();
						}
					}
					else
					{
						this.ChangeState();
					}
				}
				else
				{
					SingletonGame<AudioController>.Instance.StopAll();
					this.ChangeState();
				}
			}
			else
			{
				this.attackBoxLua.Deactive();
				this.ChangeState();
			}
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private AttackBox attackBoxLua;

	[SerializeField]
	private AttackBox attackBoxVo;

	[SerializeField]
	private Transform targetGun;

	[SerializeField]
	private ParticleSystem particleAttackLua;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string gunBone1;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string gunBone2;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneBoxHead;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneBoxBody;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string guntipAttackLua;

	[SerializeField]
	private AudioClip[] audioClips;

	private Action DieCallback;

	private bool _pause;

	private bool _flip;

	private Spine.Animation[] anims;

	private ChoSoi.EState _state;

	private bool _changeState;

	private Bone _gunBone1;

	private Bone _gunBone2;

	private Bone _boneBoxHead;

	private Bone _boneBoxBody;

	private Bone _boneAttackLua;

	private bool _isBegin;

	private bool _isCcrazy;

	private float _targetMove;

	private int _level;

	private int _mode;

	private float _damage;

	private Vector3 _targetGunPoint;

	private Vector3 _veloMoveTargetGun;

	private Vector3 _pos;

	private bool _isActiveVo;

	private Vector2 _offset;

	private float _autoActive;

	private float _checkStuck;

	private enum EState
	{
		Aim_Sung,
		Attack_Sung,
		Attack_Lua,
		Attack_KhongLo,
		Attack_Vo,
		Die,
		Hit,
		Idle,
		Run
	}
}
