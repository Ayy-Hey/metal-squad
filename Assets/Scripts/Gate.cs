using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using UnityEngine;

public class Gate : BaseEnemy
{
	public override void Hit(float damage)
	{
		base.Hit(damage);
		this.timePingPongColor = 0f;
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			this.isAutoCreateEnemy = false;
			this.State = ECharactor.DIE;
			base.CheckMission();
			base.StartCoroutine(this.StartEffect());
		}
	}

	private void Start()
	{
		BaseTrigger baseTrigger = this.zoomCameraPro;
		baseTrigger.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger.OnEnteredTrigger, new Action(delegate()
		{
			this.Init();
		}));
		BaseTrigger baseTrigger2 = this.zoomCameraPro;
		baseTrigger2.OnExitedTrigger = (Action)Delegate.Combine(baseTrigger2.OnExitedTrigger, new Action(delegate()
		{
			if (this.State == ECharactor.DIE)
			{
			}
		}));
	}

	private void LateUpdate()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.sprite.color = base.PingPongColor();
		if (!this.isAutoCreateEnemy)
		{
			return;
		}
		this.timeCreateEnemy += Time.deltaTime;
		if (this.timeCreateEnemy > 5f)
		{
			this.timeCreateEnemy = 0f;
			this.AutoCreateEnemy();
		}
	}

	public void Init()
	{
		if (this.State == ECharactor.DIE || this.isInit)
		{
			return;
		}
		this.timeStartEffectDelay = new WaitForSeconds(0.5f);
		if (!this.avaliablePass)
		{
			DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy++;
		}
		if (!GameManager.Instance.ListEnemy.Contains(this))
		{
			GameManager.Instance.ListEnemy.Add(this);
		}
		this.State = ECharactor.IDLE;
		Enemy[] array = new Enemy[]
		{
			new Enemy()
		};
		this.HP = (array[0].HP = this.HP_Current);
		this.InitEnemy(new EnemyCharactor
		{
			enemy = array
		}, 0);
		this.isInit = true;
		this.ID = 200001;
		this.colorHit = new Color(83f, 255f, 0f, 1f);
		base.isInCamera = true;
		this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
	}

	private IEnumerator StartEffect()
	{
		int number = UnityEngine.Random.Range(2, 5);
		if (this.tfTarget.Count <= 1)
		{
			number = 0;
			UnityEngine.Debug.Log("ShowEffet");
			GameManager.Instance.fxManager.ShowEffect(4, this.transform.position, Vector3.one, true, true);
			yield return this.timeStartEffectDelay;
		}
		for (int i = 0; i < number; i++)
		{
			GameManager.Instance.fxManager.ShowEffect(4, this.tfTarget[UnityEngine.Random.Range(0, this.tfTarget.Count)].position, Vector3.one, true, true);
			yield return this.timeStartEffectDelay;
		}
		if (!this.avaliablePass)
		{
			DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy--;
		}
		if (this.gate != null)
		{
			this.gate.isStartRun = true;
			GameManager.Instance.player.isAutoRun = true;
			GameManager.Instance.StateManager.EState = EGamePlay.PAUSE;
			GameManager.Instance.hudManager.HideControl();
			GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
			GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
		}
		else
		{
			CameraController.Instance.NewCheckpoint(true, 15f);
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	private void OnDisable()
	{
		try
		{
			GameManager.Instance.ListEnemy.Remove(this);
		}
		catch
		{
		}
		this.isInit = false;
		base.StopAllCoroutines();
	}

	private void AutoCreateEnemy()
	{
		int num = UnityEngine.Random.Range(1, 2);
		float num2 = UnityEngine.Random.Range(0f, 1f);
		if (num == 1)
		{
			if (num2 > 0.5f)
			{
				this.CreateEnemyKnife();
			}
			else
			{
				this.CreateEnemyGrenade();
			}
		}
		else
		{
			this.CreateEnemyKnife();
			this.CreateEnemyGrenade();
		}
	}

	private void CreateEnemyKnife()
	{
		int levelEnemy = this.LevelEnemy;
		Vector2 vector = default(Vector2);
		vector.x = CameraController.Instance.Position.x - CameraController.Instance.Size().x - 2f;
		vector.y = 0f;
		EnemyKnife enemy = EnemyManager.Instance.CreateEnemyKnife();
		this.cacheEnemyData = new EnemyDataInfo();
		this.cacheEnemyData.pos_x = vector.x;
		this.cacheEnemyData.pos_y = vector.y;
		this.cacheEnemyData.ismove = true;
		this.cacheEnemyData.type = 7;
		enemy.Init(this.cacheEnemyData, delegate
		{
			EnemyManager.Instance.PoolEnemyKnife.Store(enemy);
		});
		enemy.cacheEnemy.Vision_Min = 0.5f;
		enemy.cacheEnemy.Vision_Max = 8f;
		GameManager.Instance.ListEnemy.Add(enemy);
	}

	private void CreateEnemyGrenade()
	{
		int levelEnemy = this.LevelEnemy;
		Vector2 v = default(Vector2);
		v.x = CameraController.Instance.Position.x - CameraController.Instance.Size().x - 2f;
		v.y = 0f;
		EnemyGrenade enemy = EnemyManager.Instance.CreateEnemyGrenade();
		EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
		enemyDataInfo.ismove = true;
		enemyDataInfo.type = 2;
		enemyDataInfo.pos_x = v.x;
		enemyDataInfo.pos_y = v.y;
		enemy.Init(enemyDataInfo, delegate
		{
			EnemyManager.Instance.PoolEnemyGrenade.Store(enemy);
		});
		enemy.transform.position = v;
		enemy.cacheEnemy.Vision_Min = 0.5f;
		enemy.cacheEnemy.Vision_Max = 8f;
		GameManager.Instance.ListEnemy.Add(enemy);
	}

	[SerializeField]
	private float HP_Current = 200f;

	[SerializeField]
	private BaseTrigger zoomCameraPro;

	[SerializeField]
	private GateCampaign gate;

	[SerializeField]
	private float RightBoundaryCameraStop;

	private WaitForSeconds timeStartEffectDelay;

	[SerializeField]
	private bool avaliablePass;

	[SerializeField]
	private SpriteRenderer sprite;

	[Header("Auto Create Enemy")]
	[SerializeField]
	private bool isAutoCreateEnemy;

	private float timeCreateEnemy;

	[SerializeField]
	private int LevelEnemy = 1;
}
