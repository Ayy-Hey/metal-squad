using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrossAdPlugin;
using Photon.Pun;
using PVPManager;
using StarMission;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance
	{
		get
		{
			if (GameManager.instance == null)
			{
				GameManager.instance = UnityEngine.Object.FindObjectOfType<GameManager>();
			}
			return GameManager.instance;
		}
	}

	public bool isRescue { get; set; }

	private void OnDisable()
	{
		GameManager.isPlay = false;
	}

	private IEnumerator Start()
	{
		AdmobManager.Instance.RequestInterstitial();
		this.ListEnemy = new List<BaseEnemy>();
		GameManager.isPlay = true;
		try
		{
			this.hudManager.textTime.transform.parent.gameObject.SetActive(false);
		}
		catch
		{
		}
		this.FakeLoading.SetActive(true);
		UnityEngine.Object.Instantiate(Resources.Load("Popup/UltimateCanvas", typeof(GameObject)));
		this.textFakeLoading.text = "91%";
		if (GameMode.Instance.modePlay != GameMode.ModePlay.CoOpMode)
		{
			this.CreateRambo();
		}
		yield return new WaitForSeconds(0.2f);
		try
		{
			this.TotalEnemyKilled = 0;
			Singleton<CrossAdsManager>.Instance.HideFloatAds();
			PopupManager.Instance.SetCanvas(this.mCanvas);
			this.FakeLoading.SetActive(true);
		}
		catch
		{
			UnityEngine.Debug.LogError("request ads failed");
		}
		this.textFakeLoading.text = "92%";
		this.MAX_COIN_DROP = 0;
		this.mMission = new Mission(this);
		this.tfAutoTarget.position = Vector2.one * 999f;
		string mode = string.Empty;
		GameMode.GameStyle style = GameMode.Instance.Style;
		if (style != GameMode.GameStyle.SinglPlayer)
		{
			if (style == GameMode.GameStyle.MultiPlayer)
			{
				GameManager.Instance.isRescue = true;
				GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
				if (modePlay != GameMode.ModePlay.PvpMode)
				{
					if (modePlay == GameMode.ModePlay.CoOpMode)
					{
						yield return base.StartCoroutine(CoOpManager.Instance.InstantiateCoOpMap());
					}
				}
				else
				{
					yield return base.StartCoroutine(PVPManager.PVPManager.Instance.InstantiatePvpMap(0));
				}
			}
		}
		else
		{
			switch (GameMode.Instance.modePlay)
			{
			case GameMode.ModePlay.Campaign:
			{
				GameMode.Mode mode2 = GameMode.Instance.MODE;
				if (mode2 != GameMode.Mode.NORMAL)
				{
					if (mode2 != GameMode.Mode.HARD)
					{
						if (mode2 == GameMode.Mode.SUPER_HARD)
						{
							this.MAX_COIN_DROP = DataLoader.missionDataRoot_SuperHard[(int)ProfileManager.eLevelCurrent].missionDataLevel.Gold_Earn.Value;
						}
					}
					else
					{
						this.MAX_COIN_DROP = DataLoader.missionDataRoot_Hard[(int)ProfileManager.eLevelCurrent].missionDataLevel.Gold_Earn.Value;
					}
				}
				else
				{
					this.MAX_COIN_DROP = DataLoader.missionDataRoot_Normal[(int)ProfileManager.eLevelCurrent].missionDataLevel.Gold_Earn.Value;
				}
				PreGameOver.Instance.OnInit();
				this.bossManager.Init();
				DataLoader.LoadData();
				this.Level = ProfileManager.eLevelCurrent;
				ProfileManager.CountGamePlay.setValue(ProfileManager.CountGamePlay.Data.Value + 1);
				this.RatePower = PowerManager.Instance.RatePower((float)PowerManager.Instance.TotalPowerRequireCampaign(GameMode.Instance.MODE, (int)this.Level));
				break;
			}
			case GameMode.ModePlay.Boss_Mode:
				this.bossManager.Init();
				this.RatePower = PowerManager.Instance.RatePower((float)PowerManager.Instance.TotalPowerRequireCampaign(GameMode.Instance.EMode, (int)ProfileManager.eLevelCurrent));
				break;
			case GameMode.ModePlay.Special_Campaign:
			{
				this.Level = ProfileManager.eLevelCurrent;
				GameMode.Mode mode3 = GameMode.Instance.MODE;
				if (mode3 != GameMode.Mode.NORMAL)
				{
					if (mode3 != GameMode.Mode.HARD)
					{
						if (mode3 == GameMode.Mode.SUPER_HARD)
						{
							this.MAX_COIN_DROP = DataLoader.missionDataRoot_SuperHard_S[this.Level - ELevel.LEVEL_S0].missionDataLevel.Gold_Earn.Value;
						}
					}
					else
					{
						this.MAX_COIN_DROP = DataLoader.missionDataRoot_Hard_S[this.Level - ELevel.LEVEL_S0].missionDataLevel.Gold_Earn.Value;
					}
				}
				else
				{
					this.MAX_COIN_DROP = DataLoader.missionDataRoot_Normal_S[this.Level - ELevel.LEVEL_S0].missionDataLevel.Gold_Earn.Value;
				}
				Log.Info(string.Concat(new object[]
				{
					"max coin in special campaign:",
					this.Level,
					"__",
					this.MAX_COIN_DROP
				}));
				PreGameOver.Instance.OnInit();
				this.bossManager.Init();
				DataLoader.LoadData();
				ProfileManager.CountGamePlay.setValue(ProfileManager.CountGamePlay.Data.Value + 1);
				this.RatePower = PowerManager.Instance.RatePower((float)PowerManager.Instance.TotalPowerRequireCampaign(GameMode.Instance.MODE, (int)this.Level));
				break;
			}
			}
		}
		this.textFakeLoading.text = "95%";
		yield return new WaitForSeconds(0.2f);
		this.textFakeLoading.text = "97%";
		this.bombManager.InitObject();
		this.hudManager.InitObject();
		this.StateManager.InitObject();
		this.bulletManager.InitObject();
		this.fxManager.OnInit();
		yield return new WaitForSeconds(0.2f);
		this.textFakeLoading.text = "99%";
		if (GameMode.Instance.modePlay != GameMode.ModePlay.PvpMode)
		{
			EnemyManager.Instance.OnInit();
		}
		this.audioManager.Init();
		this.giftManager.Init();
		this.audioManager.Background();
		yield return new WaitForSeconds(0.2f);
		this.strBuilderTime = new StringBuilder();
		if (TrapManager.Instance)
		{
			TrapManager.Instance.OnInit();
			this.isTrapMode = true;
		}
		yield return new WaitUntil(() => UnityEngine.Object.FindObjectOfType(typeof(EnemyManager)) != null);
		yield return new WaitUntil(() => EnemyManager.Instance.IsInit);
		try
		{
			this.numberGrenades = ProfileManager.grenadesProfile[ProfileManager.grenadeCurrentId].TotalBomb;
		}
		catch
		{
		}
		if (GameMode.Instance.modePlay != GameMode.ModePlay.Endless && GameMode.Instance.modePlay != GameMode.ModePlay.Campaign && GameMode.Instance.modePlay != GameMode.ModePlay.Special_Campaign && GameMode.Instance.modePlay != GameMode.ModePlay.PvpMode)
		{
			if (GameMode.Instance.modePlay != GameMode.ModePlay.CoOpMode)
			{
				goto IL_87C;
			}
		}
		try
		{
			this.hudManager.textTime.transform.parent.gameObject.SetActive(true);
		}
		catch
		{
		}
		IL_87C:
		this.coinManager.OnInit();
		this.StateManager.SetBegin();
		CameraController.Instance.BeginCamera();
		this.isInit = true;
		this.textFakeLoading.text = "100%";
		this.FakeLoading.SetActive(false);
		yield break;
	}

	public void UpdateTime(float deltaTime)
	{
		if (this.mMission != null)
		{
			this.mMission.TimeGamePlay += deltaTime;
			int num = (int)(this.mMission.TimeGamePlay / 60f);
			int num2 = (int)(this.mMission.TimeGamePlay - (float)(num * 60));
			this.strBuilderTime.Length = 0;
			if (num < 10)
			{
				this.strBuilderTime.Append("0");
			}
			this.strBuilderTime.Append(num);
			this.strBuilderTime.Append(":");
			if (num2 < 10)
			{
				this.strBuilderTime.Append("0");
			}
			this.strBuilderTime.Append(num2);
			try
			{
				this.hudManager.textTime.text = this.strBuilderTime.ToString();
			}
			catch
			{
			}
		}
	}

	public void ResetTime()
	{
		if (this.mMission != null)
		{
			this.mMission.TimeGamePlay = 0f;
			try
			{
				this.hudManager.textTime.text = "00:00";
			}
			catch
			{
			}
		}
	}

	private void Update()
	{
		if (!this.isInit || this.player == null)
		{
			return;
		}
		if (!this.player.IsInit)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Endless && this.player.isAutoRun)
		{
			this.player.OnMovement(BaseCharactor.EMovementBasic.Right);
		}
		if (this.player != null && !this.player._PlayerInput.IsShooting)
		{
			this.tfAutoTarget.transform.position = Vector3.one * 999f;
		}
		switch (this.StateManager.EState)
		{
		case EGamePlay.NONE:
		case EGamePlay.PAUSE:
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !PlayerManagerStory.Instance.isPreGameOver && PopupManager.Instance.CloseAll())
			{
				return;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !this.StateManager.popupRescue.gameObject.activeInHierarchy)
			{
				if (this.player != null && this.player.gameObject.activeSelf)
				{
					if (this.StateManager.inforGamePlayPause.gameObject.activeInHierarchy)
					{
						this.StateManager.inforGamePlayPause.OnContinue();
					}
					else
					{
						this.hudManager.OnResumeGame();
					}
				}
				return;
			}
			break;
		case EGamePlay.BEGIN:
			if (this.bombManager.isInit)
			{
				this.bombManager.UpdateObject(deltaTime);
			}
			this.fxManager.OnUpdate(deltaTime);
			break;
		case EGamePlay.RUNNING:
			if (this.giftManager != null)
			{
				this.giftManager.OnUpdate(deltaTime);
			}
			if (this.isTrapMode)
			{
				TrapManager.Instance.OnUpdate(deltaTime);
			}
			if (!object.ReferenceEquals(this.skillManager, null))
			{
				this.skillManager.UpdateSkill(deltaTime);
			}
			if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
			{
				this.UpdateTime(deltaTime);
			}
			for (int i = 0; i < this.ListRambo.Count; i++)
			{
				this.ListRambo[i].OnUpdate(deltaTime);
			}
			this.coinManager.OnUpdate(deltaTime);
			if (this.bulletManager.isInit)
			{
				this.bulletManager.UpdateObject(deltaTime);
			}
			if (this.bombManager.isInit)
			{
				this.bombManager.UpdateObject(deltaTime);
			}
			this.fxManager.OnUpdate(deltaTime);
			if (this.isJetpackMode)
			{
				JetpackManager.Instance.OnUpdate(deltaTime);
			}
			switch (GameMode.Instance.modePlay)
			{
			case GameMode.ModePlay.Campaign:
				EnemyManager.Instance.OnUpdate(deltaTime);
				break;
			case GameMode.ModePlay.Boss_Mode:
				EnemyManager.Instance.OnUpdate(deltaTime);
				break;
			case GameMode.ModePlay.PvpMode:
				EnemyManager.Instance.OnUpdate(deltaTime);
				break;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !PlayerManagerStory.Instance.isPreGameOver && !PopupManager.isblockInput)
			{
				if (PopupManager.Instance.mDialog != null && PopupManager.Instance.mDialog.gameObject.activeSelf)
				{
					PopupManager.Instance.mDialog.OnClose();
					return;
				}
				if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
				{
					return;
				}
				if (PopupManager.Instance.CloseAll())
				{
					return;
				}
				if (this.StateManager.popupRescue.gameObject.activeSelf)
				{
					this.StateManager.popupRescue.Cancel();
					return;
				}
				if (this.player.HPCurrent <= 0f || !this.player.gameObject.activeSelf || GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer)
				{
					return;
				}
				if (SplashScreen._isBuildMarketing)
				{
					UnityEngine.Debug.Log("Show skill");
					GameManager.Instance.skillManager.OnClickedSkill();
					return;
				}
				EventDispatcher.PostEvent("PauseGame");
				return;
			}
			break;
		case EGamePlay.WAITING_BOSS:
			if (this.bulletManager.isInit)
			{
				this.bulletManager.UpdateObject(deltaTime);
			}
			this.fxManager.OnUpdate(deltaTime);
			break;
		case EGamePlay.LOST:
			if (this.bulletManager.isInit)
			{
				this.bulletManager.UpdateObject(deltaTime);
			}
			break;
		case EGamePlay.WIN:
			if (this.bulletManager.isInit)
			{
				this.bulletManager.UpdateObject(deltaTime);
			}
			break;
		case EGamePlay.PREVIEW:
			if (this.bulletManager.isInit)
			{
				this.bulletManager.UpdateObject(deltaTime);
			}
			break;
		}
	}

	private void FixedUpdate()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		switch (this.StateManager.EState)
		{
		case EGamePlay.NONE:
		case EGamePlay.BEGIN:
			switch (GameMode.Instance.modePlay)
			{
			case GameMode.ModePlay.Campaign:
			case GameMode.ModePlay.Special_Campaign:
				EnemyManager.Instance.OnUpdate(fixedDeltaTime);
				break;
			}
			break;
		case EGamePlay.RUNNING:
			if (this.bulletManager.isInit)
			{
				this.bulletManager.FixedUpdateObject(fixedDeltaTime);
			}
			break;
		case EGamePlay.WAITING_BOSS:
			if (this.bulletManager.isInit)
			{
				this.bulletManager.FixedUpdateObject(fixedDeltaTime);
			}
			break;
		}
	}

	private void OnDestroy()
	{
		try
		{
			if (this.bombManager.isInit)
			{
				this.bombManager.DestroyObject();
			}
			if (this.hudManager.isInit)
			{
				this.hudManager.DestroyObject();
			}
			if (this.bulletManager.isInit)
			{
				this.bulletManager.DestroyObject();
			}
			if (this.audioManager.isInit)
			{
				this.audioManager.DestroyObject();
			}
			if (this.giftManager.IsInit)
			{
				this.giftManager.DestroyObject();
			}
			this.fxManager.DestroyAll();
			this.ListEnemy.Clear();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("message: " + ex.Message);
		}
		this.isTrapMode = false;
		if (!object.ReferenceEquals(GameManager.instance, null))
		{
			GameManager.instance = null;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (this.player == null)
		{
			return;
		}
		this.HandlePauseForOnlineMode(pauseStatus);
		if (this.player != null && this.player.HPCurrent <= 0f)
		{
			return;
		}
		if (this.StateManager.EState == EGamePlay.RUNNING && !this.StateManager.popupRescue.gameObject.activeSelf && pauseStatus && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL && GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer && this.player.gameObject.activeSelf)
		{
			EventDispatcher.PostEvent("PauseGame");
		}
	}

	public void HandlePauseForOnlineMode(bool pauseStatus)
	{
		if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
		{
			return;
		}
		if (pauseStatus && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom && !this.isInit)
		{
			UnityEngine.Debug.Log("+++++++++++++ GameManager disconnect photon OnApplicationPause before init");
			PhotonNetwork.Disconnect();
			PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
			SceneManager.LoadScene("Menu");
		}
	}

	public int NumberGrenades()
	{
		int result = 0;
		try
		{
			result = this.numberGrenades - ProfileManager.grenadesProfile[ProfileManager.grenadeCurrentId].TotalBomb;
		}
		catch
		{
		}
		return result;
	}

	private void CreateRambo()
	{
		string path = string.Empty;
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			if (this.TutorialManager != null)
			{
				this.TutorialManager.CreateCharacter();
			}
			return;
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Campaign)
		{
			ELevel level = this.Level;
			if (level != ELevel.LEVEL_23)
			{
				path = SPaths.RAMBO[ProfileManager.settingProfile.IDChar];
			}
			else
			{
				path = SPaths.RAMBO[ProfileManager.settingProfile.IDChar];
			}
		}
		else
		{
			path = SPaths.RAMBO[ProfileManager.settingProfile.IDChar];
		}
		int idchar = ProfileManager.settingProfile.IDChar;
		if (idchar != 0)
		{
			if (idchar != 1)
			{
				if (idchar == 2)
				{
					path = "GameObject/Player/PlayerBoy2";
				}
			}
			else
			{
				path = "GameObject/Player/PlayerGirl";
			}
		}
		else
		{
			path = "GameObject/Player/PlayerBoy1";
		}
		this.player = (UnityEngine.Object.Instantiate(Resources.Load(path, typeof(PlayerMain))) as PlayerMain);
		this.player.OnInit(ProfileManager.settingProfile.IDChar, 0, 0, 0);
	}

	private void ControllerDestop()
	{
		if (object.ReferenceEquals(this.player, null))
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.E))
		{
			this.player._PlayerInput.SwitchGun(!this.player.isGunDefault);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.H))
		{
			this.player._PlayerInput.OnShootDown();
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.H))
		{
			this.player._PlayerInput.OnShootUp();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.J))
		{
			this.player._PlayerInput.OnJump(true);
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.J))
		{
			this.player._PlayerInput.OnJump(false);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.K))
		{
			this.player._PlayerInput.OnThrowGrenades(true);
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.K))
		{
			this.player._PlayerInput.OnThrowGrenades(false);
		}
		if (UnityEngine.Input.GetKey(KeyCode.A))
		{
			this.MoveRambo(0);
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.A))
		{
			this.TouchUpMove();
		}
		if (UnityEngine.Input.GetKey(KeyCode.D))
		{
			this.MoveRambo(1);
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.D))
		{
			this.TouchUpMove();
		}
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			this.MoveRambo(2);
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.S))
		{
			this.TouchUpMove();
		}
	}

	private void MoveRambo(int direction)
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		GameManager.Instance.player.EMovement = (BaseCharactor.EMovementBasic)direction;
	}

	private void TouchUpMove()
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		GameManager.Instance.player.EMovement = BaseCharactor.EMovementBasic.Release;
	}

	private static GameManager instance;

	public PlayerMain player;

	public GameObject NPC;

	[Header("Manager Script")]
	public GameStateManager StateManager;

	public HUDManager hudManager;

	public BulletManager bulletManager;

	public BombManager bombManager;

	public AudioManager audioManager;

	public GiftsManager giftManager;

	public SkillManager skillManager;

	public BossManager bossManager;

	public CoinManager coinManager;

	public FxManager fxManager;

	[Header("Transform")]
	public List<BaseEnemy> ListEnemy;

	public List<BaseCharactor> ListRambo;

	public int CheckPoint = -1;

	[NonSerialized]
	public bool isShowTutorialGun;

	[NonSerialized]
	public int StepTutorial;

	[NonSerialized]
	public int CountEnemyDie;

	[NonSerialized]
	public ELevel Level;

	[NonSerialized]
	public int COIN_COLLECTED;

	public GameObject FakeLoading;

	public Text textFakeLoading;

	public Mission mMission;

	private StringBuilder strBuilderTime;

	private float timeCheckEnemy;

	private int currentCheckpoint = -1;

	private int currentEnemy = -1;

	public List<Transform> listRevival;

	public Canvas mCanvas;

	public bool isDoubleCoin;

	public bool isDoubleExp;

	public Transform tfPosStartTutorial;

	public Transform tfPosEndTutorial;

	public TutorialGamePlay TutorialManager;

	public Transform tfAutoTarget;

	public bool isTrapMode;

	public bool isJetpackMode;

	[SerializeField]
	private Text txtDistance;

	private StringBuilder builderDistance;

	public int TotalEnemyKilled;

	public int MAX_COIN_DROP;

	public float RatePower;

	public Action OnGameBegin;

	public List<USung> ListUsung;

	private bool isInit;

	private int numberBulletSpecial;

	private int numberGrenades;

	public int numberHpPack;

	public float hpDamageBoss;

	internal static bool isPlay;
}
