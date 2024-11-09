using System;
using System.Collections;
using MyDataLoader;
using PVPManager;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BaseEnemy2 : BaseEnemy, IIce
{
	protected bool Flip
	{
		get
		{
			return this.skeletonAnimation.skeleton.FlipX;
		}
		set
		{
			this.skeletonAnimation.skeleton.FlipX = value;
		}
	}

	protected float Gravity
	{
		get
		{
			if (this.rigidbody2D)
			{
				return this.rigidbody2D.gravityScale;
			}
			return 0f;
		}
		set
		{
			if (this.rigidbody2D)
			{
				this.rigidbody2D.gravityScale = value;
			}
		}
	}

	private void OnDisable()
	{
		try
		{
			this.Disable();
			this.isInit = false;
			this.skeletonAnimation.state.Event -= this.EventHld;
			this.skeletonAnimation.state.Complete -= this.CompleteHld;
			GameManager.Instance.ListEnemy.Remove(this);
			this.hideCallback();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public virtual void Init(EnemyDataInfo enemyDataInfo, Action hideCallback)
	{
		base.gameObject.SetActive(true);
		this.skeletonAnimation.SkeletonDataAsset.GetSkeletonData(true);
		if (this.cacheEnemy == null)
		{
			this.cacheEnemy = new Enemy();
		}
		if (this.anims == null)
		{
			SkeletonData skeletonData = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData;
			this.anims = skeletonData.Animations.Items;
			this.skins = skeletonData.Skins.Items;
		}
		this.skeletonAnimation.AnimationState.Event += this.EventHld;
		this.skeletonAnimation.AnimationState.Complete += this.CompleteHld;
		if (!this.useDefaultSkin)
		{
			this.SetSkin();
		}
		this.cacheEnemyData = enemyDataInfo;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		this.modeLv = (int)GameMode.Instance.EMode;
		this.TimeCounterUpgradeEnemy = this.GetTimeUpgradeModeLv();
		float mode = GameMode.Instance.GetMode();
		this.cacheEnemyData.level = Mathf.Min(this.cacheEnemyData.level, this.data.datas.Length - 1);
		this.cacheEnemy.HP = this.data.datas[this.cacheEnemyData.level].hp;
		this.cacheEnemy.Damage = this.data.datas[this.cacheEnemyData.level].damage * mode;
		this.cacheEnemy.Speed = this.data.datas[this.cacheEnemyData.level].speed;
		this.cacheEnemy.Vision_Max = this.data.datas[this.cacheEnemyData.level].maxVision;
		this.cacheEnemy.Time_Reload_Attack = this.data.datas[this.cacheEnemyData.level].timeReload;
		this.HP = this.cacheEnemy.HP * mode;
		this.hideCallback = hideCallback;
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.gameObject.SetActive(true);
			this.lineBloodEnemy.Reset();
		}
		Vector3 position = enemyDataInfo.Vt2;
		this.transform.position = position;
		this.pos = position;
		this.Flip = (this.pos.x > CameraController.Instance.camPos.x);
		this.meshRenderer.sortingOrder = 6;
		this.Gravity = 2f;
		this.oldPosX = this.pos.x;
		this.SetEmptyAnims(0f);
		this.bodyCollider2D.enabled = true;
		this.OnEnemyDeaded = null;
		this.isCreateWithJson = false;
		this.coolDownChemical = 0f;
		this.timeCheckStuck = 0f;
		this.coolDownHide = 5f;
		this.isChangeState = false;
		this.attackCount = 0;
		this.timeScale = 1f;
		this.ResetTimeReload();
		this.canAttack = false;
		this.hasShow = false;
		this.isAttack = false;
		this.isMove = false;
		this.pause = false;
		this.isDie = false;
		base.StartCoroutine(this.OnInit());
	}

	private IEnumerator OnInit()
	{
		yield return this.waitInit;
		this.isInit = true;
		yield break;
	}

	private void SetSkin()
	{
		int num = 1;
		GameMode.GameStyle style = GameMode.Instance.Style;
		if (style != GameMode.GameStyle.SinglPlayer)
		{
			if (style == GameMode.GameStyle.MultiPlayer)
			{
				num = PVPManager.PVPManager.Instance.currentMapIdx;
			}
		}
		else
		{
			GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
			if (modePlay != GameMode.ModePlay.Campaign)
			{
				if (modePlay != GameMode.ModePlay.Endless)
				{
				}
			}
			else
			{
				num = (int)((int)ProfileManager.eLevelCurrent / (int)global::ELevel.LEVEL_13 + 1);
			}
		}
		num = Mathf.Clamp(num, 1, this.skins.Length - 1);
		if (this.skin != num)
		{
			this.skin = num;
			this.skeletonAnimation.Skeleton.SetSkin(this.skins[this.skin]);
		}
	}

	private void EventHld(TrackEntry trackEntry, Spine.Event e)
	{
		this.OnEvent(trackEntry, e);
	}

	private void CompleteHld(TrackEntry trackEntry)
	{
		this.OnComplete(trackEntry);
	}

	protected virtual void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
	}

	protected virtual void OnComplete(TrackEntry trackEntry)
	{
	}

	public virtual void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.isDie)
		{
			if (this.timeCheckStuck < 4f)
			{
				this.timeCheckStuck += deltaTime;
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		this.pos = this.rigidbody2D.position;
		this.oldX = this.pos.x;
		if (!this.skipAutoHide && !base.isInCamera)
		{
			bool flag = false;
			CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
			if (orientaltion != CameraController.Orientation.HORIZONTAL)
			{
				if (orientaltion == CameraController.Orientation.VERTICAL)
				{
					flag = ((!CameraController.Instance.isVerticalDown) ? (CameraController.Instance.camPos.y - CameraController.Instance.Size().y - 2f > this.pos.y) : (CameraController.Instance.camPos.y + CameraController.Instance.Size().y + 2f < this.pos.y));
				}
			}
			else
			{
				flag = (CameraController.Instance.LeftCamera() > this.pos.x);
			}
			if (flag)
			{
				this.coolDownHide -= deltaTime;
			}
			if (this.coolDownHide <= 0f)
			{
				base.gameObject.SetActive(false);
				if (!object.ReferenceEquals(DataLoader.LevelDataCurrent, null) && this.isCreateWithJson)
				{
					DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy--;
				}
				return;
			}
		}
		if (!this.skipCheckRigidbody)
		{
			base.CheckWithCamera();
		}
		else
		{
			this.CheckRenderer();
		}
		if (!this.canAttack && base.isInCamera)
		{
			this.canAttack = (Mathf.Abs(this.pos.x - GameManager.Instance.player.transform.position.x) <= this.cacheEnemy.Vision_Max);
		}
		if (!this.isChangeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		if (base.isInCamera && this.TimeCounterUpgradeEnemy > 0f)
		{
			this.TimeCounterUpgradeEnemy -= deltaTime;
			if (this.TimeCounterUpgradeEnemy < 0f)
			{
				this.modeLv++;
				this.TimeCounterUpgradeEnemy = this.GetTimeUpgradeModeLv();
				this.OnPowerUp();
			}
		}
		if (this.timeReload > 0f)
		{
			this.timeReload -= deltaTime;
		}
		if (this.canAttack && !this.isAttack && this.timeReload <= 0f)
		{
			if (GameManager.Instance.player.isInVisible)
			{
				this.ResetTimeReload();
			}
			else
			{
				this.OnAttack();
			}
		}
		if (this.coolDownChemical > 0f)
		{
			this.coolDownChemical -= deltaTime;
			if (this.coolDownChemical <= 0f)
			{
				this.coolDownChemical = 0f;
				this.cacheEnemy.Speed *= 2f;
			}
		}
		if (this.isMove)
		{
			this.timeCheckStuck += deltaTime;
			if (this.timeCheckStuck > 1f)
			{
				bool flag2 = Mathf.Abs(this.pos.x - this.oldPosX) < 0.2f;
				if (flag2)
				{
					this.OnStuckMove();
				}
				this.oldPosX = this.pos.x;
				this.timeCheckStuck = 0f;
			}
		}
		this.pos.x = this.oldX + (this.pos.x - this.oldX) / this.physicSlow;
		this.rigidbody2D.position = this.pos;
	}

	protected virtual void StartState()
	{
		this.isChangeState = true;
	}

	protected virtual void UpdateState(float deltaTime)
	{
	}

	protected virtual void OnStuckMove()
	{
	}

	protected virtual void ChangeState()
	{
		this.isChangeState = false;
	}

	protected virtual void OnPowerUp()
	{
	}

	protected virtual void OnAttack()
	{
		this.isAttack = true;
	}

	private float GetTimeUpgradeModeLv()
	{
		return (this.modeLv >= 2) ? 0f : (15f + (float)(this.modeLv * 5));
	}

	protected void ResetTimeReload()
	{
		this.isAttack = false;
		this.timeReload = this.cacheEnemy.Time_Reload_Attack - 0.25f * (float)this.modeLv * this.cacheEnemy.Time_Reload_Attack;
	}

	protected void PlayAnim(int idAnim, int track = 0, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(track, this.anims[idAnim], loop);
	}

	protected void AddAnim(int idAnim, int track = 0, bool loop = false, float timeDelay = 0f)
	{
		this.skeletonAnimation.state.AddAnimation(track, this.anims[idAnim], loop, timeDelay);
	}

	protected void SetEmptyAnim(int track, float duration = 0f)
	{
		this.skeletonAnimation.state.SetEmptyAnimation(track, duration);
	}

	protected void SetEmptyAnims(float duration = 0f)
	{
		this.skeletonAnimation.state.SetEmptyAnimations(duration);
	}

	protected virtual void Hit()
	{
		if (this.lastWeapon == EWeapon.GRENADE_CHEMICAL)
		{
			if (this.coolDownChemical <= 0f)
			{
				this.cacheEnemy.Speed /= 2f;
			}
			this.coolDownChemical = 3f;
		}
	}

	protected virtual void Die(bool isRambo)
	{
		this.isDie = true;
		this.timeCheckStuck = 0f;
		if (this.bodyCollider2D)
		{
			this.bodyCollider2D.enabled = false;
		}
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
		base.CalculatorToDie(isRambo, false);
		if (this.lastWeapon == EWeapon.FLAME || this.lastWeapon == EWeapon.GRENADE_MOLOYOV)
		{
			GameManager.Instance.fxManager.CreateFxFlame01(0, this.transform, 1f);
		}
		GameManager.Instance.ListEnemy.Remove(this);
	}

	public override void Hit(float damage)
	{
		base.Hit(damage);
		if (this.HP >= 0f)
		{
			this.Hit();
			return;
		}
		if (this.isDie)
		{
			return;
		}
		this.Die(true);
	}

	public void Hit(float damaged, EWeapon weapon)
	{
		base.AddHealthPoint(damaged, weapon);
	}

	public virtual void Pause(bool pause)
	{
		this.pause = pause;
		if (pause)
		{
			this.timeScale = this.skeletonAnimation.timeScale;
			this.skeletonAnimation.timeScale = 0f;
		}
		else
		{
			this.skeletonAnimation.timeScale = this.timeScale;
		}
	}

	private void CheckRenderer()
	{
		if (base.isInCamera)
		{
			return;
		}
		bool flag = false;
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.HORIZONTAL)
		{
			if (orientaltion == CameraController.Orientation.VERTICAL)
			{
				if (CameraController.Instance.isVerticalDown)
				{
					flag = (CameraController.Instance.camPos.y - this.pos.y <= CameraController.Instance.Size().y + 2f);
				}
				else
				{
					flag = (this.pos.y - CameraController.Instance.camPos.y <= CameraController.Instance.Size().y + 2f);
				}
			}
		}
		else
		{
			flag = (this.pos.x - CameraController.Instance.camPos.x <= CameraController.Instance.Size().x + 2f);
		}
		if (this.meshRenderer && this.meshRenderer.enabled != flag)
		{
			this.meshRenderer.enabled = flag;
		}
	}

	protected virtual void Disable()
	{
	}

	[HideInInspector]
	public bool isTrucker;

	[SerializeField]
	protected DataEVL data;

	[SerializeField]
	protected bool skipAutoHide;

	[SerializeField]
	protected bool skipCheckRigidbody;

	[SerializeField]
	protected bool useDefaultSkin;

	public SkeletonAnimation skeletonAnimation;

	protected Action hideCallback;

	protected int modeLv;

	protected Spine.Animation[] anims;

	protected Skin[] skins;

	protected int skin;

	protected bool isChangeState;

	protected bool isDie;

	protected float timeScale;

	protected bool pause;

	protected float coolDownHide;

	protected bool canAttack;

	protected float timeReload;

	protected Vector3 pos;

	protected bool isMove;

	protected bool isAttack;

	protected int attackCount;

	protected float coolDownChemical;

	protected float oldPosX;

	protected float timeCheckStuck;

	protected const float gravityDefault = 2f;

	protected const float jumpDistance = 2.5f;

	protected float physicSlow = 1.5f;

	private WaitForSeconds waitInit = new WaitForSeconds(0.1f);

	private float oldX;
}
