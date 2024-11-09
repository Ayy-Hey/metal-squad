using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupLuckySpin : PopupBase
{
	private void OnEnable()
	{
		PopupManager.Instance.SetCanvas(MenuManager.Instance.popupCanvas);
		this._oldWaitTime = ProfileManager.spinProfile.WaitTime;
		if ((float)Screen.width < (float)Screen.height * 1.7f)
		{
			this.tableObj.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}
		else
		{
			this.tableObj.localScale = Vector3.one;
		}
		this.isShowingReward = false;
		this._totalSpin = ProfileManager.spinProfile.TotalSpin;
	}

	public override void OnClose()
	{
		if (this.isSpinning || this.isShowingReward)
		{
			return;
		}
		base.StopAllCoroutines();
		base.OnClose();
	}

	private void Update()
	{
		if (!ProfileManager.spinProfile.Free && this.wait != ProfileManager.spinProfile.WaitTime)
		{
			this.wait = ProfileManager.spinProfile.WaitTime;
			this.seconds = this.wait % 60;
			this.seconds = Mathf.Max(this.seconds, 0);
			this.minutes = this.wait / 60;
			this.txtWaitTime.text = ((this.minutes <= 9) ? ("0" + this.minutes.ToString()) : this.minutes.ToString()) + ":" + ((this.seconds <= 9) ? ("0" + this.seconds.ToString()) : this.seconds.ToString());
		}
		if (!this.isSpin && AdmobManager.Instance.IsVideoReady() && ProfileManager.spinProfile.Free && this.txtFreeSpin.enabled)
		{
			this._totalSpin = ProfileManager.spinProfile.TotalSpin;
			this.ShowButtonSpin();
		}
		if (this.isSpinning)
		{
			this.UpdateSpin();
		}
	}

	public override void Show()
	{
		base.Show();
		base.transform.localScale = Vector3.one;
		this.ResetStateSpin();
		for (int i = 0; i < 10; i++)
		{
			this.itemSpins[i].img_Main.sprite = PopupManager.Instance.sprite_Item[(int)ProfileManager.spinProfile.GetSpinGift(i)];
			this.itemSpins[i].img_BG.sprite = PopupManager.Instance.sprite_BGRankItem[PopupManager.Instance.GetRankItem(ProfileManager.spinProfile.GetSpinGift(i))];
			this.itemSpins[i].txt_Amount.text = "x" + ProfileManager.spinProfile.GetSpinAmount(i).ToString();
			this.itemSpins[i].item = ProfileManager.spinProfile.GetSpinGift(i);
			this.itemSpins[i].ShowBorderEffect();
		}
	}

	public void OnSpin()
	{
		if (ProfileManager.spinProfile.TotalSpin > 0)
		{
			PopupManager.Instance.SaveReward(Item.Ticket_Spin, -1, "OnSpin", null);
			this.PlaySpin();
		}
		else if (ProfileManager.spinProfile.Free)
		{
			AdmobManager.Instance.ShowRewardBasedVideo(delegate(bool isSuccess)
			{
				if (isSuccess)
				{
					this.PlaySpin();
					ProfileManager.spinProfile.Free = false;
					
				}
			});
		}
		try
		{
			
		}
		catch
		{
		}
	}

	public void OnAddSpin()
	{
		if (this.isSpin)
		{
			return;
		}
		this.addSpin.Show();
		this.addSpin.OnClosed = delegate()
		{
			this.ShowButtonSpin();
		};
		
	}

	private void ShowTotalSpin()
	{
		this._totalSpin = ProfileManager.spinProfile.TotalSpin;
		this.txtTotalSpin.text = this._totalSpin.ToString();
	}

	private IEnumerator SpinAfterVideo()
	{
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	private void PlaySpin()
	{
		AudioClick.Instance.OnClick();
		this.ShowTotalSpin();
		this.isSpin = true;
		this.btnSpin.interactable = false;
		this.obj_EffectBtnSpin.SetActive(this.btnSpin.interactable);
		this.GetSpin();
		this.spinId = 0;
		this.spinOldId = -1;
		for (int i = 1; i < this.itemSpins.Length; i++)
		{
			this.ActiveBlackSpin(i, true);
		}
		this.animSpin.clip = this.spinClips[this.lockSpinId];
		this.animSpin.Play();
		SingletonGame<AudioController>.Instance.PlaySound(this.audioSpin, 1f);
		this.isSpinning = true;
		DailyQuestManager.Instance.MissionLuckySpin(null);
	}

	private void GetSpin()
	{
		this.lockSpinId = 0;
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			num += (int)(ProfileManager.spinProfile.GetSpinRate(i) * 1000f);
		}
		int num2 = UnityEngine.Random.Range(0, num);
		int num3 = 0;
		for (int j = 0; j < 10; j++)
		{
			num3 += (int)(ProfileManager.spinProfile.GetSpinRate(j) * 1000f);
			if (num3 > num2 && ProfileManager.spinProfile.GetSpinRate(j) != 0f)
			{
				this.lockSpinId = j;
				break;
			}
		}
	}

	private void UpdateSpin()
	{
		if (this.animSpin.isPlaying)
		{
			this.selectEuler = this.rectSelect.eulerAngles;
			this.spinId = ((int)(this.selectEuler.z / 18f) + 1) / 2;
			this.spinId = ((this.spinId <= 0) ? this.spinId : (10 - this.spinId));
			this.spinId = Mathf.Max(0, this.spinId);
			if (this.spinId != this.spinOldId)
			{
				this.ShowBorderWenSpinning();
				this.spinOldId = this.spinId;
			}
		}
		else
		{
			this.isSpinning = false;
			SingletonGame<AudioController>.Instance.PlaySound(this.audioSpinDone, 1f);
			base.StartCoroutine(this.StopSpin());
		}
	}

	private IEnumerator StopSpin()
	{
		yield return new WaitForSeconds(0.4f);
		int i = 0;
		float time = 0.2f;
		WaitForSeconds waitTime = new WaitForSeconds(time);
		while (i < 3)
		{
			yield return waitTime;
			this.ActiveHighlightSpin(this.lockSpinId, false);
			yield return waitTime;
			this.ActiveHighlightSpin(this.lockSpinId, true);
			time -= 0.05f;
			i++;
		}
		yield return new WaitForSeconds(0.25f);
		this.SpinDone();
		yield break;
	}

	private void SpinDone()
	{
		string value = "CONGRATULATIONS!";
		if (!string.IsNullOrEmpty(value))
		{
			InforReward[] array = new InforReward[]
			{
				new InforReward()
			};
			array[0].amount = ProfileManager.spinProfile.GetSpinAmount(this.lockSpinId);
			array[0].item = ProfileManager.spinProfile.GetSpinGift(this.lockSpinId);
			PopupManager.Instance.ShowCongratulation(array, true, null);
			this.GetSpinReward();
		}
		this.isShowingReward = false;
	}

	private void GetSpinReward()
	{
		if (!this.isSpin)
		{
			return;
		}
		this.isSpin = false;
		this.isShowGift = false;
		ProfileManager.spinProfile.ClampSpinGift(this.lockSpinId);
		this.ResetStateSpin();
		MenuManager.Instance.topUI.ReloadCoin();
	}

	private void ResetStateSpin()
	{
		this.rectSelect.eulerAngles = Vector3.zero;
		for (int i = 0; i < this.itemSpins.Length; i++)
		{
			this.ActiveHighlightSpin(i, false);
			this.ActiveBlackSpin(i, false);
		}
		this.ShowButtonSpin();
	}

	private void ShowButtonSpin()
	{
		this.ShowTotalSpin();
		this.btnSpin.interactable = ((AdmobManager.Instance.IsVideoReady() && ProfileManager.spinProfile.Free) || ProfileManager.spinProfile.TotalSpin > 0);
		this.obj_EffectBtnSpin.SetActive(this.btnSpin.interactable);
		if (ProfileManager.spinProfile.TotalSpin > 0)
		{
			this.txtSpin.alignment = TextAnchor.MiddleCenter;
			this.txtSpin.text = PopupManager.Instance.GetText(Localization0.Spin, null).ToUpper();
			this.objVideo.gameObject.SetActive(false);
		}
		else if (ProfileManager.spinProfile.Free)
		{
			this.txtSpin.alignment = TextAnchor.MiddleLeft;
			this.txtSpin.text = PopupManager.Instance.GetText(Localization0.Spin_now, null).ToUpper();
			this.objVideo.gameObject.SetActive(true);
			base.StartCoroutine(this.WaitVideo());
		}
		Behaviour behaviour = this.txtWaitTime;
		bool enabled = !ProfileManager.spinProfile.Free;
		this.txtFreeSpin.enabled = enabled;
		behaviour.enabled = enabled;
	}

	private IEnumerator WaitVideo()
	{
		yield return new WaitUntil(() => AdmobManager.Instance.IsVideoReady() && ProfileManager.spinProfile.Free);
		this.obj_EffectBtnSpin.SetActive(true);
		yield break;
	}

	private void ActiveHighlightSpin(int id, bool enable = true)
	{
		this.itemSpins[id].obj_Active.SetActive(enable);
	}

	private void ActiveBlackSpin(int id, bool enable = true)
	{
		this.itemSpins[id].obj_Lock.SetActive(enable);
	}

	private void ShowBorderWenSpinning()
	{
		if (this.spinOldId >= 0)
		{
			this.ActiveHighlightSpin(this.spinOldId, false);
			this.ActiveBlackSpin(this.spinOldId, true);
		}
		this.ActiveBlackSpin(this.spinId, false);
		this.ActiveHighlightSpin(this.spinId, true);
	}

	[Header("Free spin :")]
	public Button btnSpin;

	public GameObject obj_EffectBtnSpin;

	public CardBase[] itemSpins;

	public PopupAddSpin addSpin;

	public Text txtTotalSpin;

	public Animation animSpin;

	public AnimationClip[] spinClips;

	public RectTransform rectSelect;

	public Text txtSpin;

	public GameObject objVideo;

	public Text txtFreeSpin;

	public Text txtWaitTime;

	public AudioClip audioSpin;

	public AudioClip audioSpinDone;

	[Header("-----------------------")]
	private int lockSpinId;

	private int spinId;

	private int spinOldId;

	private bool isSpinning;

	private bool isShowingReward;

	private bool isShowGift;

	private int _oldWaitTime;

	private bool isSpin;

	private Vector3 selectEuler;

	[SerializeField]
	private RectTransform tableObj;

	private int _totalSpin;

	private bool isReadyToClaimed;

	private int wait;

	private int seconds;

	private int minutes;
}
