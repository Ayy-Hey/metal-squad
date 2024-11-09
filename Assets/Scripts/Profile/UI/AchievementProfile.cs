using System;
using UnityEngine;
using UnityEngine.UI;

namespace Profile.UI
{
	public class AchievementProfile : MonoBehaviour
	{
		public void Show(GameObject root, Action<bool> callback)
		{
			this.callback = callback;
			this.root = root;
			int num = 0;
			foreach (string path in AchievementManager.Instance.GetLastAchievement())
			{
				this.imgLastAchievement[num].enabled = true;
				this.imgLastAchievement[num].sprite = Resources.Load<Sprite>(path);
				num++;
			}
		}

		public void ShowAchievement()
		{
			
			this.popupAchievement.ShowAchievement(delegate(bool cb)
			{
				this.callback(cb);
			});
		}

		[SerializeField]
		private Image[] imgLastAchievement;

		[SerializeField]
		private PopupQuest popupAchievement;

		private Action<bool> callback;

		private GameObject root;
	}
}
