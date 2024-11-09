using System;
using UnityEngine;
using UnityEngine.UI;

namespace CrossAdPlugin
{
	public class CrossAdsItem : MonoBehaviour
	{
		public void SetAdapter(AdsItem adsItem, OnItemClick onClickDel, int index)
		{
			this.onClickDel = onClickDel;
			this.title.text = adsItem.adsTitle;
			this.description.text = adsItem.adsDescription;
			this.index = index;
			if (this.cachedTexture == null)
			{
				CrossAdTextureInfo info = new CrossAdTextureInfo(adsItem.adsIconUrl, 0);
				base.StartCoroutine(BPNetworkUtils.DownloadImage(info, delegate(CrossAdTextureInfo rInfo)
				{
					this.cachedTexture = rInfo.DonwloadedTexture;
					this.icon.texture = rInfo.DonwloadedTexture;
				}));
			}
			else
			{
				this.icon.texture = this.cachedTexture;
			}
		}

		public void OnButtonClick()
		{
			if (this.onClickDel != null)
			{
				this.onClickDel(this.index);
			}
		}

		public RawImage icon;

		public Text title;

		public Text description;

		private Texture cachedTexture;

		private OnItemClick onClickDel;

		private int index;
	}
}
