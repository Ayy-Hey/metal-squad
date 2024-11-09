using System;
using UnityEngine;
using UnityEngine.UI;

namespace Profile.UI
{
	public class VipProfile : MonoBehaviour
	{
		public void Show(GameObject root, Action<bool> callback)
		{
			this.callback = callback;
			this.root = root;
			int level = (int)ProfileManager.inAppProfile.vipProfile.level;
			int num = Mathf.Min(level + 1, Enum.GetValues(typeof(E_Vip)).Length - 2);
			float num2 = (float)ProfileManager.inAppProfile.vipProfile.Point;
			this.imgIcon.sprite = this.spritesIcon[Mathf.Min(level + 1, this.spritesIcon.Length - 1)];
			this.txtExp.text = ProfileManager.inAppProfile.vipProfile.Point + " / " + DataLoader.vipData.Levels[num].point;
			this.slider.value = (num2 - (float)((level >= 0) ? DataLoader.vipData.Levels[level].point : 0)) / (float)(DataLoader.vipData.Levels[num].point - ((level >= 0) ? DataLoader.vipData.Levels[level].point : 0));
			this.CheckThongBao();
		}

		public void ShowPopup()
		{
			PopupManager.Instance.ShowVipPopup(delegate(bool isClosed)
			{
				this.CheckThongBao();
				this.callback(true);
			}, false);
		}

		private void CheckThongBao()
		{
			this.imgThongBao.enabled = ProfileManager.inAppProfile.vipProfile.HasAnyGift();
		}

		[SerializeField]
		private Image imgIcon;

		[SerializeField]
		private Sprite[] spritesIcon;

		[SerializeField]
		private Text txtExp;

		[SerializeField]
		private Slider slider;

		[SerializeField]
		private Image imgThongBao;

		private Action<bool> callback;

		private GameObject root;
	}
}
