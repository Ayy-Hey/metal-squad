using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager.Popup
{
	public class PopupRescue : MonoBehaviour
	{
		private void Awake()
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
		}

		private void Start()
		{
			if (ProfileManager.inAppProfile.vipProfile.level < E_Vip.Vip1)
			{
				this.TotalReviveWithGem = 0;
			}
			else
			{
				this.TotalReviveWithGem = DataLoader.vipData.Levels[(int)ProfileManager.inAppProfile.vipProfile.level].dailyReward.AddReviveWithGem;
			}
		}

		private void Update()
		{
			if (!this.isRunning)
			{
				return;
			}
			this.time_countdown -= Time.deltaTime;
			this.txtTimeCountdown.text = Mathf.Max(0, (int)this.time_countdown) + string.Empty;
			if (this.time_countdown <= 0f)
			{
				this.Cancel();
			}
		}

		public void Show()
		{
			this.GoldRevive = 1000;
			GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
			if (modePlay != GameMode.ModePlay.Campaign)
			{
				this.idMap = -1;
				this.GoldRevive = 2000;
				if (AdmobManager.Instance.IsVideoReady())
				{
					this.objButtonGold.SetActive(false);
					Vector2 anchoredPosition = this.rectButtonVideo.anchoredPosition;
					anchoredPosition.x = 0f;
					this.rectButtonVideo.anchoredPosition = anchoredPosition;
				}
				FireBaseManager.Instance.LogEvent("ShowRevive_Other");
			}
			else
			{
				this.idMap = (int)((int)GameManager.Instance.Level / (int)ELevel.LEVEL_13);
				this.GoldRevive = 1000 + this.idMap * 1000;
				FireBaseManager.Instance.LogEvent("ShowRevive_Campaign_Map_" + this.idMap);
				if (AdmobManager.Instance.IsVideoReady())
				{
					this.objButtonGold.SetActive(false);
					Vector2 anchoredPosition2 = this.rectButtonVideo.anchoredPosition;
					anchoredPosition2.x = 0f;
					this.rectButtonVideo.anchoredPosition = anchoredPosition2;
				}
			}
			this.txtGold.text = this.GoldRevive.ToString();
			if (!this.isShowGold)
			{
				this.isShowGold = true;
				this.ShowGold();
				this.objEffectVideo.SetActive(AdmobManager.Instance.IsVideoReady());
				this.objEffectGold.SetActive(!AdmobManager.Instance.IsVideoReady());
			}
			else if (this.TotalReviveWithGem > 0)
			{
				this.TotalReviveWithGem--;
				this.ShowGem();
			}
			this.time_countdown = 5f;
			this.isRunning = true;
		}

		private void ShowGold()
		{
			this.OnCountDownEnd = null;
			base.gameObject.SetActive(true);
			this.objChild.SetActive(true);
			this.txtTimeDelay.text = string.Empty;
			this.ShowCoin();
			this.TH2.SetActive(false);
			this.TH1.SetActive(true);
			this.objOffer.SetActive(SaleManager.Instance.ValueSale > 0f);
			if (AutoCheckLevel.isAutoCheck)
			{
				this.Cancel();
			}
		}

		private void ShowGem()
		{
			this.OnCountDownEnd = null;
			base.gameObject.SetActive(true);
			this.objChild.SetActive(true);
			this.txtTimeDelay.text = string.Empty;
			this.ShowCoin();
			this.TH1.SetActive(false);
			this.TH2.SetActive(true);
			this.objOffer.SetActive(SaleManager.Instance.ValueSale > 0f);
			if (AutoCheckLevel.isAutoCheck)
			{
				this.Cancel();
			}
		}

		private void ShowCoin()
		{
			this.txtCoin.text = ProfileManager.userProfile.Coin.ToString();
			this.txtMS.text = ProfileManager.userProfile.Ms.ToString();
		}

		public void ShowPopupMS()
		{
			this.isRunning = false;
			PopupManager.Instance.ShowInapp(INAPP_TYPE.MS, delegate(bool callback)
			{
				if (callback)
				{
					this.isRunning = true;
					this.ShowCoin();
					this.objChild.SetActive(true);
				}
			});
		}

		public void ShowPopupCoin()
		{
			this.isRunning = false;
			PopupManager.Instance.ShowInapp(INAPP_TYPE.COINS, delegate(bool callback)
			{
				if (callback)
				{
					this.isRunning = true;
					this.ShowCoin();
					this.objChild.SetActive(true);
				}
			});
		}

		public void ReviveWithGold()
		{
			if (this._Coroutine != null)
			{
				base.StopCoroutine(this._Coroutine);
			}
			this.isRunning = false;
			this.objChild.SetActive(false);
			this._Coroutine = base.StartCoroutine(this.IECountDown());
			this.OnCountDownEnd = delegate()
			{
				this.ReviveSuccess(0.8f);
			};
			if (ProfileManager.userProfile.Coin < this.GoldRevive)
			{
				base.gameObject.SetActive(false);
				PopupManager.Instance.ShowInapp(INAPP_TYPE.COINS, delegate(bool callback)
				{
					if (callback)
					{
						base.gameObject.SetActive(true);
						this.ShowCoin();
						this.objChild.SetActive(true);
						this.txtTimeDelay.text = string.Empty;
						this.isRunning = true;
					}
				});
				return;
			}
			PopupManager.Instance.SaveReward(Item.Gold, -this.GoldRevive, "Revive_Mode:" + GameMode.Instance.modePlay, null);
			FireBaseManager.Instance.LogEvent("Revive_Gold_" + this.idMap);
			string parameterValue = "GamePlay";
			
		}

		public void ReviveWithGem()
		{
			if (this._Coroutine != null)
			{
				base.StopCoroutine(this._Coroutine);
			}
			this.isRunning = false;
			this.objChild.SetActive(false);
			this._Coroutine = base.StartCoroutine(this.IECountDown());
			this.OnCountDownEnd = delegate()
			{
				this.ReviveSuccess(1f);
			};
			if (ProfileManager.userProfile.Ms < 5)
			{
				base.gameObject.SetActive(false);
				PopupManager.Instance.ShowInapp(INAPP_TYPE.MS, delegate(bool callback)
				{
					if (callback)
					{
						base.gameObject.SetActive(true);
						this.ShowCoin();
						this.objChild.SetActive(true);
						this.txtTimeDelay.text = string.Empty;
						this.isRunning = true;
					}
				});
				return;
			}
			PopupManager.Instance.SaveReward(Item.Gem, -5, "Revive_Mode:" + GameMode.Instance.modePlay, null);
			string parameterValue = "GamePlay";
			
		}

		public void ReviveWithVideo()
		{
			string parameterValue = "GamePlay";
			try
			{
				
			}
			catch
			{
			}
			this.objButtonVideo.SetActive(false);
			this.isRunning = false;
			AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isSuccess)
			{
				if (isSuccess)
				{
					if (this._Coroutine != null)
					{
						base.StopCoroutine(this._Coroutine);
					}
					this.objChild.SetActive(false);
					this._Coroutine = base.StartCoroutine(this.IECountDown());
					this.OnCountDownEnd = delegate()
					{
						this.ReviveSuccess(1f);
					};
					this.isReadyToClaimed = false;
					FireBaseManager.Instance.LogEvent("Revive_Video_" + this.idMap);
					
				}
				else
				{
					this.isRunning = true;
					this.objButtonVideo.SetActive(true);
				}
			});
		}

		public void Cancel()
		{
			this.isRunning = false;
			GameManager.Instance.StateManager.GameOverNow();
			base.gameObject.SetActive(false);
		}

		public void ReviveSuccess(float percent)
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
			{
				return;
			}
			GameManager.Instance.StateManager.EState = EGamePlay.RUNNING;
			this.isRunning = false;
			try
			{
				GameManager.Instance.bombManager.SetResume();
				EnemyManager.Instance.OnResume();
				if (GameManager.Instance.isJetpackMode)
				{
					JetpackManager.Instance.trapJetpack.Reset();
				}
				GameManager.Instance.mMission.isRevive = true;
				if (!object.ReferenceEquals(GameManager.Instance.StateManager.descr, null))
				{
					GameManager.Instance.StateManager.descr.resume();
				}
				GameManager.Instance.hudManager.ShowControl(1.1f);
			}
			catch
			{
				ControlManager.Instance.OnShowControl(1.1f, false);
			}
			GameManager.Instance.audioManager.Background();
			GameManager.Instance.player.OnRevive(percent);
			base.gameObject.SetActive(false);
			GameManager.Instance.isRescue = (this.TotalReviveWithGem <= 0);
		}

		private IEnumerator IECountDown()
		{
			this.txtTimeDelay.text = "3";
			yield return new WaitForSeconds(1f);
			this.txtTimeDelay.text = "2";
			yield return new WaitForSeconds(1f);
			this.txtTimeDelay.text = "1";
			yield return new WaitForSeconds(1f);
			if (this.OnCountDownEnd != null)
			{
				this.OnCountDownEnd();
			}
			yield break;
		}

		public TextLocalization[] listTextLocalization;

		[SerializeField]
		public Text txtCoin;

		[SerializeField]
		public Text txtMS;

		[SerializeField]
		private GameObject TH1;

		[SerializeField]
		private GameObject TH2;

		public GameObject objOffer;

		public GameObject objButtonVideo;

		public GameObject objButtonGold;

		public RectTransform rectButtonVideo;

		public GameObject objChild;

		public Text txtTimeDelay;

		private Coroutine _Coroutine;

		public Action OnCountDownEnd;

		private bool isShowGold;

		private int TotalReviveWithGem;

		public Text txtGold;

		public GameObject objEffectGold;

		public GameObject objEffectVideo;

		private int GoldRevive = 1000;

		private int idMap = -1;

		private float time_countdown;

		private bool isRunning;

		public Text txtTimeCountdown;

		private bool isReadyToClaimed;
	}
}
