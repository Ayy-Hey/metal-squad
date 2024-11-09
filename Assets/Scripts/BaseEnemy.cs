using System;
using System.Collections.Generic;
using MyDataLoader;
using UnityEngine;

public class BaseEnemy : CachingMonoBehaviour, IHealth
{
	public bool isMainLignting { get; set; }

	public bool isInCamera { get; set; }

	public void AddHealthPoint(float hp, EWeapon lastWeapon)
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		if (this.cacheEnemy == null || (!this.isInCamera && lastWeapon != EWeapon.EXCEPTION))
		{
			return;
		}
		if (this.AddHealthPointAction != null)
		{
			this.AddHealthPointAction(hp, lastWeapon);
		}
		this.lastWeapon = lastWeapon;
		this.HP += hp;
		this.Hit(hp);
        Debug.Log("hp " + this.HP);
		if (this.HP <= 0f)
		{
			if (this.isMainLignting)
			{
				GameManager.Instance.fxManager.ReleaseEffectLighting();
			}
			if (this.OnEnemyDeaded != null)
			{
				this.OnEnemyDeaded();
			}
			this.isMainLignting = false;
			this.ListLigntingConnected.Clear();
		}
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Show(this.HP, this.cacheEnemy.HP * GameMode.Instance.GetMode(), true);
		}
	}

	public virtual void InitEnemy(EnemyCharactor DataEnemy, int Level)
	{
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.isKinematic = false;
		}
		this.cacheEnemy = null;
		this.cacheEnemy = (Enemy)DataEnemy.enemy[Mathf.Max(Level, 0)].Clone();
		this.HP = this.cacheEnemy.HP;
		this.HP *= GameMode.Instance.GetMode();
		this.cacheEnemy.Vision();
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.gameObject.SetActive(true);
			this.lineBloodEnemy.Reset();
		}
		this.isParachuter = false;
		if (this.meshRenderer != null)
		{
			this.meshRenderer.sortingOrder = 6;
		}
		this.TimeCounterUpgradeEnemy = 0f;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		this.OnEnemyDeaded = null;
		this.Time_Get_Target = float.MinValue;
		this.isCreateWithJson = false;
		this.hasShow = false;
	}

	public void SetIdle()
	{
		if (this.State == ECharactor.IDLE)
		{
			return;
		}
		this.State = ECharactor.IDLE;
		if (this.rigidbody2D != null && this.rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
		{
			this.rigidbody2D.velocity = Vector2.zero;
		}
	}

	public void SetRun()
	{
		if (this.State == ECharactor.RUN || this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.RUN;
	}

	public virtual void SetDie()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
	}

	public virtual void SetAttack()
	{
		if (this.rigidbody2D != null && this.rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
		{
			this.rigidbody2D.velocity = Vector2.zero;
		}
	}

	public virtual void SetHit()
	{
	}

	public ECharactor GetState()
	{
		return this.State;
	}

	public virtual void ResetAIEnemy(bool isResetPosition)
	{
	}

	public virtual void SetFlip(bool isFlip)
	{
	}

	public virtual void AI()
	{
	}

	public virtual void Hit(float damage)
	{
	}

	public virtual void SetParachuter(float gravity = 0.5f)
	{
	}

	public void ReloadInfor(float rateHP, float rateDamaged)
	{
	}

	public Vector2 GetPosition()
	{
		return this.transform.position;
	}

	public Vector2 Origin()
	{
		if (!this.tfOrigin)
		{
			return this.transform.position;
		}
		return this.tfOrigin.position;
	}

	public Vector3 Origin3()
	{
		if (this.tfOrigin)
		{
			return this.tfOrigin.position;
		}
		return this.transform.position;
	}

	public Vector2 GetTarget()
	{
		if (this.tfTarget.Count <= 0)
		{
			this.cacheTarget = this.Origin();
		}
		else
		{
			if (Time.timeSinceLevelLoad - this.Time_Get_Target > 1f)
			{
				this.Time_Get_Target = Time.timeSinceLevelLoad;
				this.idCacheTarget = UnityEngine.Random.Range(0, this.tfTarget.Count);
			}
			this.cacheTarget = new Vector2(this.tfTarget[this.idCacheTarget].localPosition.x + this.GetPosition().x, this.tfTarget[this.idCacheTarget].localPosition.y + this.GetPosition().y);
		}
		return this.cacheTarget;
	}

	protected void CheckWithCamera()
	{
		float num = 0f;
		float num2 = 0f;
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.HORIZONTAL)
		{
			if (orientaltion == CameraController.Orientation.VERTICAL)
			{
				num = this.transform.position.y - CameraController.Instance.Position.y;
				num2 = CameraController.Instance.Size().y + 2f;
			}
		}
		else
		{
			num = this.transform.position.x - CameraController.Instance.Position.x;
			num2 = CameraController.Instance.Size().x + 3f;
		}
		if (num > 0f && num > num2)
		{
			if (this.rigidbody2D != null)
			{
				this.rigidbody2D.bodyType = RigidbodyType2D.Static;
			}
			if (this.meshRenderer)
			{
				this.meshRenderer.enabled = false;
			}
		}
		else
		{
			if (this.rigidbody2D != null)
			{
				this.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
			}
			if (this.meshRenderer)
			{
				this.meshRenderer.enabled = true;
			}
		}
		if (!this.hasShow && this.isInCamera)
		{
			this.hasShow = true;
		}
	}

	protected Color PingPongColor()
	{
		this.timePingPongColor += Time.deltaTime;
		if (this.timePingPongColor >= 0.3f)
		{
			return Color.white;
		}
		return Color.Lerp(Color.white, this.colorHit, Mathf.PingPong(Time.time * 10f, 1f));
	}

	protected float Distance(float x1, float x2)
	{
		return Mathf.Abs(x2 - x1);
	}

	protected void CalculatorToDie(bool isRamboKill, bool isTrucker)
	{
		if (!isRamboKill)
		{
			return;
		}
		if (this.cacheEnemyData.gift)
		{
			GameManager.Instance.giftManager.Create(this.transform.position, this.cacheEnemyData.gift_value);
		}
		if (this.cacheEnemyData.DropCoin)
		{
			GameManager.Instance.coinManager.CreateCoin(this.cacheEnemyData.ValueCoin, this.Origin());
			this.cacheEnemyData.DropCoin = false;
		}
		GameManager.Instance.CountEnemyDie++;
		GameManager.Instance.hudManager.combo.ShowCombo(this.lastWeapon);
		GameManager.Instance.audioManager.PlayEnemyDie();
		if (isTrucker)
		{
			GameManager.Instance.mMission.CountKillEnemyTruck++;
		}
		this.CheckMission();
	}

	protected void CheckMission()
	{
		int idWeapon = -1;
		int typeWeapon = -1;
		EWeapon eweapon = this.lastWeapon;
		switch (eweapon)
		{
		case EWeapon.SPREAD_GUN:
			idWeapon = 1;
			typeWeapon = 0;
			GameManager.Instance.mMission.weaponsSpecial[3].AddEnemy(this.ID);
			break;
		case EWeapon.FLAME:
			idWeapon = 1;
			typeWeapon = 1;
			GameManager.Instance.mMission.weaponsSpecial[0].AddEnemy(this.ID);
			break;
		case EWeapon.THUNDER:
			idWeapon = 1;
			typeWeapon = 2;
			GameManager.Instance.mMission.weaponsSpecial[5].AddEnemy(this.ID);
			break;
		case EWeapon.LASER:
			idWeapon = 1;
			typeWeapon = 3;
			GameManager.Instance.mMission.weaponsSpecial[2].AddEnemy(this.ID);
			break;
		case EWeapon.ROCKET:
			idWeapon = 1;
			typeWeapon = 4;
			GameManager.Instance.mMission.weaponsSpecial[4].AddEnemy(this.ID);
			break;
		default:
			switch (eweapon)
			{
			case EWeapon.M4A1:
				idWeapon = 0;
				typeWeapon = 0;
				GameManager.Instance.mMission.weaponsRigle[0].AddEnemy(this.ID);
				break;
			case EWeapon.MACHINE:
				idWeapon = 0;
				typeWeapon = 1;
				GameManager.Instance.mMission.weaponsRigle[1].AddEnemy(this.ID);
				break;
			case EWeapon.ICE:
				idWeapon = 0;
				typeWeapon = 2;
				GameManager.Instance.mMission.weaponsRigle[2].AddEnemy(this.ID);
				break;
			case EWeapon.SNIPER:
				idWeapon = 0;
				typeWeapon = 3;
				GameManager.Instance.mMission.weaponsRigle[1].AddEnemy(this.ID);
				break;
			case EWeapon.MGL140:
				idWeapon = 0;
				typeWeapon = 4;
				GameManager.Instance.mMission.weaponsRigle[4].AddEnemy(this.ID);
				break;
			default:
				switch (eweapon)
				{
				case EWeapon.GRENADE_M61:
					idWeapon = 2;
					typeWeapon = 0;
					GameManager.Instance.mMission.CountKillByGrenades++;
					break;
				case EWeapon.GRENADE_ICE:
					idWeapon = 2;
					typeWeapon = 1;
					GameManager.Instance.mMission.CountKillByGrenades++;
					break;
				case EWeapon.GRENADE_MOLOYOV:
					idWeapon = 2;
					typeWeapon = 2;
					GameManager.Instance.mMission.CountKillByGrenades++;
					break;
				case EWeapon.GRENADE_CHEMICAL:
					idWeapon = 2;
					typeWeapon = 3;
					GameManager.Instance.mMission.CountKillByGrenades++;
					break;
				default:
					switch (eweapon)
					{
					case EWeapon.HUMMER:
						idWeapon = 3;
						typeWeapon = 0;
						GameManager.Instance.mMission.KillEnemyByKnife++;
						break;
					case EWeapon.AXE:
						idWeapon = 3;
						typeWeapon = 1;
						GameManager.Instance.mMission.KillEnemyByKnife++;
						break;
					case EWeapon.SWORD:
						idWeapon = 3;
						typeWeapon = 2;
						GameManager.Instance.mMission.KillEnemyByKnife++;
						break;
					}
					break;
				}
				break;
			}
			break;
		}
		if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
		{
			GameManager.Instance.mMission.StartCheck();
		}
		if (this.ID == 21 || this.ID == 22)
		{
			this.ID = 20;
		}
		UnityEngine.Debug.Log("ID___" + this.ID);
		DailyQuestManager.Instance.MissionEnemy(this.ID, idWeapon, typeWeapon, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
		{
			if (isCompleted)
			{
				UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
			}
		});
		AchievementManager.Instance.MissionEnemy(this.ID, idWeapon, typeWeapon, delegate(bool isCompleted, AchievementManager.InforQuest infor)
		{
			if (isCompleted)
			{
				UIShowInforManager.Instance.OnShowAchievement(infor.Desc, infor.pathIcon);
			}
		});
	}

	private void ReductionLevelofDifficult()
	{
	}

	public void ResetIfStuck()
	{
		if (this.boxCollider)
		{
			this.boxCollider.enabled = true;
		}
		this.HP = Mathf.Max(1f, this.HP);
	}

	public Action OnEnemyDeaded;

	public Action<float, EWeapon> AddHealthPointAction;

	public ECharactor State;

	public EWeapon lastWeapon;

	protected WaitForSeconds timeHide = new WaitForSeconds(1f);

	public LineBloodEnemy lineBloodEnemy;

	public Enemy cacheEnemy;

	public EnemyDataInfo cacheEnemyData;

	public bool isInit;

	public float HP;

	[NonSerialized]
	public bool isParachuter;

	public float Radius;

	public Transform tfOrigin;

	public BoxCollider2D boxCollider;

	public Collider2D bodyCollider2D;

	public MeshRenderer meshRenderer;

	public List<BaseEnemy> ListLigntingConnected;

	public int CheckPoint;

	[SerializeField]
	protected List<Transform> tfTarget;

	private Vector2 cacheTarget;

	private float Time_Get_Target;

	private int idCacheTarget;

	protected int ID;

	protected float speedPingpong;

	protected float timePingPongColor = 1f;

	protected Color colorHit = Color.red;

	protected BaseEnemy.ELevel eLevel;

	protected int TIME_UPGRADE_TO_HARD;

	protected int TIME_UPGRADE_TO_SUPER_HARD;

	protected float TimeCounterUpgradeEnemy;

	public bool isCreateWithJson;

	protected bool hasShow;

	public enum ELevel
	{
		NORMAL,
		HARD,
		SUPER_HARD
	}
}
