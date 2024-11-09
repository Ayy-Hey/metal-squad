using System;
using UnityEngine;

namespace CrossAdPlugin
{
	public class AdListManager : MonoBehaviour
	{
		public void Init(CrossAdsConfig config)
		{
			if (config != null)
			{
				this.config = config;
				this.adsList = config.adsList;
			}
		}

		private void ShowCrossAdsList()
		{
			if (this.adsList != null)
			{
				base.gameObject.SetActive(true);
				for (int i = 0; i < this.adsList.Length; i++)
				{
					AdsItem adsItem = this.adsList[i];
					CrossAdsItem crossAdsItem = UnityEngine.Object.Instantiate<CrossAdsItem>(this.adItemPrefab, Vector3.zero, Quaternion.identity);
					crossAdsItem.gameObject.name = i.ToString();
					crossAdsItem.SetAdapter(adsItem, new OnItemClick(this.OnClickAdsItem), i);
					crossAdsItem.transform.parent = this.scrollViewContent;
				}
			}
		}

		public void OnClickAdsItem(int index)
		{
			Application.OpenURL(this.adsList[index].adsUrl);
		}

		public void ShowAd()
		{
			this.ShowCrossAdsList();
		}

		public void HideAd()
		{
			base.gameObject.SetActive(false);
		}

		public CrossAdsItem adItemPrefab;

		public Transform scrollViewContent;

		private AdsItem[] adsList;

		private CrossAdsConfig config;
	}
}
