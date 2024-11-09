using System;
using Spine;
using UnityEngine;

public class HelicopterAttack : BaseEnemySpine
{
	private void Update()
	{
		this.UpdateObject();
	}

	private void OnDisable()
	{
		try
		{
			EnemyManager.Instance.PoolHelicopter.Store(this);
			GameManager.Instance.ListEnemy.Remove(this);
			CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
			GameManager.Instance.CountEnemyDie++;
		}
		catch
		{
		}
		this.skeletonAnimation.state.Event -= this.HandleEvent;
		this.skeletonAnimation.state.Complete -= this.HandleComplete;
		this.isInit = false;
		this.State = ECharactor.NONE;
	}

	public void InitEnemy(int level)
	{
		base.InitEnemy(DataLoader.maybay[0], level);
		this.GunTip1 = this.skeletonAnimation.skeleton.FindBone("GunTip01");
		this.GunTip1_02 = this.skeletonAnimation.skeleton.FindBone("GunTip01_02");
		this.GunTip2 = this.skeletonAnimation.skeleton.FindBone("GunTip02");
		this.GunTip2_02 = this.skeletonAnimation.skeleton.FindBone("GunTip02_02");
		this.GunTipBlood = this.skeletonAnimation.skeleton.FindBone("Blood_Tip");
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.HP = this.cacheEnemy.HP * GameMode.Instance.GetMode();
		this.Damaged = this.cacheEnemy.Damage * GameMode.Instance.GetMode();
		this.Speed = this.cacheEnemy.Speed * Mathf.Min(GameMode.Instance.GetMode(), 2f);
		base.SetRun();
		this.step = UnityEngine.Random.Range(0, 2);
		this.timeReloadAttack = Time.timeSinceLevelLoad;
		this.State = ECharactor.RUN;
		GameManager.Instance.audioManager.Maybay1();
		this.lastWeapon = EWeapon.NONE;
		this.isInit = true;
		this.transform.parent = CameraController.Instance.transform;
	}

