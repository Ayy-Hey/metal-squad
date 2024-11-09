using System;
using System.Collections;
using System.Collections.Generic;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_Mini_MaraDevil : BaseBoss
{
	private void Start()
	{
		this.Init();
	}

	public override void Init()
	{
		UnityEngine.Debug.Log("Init");
		base.Init();
		this.InitEnemy();
	}

	public void InitEnemy()
	{
		UnityEngine.Debug.Log("InitEnemy");
		this.LoadBossData();
		this.InitSkeletonAnimation();
		this.InitAnimations();
		this.InitBullets();
		base.gameObject.transform.parent = null;
		this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_IDLE;
		this.changeState = false;
		this.rotateTimer = (float)UnityEngine.Random.Range(0, 180);
		this.ClearMiniDoneFlag();
		this.UpdateMiniBossState();
		float mode = GameMode.Instance.GetMode();
		this.bossData.enemy[0].Damage *= mode;
		this.isInit = true;
		this.ramboTransform = GameManager.Instance.player.transform;
	}

	private void InitBullets()
	{
		UnityEngine.Debug.Log("InitBullets");
		if (this.listBulletBoss != null)
		{
			this.listBulletBoss[0].gameObject.transform.parent.parent = null;
			this.poolingBulletsBoss = new ObjectPooling<StraightBullet>(this.listBulletBoss.Count, null, null);
			for (int i = 0; i < this.listBulletBoss.Count; i++)
			{
				this.poolingBulletsBoss.Store(this.listBulletBoss[i]);
			}
		}
		else
		{
			UnityEngine.Debug.Log("listBulletBoss == null");
		}
	}

	private void UpdateBullet(float deltaTime)
	{
		for (int i = 0; i < this.listBulletBoss.Count; i++)
		{
			if (this.listBulletBoss[i].isInit)
			{
				this.listBulletBoss[i].UpdateObject(deltaTime);
			}
		}
	}

	private IEnumerator CreatBullet()
	{
		float startAngle = UnityEngine.Random.Range(220f, 230f);
		for (int i = 0; i < this.bulletCnt; i++)
		{
			this._bullet = null;
			this._bullet = this.poolingBulletsBoss.New();
			if (this._bullet == null)
			{
				this._bullet = UnityEngine.Object.Instantiate<StraightBullet>(this.listBulletBoss[0], this.transform.position, Quaternion.identity);
				this._bullet.gameObject.transform.parent = this.listBulletBoss[0].gameObject.transform.parent;
				this.listBulletBoss.Add(this._bullet);
			}
			float angle = (startAngle + (float)i * this.deltaAngle) * 0.0174532924f;
			float x = Mathf.Cos(angle) * this.spawnCircleRadius;
			float y = Mathf.Sin(angle) * this.spawnCircleRadius;
			Vector3 director = new Vector3(x, y, 0f);
			this._bullet.InitObject(this.ATTACK_1_DamagePercent * this.bossData.enemy[0].Damage, this.ATTACK_1_SpeedPercent * this.bossData.enemy[0].Speed, 0f, this.transform.position + director, director.normalized, new Action<StraightBullet>(this.OnHideBullet));
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	private void OnHideBullet(StraightBullet bullet)
	{
		this.poolingBulletsBoss.Store(bullet);
	}

	private void LoadBossData()
	{
		UnityEngine.Debug.Log("LoadBossData");
		try
		{
			if (!this.bossDataText)
			{
				string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
				string text2 = ProfileManager.DataEncryption.decrypt2(text);
				this.bossData = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("[ERROR] Fail to load boss data: " + ex.Message);
		}
	}

	private void Update()
	{
		if (this.PauseCondition())
		{
			this.PauseGame();
			return;
		}
		this.ResumeGame();
		this.UpdateCachePosition();
		if (this.miniBossState == Boss_Mini_MaraDevil.MiniState.MINI_IDLE)
		{
			this.RotateAroundTarget(this.circleRadius);
		}
		else
		{
			this.FollowTarget();
		}
		if (this.ChangeMiniBossState())
		{
			this.changeState = true;
		}
		else
		{
			this.changeState = false;
		}
		if (this.changeState)
		{
			this.UpdateMiniBossState();
			this.changeState = false;
		}
		this.UpdateBullet(Time.deltaTime);
	}

	private void UpdateCachePosition()
	{
		this.ramboPosition = this.ramboTransform.position;
	}

	private void UpdateMiniBossState()
	{
		UnityEngine.Debug.Log("miniBossState: " + this.miniBossState);
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		switch (this.miniBossState)
		{
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_1:
			this.OnMINI_ATTACK_1();
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_1:
			this.OnMINI_ATTACK_2_1();
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_2:
			this.OnMINI_ATTACK_2_2();
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_3:
			this.OnMINI_ATTACK_2_3();
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_4:
			this.OnMINI_ATTACK_2_4();
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_IDLE:
			this.OnMINI_IDLE();
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_THONG_BAO:
			this.OnMINI_THONG_BAO();
			break;
		default:
			this.OnMINI_IDLE();
			break;
		}
	}

	private bool ChangeMiniBossState()
	{
		if (this.State == ECharactor.DIE)
		{
			return false;
		}
		bool flag = false;
		switch (this.miniBossState)
		{
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_1:
			if (this.TriggerIDLE())
			{
				if (this.ATTACK_1_Current_Count < this.ATTACK_1_Count)
				{
					flag = true;
				}
				else
				{
					this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_IDLE;
					flag = true;
				}
			}
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_1:
			if (this.TriggerMINI_ATTACK_2_2())
			{
				this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_2;
				flag = true;
			}
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_2:
			if (this.TriggerMINI_ATTACK_2_3())
			{
				if (!this.DoneReachTarget())
				{
					flag = true;
				}
				else
				{
					this.cntMINI_ATTACK_2_3 = 3;
					this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_3;
					flag = true;
				}
			}
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_3:
			if (this.TriggerMINI_ATTACK_2_4())
			{
				if (this.cntMINI_ATTACK_2_3 > 0)
				{
					this.cntMINI_ATTACK_2_3--;
					flag = true;
				}
				else
				{
					this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_4;
					flag = true;
				}
			}
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_4:
			if (this.TriggerIDLE())
			{
				this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_IDLE;
				flag = true;
			}
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_IDLE:
			if (this.TriggerMINI_ATTACK_1())
			{
				this.meshRenderer.sortingOrder = this.afterInitOrderInLayer;
				this.meshRenderer.sortingLayerName = this.afterInitLayer;
				this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_1;
				this.ATTACK_1_Current_Count = 0;
				flag = true;
			}
			else if (this.TriggerMINI_ATTACK_2_1())
			{
				this.meshRenderer.sortingOrder = this.afterInitOrderInLayer;
				this.meshRenderer.sortingLayerName = this.afterInitLayer;
				this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_1;
				flag = true;
			}
			else if (this.TriggerMINI_THONG_BAO())
			{
				this.MINI_THONG_BAO_Current_Count = 0f;
				this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_THONG_BAO;
				flag = true;
			}
			else if (this.TriggerIDLE())
			{
				this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_IDLE;
				flag = true;
			}
			break;
		case Boss_Mini_MaraDevil.MiniState.MINI_THONG_BAO:
			if (this.TriggerIDLE())
			{
				if (this.MINI_THONG_BAO_Current_Count < this.MINI_THONG_BAO_Count)
				{
					flag = true;
				}
				else
				{
					this.miniBossState = Boss_Mini_MaraDevil.MiniState.MINI_IDLE;
					flag = true;
				}
			}
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			UnityEngine.Debug.Log("Mini Boss Change state to " + this.miniBossState);
			this.ClearMiniDoneFlag();
		}
		return flag;
	}

	private bool TriggerIDLE()
	{
		return this.doneMINI_ATTACK_1 || this.doneMINI_ATTACK_2_4 || this.doneMINI_IDLE || this.doneMINI_THONG_BAO;
	}

	private bool TriggerMINI_ATTACK_1()
	{
		return this.triggerMINI_ATTACK_1;
	}

	private bool TriggerMINI_ATTACK_2_1()
	{
		return this.triggerMINI_ATTACK_2_1;
	}

	private bool TriggerMINI_ATTACK_2_2()
	{
		return this.doneMINI_ATTACK_2_1;
	}

	private bool TriggerMINI_ATTACK_2_3()
	{
		return this.doneMINI_ATTACK_2_2;
	}

	private bool TriggerMINI_ATTACK_2_4()
	{
		return this.doneMINI_ATTACK_2_3;
	}

	private bool TriggerMINI_THONG_BAO()
	{
		return this.triggerMINI_THONG_BAO;
	}

	private void OnMINI_ATTACK_1()
	{
		this.ATTACK_1_Current_Count++;
		this.SetTargetPosition(this.bigBossTransform.position);
		this.SetAttackSpeed(1f);
		this.PlayAnim(Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_1, false, 1f);
		base.StartCoroutine(this.CreatBullet());
		this.PlaySound(this.ATTACK_1_Clip, this.ATTACK_1_Volume);
	}

	private void OnMINI_ATTACK_2_1()
	{
		this.ATTACKBox.Active(this.ATTACK_2_DamagePercent * this.bossData.enemy[0].Damage, true, null);
		this.SetTargetPosition(new Vector3(this.transform.position.x + UnityEngine.Random.Range(-2f, 2f), this.transform.position.y + 1f, this.transform.position.z));
		this.SetAttackSpeed(0.5f);
		this.PlayAnim(Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_1, false, 1f);
		this.PlaySound(this.ATTACK_2_1_Clip, this.ATTACK_2_Volume);
	}

	private void OnMINI_ATTACK_2_2()
	{
		this.SetTargetPosition(new Vector3(this.transform.position.x + (float)UnityEngine.Random.Range(-1, 1), this.minYPos, this.transform.position.z));
		this.SetAttackSpeed(5f);
		this.PlayAnim(Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_2, false, 1f);
		this.PlaySound(this.ATTACK_2_2_Clip, this.ATTACK_2_Volume);
	}

	private void OnMINI_ATTACK_2_3()
	{
		this.SetTargetPosition(new Vector3(this.ramboPosition.x + ((UnityEngine.Random.Range(0, 10) % 2 != 0) ? 100f : -100f), this.minYPos, this.transform.position.z));
		this.SetAttackSpeed(((UnityEngine.Random.Range(1, 10) % 2 != 0) ? 0f : this.ATTACK_1_SpeedPercent) * this.bossData.enemy[0].Speed);
		this.PlayAnim(Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_3, false, 1f);
		this.PlaySound(this.ATTACK_2_3_Clip, this.ATTACK_2_Volume);
	}

	private void OnMINI_ATTACK_2_4()
	{
		this.SetTargetPosition(this.bigBossTransform.position);
		this.SetAttackSpeed(30f);
		this.PlayAnim(Boss_Mini_MaraDevil.MiniState.MINI_ATTACK_2_4, false, 1f);
		this.PlaySound(this.ATTACK_2_1_Clip, this.ATTACK_2_Volume);
	}

	private void OnMINI_IDLE()
	{
		this.ATTACKBox.Deactive();
		this.SetTargetPosition(this.bigBossTransform.position);
		this.SetAttackSpeed(5f);
		this.PlayAnim(Boss_Mini_MaraDevil.MiniState.MINI_IDLE, false, 1f);
	}

	private void OnMINI_THONG_BAO()
	{
		this.MINI_THONG_BAO_Current_Count += 1f;
		this.SetTargetPosition(this.bigBossTransform.position);
		this.SetAttackSpeed(1f);
		this.PlayAnim(Boss_Mini_MaraDevil.MiniState.MINI_THONG_BAO, false, 1f);
		this.PlaySound(this.THONG_BAO_Clip, this.THONG_BAO_Volume);
	}

	private void SetTargetPosition(Vector3 position)
	{
		this.targetPosition = position;
	}

	private void SetAttackSpeed(float speed)
	{
		this.attackSpeed = speed;
	}

	private bool DoneReachTarget()
	{
		return Vector2.Distance(this.transform.position, this.targetPosition) < 0.5f;
	}

	private void FollowTarget()
	{
		this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition, Time.fixedDeltaTime * this.attackSpeed);
	}

	private void RotateAroundTarget(float radius)
	{
		this.rotateTimer += Time.fixedDeltaTime;
		float x = Mathf.Cos(this.rotateTimer) * radius;
		float y = Mathf.Sin(this.rotateTimer) * radius;
		this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition + new Vector3(x, y, 0f), Time.fixedDeltaTime * this.attackSpeed);
	}

	private void ClearMiniDoneFlag()
	{
		this.doneMINI_ATTACK_1 = false;
		this.doneMINI_ATTACK_2_1 = false;
		this.doneMINI_ATTACK_2_2 = false;
		this.doneMINI_ATTACK_2_3 = false;
		this.doneMINI_ATTACK_2_4 = false;
		this.doneMINI_IDLE = false;
		this.doneMINI_THONG_BAO = false;
		this.triggerMINI_ATTACK_1 = false;
		this.triggerMINI_ATTACK_2_1 = false;
		this.triggerMINI_THONG_BAO = false;
	}

	private void InitSkeletonAnimation()
	{
		UnityEngine.Debug.Log("InitSkeletonAnimation");
		this.skeletonAnimation.timeScale = 0f;
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		UnityEngine.Debug.Log("HandleEvent");
	}

	private void HandleComplete(TrackEntry entry)
	{
		UnityEngine.Debug.Log("HandleComplete: " + entry.ToString());
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString();
		switch (text)
		{
		case "attack1":
			this.doneMINI_ATTACK_1 = true;
			break;
		case "attack2-1":
			this.doneMINI_ATTACK_2_1 = true;
			break;
		case "attack2-2":
			this.doneMINI_ATTACK_2_2 = true;
			break;
		case "attack2-3":
			this.doneMINI_ATTACK_2_3 = true;
			break;
		case "attack2-4":
			this.doneMINI_ATTACK_2_4 = true;
			break;
		case "idel":
			this.doneMINI_IDLE = true;
			break;
		case "thong bao":
			this.doneMINI_THONG_BAO = true;
			break;
		}
	}

	private bool PauseCondition()
	{
		return !this.isInit || this.State == ECharactor.DIE || (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW) || GameManager.Instance.StateManager.EState == EGamePlay.PAUSE;
	}

	private void PauseGame()
	{
		if (this.skeletonAnimation.timeScale != 0f)
		{
			this.cacheTimeScale = this.skeletonAnimation.timeScale;
			this.skeletonAnimation.timeScale = 0f;
		}
	}

	private void ResumeGame()
	{
		if (this.skeletonAnimation.timeScale == 0f)
		{
			this.skeletonAnimation.timeScale = this.cacheTimeScale;
		}
	}

	private void InitAnimations()
	{
		this.animations = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		UnityEngine.Debug.Log("Num of animations: " + this.animations.Length);
	}

	private void PlayAnim(Boss_Mini_MaraDevil.MiniState state, bool loop = false, float speedAnim = 1f)
	{
		UnityEngine.Debug.Log("Play anim: " + (int)state);
		this.skeletonAnimation.timeScale = speedAnim;
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.animations[(int)state], loop);
	}

	private void PlayAnim(Boss_Mini_MaraDevil.MiniState state, int order)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(order, this.animations[(int)state], false);
	}

	private void PlaySound(AudioClip sound, float volume = 1f)
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(sound, volume);
		}
		catch
		{
		}
	}

	[Header("**************** Boss_Mini_MaraDevil ***************")]
	[SerializeField]
	private TextAsset bossDataText;

	private EnemyCharactor bossData;

	[Header("============ Spine Anim ===========")]
	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	private float cacheTimeScale;

	private Spine.Animation[] animations;

	public Transform bigBossTransform;

	[SerializeField]
	private int afterInitOrderInLayer;

	[SerializeField]
	private string afterInitLayer;

	[Header("============ Attack ===============")]
	[SerializeField]
	private Transform ramboTransform;

	[SerializeField]
	private float circleRadius;

	private Vector3 ramboPosition;

	private Vector3 targetPosition;

	private float attackSpeed;

	private float rotateTimer;

	[Header("__________ Attack 1")]
	[SerializeField]
	private float ATTACK_1_DamagePercent;

	[SerializeField]
	private float ATTACK_1_SpeedPercent;

	[SerializeField]
	private int bulletCnt;

	[SerializeField]
	private List<StraightBullet> listBulletBoss;

	private ObjectPooling<StraightBullet> poolingBulletsBoss;

	private StraightBullet _bullet;

	[SerializeField]
	private float deltaAngle;

	[SerializeField]
	private float spawnCircleRadius;

	[SerializeField]
	private int ATTACK_1_Count;

	private int ATTACK_1_Current_Count;

	[SerializeField]
	private AudioClip ATTACK_1_Clip;

	[SerializeField]
	private float ATTACK_1_Volume;

	[Header("__________ Attack 2")]
	public float minYPos;

	[SerializeField]
	private AttackBox ATTACKBox;

	[SerializeField]
	private float ATTACK_2_DamagePercent;

	[SerializeField]
	private float ATTACK_2_SpeedPercent;

	[SerializeField]
	private AudioClip ATTACK_2_1_Clip;

	[SerializeField]
	private AudioClip ATTACK_2_2_Clip;

	[SerializeField]
	private AudioClip ATTACK_2_3_Clip;

	[SerializeField]
	private AudioClip ATTACK_2_4_Clip;

	[SerializeField]
	private float ATTACK_2_Volume;

	[SerializeField]
	[Header("__________ Thong Bao")]
	private float MINI_THONG_BAO_Count;

	private float MINI_THONG_BAO_Current_Count;

	[SerializeField]
	private AudioClip THONG_BAO_Clip;

	[SerializeField]
	private float THONG_BAO_Volume;

	[SerializeField]
	[Header("============ State ===============")]
	private bool isRightMiniBoss;

	public Boss_Mini_MaraDevil.MiniState miniBossState;

	public bool triggerMINI_ATTACK_1;

	public bool triggerMINI_ATTACK_2_1;

	public bool triggerMINI_THONG_BAO;

	private bool changeState;

	private bool doneMINI_ATTACK_1;

	private bool doneMINI_ATTACK_2_1;

	private bool doneMINI_ATTACK_2_2;

	private bool doneMINI_ATTACK_2_3;

	private int cntMINI_ATTACK_2_3;

	private bool doneMINI_ATTACK_2_4;

	private bool doneMINI_IDLE;

	private bool doneMINI_THONG_BAO;

	public enum MiniState
	{
		MINI_ATTACK_1,
		MINI_ATTACK_2_1,
		MINI_ATTACK_2_2,
		MINI_ATTACK_2_3,
		MINI_ATTACK_2_4,
		MINI_IDLE,
		MINI_THONG_BAO
	}
}
