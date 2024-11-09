using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EyeBotSupporter : BaseRambo, IHealth
{
	public float MAXHPCurrent { get; set; }

	public void AddHealthPoint(float hp, EWeapon weapon)
	{
	}

	public void OnBegin(PlayerMain player)
	{
		this.player = player;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		float num = (float)DataLoader.characterData[0].skills[0].HP[ProfileManager.rambos[0].LevelUpgrade];
		base.HPCurrent = num;
		this.MAXHPCurrent = num;
		this.localPos = this.boneTarget.localPosition;
		this.SetIdle();
		this.isCompleteGame = false;
		this.isAuto = false;
		this.MIN_D = ((CameraController.Instance.orientaltion != CameraController.Orientation.HORIZONTAL) ? 0.5f : UnityEngine.Random.Range(1.5f, 3f));
		Physics2D.IgnoreLayerCollision(22, 12, CameraController.Instance.orientaltion == CameraController.Orientation.HORIZONTAL);
		base.NormalSpeed();
		this.isInit = true;
		this.TIME_DELAY = DataLoader.characterData[0].skills[0].TimeReload[ProfileManager.rambos[0].LevelUpgrade];
		UnityEngine.Debug.Log("TIME_DELAY: " + this.TIME_DELAY);
		if (!player.IsRemotePlayer)
		{
			this.DAMAGE = DataLoader.characterData[0].skills[0].Damage[ProfileManager.rambos[0].LevelUpgrade];
		}
		else
		{
			this.DAMAGE = 0f;
		}
		this.SPEED_BULLET = (float)DataLoader.characterData[0].skills[0].ShellPerShot[ProfileManager.rambos[0].LevelUpgrade];
		num = (float)DataLoader.characterData[0].skills[0].HP[ProfileManager.rambos[0].LevelUpgrade];
		this.MAXHPCurrent = num;
		base.HPCurrent = num;
		this.EMovement = BaseCharactor.EMovementBasic.Release;
		GameManager.Instance.bulletManager.LoadAndCreateBulletEyeBot();
	}

	public override void OnUpdate(float deltaTime)
	{
		if (!this.isInit || this.EMovement == BaseCharactor.EMovementBasic.FREEZE)
		{
			return;
		}
		base.OnUpdate(deltaTime);
		if (this.isCompleteGame || this.isAuto)
		{
			return;
		}
		this.MoveFollowPlayer();
		Vector2 position = this.player.GetPosition();
		float num = Mathf.Abs(this.transform.position.x - position.x);
		TrackEntry current = this.skeletonAnimation.state.GetCurrent(0);
		if (this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			this.SetDie();
		}
		this.AutoAttack(deltaTime);
		base.HPCurrent -= this.MAXHPCurrent / 15f * deltaTime;
		this.line.localScale = new Vector3(Mathf.Clamp01(base.HPCurrent / this.MAXHPCurrent), 1f, 1f);
		if (base.HPCurrent <= 0f)
		{
			this.SetDie();
		}
	}

	private void SetIdle()
	{
		if (this.EMovement == BaseCharactor.EMovementBasic.DIE || this.EMovement == BaseCharactor.EMovementBasic.Release)
		{
			return;
		}
		this.EMovement = BaseCharactor.EMovementBasic.Release;
		this.skeletonAnimation.state.SetAnimation(0, this.walkAnim3, false);
	}

	private void SetAttack()
	{
		TrackEntry current = this.skeletonAnimation.state.GetCurrent(5);
		if (current == null || current.IsComplete)
		{
			this.skeletonAnimation.state.SetAnimation(5, this.attackAnim, false);
		}
	}

	private void SetRun()
	{
		if (this.EMovement == BaseCharactor.EMovementBasic.DIE || this.EMovement == BaseCharactor.EMovementBasic.Right)
		{
			return;
		}
		this.EMovement = BaseCharactor.EMovementBasic.Right;
		this.skeletonAnimation.state.SetAnimation(0, this.walkAnim1, false);
	}

	private void SetDie()
	{
		if (this.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		this.EMovement = BaseCharactor.EMovementBasic.DIE;
		try
		{
			GameManager.Instance.ListRambo.Remove(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		if (this.dieCallBack != null)
		{
			this.dieCallBack(false);
		}
		this.skeletonAnimation.state.ClearTracks();
		this.skeletonAnimation.state.SetAnimation(0, this.deathAnim, false);
	}

	public void MoveFollowPlayer()
	{
		Vector3 position = this.player.tfOrigin.position;
		bool flag = this.targetPos.x > position.x + 2f || this.targetPos.x < position.x - 2f || this.targetPos.y > position.y + 2f || this.targetPos.y < position.y + 1f;
		if (flag)
		{
			this.targetPos.x = UnityEngine.Random.Range(position.x - 2f, position.x + 2f);
			this.targetPos.x = Mathf.Clamp(this.targetPos.x, CameraController.Instance.camPos.x - CameraController.Instance.Size().x, CameraController.Instance.camPos.x + CameraController.Instance.Size().x);
			this.targetPos.y = UnityEngine.Random.Range(position.y + 1f, position.y + 2f);
		}
		float num = Vector2.Distance(this.targetPos, this.transform.position);
		BaseCharactor.EMovementBasic emovement = this.EMovement;
		if (emovement != BaseCharactor.EMovementBasic.Right)
		{
			if (emovement == BaseCharactor.EMovementBasic.Release)
			{
				if (num > 0.2f)
				{
					this.SetRun();
				}
			}
		}
		else if (num < 0.2f)
		{
			this.SetIdle();
		}
		this.transform.position = Vector3.SmoothDamp(this.transform.position, this.targetPos, ref this.velo, 0.5f);
	}

	private void AutoAttack(float deltaTime)
	{
		Vector2 vector = this.headGun1.position;
		Vector2 v = this.headGun2.position;
		if (this.enemy != null && this.enemy.HP > 0f && CameraController.Instance.IsInView(this.enemy.transform))
		{
			if (this.CanShoot(this.enemy))
			{
				this.moveToAttack = false;
				this.boneTarget.position = this.enemy.Origin();
				Vector2 normalized = (this.enemy.GetTarget() - vector).normalized;
				this.skeletonAnimation.skeleton.FlipX = (normalized.x < 0f);
				if (Time.timeSinceLevelLoad - this.timeDelay >= this.TIME_DELAY)
				{
					this.timeDelay = Time.timeSinceLevelLoad;
					RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, normalized, 1000f);
					this.SetAttack();
					GameManager.Instance.bulletManager.CreateEyeBotBullet(this.player, vector, normalized, this.DAMAGE, this.SPEED_BULLET);
					GameManager.Instance.bulletManager.CreateEyeBotBullet(this.player, v, normalized, this.DAMAGE, this.SPEED_BULLET);
				}
			}
			else
			{
				this.moveToAttack = true;
			}
		}
		else
		{
			this.enemy = this.GetTarget();
			if (this.enemy == null || !this.CanShoot(this.enemy))
			{
				LeanTween.moveLocal(this.boneTarget.gameObject, this.localPos, 0f).setOnComplete(delegate()
				{
					this.skeletonAnimation.skeleton.FlipX = this.player._PlayerSpine.FlipX;
				});
			}
			else
			{
				Vector2 normalized2 = (this.enemy.GetTarget() - vector).normalized;
				this.skeletonAnimation.skeleton.FlipX = (normalized2.x < 0f);
				this.skeletonAnimation.state.SetAnimation(3, this.aimAnim, false);
				LeanTween.move(this.boneTarget.gameObject, this.enemy.GetTarget(), 0.5f).setDelay(0.1f);
			}
		}
	}

	private float GetAngel(Transform target)
	{
		Vector2 from = (!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left;
		Vector2 a = target.position;
		Vector2 to = a - base.Origin();
		return Vector2.Angle(from, to);
	}

	private void ResetTargetShoot()
	{
		if (this.GetAngel(this.boneTarget) > 75f)
		{
			Vector2 v = ((!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left) + (Vector2)this.transform.position;
			this.boneTarget.position = v;
		}
	}

	private bool CanShoot(BaseEnemy enemy)
	{
		Vector2 b = this.headGun1.position;
		Vector2 normalized = (enemy.GetTarget() - b).normalized;
		Vector2 from = (normalized.x >= 0f) ? Vector2.right : Vector2.left;
		return Vector2.Angle(from, normalized) < 75f;
	}

	private BaseEnemy GetTarget()
	{
		BaseEnemy result = null;
		float num = float.MaxValue;
		for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
		{
			if (CameraController.Instance.IsInView(GameManager.Instance.ListEnemy[i].transform))
			{
				if (GameManager.Instance.ListEnemy[i].HP > 0f)
				{
					Vector3 position = GameManager.Instance.ListEnemy[i].transform.position;
					float num2 = Vector2.Distance(position, this.transform.position);
					if (num2 < num)
					{
						num = num2;
						result = GameManager.Instance.ListEnemy[i];
					}
				}
			}
		}
		return result;
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		try
		{
			string name = trackEntry.Animation.Name;
			if (name != null)
			{
				if (!(name == "hit"))
				{
					if (!(name == "walk1-1"))
					{
						if (!(name == "walk1-3"))
						{
							if (!(name == "attack"))
							{
								if (name == "die")
								{
									base.gameObject.SetActive(false);
									GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
								}
							}
							else
							{
								this.skeletonAnimation.state.ClearTrack(trackEntry.TrackIndex);
							}
						}
						else
						{
							this.skeletonAnimation.state.ClearTrack(trackEntry.TrackIndex);
							this.skeletonAnimation.state.SetAnimation(0, this.idleAnim, true);
						}
					}
					else
					{
						this.skeletonAnimation.state.ClearTrack(trackEntry.TrackIndex);
						this.skeletonAnimation.state.SetAnimation(0, this.walkAnim2, true);
					}
				}
				else
				{
					this.skeletonAnimation.state.ClearTrack(trackEntry.TrackIndex);
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	[SpineAnimation("aim", "", true, false)]
	public string aimAnim;

	[SpineAnimation("attack", "", true, false)]
	public string attackAnim;

	[SpineAnimation("hit", "", true, false)]
	public string hitAnim;

	[SpineAnimation("die", "", true, false)]
	public string deathAnim;

	[SpineAnimation("idle", "", true, false)]
	public string idleAnim;

	[SpineAnimation("walk", "", true, false)]
	public string walkAnim1;

	[SpineAnimation("walk", "", true, false)]
	public string walkAnim2;

	[SpineAnimation("walk", "", true, false)]
	public string walkAnim3;

	private bool isInit;

	private bool isCompleteGame;

	private bool isAuto;

	private float MIN_D = 2.5f;

	private float TIME_ATTACK = 0.4f;

	private float DAMAGE = 2f;

	private float SPEED_BULLET = 10f;

	private int bulletCount;

	private float timeDelay;

	private float TIME_DELAY = 0.9f;

	public Transform line;

	public Action<bool> dieCallBack;

	private Vector3 targetPos;

	[SerializeField]
	private Transform boneTarget;

	[SerializeField]
	private Transform headGun1;

	[SerializeField]
	private Transform headGun2;

	private Vector3 localPos;

	private BaseEnemy enemy;

	private bool moveToAttack;

	[SerializeField]
	private float speed = 0.1f;

	private Vector3 velo;

	private PlayerMain player;
}
