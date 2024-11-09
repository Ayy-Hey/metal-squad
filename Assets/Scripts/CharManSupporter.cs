using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class CharManSupporter : BaseRambo, IHealth
{
	public void AddHealthPoint(float hp, EWeapon lastWeapon)
	{
		if (hp > 0f)
		{
		}
		base.HPCurrent += hp;
		base.HPCurrent = Mathf.Min(base.HPCurrent, (float)this.MAXHP);
		Vector3 localScale = new Vector3(Mathf.Clamp01(base.HPCurrent / (float)this.MAXHP), 1f, 1f);
		this.line.localScale = localScale;
		if (base.HPCurrent <= 0f)
		{
			this.SetDie();
		}
	}

	public void OnBegin()
	{
		base.HPCurrent = (float)this.MAXHP;
		this.GunTip = this.skeletonAnimation.skeleton.FindBone("GunTip_MachineGun");
		this.GunTip2 = this.skeletonAnimation.skeleton.FindBone("GunTip_MachineGun02");
		this.skeletonAnimation.state.SetAnimation(0, "parachute", true);
		this.rigidbody2D.gravityScale = 0.5f;
		this.isCompleteGame = false;
		this.isAuto = false;
		this.LastTimeStuck = Time.timeSinceLevelLoad;
		this.timeAttack = Time.timeSinceLevelLoad;
		this.animShoot.gameObject.SetActive(false);
		this.MIN_D = ((CameraController.Instance.orientaltion != CameraController.Orientation.HORIZONTAL) ? 0.5f : UnityEngine.Random.Range(1.5f, 3f));
		Physics2D.IgnoreLayerCollision(22, 12, CameraController.Instance.orientaltion == CameraController.Orientation.HORIZONTAL);
		base.NormalSpeed();
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.isInit = true;
		this.TIME_ATTACK = ProfileManager.charManSupporterProfile.Time_Attack;
		this.DAMAGE = ProfileManager.charManSupporterProfile.Damage;
		this.SPEED_BULLET = ProfileManager.charManSupporterProfile.Speed_Bullet;
		base.HPCurrent = (float)ProfileManager.charManSupporterProfile.HP;
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		if (entry.TrackIndex > 0)
		{
			this.skeletonAnimation.state.ClearTrack(entry.TrackIndex);
		}
		string text = entry.ToString();
		if (text != null)
		{
			if (text == "death_MachineGun")
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void OnCompleted()
	{
		this.isCompleteGame = true;
		this.skeletonAnimation.state.ClearTracks();
		this.skeletonAnimation.state.SetAnimation(0, this.victoryAnim, true);
	}

	public void MoveEndMap(Vector3 target)
	{
		if (this.isCompleteGame)
		{
			return;
		}
		TrackEntry current = this.skeletonAnimation.state.GetCurrent(0);
		this.isAuto = true;
		if (this.transform.position.x >= target.x)
		{
			this.SetIdle();
			this.OnCompleted();
			return;
		}
		this.skeletonAnimation.skeleton.FlipX = false;
		this.rigidbody2D.velocity = new Vector2(3f, this.rigidbody2D.velocity.y);
		this.SetRun();
	}

	private void CheckStuck(float deltaTime)
	{
		Vector3 v = new Vector3(this.GunTip.WorldX + this.transform.position.x, this.GunTip.WorldY + this.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast(v, (!this.skeletonAnimation.skeleton.FlipX) ? Vector2.right : Vector2.left, 0.2f);
		if (hit && (hit.collider.CompareTag("platform") || hit.collider.CompareTag("Tank")))
		{
			this.timeStuck += deltaTime;
		}
		else
		{
			this.timeStuck = 0f;
		}
		if (this.timeStuck >= 1f)
		{
			this.IsGround = false;
			this.rigidbody2D.velocity = Vector2.zero;
			this.rigidbody2D.AddForce(new Vector2(0f, 1500f), ForceMode2D.Impulse);
			this.SetJump();
			this.timeStuck = 0f;
		}
	}

	private void CheckStuck2()
	{
		if (Time.timeSinceLevelLoad - this.LastTimeStuck >= 3f)
		{
			this.LastTimeStuck = Time.timeSinceLevelLoad;
			bool flag = this.StuckBound.Contains(base.GetPosition());
			Vector2 position = base.GetPosition();
			position.x -= 0.1f;
			position.y -= 0.1f;
			this.StuckBound = new Rect(position, new Vector2(0.2f, 0.2f));
			if (flag)
			{
				this.IsGround = false;
				this.rigidbody2D.velocity = Vector2.zero;
				this.rigidbody2D.AddForce(new Vector2(0f, 1500f), ForceMode2D.Impulse);
				this.SetJump();
			}
		}
	}

	private void CheckStuck3()
	{
		if (CameraController.Instance.orientaltion == CameraController.Orientation.HORIZONTAL)
		{
			return;
		}
		float num = GameManager.Instance.player.Origin().y - base.Origin().y;
		if (num > 1f && this.IsGround)
		{
			this.IsGround = false;
			this.rigidbody2D.velocity = Vector2.zero;
			this.rigidbody2D.AddForce(new Vector2(0f, 1500f), ForceMode2D.Impulse);
			this.SetJump();
		}
	}

	private void Update()
	{
	}

	public override void OnUpdate(float deltaTime)
	{
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
	}

	private void SetIdle()
	{
	}

	private void SetRun()
	{
	}

	private void SetJump()
	{
	}

	private void SetDie()
	{
	}

	private void AutoAttack(float deltaTime)
	{
		Vector2 v = new Vector2(this.transform.position.x + this.GunTip.WorldX, this.transform.position.y + this.GunTip.WorldY);
		Vector2 vector = new Vector2(this.GunTip.WorldX - this.GunTip2.WorldX, this.GunTip.WorldY - this.GunTip2.WorldY);
		Vector2 normalized = vector.normalized;
		if (this.animShoot.gameObject.activeSelf)
		{
			this.animShoot.transform.position = v;
			this.animShoot.transform.rotation = Quaternion.FromToRotation(Vector3.right, normalized);
		}
	}

	private BaseEnemy GetTarget()
	{
		BaseEnemy result = null;
		float num = float.MaxValue;
		for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
		{
			if (CameraController.Instance.IsInView(GameManager.Instance.ListEnemy[i].transform))
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
		return result;
	}

	private float MIN_D = 2.5f;

	private float TIME_ATTACK = 0.3f;

	private float DAMAGE = 2f;

	private float SPEED_BULLET = 10f;

	public int MAXHP = 300;

	[SpineAnimation("Jump", "", true, false)]
	public string jumpAnim;

	[SpineAnimation("death", "", true, false)]
	public string deathAnim;

	[SpineAnimation("idle", "", true, false)]
	public string idleAnim;

	[SpineAnimation("run", "", true, false)]
	public string runAnim;

	[SpineAnimation("idle_sit", "", true, false)]
	public string idle_Sit_Anim;

	[SpineAnimation("walk", "", true, false)]
	public string walkAnim;

	[SpineAnimation("victory", "", true, false)]
	public string victoryAnim;

	private Bone GunTip;

	private Bone GunTip2;

	private bool isCompleteGame;

	private bool isAuto;

	private float timeStuck;

	public Transform line;

	private float LastTimeStuck;

	private Rect StuckBound;

	private const float STUCK_BOUND_WIDTH = 0.2f;

	private const float STUCK_BOUND_HEIGHT = 0.2f;

	private const float CHECK_STUCK_DELTA = 3f;

	private float timeAttack;

	public Animator animShoot;

	public Animator animEffectJump;

	public SkeletonUtilityBone BodyControl;

	public SkeletonUtilityBone HandControl;

	private Vector3[] ArrPosHand_Girl = new Vector3[]
	{
		new Vector3(0.1555f, -0.057f),
		new Vector3(0.176f, -0.057f),
		new Vector3(0.181f, -0.137f),
		new Vector3(0.265f, -0.185f),
		new Vector3(0.381f, -0.205f),
		new Vector3(0.395f, -0.182f),
		new Vector3(0.422f, -0.146f),
		new Vector3(0.129f, -0.052f)
	};

	private float[] AngleBody = new float[]
	{
		-25f,
		-18f,
		0f,
		0f,
		12f,
		25f,
		25f,
		-50f
	};

	private Vector3 velocity = Vector3.zero;

	public LayerMask layermask;

	public LayerMask layermaskIgnore;

	private bool isInit;
}
