using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using SpawnEnemy;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGamePlay : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.listTextLocalization.Length; i++)
		{
			if (this.listTextLocalization[i].isUpcaseText)
			{
				this.listTextLocalization[i].txt_Text.text = (this.listTextLocalization[i].textStart + PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd).ToUpper();
			}
			else
			{
				this.listTextLocalization[i].txt_Text.text = this.listTextLocalization[i].textStart + PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd;
			}
		}
		this.txtMesh[0].text = PopupManager.Instance.GetText(Localization0.Tutorial_Move, null);
		this.txtMesh[1].text = PopupManager.Instance.GetText(Localization0.Tutorial_Jump, null);
		this.txtMesh[2].text = PopupManager.Instance.GetText(Localization0.Tutorial_Fire1, null);
		this.txtMesh[3].text = PopupManager.Instance.GetText(Localization0.Tutorial_Fire2, null);
	}

	public void CreateCharacter()
	{
		Time.timeScale = 1f;
		string path = "GameObject/Player/PlayerBoy2";
		GameManager.Instance.player = (UnityEngine.Object.Instantiate(Resources.Load(path, typeof(PlayerMain))) as PlayerMain);
		GameManager.Instance.player.OnInit(2, 0, 0, 0);
		BaseTrigger baseTrigger = this.triggerJump;
		baseTrigger.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger.OnEnteredTrigger, new Action(delegate()
		{
			if (this.isJump)
			{
				return;
			}
			TutorialGamePlay.Step = 1;
			GameManager.Instance.player._PlayerInput.OnRemoveInput(true);
			GameManager.Instance.hudManager.ShowControlWithTutorial();
			this.isJump = true;
			try
			{
				this.triggerJump.transform.Find("Arrow").gameObject.SetActive(false);
			}
			catch
			{
			}
			EventDispatcher.PostEvent<MyMessage>("Show_Anim_Gogo", new MyMessage
			{
				IntValue = 3
			});
		}));
		BaseTrigger baseTrigger2 = this.triggerPreAttack;
		baseTrigger2.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger2.OnEnteredTrigger, new Action(delegate()
		{
			if (this.isPreAttack)
			{
				return;
			}
			try
			{
				this.triggerPreAttack.transform.Find("Arrow").gameObject.SetActive(false);
			}
			catch
			{
			}
			this.isPreAttack = true;
			this.CreateEnemiesTurn1();
			this.triggerJump.gameObject.SetActive(false);
			TutorialGamePlay.Step = 2;
			MyMessage myMessage = new MyMessage();
			myMessage.IntValue = 3;
			GameManager.Instance.player._PlayerInput.OnRemoveInput(true);
			GameManager.Instance.hudManager.ShowControlWithTutorial();
			EventDispatcher.PostEvent<MyMessage>("Show_Anim_Gogo", myMessage);
		}));
		this.OnClearEnemiesTurn1 = (Action)Delegate.Combine(this.OnClearEnemiesTurn1, new Action(delegate()
		{
			this.triggerBlockEndMap.gameObject.SetActive(true);
		}));
		BaseTrigger baseTrigger3 = this.triggerBlockEndMap;
		baseTrigger3.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger3.OnEnteredTrigger, new Action(delegate()
		{
			if (this.isBlockEndMap)
			{
				return;
			}
			try
			{
				this.triggerBlockEndMap.transform.Find("Arrow").gameObject.SetActive(false);
			}
			catch
			{
			}
			this.isBlockEndMap = true;
			this.autoSpawnEnemies.OnInit();
		}));
		AutoSpawnEnemies autoSpawnEnemies = this.autoSpawnEnemies;
		autoSpawnEnemies.OnStarted = (Action)Delegate.Combine(autoSpawnEnemies.OnStarted, new Action(delegate()
		{
			TutorialGamePlay.Step = 3;
			GameManager.Instance.player._PlayerInput.OnRemoveInput(true);
			GameManager.Instance.hudManager.ShowControlWithTutorial();
		}));
		AutoSpawnEnemies autoSpawnEnemies2 = this.autoSpawnEnemies;
		autoSpawnEnemies2.OnCompleted = (Action)Delegate.Combine(autoSpawnEnemies2.OnCompleted, new Action(delegate()
		{
			EventDispatcher.PostEvent<MyMessage>("Show_Anim_Gogo", new MyMessage
			{
				IntValue = 3
			});
			this.triggerBlockEndMap2.gameObject.SetActive(true);
		}));
		BaseTrigger baseTrigger4 = this.triggerBlockEndMap2;
		baseTrigger4.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger4.OnEnteredTrigger, new Action(delegate()
		{
			if (this.isBlockEndMap2)
			{
				return;
			}
			try
			{
				this.triggerBlockEndMap2.transform.Find("Arrow").gameObject.SetActive(false);
			}
			catch
			{
			}
			this.isBlockEndMap2 = true;
			this.triggerBlockEndMap3.gameObject.SetActive(true);
		}));
		BaseTrigger baseTrigger5 = this.triggerBlockEndMap3;
		baseTrigger5.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger5.OnEnteredTrigger, new Action(delegate()
		{
			if (this.isBlockEndMap3)
			{
				return;
			}
			try
			{
				this.triggerBlockEndMap3.transform.Find("Arrow").gameObject.SetActive(false);
			}
			catch
			{
			}
			this.isBlockEndMap3 = true;
			if (this._Boss_Ultron != null)
			{
				GameManager.Instance.audioManager.Boss_Background();
				GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
				GameManager.Instance.player.EMovement = BaseCharactor.EMovementBasic.Release;
				this._Boss_Ultron.Init();
				this.cinematicBoss.Play();
				GameManager.Instance.hudManager.HideControl();
			}
		}));
		this.cinematicBoss.OnCinematicFinished.AddListener(delegate()
		{
			GameManager.Instance.player.gameObject.SetActive(true);
			GameManager.Instance.hudManager.ShowControl(1.1f);
			try
			{
				this.triggerBlockEndMap.gameObject.SetActive(false);
				this.triggerBlockEndMap2.gameObject.SetActive(false);
				this.triggerBlockEndMap3.gameObject.SetActive(false);
			}
			catch
			{
			}
		});
		this._Boss_Ultron.OnDead = delegate()
		{
			try
			{
				GameManager.Instance.player._PlayerSpine.OnVictory();
			}
			catch
			{
			}
			this.IWin();
		};
	}

	private void IWin()
	{
		this.isInit = false;
		GameManager.Instance.hudManager.HideControl();
		PlayerManagerStory.Instance.OnRunGameOver();
	}

	private void CreateEnemiesTurn1()
	{
		for (int i = 0; i < this.tfPosGrenade.Length; i++)
		{
			this.CreateEnemyGrenades(this.tfPosGrenade[i].position, true);
		}
		for (int j = 0; j < this.tfPosTanker.Length; j++)
		{
			this.CreateEnemySniper(this.tfPosTanker[j].position, false);
		}
	}

	private void CreateEnemySniper(Vector3 pos, bool ismove = true)
	{
		EnemySniper enemy = EnemyManager.Instance.CreateSniper();
		EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
		enemyDataInfo.pos_x = pos.x;
		enemyDataInfo.pos_y = pos.y;
		enemyDataInfo.ismove = ismove;
		enemyDataInfo.type = 0;
		enemyDataInfo.level = 5;
		enemy.Init(enemyDataInfo, delegate
		{
			EnemyManager.Instance.PoolSniper.Store(enemy);
		});
		EnemySniper enemy2 = enemy;
		enemy2.OnEnemyDeaded = (Action)Delegate.Combine(enemy2.OnEnemyDeaded, new Action(delegate()
		{
			this.MAX_ENEMY_TURN_1--;
			if (this.MAX_ENEMY_TURN_1 <= 0)
			{
				this.OnClearEnemiesTurn1();
			}
		}));
		enemy.cacheEnemy.Vision_Min = 4f;
		enemy.cacheEnemy.Vision_Max = 6f;
		GameManager.Instance.ListEnemy.Add(enemy);
	}

	private void CreateEnemyGrenades(Vector3 pos, bool ismove = true)
	{
		EnemyGrenade enemy = EnemyManager.Instance.CreateEnemyGrenade();
		EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
		enemyDataInfo.ismove = ismove;
		enemyDataInfo.type = 2;
		enemyDataInfo.pos_x = pos.x;
		enemyDataInfo.pos_y = pos.y;
		enemyDataInfo.level = 5;
		enemy.Init(enemyDataInfo, delegate
		{
			EnemyManager.Instance.PoolEnemyGrenade.Store(enemy);
		});
		EnemyGrenade enemy2 = enemy;
		enemy2.OnEnemyDeaded = (Action)Delegate.Combine(enemy2.OnEnemyDeaded, new Action(delegate()
		{
			this.MAX_ENEMY_TURN_1--;
			if (this.MAX_ENEMY_TURN_1 <= 0)
			{
				this.OnClearEnemiesTurn1();
			}
		}));
		enemy.cacheEnemy.Vision_Min = 4f;
		enemy.cacheEnemy.Vision_Max = 6f;
		GameManager.Instance.ListEnemy.Add(enemy);
	}

	public void OnReady()
	{
		this.objGroundStart.SetActive(true);
		this.isInit = true;
		CameraController.Instance.GetComponent<ProCamera2D>().enabled = true;
		CameraController.Instance.AddPlayer(GameManager.Instance.player.transform, 0f);
		GameManager.Instance.hudManager.ShowControlWithTutorial();
		this.objTutOnlyAndroid.SetActive(ProfileManager.settingProfile.TypeControl == 2);
	}

	private void Update()
	{
		if (!this.isInit || GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
		{
			return;
		}
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			this.autoSpawnEnemies.OnUpdate(Time.deltaTime);
		}
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		if (this.popupFakePause.activeSelf)
		{
			this.OnContinue();
			return;
		}
		this.OnPause();
	}

	public void OnLoadMenu()
	{
		if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
		{
			return;
		}
		SingletonGame<AudioController>.Instance.PlaySound(this._audioClick, 1f);
		GameManager.Instance.StateManager.EState = EGamePlay.PAUSE;
		Time.timeScale = 1f;
		GameMode.Instance.MODE = GameMode.Mode.NORMAL;
		FormCampaign.isCampaignContinue = true;
		DataLoader.LoadDataCampaign();
		GameManager.Instance.hudManager.OnLoadScene("UICampaign", true);
	}

	public void OnPause()
	{
		if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
		{
			return;
		}
		SingletonGame<AudioController>.Instance.PlaySound(this._audioClick, 1f);
		GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
		Time.timeScale = 0f;
		this.popupFakePause.SetActive(true);
		this.popupFakePauseChild.SetActive(true);
		ControlManager.Instance.OnHideControl();
		this.txtTimeDelay.text = string.Empty;
	}

	public void OnContinue()
	{
		if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
		{
			return;
		}
		SingletonGame<AudioController>.Instance.PlaySound(this._audioClick, 1f);
		this.txtTimeDelay.text = string.Empty;
		if (this._Coroutine != null)
		{
			base.StopCoroutine(this._Coroutine);
		}
		Time.timeScale = 1f;
		this._Coroutine = base.StartCoroutine(this.IEWaitGameRunning());
	}

	private IEnumerator IEWaitGameRunning()
	{
		this.popupFakePauseChild.SetActive(false);
		this.txtTimeDelay.text = "3";
		yield return new WaitForSeconds(1f);
		this.txtTimeDelay.text = "2";
		yield return new WaitForSeconds(1f);
		this.txtTimeDelay.text = "1";
		ControlManager.Instance.OnShowControl(1.1f, false);
		yield return new WaitForSeconds(1f);
		this.popupFakePause.SetActive(false);
		this.txtTimeDelay.text = string.Empty;
		GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
		yield break;
	}

	public static bool isSkip;

	public static int Step;

	[SerializeField]
	private GameObject objCanvas;

	[SerializeField]
	private GameObject objbtnSkip;

	public GameObject objGroundStart;

	public BaseTrigger triggerJump;

	private bool isJump;

	public BaseTrigger triggerPreAttack;

	private bool isPreAttack;

	public Transform[] tfPosTanker;

	public Transform[] tfPosGrenade;

	private Action OnClearEnemiesTurn1;

	private int MAX_ENEMY_TURN_1 = 4;

	public BaseTrigger triggerBlockEndMap;

	private bool isBlockEndMap;

	public AutoSpawnEnemies autoSpawnEnemies;

	public BaseTrigger triggerBlockEndMap2;

	public BaseTrigger triggerBlockEndMap3;

	private bool isBlockEndMap2;

	private bool isBlockEndMap3;

	public Boss_Ultron _Boss_Ultron;

	public ProCamera2DCinematics cinematicBoss;

	public GameObject popupFakePause;

	public GameObject popupFakePauseChild;

	public Text txtTimeDelay;

	private bool isInit;

	public Transform tfWeaponDrop;

	public GameObject objTutOnlyAndroid;

	public AudioClip _audioClick;

	public TextLocalization[] listTextLocalization;

	public TextMesh[] txtMesh;

	private Coroutine _Coroutine;
}
