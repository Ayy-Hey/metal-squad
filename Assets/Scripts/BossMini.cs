using System;
using System.Collections;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BossMini : BaseEnemySpine
{
	private void OnValidate()
	{
		try
		{
			this.bossData = Resources.Load<TextAsset>("Charactor/Boss/" + base.GetType().ToString());
		}
		catch (Exception ex)
		{
			MonoBehaviour.print(ex.Message);
		}
	}

	private void Update()
	{
		this.OnPause(GameManager.Instance.StateManager.EState == EGamePlay.PAUSE);
		if (!this.isInit || this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this.Begin())
		{
			return;
		}
		this.OnUpdate();
	}

	private void OnPause(bool v)
	{
		if (!this._isPause && v)
		{
			this._isPause = v;
			this._pauseVelocity = this.rigidbody2D.velocity;
			this.rigidbody2D.velocity = Vector2.zero;
			this.rigidbody2D.gravityScale = 0f;
		}
		if (this._isPause && !v)
		{
			this._isPause = v;
			this.rigidbody2D.velocity = this._pauseVelocity;
			this.rigidbody2D.gravityScale = 1f;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!this._isBegin)
		{
			return;
		}
		if (collision.transform.CompareTag("Obstacle"))
		{
			this.skeletonAnimation.state.ClearTracks();
			this.rigidbody2D.velocity = Vector2.zero;
			this._nameBoxStay = collision.transform.name;
			for (int i = 0; i < this._namesGroundBox.Length; i++)
			{
				if (this._nameBoxStay == this._namesGroundBox[i])
				{
					this._idBoxStay = i;
					break;
				}
			}
			if (this.State == ECharactor.JUMP || this.State == ECharactor.SIT_RUN)
			{
				this.NextState();
			}
		}
	}

	public void InitEnemy()
	{
		try
		{
			float mode = GameMode.Instance.GetMode();
			EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>(this.bossData.text);
			enemyCharactor.enemy[0].Damage *= mode;
			enemyCharactor.enemy[0].Speed *= ((mode > 2f) ? 2f : mode);
			this.InitEnemy(enemyCharactor, 0);
			this.cacheEnemy.HP = this.HP;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.skeletonAnimation.skeleton.FlipX = false;
		this.attackAnim = "Attack";
		this.idleAnim = "Idle";
		this.deadAnim = "Death";
		this._poolingBossMini = new ObjectPooling<BulletBossMini>(this.listBullet.Length, null, null);
		for (int i = 0; i < this.listBullet.Length; i++)
		{
			this.listBullet[i].gameObject.SetActive(false);
			this.listBullet[i].Off = new Action<BulletBossMini>(this.OffBullet);
			this._poolingBossMini.Store(this.listBullet[i]);
		}
		this._namesGroundBox = new string[this.listGroundBox.Length];
		for (int j = 0; j < this.listGroundBox.Length; j++)
		{
			this._namesGroundBox[j] = this.listGroundBox[j].transform.name;
		}
		this.gunTip1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip01");
		this.gunTip2 = this.skeletonAnimation.skeleton.FindBone("Gun_tip02");
		this._ramboTrans = GameManager.Instance.player.transform;
		this.bulletSpeed = this.cacheEnemy.Speed * 4f;
		this.bulletSpeed = Mathf.Min(this.bulletSpeed, 10f);
		this.boxHit.InitEnemy(this.cacheEnemy.HP);
		this.boxHit.OnHitDamage = new Action<float>(this.OnHitDamage);
		this.boxHit.OnHitWeapon = new Action<EWeapon>(this.OnHitWeapon);
		this.PlayAnim(0, this.walkAnim, true, false);
		this.State = ECharactor.ATTACK;
		this._changeState = false;
		this.isInit = true;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		base.StartCoroutine(this.ShowWarning());
	}

	private IEnumerator ShowWarning()
	{
		GameManager.Instance.hudManager.WarningBoss.SetActive(true);
		yield return new WaitForSeconds(2f);
		GameManager.Instance.hudManager.WarningBoss.SetActive(false);
		yield break;
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		this.skeletonAnimation.state.ClearTrack(entry.TrackIndex);
	}

	private void OnHitWeapon(EWeapon obj)
	{
		this.Hit(obj);
	}

	private void OnHitDamage(float obj)
	{
		this.Hit(obj);
	}

	private bool Begin()
	{
		if (this._isBegin)
		{
			return true;
		}
		Vector2 target = new Vector2(CameraController.Instance.Position.x + CameraController.Instance.Size().x - 2f, this.transform.position.y);
		this.transform.position = Vector2.MoveTowards(this.transform.position, target, Time.deltaTime * this.cacheEnemy.Speed);
		this.skeletonAnimation.AnimationName = this.walkAnim;
		if (this.transform.position.x == target.x)
		{
			this._isBegin = true;
			this._idBoxStay = 1;
		}
		return false;
	}

	private void EditorTest()
	{
	}

	private void OnUpdate()
	{
		this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x < this._ramboTrans.position.x);
		if (!this._changeState)
		{
			this._changeState = true;
			this._oldState = this.State;
			ECharactor state = this.State;
			switch (state)
			{
			case ECharactor.IDLE:
				this.PlayAnim(0, this.idleAnim, true, true);
				base.StartCoroutine(this.OnIdle());
				break;
			default:
				if (state == ECharactor.ATTACK)
				{
					base.StartCoroutine(this.OnFire());
				}
				break;
			case ECharactor.JUMP:
				if (Mathf.Abs(this._disCam) > 4f && (this._idBoxStay == 2 || this._idBoxStay == 3))
				{
					this.NextState();
				}
				else
				{
					RaycastHit2D raycastHit2D = Physics2D.Raycast(this.transform.position, Vector2.up, 10f, this.groundMask);
					this._velocityX = ((Mathf.Abs(this._disCam) <= 2f) ? (-this._disCam * 4f) : (-this._disCam * 2.5f));
					this._velocityX *= ((this._idBoxStay <= 1) ? 1f : 1.2f);
					this._velocityX = ((this._velocityX <= 0f) ? Mathf.Min(-5f, this._velocityX) : Mathf.Max(5f, this._velocityX));
					this._velocityX = (((this._idBoxStay != 0 && this._idBoxStay != 1) || Mathf.Abs(this._disCam) < 4f) ? (((this._idBoxStay != 0 && this._idBoxStay != 1) || UnityEngine.Random.Range(0, 3) != 0 || !(raycastHit2D.transform != null)) ? this._velocityX : 0f) : 0f);
					this._velocityY = ((this._velocityX != 0f) ? ((this._idBoxStay != 1) ? ((this._idBoxStay != 3) ? 8f : 6.5f) : 6.75f) : (raycastHit2D.distance * 4.75f));
					this.rigidbody2D.velocity = new Vector2(this._velocityX, this._velocityY);
					this._fireWhenJump = (Mathf.Abs(this._disRamY) <= 2f);
					this.PlayAnim(1, this.jumpAnim, true, false);
					if (this._fireWhenJump)
					{
						base.StartCoroutine(this.OnFire());
					}
				}
				break;
			case ECharactor.SIT_RUN:
				this.PlayAnim(1, this.downAnim, true, false);
				base.StartCoroutine(this.OnDown());
				break;
			case ECharactor.WALK:
			{
				this.PlayAnim(0, this.walkAnim, true, true);
				this._targetMove = this.transform.position + ((UnityEngine.Random.Range(0, 2) != 1) ? (Vector3.left * 2f) : (Vector3.right * 2f));
				float num = this.listGroundBox[this._idBoxStay].bounds.size.x / 2f - 1f;
				this._targetMove.x = ((this._targetMove.x >= this.listGroundBox[this._idBoxStay].transform.position.x + num || this._targetMove.x <= this.listGroundBox[this._idBoxStay].transform.position.x - num) ? ((this._disCam <= 0f) ? (this.listGroundBox[this._idBoxStay].transform.position.x + num) : (this.listGroundBox[this._idBoxStay].transform.position.x - num)) : this._targetMove.x);
				this._targetMove.x = ((Mathf.Abs(this._targetMove.x) > 5f) ? ((float)((this._targetMove.x <= 0f) ? -5 : 5)) : this._targetMove.x);
				break;
			}
			}
		}
		else
		{
			ECharactor state2 = this.State;
			switch (state2)
			{
			case ECharactor.IDLE:
				if (this._damageByKnife)
				{
					this._damageByKnife = false;
					this.NextState();
				}
				break;
			default:
				if (state2 != ECharactor.ATTACK)
				{
					if (state2 == ECharactor.WALK)
					{
						this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetMove, this.cacheEnemy.Speed * Time.deltaTime);
						if (this.transform.position.x == this._targetMove.x)
						{
							this.NextState();
						}
					}
				}
				break;
			case ECharactor.JUMP:
				this.box.enabled = (this.rigidbody2D.velocity.y < 0.001f);
				break;
			}
		}
	}

	private void PlayAnim(int track, string anim, bool loop = false, bool clear = false)
	{
		if (clear)
		{
			this.skeletonAnimation.state.ClearTracks();
		}
		this.skeletonAnimation.state.SetAnimation(track, anim, loop);
	}

	private void FlipEnemy(bool isFlip)
	{
		this.skeletonAnimation.skeleton.FlipX = isFlip;
	}

	private void NextState()
	{
		this._disCam = this.transform.position.x - CameraController.Instance.Position.x;
		this._disRamX = this.transform.position.x - this._ramboTrans.position.x;
		this._disRamY = this.transform.position.y - this._ramboTrans.position.y;
		if (Mathf.Abs(this._disRamX) >= 2.5f && this._disRamY > -1f && this._disRamY < 0.2f)
		{
			this.SwitchState(0);
			this._changeState = false;
		}
		else
		{
			switch (this._idBoxStay)
			{
			case 0:
				this.RandomState(new int[]
				{
					0,
					1,
					3
				});
				break;
			case 1:
				this.RandomState(new int[]
				{
					0,
					1,
					3
				});
				break;
			case 2:
				this.RandomState(new int[]
				{
					0,
					1,
					2,
					3
				});
				break;
			case 3:
				this.RandomState(new int[]
				{
					0,
					1,
					2,
					3
				});
				break;
			}
			if (this.State == this._oldState)
			{
				this.NextState();
			}
			else
			{
				this._changeState = false;
			}
		}
	}

	private void RandomState(int[] states)
	{
		int num = UnityEngine.Random.Range(0, states.Length);
		this.SwitchState(states[num]);
	}

	private void SwitchState(int state)
	{
		switch (state)
		{
		case 0:
			this.State = ECharactor.ATTACK;
			break;
		case 1:
			this.State = ECharactor.JUMP;
			break;
		case 2:
			this.State = ECharactor.SIT_RUN;
			break;
		case 3:
			this.State = ECharactor.WALK;
			break;
		default:
			this.State = ECharactor.IDLE;
			break;
		}
	}

	private void OffBullet(BulletBossMini obj)
	{
		obj.gameObject.SetActive(false);
		obj.Off = null;
		this._poolingBossMini.Store(obj);
	}

	private IEnumerator OnDown()
	{
		this.box.enabled = false;
		yield return new WaitForSeconds(0.45f);
		this.box.enabled = true;
		yield break;
	}

	private IEnumerator OnFire()
	{
		if (UnityEngine.Random.Range(0, 3) == 1)
		{
			for (int i = 0; i < this.numberBulletPerShot; i++)
			{
				this.PlayAnim(0, this.attackAnim, false, false);
				yield return new WaitForSeconds(0.12f);
				this.CreateBullet(0f, 0f);
				this.CreateBullet(0f, 30f);
				this.CreateBullet(0f, -30f);
				yield return new WaitForSeconds(0.1f);
			}
		}
		else
		{
			for (int j = 0; j < this.numberBulletPerShot; j++)
			{
				this.PlayAnim(0, this.attackAnim, false, false);
				yield return new WaitForSeconds(0.12f);
				this.CreateBullet(0f, 0f);
				this.CreateBullet(0.15f, 0f);
				yield return new WaitForSeconds(0.1f);
			}
		}
		if (!this._fireWhenJump)
		{
			this.NextState();
		}
		else
		{
			this._fireWhenJump = false;
		}
		yield break;
	}

	private IEnumerator OnIdle()
	{
		yield return new WaitForSeconds(this.timeStay);
		this.NextState();
		yield break;
	}

	private void CreateBullet(float deltaY, float angle = 0f)
	{
		this.bullet = null;
		this.bullet = this._poolingBossMini.New();
		if (this.bullet == null)
		{
			this.bullet = UnityEngine.Object.Instantiate<BulletBossMini>(this.listBullet[0]);
			this.bullet.gameObject.transform.parent = this.listBullet[0].transform.parent;
		}
		this.bullet.Off = new Action<BulletBossMini>(this.OffBullet);
		this.bullet.gameObject.SetActive(true);
		this.bullet.InitObject();
		Vector3 pos = new Vector3(this.gunTip1.WorldX, this.gunTip1.WorldY + deltaY, 0f) + this.transform.position;
		Vector3 vector = new Vector3(this.gunTip2.WorldX - this.gunTip1.WorldX, this.gunTip2.WorldY - this.gunTip1.WorldY);
		Quaternion quaternion = Quaternion.LookRotation(vector, Vector3.back);
		quaternion.x = (quaternion.y = 0f);
		this.bullet.gameObject.transform.eulerAngles = new Vector3(0f, 0f, quaternion.eulerAngles.z + 90f + angle);
		this.bullet.SetValue(EWeapon.NONE, 1, pos, vector, this.cacheEnemy.Damage, this.bulletSpeed, 90f);
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.HP += damage;
		if (this.lastWeapon == EWeapon.HUMMER || this.lastWeapon == EWeapon.AXE || this.lastWeapon == EWeapon.SWORD)
		{
			this._damageByKnife = true;
		}
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(2, 0, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			base.StopAllCoroutines();
			this.Die();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			PlayerManagerStory.Instance.OnRunGameOver();
		}
		else
		{
			this.SetHit();
		}
	}

	private void Die()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		this.skeletonAnimation.skeleton.SetColor(Color.white);
		this.skeletonAnimation.state.ClearTracks();
		this.skeletonAnimation.state.SetAnimation(2, this.deadAnim, false);
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
	}

	public void Hit(EWeapon weapon)
	{
		MonoBehaviour.print(111);
		this.lastWeapon = weapon;
		if (this.lastWeapon == EWeapon.HUMMER || this.lastWeapon == EWeapon.AXE || this.lastWeapon == EWeapon.SWORD)
		{
			this._damageByKnife = true;
		}
	}

	[SerializeField]
	[Header("*******************************")]
	private TextAsset bossData;

	[SpineAnimation("", "", true, false)]
	public string jumpAnim;

	[SpineAnimation("", "", true, false)]
	public string downAnim;

	[SerializeField]
	private BulletBossMini[] listBullet;

	[SerializeField]
	private int numberBulletPerShot;

	private float bulletSpeed;

	[SerializeField]
	private LayerMask groundMask;

	[SerializeField]
	private Collider2D box;

	public BoxHit boxHit;

	[SerializeField]
	private Collider2D[] listGroundBox;

	[SerializeField]
	private float timeStay;

	private Bone gunTip1;

	private Bone gunTip2;

	private bool _changeState;

	public ObjectPooling<BulletBossMini> _poolingBossMini;

	private Transform _ramboTrans;

	private RaycastHit2D hitUp;

	private RaycastHit2D hitDown;

	private RaycastHit2D hitUpLeft;

	private RaycastHit2D hitUpRight;

	private bool _isBegin;

	private Vector3 _targetMove;

	private int _idBoxStay;

	private bool _fireWhenJump;

	private float _velocityX;

	private float _velocityY;

	private float _disCam;

	private float _disRamX;

	private float _disRamY;

	private ECharactor _oldState;

	private Vector2 _pauseVelocity;

	private bool _isPause;

	private string[] _namesGroundBox;

	private string _nameBoxStay;

	private bool _damageByKnife;

	private int count;

	private BulletBossMini bullet;
}
