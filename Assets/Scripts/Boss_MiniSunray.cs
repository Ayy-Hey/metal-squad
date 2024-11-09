using System;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_MiniSunray : BaseBoss
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
		this.mainCamera = Camera.main;
	}

	public void InitEnemy()
	{
		UnityEngine.Debug.Log("InitEnemy");
		this.LoadBossData();
		this.InitSkeletonAnimation();
		this.InitAnimations();
		float mode = GameMode.Instance.GetMode();
		this.bossData.enemy[0].Damage *= mode;
		this.InitEnemy(this.bossData, 0);
		base.isInCamera = true;
		base.gameObject.transform.parent = null;
		this.bossState = Boss_MiniSunray.MiniSunrayState.IDLE;
		this.changeState = false;
		this.ClearMiniDoneFlag();
		this.UpdateBossState();
		this.ramboTransform = GameManager.Instance.player.transform;
		this.isInit = true;
		this.meshRenderer.sortingOrder = this.afterInitOrderInLayer;
		this.meshRenderer.sortingLayerName = this.afterInitLayer;
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
		this.UpdateCameraCornerPosition();
		if (this.ChangeBossState())
		{
			this.changeState = true;
		}
		else
		{
			this.changeState = false;
		}
		if (this.changeState)
		{
			this.UpdateBossState();
			this.changeState = false;
		}
		this.UpdateBossPosition();
	}

	private void UpdateBossPosition()
	{
		this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.moveSpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
	}

	private void UpdateCameraCornerPosition()
	{
		this.bottomLeftCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, this.mainCamera.nearClipPlane));
		this.topLeftCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3(0f, (float)this.mainCamera.pixelHeight, this.mainCamera.nearClipPlane));
		this.bottomRightCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3((float)this.mainCamera.pixelWidth, 0f, this.mainCamera.nearClipPlane));
		this.topRightCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3((float)this.mainCamera.pixelWidth, (float)this.mainCamera.pixelHeight, this.mainCamera.nearClipPlane));
		this.bottomLeftCameraPos += new Vector3(0f, 0f, -this.bottomLeftCameraPos.z + this.transform.position.z);
		this.topLeftCameraPos += new Vector3(0f, 0f, -this.topLeftCameraPos.z + this.transform.position.z);
		this.bottomRightCameraPos += new Vector3(0f, 0f, -this.bottomRightCameraPos.z + this.transform.position.z);
		this.topRightCameraPos += new Vector3(0f, 0f, -this.topRightCameraPos.z + this.transform.position.z);
	}

	private void UpdateCachePosition()
	{
		if (GameManager.Instance.player.IsInVisible())
		{
			this.ramboPosition = new Vector3(UnityEngine.Random.Range(this.topLeftCameraPos.x, this.topRightCameraPos.x), this.ramboTransform.position.y, this.ramboTransform.position.z);
		}
		else
		{
			this.ramboPosition = this.ramboTransform.position;
		}
	}

	private void UpdateBossState()
	{
		UnityEngine.Debug.Log("bossState: " + this.bossState);
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		Boss_MiniSunray.MiniSunrayState miniSunrayState = this.bossState;
		if (miniSunrayState != Boss_MiniSunray.MiniSunrayState.ATTACK)
		{
			if (miniSunrayState != Boss_MiniSunray.MiniSunrayState.IDLE)
			{
				this.OnIdle();
			}
			else
			{
				this.OnIdle();
			}
		}
		else
		{
			this.OnAttak();
		}
	}

	private void OnAttak()
	{
		this.transform.position = this.bigBossTransform.position;
		this.meshRenderer.enabled = true;
		this.Attack_Box.Active(this.damagePercent * this.cacheEnemy.Damage, true, null);
		Vector3 a = this.ramboPosition - this.bigBossTransform.position;
		Vector3 a2 = this.bigBossTransform.position + 0.3f * a;
		float x = Mathf.Cos(this.angle * 0.0174532924f) * 2f;
		float y = Mathf.Sin(this.angle * 0.0174532924f) * 2f;
		Vector3 vector = a2 + new Vector3(x, y, this.bigBossTransform.position.z);
		a = this.ramboPosition + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f) - vector;
		Vector3 end = vector + 3f * a;
		this.CreateMoveLine(vector, end);
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		this.timerLeft = this.timer;
		this.PlayAnim(Boss_MiniSunray.MiniSunrayState.ATTACK, true, 0.5f);
	}

	private void OnIdle()
	{
		this.meshRenderer.enabled = false;
		this.Attack_Box.Deactive();
	}

	private bool ChangeBossState()
	{
		if (this.State == ECharactor.DIE)
		{
			return false;
		}
		bool flag = false;
		Boss_MiniSunray.MiniSunrayState miniSunrayState = this.bossState;
		if (miniSunrayState != Boss_MiniSunray.MiniSunrayState.ATTACK)
		{
			if (miniSunrayState != Boss_MiniSunray.MiniSunrayState.IDLE)
			{
				flag = false;
			}
			else if (this.TriggerAttack())
			{
				this.bossState = Boss_MiniSunray.MiniSunrayState.ATTACK;
				flag = true;
			}
		}
		else if (this.DoneMovePath())
		{
			this.bossState = Boss_MiniSunray.MiniSunrayState.IDLE;
			flag = true;
		}
		if (flag)
		{
			UnityEngine.Debug.Log("Mini Boss Change state to " + this.bossState);
			this.ClearMiniDoneFlag();
		}
		return flag;
	}

	private bool TriggerAttack()
	{
		return this.triggerMiniAttack;
	}

	private void ClearMiniDoneFlag()
	{
		this.triggerMiniAttack = false;
	}

	private void CreateMovePath(int pathCount, float xMin, float yMin, float xMax, float yMax, float z)
	{
		this.movePath = new Vector3[pathCount];
		for (int i = 0; i < pathCount; i++)
		{
			this.movePath[i] = new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), z);
		}
	}

	private void CreateMoveLine(Vector3 start, Vector3 end)
	{
		this.movePath = new Vector3[2];
		this.movePath[0] = start;
		this.movePath[1] = end;
	}

	private bool DoneMovePath()
	{
		bool result = false;
		if (this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.timerLeft -= Time.fixedDeltaTime;
			if (this.timerLeft < 0f)
			{
				this.moveStep++;
				if (this.moveStep < this.movePath.Length)
				{
					this.PlayAnim(Boss_MiniSunray.MiniSunrayState.ATTACK, true, 3f);
					this.targetRun = this.movePath[this.moveStep];
					result = false;
				}
				else
				{
					result = true;
				}
			}
		}
		return result;
	}

	private bool ReachedToTarget(Vector3 curPos, Vector3 target)
	{
		return Vector2.Distance(curPos, target) < 0.1f;
	}

	private void InitSkeletonAnimation()
	{
		UnityEngine.Debug.Log("InitSkeletonAnimation");
		this.skeletonAnimation.timeScale = 0f;
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

	private void PlayAnim(Boss_MiniSunray.MiniSunrayState state, bool loop = false, float speedAnim = 1f)
	{
		UnityEngine.Debug.Log("Play anim: " + (int)state);
		this.skeletonAnimation.timeScale = speedAnim;
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.animations[(int)state], loop);
	}

	private void PlayAnim(Boss_MiniSunray.MiniSunrayState state, int order)
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

	private void LoadBossData()
	{
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		this.bossData = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
	}

	[SerializeField]
	[Header("**************** Boss_MiniSunray ***************")]
	private TextAsset bossDataText;

	private EnemyCharactor bossData;

	[SerializeField]
	[Header("============ Spine Anim ===========")]
	private SkeletonAnimation skeletonAnimation;

	private float cacheTimeScale;

	private Spine.Animation[] animations;

	public Transform bigBossTransform;

	[SerializeField]
	private string afterInitLayer;

	[SerializeField]
	private int afterInitOrderInLayer;

	private Camera mainCamera;

	private Vector3 topLeftCameraPos;

	private Vector3 topRightCameraPos;

	private Vector3 bottomLeftCameraPos;

	private Vector3 bottomRightCameraPos;

	[Header("============ Attack ===========")]
	[SerializeField]
	private AttackBox Attack_Box;

	[SerializeField]
	private float damagePercent;

	[SerializeField]
	private float timer;

	private float timerLeft;

	[SerializeField]
	private float moveSpeedPercent;

	public float angle;

	private Vector3 targetRun;

	private Vector3[] movePath;

	private int moveStep;

	private Transform ramboTransform;

	private Vector3 ramboPosition;

	[Header("============ State ===============")]
	public Boss_MiniSunray.MiniSunrayState bossState;

	public bool triggerMiniAttack;

	private bool changeState;

	private bool doneAttack;

	public enum MiniSunrayState
	{
		ATTACK,
		IDLE
	}
}
