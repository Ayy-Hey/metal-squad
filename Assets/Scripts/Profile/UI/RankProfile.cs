using System;
using Rank;
using UnityEngine;
using UnityEngine.UI;

namespace Profile.UI
{
	public class RankProfile : MonoBehaviour
	{
		public void Show(GameObject root, Action<bool> callback)
		{
			this.callback = callback;
			this.root = root;
			RankInfor rankCurrentByExp = RankManager.Instance.GetRankCurrentByExp(ProfileManager.ExpRankProfile.Data.Value);
			this.txtName.text = PopupManager.Instance.GetText((Localization0)rankCurrentByExp.IdName, null);
			this.txtExp.text = RankManager.Instance.ExpCurrent();
			this.imgIcon.sprite = this.spriteIcons[rankCurrentByExp.Level];
			this.slider.value = RankManager.Instance.ExpCurrent01();
		}

		public void ShowDailyQuest()
		{
			this.popupDailyQuest.ShowDailyQuest(delegate(bool cb)
			{
				this.callback(cb);
			});
		}

		[SerializeField]
		private Text txtName;

		[SerializeField]
		private Text txtExp;

		[SerializeField]
		private Image imgIcon;

		[SerializeField]
		private Sprite[] spriteIcons;

		[SerializeField]
		private Slider slider;

		[SerializeField]
		private PopupQuest popupDailyQuest;

		private Action<bool> callback;

		private GameObject root;
	}
}