	public override void UpdateObject()
	{
		if (this.State == ECharactor.DIE || !this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.AI();
		ECharactor state = this.State;
		if (state != ECharactor.RUN)
		{
			if (state == ECharactor.IDLE)
			{
				this.skeletonAnimation.skeleton.FlipX = (this.step == 0);
				this.transform.localRotation = Quaternion.RotateTowards(this.transform.localRotation, Quaternion.AngleAxis((float)((!this.skeletonAnimation.skeleton.FlipX) ? -20 : 20), Vector3.forward), 50f * Time.deltaTime);
				this.skeletonAnimation.AnimationName = this.idleAnim;
				this.skeletonAnimation.loop = true;
				if (Time.timeSinceLevelLoad - this.timeReloadAttack > this.cacheEnemy.Time_Reload_Attack)
				{
					if (GameManager.Instance.player.IsInVisible())
					{
						base.SetRun();
					}
					else
					{
						this.timeReloadAttack = Time.timeSinceLevelLoad;
						this.skeletonAnimation.state.SetAnimation(1, this.AttackAnim, false);
					}
				}
			}
		}
		else
		{
			this.transform.localRotation = Quaternion.RotateTowards(this.transform.localRotation, Quaternion.AngleAxis(0f, Vector3.forward), 50f * Time.deltaTime);
			this.skeletonAnimation.AnimationName = this.walkAnim;
			this.skeletonAnimation.loop = true;
		}
	}

	public override void AI()
	{
		Vector2 vector = default(Vector2);
		this.timeMove += Time.deltaTime;
		int num = this.step;
		if (num != 0)
		{
			if (num != 1)
			{
				this.step = 1;
			}
			else
			{
				Vector2 normalized = (new Vector2(CameraController.Instance.transform.position.x - CameraController.Instance.Size().x + 1f, this.transform.position.y) - base.GetPosition()).normalized;
				Vector2 vector2 = new Vector2(CameraController.Instance.transform.position.x - CameraController.Instance.Size().x + 1f, this.transform.position.y);
				float num2 = Mathf.Abs(vector2.x - base.GetPosition().x);
				this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > CameraController.Instance.transform.position.x - CameraController.Instance.Size().x + 1f);
				if (num2 > 0.1f)
				{
					base.SetRun();
					this.transform.Translate(normalized * Time.deltaTime * this.Speed);
					Vector3 position = this.transform.position;
					position.y = CameraController.Instance.transform.position.y + CameraController.Instance.Size().y - 1.5f;
					this.transform.position = Vector3.Lerp(this.transform.position, position, Time.deltaTime);
				}
				else
				{
					this.SetIdle();
					if (this.timeMove >= 10f)
					{
						this.timeReloadAttack = Time.timeSinceLevelLoad;
						this.step = 0;
					}
				}
			}
		}
		else
		{
			Vector2 normalized2 = (new Vector2(CameraController.Instance.transform.position.x + CameraController.Instance.Size().x - 1f, this.transform.position.y) - base.GetPosition()).normalized;
			Vector2 vector3 = new Vector2(CameraController.Instance.transform.position.x + CameraController.Instance.Size().x - 1f, this.transform.position.y);
			float num3 = Mathf.Abs(vector3.x - base.GetPosition().x);
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x > CameraController.Instance.transform.position.x + CameraController.Instance.Size().x - 1f);
			if (num3 > 0.1f)
			{
				base.SetRun();
				this.transform.Translate(normalized2 * Time.deltaTime * this.Speed);
				Vector3 position2 = this.transform.position;
				position2.y = CameraController.Instance.transform.position.y + CameraController.Instance.Size().y - 1.5f;
				this.transform.position = Vector3.Lerp(this.transform.position, position2, Time.deltaTime);
			}
			else
			{
				this.SetIdle();
				if (this.timeMove >= 10f)
				{
					this.timeReloadAttack = Time.timeSinceLevelLoad;
					this.step = 1;
				}
			}
		}
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		base.Hit(damage);
		Color color = new Color(1f, 1f, 1f, 1f);
		Color color2 = new Color(1f, 0.5f, 0f, 1f);
		this.SetHit();
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			this.transform.parent = null;
			GameManager.Instance.audioManager.maybay1.Stop();
			GameManager.Instance.hudManager.combo.ShowCombo(this.lastWeapon);
			try
			{
				GameManager.Instance.ListEnemy.Remove(this);
			}
			catch
			{
			}
			base.CalculatorToDie(true, false);
			this.State = ECharactor.DIE;
			this.isInit = false;
			GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
			base.gameObject.SetActive(false);
		}
	}

	private void CreateBullet(Vector2 target)
	{
		target.x = ((!this.skeletonAnimation.skeleton.FlipX) ? Mathf.Max(CameraController.Instance.transform.position.x - 3f, target.x) : Mathf.Min(CameraController.Instance.transform.position.x + 3f, target.x));
		Vector2 direction = target - new Vector2(this.transform.position.x + this.GunTip1.WorldX, this.transform.position.y + this.GunTip1.WorldY);
		Vector3 pos = new Vector3(this.GunTip1.WorldX + this.transform.position.x, this.GunTip1.WorldY + this.transform.position.y);
		GameManager.Instance.bulletManager.CreateBulletEnemy(0, direction, pos, this.Damaged, this.cacheEnemy.Speed_Bullet, 0f);
		pos = new Vector3(this.GunTip2.WorldX + this.transform.position.x, this.GunTip2.WorldY + this.transform.position.y);
		GameManager.Instance.bulletManager.CreateBulletEnemy(0, direction, pos, this.Damaged, this.cacheEnemy.Speed_Bullet, 0f);
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		if (e.Data.Name.Equals("Shoot"))
		{
			this.CreateBullet(GameManager.Instance.player.Origin());
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString().Split(new char[]
		{
			'_'
		})[0];
		if (text != null)
		{
			if (!(text == "Attack01") && !(text == "Attack02"))
			{
				if (!(text == "Death01") && !(text == "Death02"))
				{
				}
			}
			else
			{
				this.skeletonAnimation.state.SetEmptyAnimation(1, 0f);
				this.SetIdle();
			}
		}
	}

	public new void SetIdle()
	{
		if (this.State == ECharactor.IDLE)
		{
			return;
		}
		base.SetIdle();
		this.timeMove = 0f;
		this.timeReloadAttack = Time.timeSinceLevelLoad;
	}

	private const int TIME_NEXT_MOVE = 10;

	private float timeReloadAttack;

	private float timeMove;

	private int step;

	private Bone GunTip1;

	private Bone GunTip1_02;

	private Bone GunTip2;

	private Bone GunTip2_02;

	private float Damaged;

	private float Speed;
}
