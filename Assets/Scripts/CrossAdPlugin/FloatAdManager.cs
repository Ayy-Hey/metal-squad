using System;
using UnityEngine;
using UnityEngine.UI;

namespace CrossAdPlugin
{
	public class FloatAdManager : Singleton<FloatAdManager>
	{
		protected new virtual void Awake()
		{
			base.Awake();
			this.mFloatAdRectTran = this.floatAdButton.GetComponent<RectTransform>();
		}

		public void OnClickFloatAds()
		{
			if (this.adsItem != null)
			{
				Application.OpenURL(this.adsItem.adsUrl);
			}
		}

		private void FixedUpdate()
		{
			if (!this.isEnabled)
			{
				this.floatAdButton.SetActive(false);
			}
			else if (Singleton<CrossAdsManager>.Instance.Config != null && Singleton<CrossAdsManager>.Instance.Config.adsList != null)
			{
				CrossAdsManager instance = Singleton<CrossAdsManager>.Instance;
				if (Time.realtimeSinceStartup - this.adShowLastTime > instance.Config.changeAdsDuration)
				{
					this.adShowLastTime = Time.realtimeSinceStartup;
					int nextAdIndex = this.GetNextAdIndex(instance);
					BPDebug.LogMessage("adIndex Showed: " + nextAdIndex, false);
					if (nextAdIndex != -1 && instance.CachedTexture[nextAdIndex] != null)
					{
						this.adsItem = instance.CachedAdsItem[nextAdIndex];
						this.adsIcon.texture = instance.CachedTexture[nextAdIndex];
						this.floatAdButton.SetActive(true);
						this.isCanShow = true;
					}
					else
					{
						this.adsItem = null;
						this.floatAdButton.SetActive(false);
					}
				}
			}
			else
			{
				this.floatAdButton.SetActive(false);
			}
		}

		private int GetNextAdIndex(CrossAdsManager cm)
		{
			int num = -1;
			float num2 = 2.14748365E+09f;
			for (int i = 0; i < cm.CachedShowedAdTimer.Length; i++)
			{
				float num3 = cm.CachedShowedAdTimer[i] / cm.CachedAdsItem[i].percentage;
				if (num2 > num3)
				{
					num2 = num3;
					num = i;
				}
			}
			if (num != -1)
			{
				cm.CachedShowedAdTimer[num] += cm.Config.changeAdsDuration;
			}
			return num;
		}

		public void HideAd()
		{
			this.isEnabled = false;
		}

		public void ShowAd(Vector2 atPos, Vector2 size)
		{
			this.mFloatAdRectTran.position = atPos;
			this.mFloatAdRectTran.sizeDelta = size;
			this.adShowLastTime = 0f;
			this.isEnabled = true;
			if (this.isCanShow)
			{
				this.floatAdButton.SetActive(true);
			}
		}

		public RawImage adsIcon;

		public GameObject floatAdButton;

		private AdsItem adsItem;

		private CrossAdsConfig config;

		private bool isEnabled;

		private float waitForNextShow;

		private Coroutine mSeqCoroutine;

		private float adShowLastTime;

		private bool isCanShow;

		private RectTransform mFloatAdRectTran;
	}
}
