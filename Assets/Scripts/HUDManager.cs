using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	public void InitObject()
	{
		this.isInit = true;
		this.strBuilderHp = new StringBuilder();
		this.strBuilderBullet = new StringBuilder();
		this.TimeHideAnimGogo = new WaitForSeconds(3f);
		this.TimeShowPopup = new WaitForSeconds(3f);
		if (PreGameOver.Instance)
		{
			CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
			if (orientaltion != CameraController.Orientation.HORIZONTAL)
			{
				if (orientaltion == CameraController.Orientation.VERTICAL)
				{
					this.Map_Length = PreGameOver.Instance.transform.GetChild(1).position.y;
				}
			}
			else
			{
				this.Map_Length = PreGameOver.Instance.transform.GetChild(1).position.x;
			}
		}
		for (int i = 0; i < this.avatar.Length; i++)
		{
			this.avatar[i].gameObject.SetActive(ProfileManager.settingProfile.IDChar == i);
		}
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			for (int j = 0; j < this.avatar.Length; j++)
			{
				this.avatar[j].gameObject.SetActive(false);
			}
			this.avatar[2].gameObject.SetActive(true);
		}
		EventDispatcher.RegisterListener("PauseGame", new Action(GameManager.Instance.StateManager.SetGamePause));
		EventDispatcher.RegisterListener("ResumeGame", new Action(GameManager.Instance.StateManager.SetGameRuning));
		EventDispatcher.RegisterListener("LostGame", new Action(GameManager.Instance.StateManager.GameOver));
		EventDispatcher.RegisterListener("CompletedGame", new Action(GameManager.Instance.StateManager.SetGameWin));
		EventDispatcher.RegisterListener<MyMessage>("HpValueChange", new Action<MyMessage>(this.HpValueChange));
		EventDispatcher.RegisterListener("BulletValueChange", new Action(this.BulletValueChange));
		EventDispatcher.RegisterListener<MyMessage>("Show_Anim_Gogo", new Action<MyMessage>(this.ShowAnimGogo));
		EventDispatcher.RegisterListener("COIN_COLLECTED", new Action(GameManager.Instance.coinManager.CoinCollected));
		GameMode.GameStyle style = GameMode.Instance.Style;
		if (style != GameMode.GameStyle.SinglPlayer)
		{
			if (style != GameMode.GameStyle.MultiPlayer)
			{
			}
		}
		else if ((GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL) || GameMode.Instance.modePlay == GameMode.ModePlay.Special_Campaign)
		{
			for (int k = 0; k < DataLoader.LevelDataCurrent.points.Count; k++)
			{
				this.totalEnemy += DataLoader.LevelDataCurrent.points[k].totalEnemy;
			}
		}
	}

	public void DestroyObject()
	{
		this.isInit = false;
		EventDispatcher.RemoveListener("COIN_COLLECTED", new Action(GameManager.Instance.coinManager.CoinCollected));
		EventDispatcher.RemoveListener("PauseGame", new Action(GameManager.Instance.StateManager.SetGamePause));
		EventDispatcher.RemoveListener("ResumeGame", new Action(GameManager.Instance.StateManager.SetGameRuning));
		EventDispatcher.RemoveListener("LostGame", new Action(GameManager.Instance.StateManager.GameOver));
		EventDispatcher.RemoveListener<MyMessage>("HpValueChange", new Action<MyMessage>(this.HpValueChange));
		EventDispatcher.RemoveListener("BulletValueChange", new Action(this.BulletValueChange));
		EventDispatcher.RemoveListener<MyMessage>("Show_Anim_Gogo", new Action<MyMessage>(this.ShowAnimGogo));
		EventDispatcher.RemoveListener("CompletedGame", new Action(GameManager.Instance.StateManager.SetGameWin));
	}

	private void Update()
	{
		if (this.async == null)
		{
			return;
		}
		this.textLoading.text = ((int)(this.async.progress * 100f)).ToString() + "%";
	}

	private void ShowAnimGogo(MyMessage msg)
	{
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Campaign || GameMode.Instance.modePlay == GameMode.ModePlay.Endless || GameMode.Instance.modePlay == GameMode.ModePlay.Special_Campaign)
		{
			this.gogoController.Show((GoGoController.Direction)msg.IntValue);
		}
	}

	private void HpValueChange(MyMessage msg)
	{
		float num = GameManager.Instance.player.HPCurrent / GameManager.Instance.player._PlayerData.ramboProfile.HP;
		this.BGWarningHP.SetActive(num < 0.15f);
		this.slideHp.value = num;
	}

	private void BulletValueChange()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		int totalBullet = GameManager.Instance.player.GunCurrent.WeaponCurrent.cacheGunProfile.TotalBullet;
		int capacity = GameManager.Instance.player.GunCurrent.WeaponCurrent.cacheGunProfile.CAPACITY;
		int num = (int)((float)totalBullet / (float)capacity * 10f);
		if (totalBullet == 9999)
		{
			this.txtTotalBullet.text = string.Empty;
			for (int i = 0; i < this.bullets.Length; i++)
			{
				this.bullets[i].gameObject.SetActive(true);
			}
			return;
		}
		this.txtTotalBullet.text = totalBullet.ToString();
		num = Mathf.Clamp(num, 1, 10);
		for (int j = 0; j < this.bullets.Length; j++)
		{
			this.bullets[j].gameObject.SetActive(j < num);
		}
	}

	public void ShowControl(float timedelay = 1.1f)
	{
		this.UITop_Left.SetActive(true);
		this.UITop_Right.SetActive(true);
		GameManager.Instance.player._PlayerInput.OnInit();
		ControlManager.Instance.OnShowControl(1.1f, false);
	}

	public void ShowControlStartGame()
	{
		this.UITop_Left.SetActive(true);
		this.UITop_Right.SetActive(true);
		GameManager.Instance.player._PlayerInput.OnInit();
		ControlManager.Instance.OnShowControlStartGame();
	}

	public void ShowControlWithTutorial()
	{
		this.UITop_Left.SetActive(true);
		this.UITop_Right.SetActive(true);
		GameManager.Instance.player._PlayerInput.OnInit();
		ControlManager.Instance.OnShowControlWithTutorial(false);
	}

	public void HideControl()
	{
		if (GameManager.Instance.isJetpackMode)
		{
			JetpackManager.Instance.ObjFly.SetActive(false);
			JetpackManager.Instance.ObjFire.SetActive(false);
		}
		ControlManager.Instance.OnHideControl();
		this.UITop_Left.SetActive(false);
		this.UITop_Right.SetActive(false);
		if (GameManager.Instance.player != null)
		{
			GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
		}
	}

	public IEnumerator ShowEffectTarget(Vector2[] arrPos, float mTime)
	{
		if (this.arrTargetEffect.Length > 0 && arrPos.Length > 0)
		{
			this.arrTargetEffect[0].gameObject.SetActive(true);
			this.arrTargetEffect[0].position = arrPos[0];
			for (int i = 1; i < arrPos.Length; i++)
			{
				yield return new WaitForSeconds(mTime);
				this.arrTargetEffect[i].gameObject.SetActive(true);
				this.arrTargetEffect[i].position = arrPos[i];
			}
		}
		yield break;
	}

	public void ClearEffectTarget()
	{
		for (int i = 0; i < this.arrTargetEffect.Length; i++)
		{
			this.arrTargetEffect[i].gameObject.SetActive(false);
		}
	}

	public void HideLineBoss()
	{
		for (int i = 0; i < this.arrLineBloodBoss.Length; i++)
		{
			this.arrLineBloodBoss[i].rectTransform.sizeDelta = new Vector2(0f, this.arrLineBloodBoss[i].rectTransform.sizeDelta.y);
		}
	}

	public void ShowSkipPreviewBoss(UnityAction btnOnclick)
	{
		if (this.btnSkipPreviewBoss)
		{
			this.btnSkipPreviewBoss.gameObject.SetActive(true);
			this.btnSkipPreviewBoss.onClick.AddListener(btnOnclick);
			this.btnSkipPreviewBoss.onClick.AddListener(new UnityAction(this.HideSkipPreviewBoss));
		}
	}

	public void HideSkipPreviewBoss()
	{
		if (this.btnSkipPreviewBoss)
		{
			this.btnSkipPreviewBoss.gameObject.SetActive(false);
		}
	}

	public void OnTouchDownFlyJetpack()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
	}

	public void OnTouchUpFlyJetpack()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
	}

	public void OnLoadScene(string nameScene, bool isSkipInter)
	{
		if (this.async != null)
		{
			return;
		}
		try
		{
			GameManager.Instance.audioManager.MissionCompleted.Stop();
			GameManager.Instance.audioManager.MissionFailed.Stop();
		}
		catch (Exception ex)
		{
		}
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			this.objLoading.SetActive(true);
			this.async = SceneManager.LoadSceneAsync("UICampaign");
			return;
		}
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			isSkipInter = true;
		}
		if (!isSkipInter && !this.isSkipAds)
		{
            this.async = SceneManager.LoadSceneAsync(nameScene);
   //         AdmobManager.Instance.ShowInterstitial(delegate
			//{
			//	this.async = SceneManager.LoadSceneAsync(nameScene);
			//}, true);
		}
		else
		{
			this.objLoading.SetActive(true);
			this.async = SceneManager.LoadSceneAsync(nameScene);
		}
		this.objLoading.SetActive(true);
	}

	public void OnResumeGame()
	{
		GameManager.Instance.audioManager.Click();
		EventDispatcher.PostEvent("ResumeGame");
		GameManager.Instance.audioManager.Background();
		ControlManager.Instance.BombValueChange();
	}

	public void OnPause()
	{
		GameManager.Instance.audioManager.Click();
		EventDispatcher.PostEvent("PauseGame");
	}

	private StringBuilder strBuilderHp;

	private StringBuilder strBuilderBullet;

	private WaitForSeconds TimeHideAnimGogo;

	private WaitForSeconds TimeShowPopup;

	[HideInInspector]
	public bool isInit;

	[Header("UI Canvas")]
	public Text txtTotalBullet;

	public Text txtStart;

	public Slider slideHp;

	public Image[] arrLineBloodBoss;

	public Combo combo;

	[Header("GameObject")]
	public GoGoController gogoController;

	public RectTransform LineBlood;

	public Transform[] arrTargetEffect;

	public GameObject BGWarningHP;

	public GameObject WarningBoss;

	public Image[] avatar;

	public Image[] bullets;

	public AsyncOperation async;

	public GameObject objLoading;

	public Text textLoading;

	public Text textTime;

	private int totalEnemy;

	public GameObject UITop_Left;

	public GameObject UITop_Right;

	public Button btnSkipPreviewBoss;

	public int Max_Hp_Booster = -1;

	public float Map_Length;

	public bool isSkipAds;
}
