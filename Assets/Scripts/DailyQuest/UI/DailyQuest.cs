using System;
using System.Text;
using Rank.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DailyQuest.UI
{
	public class DailyQuest : MonoBehaviour
	{
		public void Show(Action callback, Action<bool> callbackRank)
		{
			this.callback = callback;
			this.callbackRank = callbackRank;
			this.obj_ListDailyQuest.SetActive(true);
			this.obj_ActiveDailyQuest.SetActive(true);
			this.inforItem.Hide();
			for (int i = 0; i < this.Mission.Length; i++)
			{
				string text = PopupManager.Instance.GetText((Localization0)DailyQuestManager.Instance.DailyQuests[i].IdDesc, DailyQuestManager.Instance.DailyQuests[i].ValueDesc);
				int gold = DailyQuestManager.Instance.DailyQuests[i].Gold;
				int gem = DailyQuestManager.Instance.DailyQuests[i].Gem;
				int total = DailyQuestManager.Instance.GetTotal(i)[0];
				int total2 = DailyQuestManager.Instance.DailyQuests[i].Total;
				this.Mission[i].Show(text, gold > 0, Mathf.Max(gold, gem), total, total2, DailyQuestManager.Instance.DailyQuests[i].IDPopup);
				this.Mission[i].SetState((EStateDailyQuest)DailyQuestManager.Instance.DailyQuests[i].State);
				
			}
			this.strBuilder = new StringBuilder();
			this.strBuilder.Append(24 - DateTime.Now.Hour);
			this.strBuilder.Append("h ");
			this.strBuilder.Append(60 - DateTime.Now.Minute);
			this.strBuilder.Append("m ");
			this.strBuilder.Append(60 - DateTime.Now.Second);
			this.strBuilder.Append("s ");
			this.txtTimer.text = this.strBuilder.ToString();
			this.OnSorting();
		}

		private void LateUpdate()
		{
			if (this.strBuilder == null)
			{
				return;
			}
			this.strBuilder.Length = 0;
			int value = 24 - DateTime.Now.Hour;
			this.strBuilder.Append(value);
			this.strBuilder.Append("h ");
			int value2 = 60 - DateTime.Now.Minute;
			this.strBuilder.Append(value2);
			this.strBuilder.Append("m ");
			int value3 = 60 - DateTime.Now.Second;
			this.strBuilder.Append(value3);
			this.strBuilder.Append("s ");
			this.txtTimer.text = this.strBuilder.ToString();
		}

		public void Hide()
		{
			this.obj_ListDailyQuest.SetActive(false);
			this.obj_ActiveDailyQuest.SetActive(false);
		}

		public void GetReward(int id)
		{
			EStateDailyQuest state = (EStateDailyQuest)DailyQuestManager.Instance.DailyQuests[id].State;
			if (state == EStateDailyQuest.DONE)
			{
				
				AudioClick.Instance.OnClick();
				DailyQuestManager.Instance.DailyQuests[id].State = 2;
				this.Mission[id].SetState((EStateDailyQuest)DailyQuestManager.Instance.DailyQuests[id].State);
				int num = (int)ProfileManager.inAppProfile.vipProfile.level;
				int num2 = (num < 0 || !DataLoader.vipData.Levels[num].dailyReward.isX2RewardDailyQuest) ? 1 : 2;
				if (num2 == 1)
				{
					num = -1;
				}
				InforReward[] array = new InforReward[]
				{
					new InforReward()
				};
				array[0].amount = Mathf.Max(DailyQuestManager.Instance.DailyQuests[id].Gold, DailyQuestManager.Instance.DailyQuests[id].Gem) * num2;
				array[0].item = ((DailyQuestManager.Instance.DailyQuests[id].Gold <= 0) ? Item.Gem : Item.Gold);
				array[0].vipLevel = num;
				PopupManager.Instance.ShowCongratulation(array, true, this.callback);
				PopupManager.Instance.SaveReward(array[0].item, array[0].amount, "DailyQuest_" + id, null);
			
				MenuManager.Instance.topUI.ShowCoin();
				this.OnSorting();
			}
		}

		public void ShowInforExp(int id)
		{
			this.inforItem.Show(new Vector2(279f, (float)(49 - id * 155)), "Rank EXP - Use for rank up");
		}

		public void HideInfor()
		{
			this.inforItem.Hide();
		}

		private void OnSorting()
		{
			for (int i = 0; i < this.Mission.Length; i++)
			{
				if (DailyQuestManager.Instance.DailyQuests[i].State == 2)
				{
					this.Mission[i].transform.SetAsLastSibling();
				}
			}
			for (int j = 0; j < this.Mission.Length; j++)
			{
				if (DailyQuestManager.Instance.DailyQuests[j].State == 1)
				{
					this.Mission[j].transform.SetSiblingIndex(0);
				}
			}
		}

		public MissionDailyQuest[] Mission;

		[SerializeField]
		private Text txtTimer;

		private Action callback;

		private Action<bool> callbackRank;

		[SerializeField]
		private InforItem inforItem;

		private StringBuilder strBuilder;

		public GameObject obj_ListDailyQuest;

		public GameObject obj_ActiveDailyQuest;
	}
}
