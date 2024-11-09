using System;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BaseHuman : BaseEnemySpine
{
	public override void InitEnemy(EnemyCharactor DataEnemy, int Level)
	{
		base.InitEnemy(DataEnemy, Level);
		this.ListLigntingConnected.Clear();
		base.isMainLignting = false;
		if (this.boxCollider != null)
		{
			this.boxCollider.enabled = true;
		}
		this.rigidbody2D.isKinematic = false;
		this.AttackAnim2 = base.FindAnimation(this.attackAnim2);
		this.DeathAnim2 = base.FindAnimation(this.deadAnim2);
		this.DeathAnim3 = base.FindAnimation(this.deadAnim3);
		this.DeathAnim4 = base.FindAnimation(this.deadAnim4);
		this.DeathAnim5 = base.FindAnimation(this.deadAnim5);
		this.DeathFire = base.FindAnimation(this.deadAnimFire);
		this.time_wait_pingpong = -1f;
		this.timeFlip = Time.timeSinceLevelLoad;
		this.Time_Reload_Attack = float.MinValue;
		this.GunTipBlood = this.skeletonAnimation.skeleton.FindBone("blood_tip");
		this.speedPingpong = 1f;
		this.isTrucker = false;
		this.TimeAutoDie = 0f;
	}

	public void SetDie(bool isByRambo = true)
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		if (!isByRambo && !object.ReferenceEquals(DataLoader.LevelDataCurrent, null) && this.isCreateWithJson)
		{
			DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy--;
		}
		this.skeletonAnimation.state.ClearTrack(0);
		this.skeletonAnimation.state.SetAnimation(0, this.GetAnimDeath(), false);
		if (this.rigidbody2D != null && this.isInit)
		{
			this.rigidbody2D.velocity = Vector2.zero;
		}
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
		this.boxCollider.enabled = false;
		this.rigidbody2D.isKinematic = true;
		base.CalculatorToDie(isByRambo, this.isTrucker);
	}

	protected void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.CompareTag("Ground") || coll.gameObject.CompareTag("Obstacle") || coll.gameObject.CompareTag("platform"))
		{
			if (!object.ReferenceEquals(this.rigidbody2D, null))
			{
				this.rigidbody2D.gravityScale = 3f;
			}
			this.isInit = true;
		}
		this.isJump = false;
		if (coll.gameObject.CompareTag("Gulf"))
		{
			this.SetDie(false);
		}
	}

	private Spine.Animation GetAnimDeath()
	{
		Spine.Animation result = null;
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			result = this.DeadAnim;
			return result;
		}
		switch (this.lastWeapon)
		{
		case EWeapon.FLAME:
			try
			{
				result = this.DeathFire;
			}
			catch
			{
				result = this.DeadAnim;
			}
			break;
		case EWeapon.THUNDER:
		case EWeapon.LASER:
			result = this.DeathAnim5;
			break;
		default:
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			if (num < 0.3f)
			{
				result = this.DeadAnim;
			}
			else if (num >= 0.3f && num < 0.6f)
			{
				result = this.DeathAnim2;
			}
			else
			{
				bool flag = true;
				if (GameManager.Instance.player != null)
				{
					flag = (GameManager.Instance.player.transform.position.x < this.transform.position.x);
				}
				if (flag)
				{
					result = ((!this.skeletonAnimation.skeleton.FlipX) ? this.DeathAnim4 : this.DeathAnim3);
				}
				else
				{
					result = (this.skeletonAnimation.skeleton.FlipX ? this.DeathAnim4 : this.DeathAnim3);
				}
			}
			break;
		}
		}
		return result;
	}

	protected void HideTexture()
	{
		float num = 2f + CameraController.Instance.Size().x;
		float num2 = 2f + CameraController.Instance.Size().y;
		if (GameManager.Instance.StateManager.EState == EGamePlay.NONE || GameManager.Instance.StateManager.EState == EGamePlay.BEGIN)
		{
			num = 30f;
		}
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.HORIZONTAL)
		{
			if (orientaltion == CameraController.Orientation.VERTICAL)
			{
				float num3 = Mathf.Abs(this.transform.position.y - CameraController.Instance.transform.position.y);
				if (!object.ReferenceEquals(this.meshRenderer, null))
				{
					this.meshRenderer.enabled = (num3 < num2);
				}
			}
		}
		else
		{
			float num3 = Mathf.Abs(this.transform.position.x - CameraController.Instance.transform.position.x);
			if (!object.ReferenceEquals(this.meshRenderer, null))
			{
				this.meshRenderer.enabled = (num3 < num);
			}
		}
	}

	protected float Time_Reload_Attack;

	protected bool isAutoRun;

	protected float time_wait_pingpong = -1f;

	protected float timeFlip;

	[SpineAnimation("", "", true, false, startsWith = "Attack02")]
	public string attackAnim2;

	public Spine.Animation AttackAnim2;

	[SpineAnimation("", "", true, false, startsWith = "Death02")]
	public string deadAnim2;

	[SpineAnimation("", "", true, false, startsWith = "Death03")]
	public string deadAnim3;

	[SpineAnimation("", "", true, false, startsWith = "Death04")]
	public string deadAnim4;

	[SpineAnimation("", "", true, false, startsWith = "Death05")]
	public string deadAnim5;

	[SpineAnimation("", "", true, false, startsWith = "Death07")]
	public string deadAnimFire;

	public Spine.Animation DeathAnim2;

	public Spine.Animation DeathAnim3;

	public Spine.Animation DeathAnim4;

	public Spine.Animation DeathAnim5;

	public Spine.Animation DeathFire;

	public Vector3[] arrPosHand;

	[NonSerialized]
	public bool isEnemyTrack;

	[NonSerialized]
	public bool isTrucker;

	protected float TimeAutoDie;

	protected bool isJump;
}
