using System;
using UnityEngine;
using UnityEngine.UI;

namespace Achievement.UI
{
	public class ElementAchievement : MonoBehaviour
	{
		public void Show(InforAchievement infor, int index, Action<bool> callback, Action<bool> callbackRank)
		{
			this.txt_Claim.text = PopupManager.Instance.GetText(Localization0.Claim, null);
			base.gameObject.SetActive(true);
			Vector3 position = this.rectransform.position;
			position.z = 0f;
			this.rectransform.position = position;
			this.rectransform.localScale = Vector3.one;
			this.infor = infor;
			this.index = index;
			this.callback = callback;
			this.callbackRank = callbackRank;
			this.imgIcon.sprite = Resources.Load<Sprite>(infor.Image);
			this.txtIconValue.text = infor.ValueIcon;
			this.txtName.text = PopupManager.Instance.GetText((Localization0)infor.IdName, infor.ValueName) + " " + this.ConvertToRomanNumber(DataLoader.achievement[infor.index].LevelCurrent + 1);
			this.description.text = PopupManager.Instance.GetText((Localization0)infor.IdDesc, infor.ValueDesc);
			this.txtCoin.text = infor.MS_Secured.ToString();
			this.txtExp.text = infor.Exp_Secured.ToString();
			this.txtValueSlider.text = infor.Total + "/" + infor.TotalRequirement;
			EChievement state = infor.State;
			if (state != EChievement.DOING)
			{
				if (state != EChievement.DONE)
				{
					if (state == EChievement.CLEAR)
					{
						this.objSlide.gameObject.SetActive(false);
						this.objReward.SetActive(false);
						this.objClaim.SetActive(false);
						this.objCompleted.SetActive(true);
					}
				}
				else
				{
					this.objSlide.gameObject.SetActive(false);
					this.objReward.SetActive(true);
					this.objClaim.SetActive(true);
					this.objCompleted.SetActive(false);
				}
			}
			else
			{
				float value = Mathf.Clamp01((float)infor.Total / (float)infor.TotalRequirement);
				this.objSlide.gameObject.SetActive(true);
				this.slide.value = value;
				this.objReward.SetActive(true);
				this.objClaim.SetActive(false);
				this.objCompleted.SetActive(false);
			}
		}

		public string ConvertToRomanNumber(int number)
		{
			switch (number)
			{
			case 1:
				return "I";
			case 2:
				return "II";
			case 3:
				return "III";
			case 4:
				return "IV";
			case 5:
				return "V";
			case 6:
				return "VI";
			case 7:
				return "VII";
			case 8:
				return "VIII";
			case 9:
				return "IX";
			case 10:
				return "X";
			case 11:
				return "XI";
			case 12:
				return "XII";
			default:
				return "...";
			}
		}

		private void Show(InforAchievement infor, int index)
		{
			this.infor = infor;
			this.index = index;
			this.imgIcon.sprite = Resources.Load<Sprite>(infor.Image);
			this.txtIconValue.text = infor.ValueIcon;
			this.txtName.text = PopupManager.Instance.GetText((Localization0)infor.IdName, infor.ValueName) + " " + (DataLoader.achievement[infor.index].LevelCurrent + 1);
			this.description.text = PopupManager.Instance.GetText((Localization0)infor.IdDesc, infor.ValueDesc);
			this.txtCoin.text = infor.MS_Secured.ToString();
			this.txtExp.text = infor.Exp_Secured.ToString();
			this.txtValueSlider.text = infor.Total + "/" + infor.TotalRequirement;
			EChievement state = infor.State;
			if (state != EChievement.DOING)
			{
				if (state != EChievement.DONE)
				{
					if (state == EChievement.CLEAR)
					{
						this.objSlide.gameObject.SetActive(false);
						this.objReward.SetActive(false);
						this.objClaim.SetActive(false);
						this.objCompleted.SetActive(true);
					}
				}
				else
				{
					this.objSlide.gameObject.SetActive(false);
					this.objReward.SetActive(true);
					this.objClaim.SetActive(true);
					this.objCompleted.SetActive(false);
				}
			}
			else
			{
				float value = Mathf.Clamp01((float)infor.Total / (float)infor.TotalRequirement);
				this.objSlide.gameObject.SetActive(true);
				this.slide.value = value;
				this.objReward.SetActive(true);
				this.objClaim.SetActive(false);
				this.objCompleted.SetActive(false);
			}
		}

		public void Claim()
		{
			
			AudioClick.Instance.OnClick();
			int num = 0;
			int num2 = (int)ProfileManager.inAppProfile.vipProfile.level;
			if (num2 >= 0)
			{
				num = DataLoader.vipData.Levels[num2].dailyReward.GemBonusAchievement;
			}
			int num3 = (num2 < 0 || !DataLoader.vipData.Levels[num2].dailyReward.isX2Achievement) ? 1 : 2;
			if (num3 == 1)
			{
				num2 = -1;
			}
			InforReward[] array = (num <= 0) ? new InforReward[2] : new InforReward[3];
			array[0] = new InforReward();
			array[0].item = Item.Exp;
			array[0].amount = this.infor.Exp_Secured.Value * num3;
			array[0].vipLevel = num2;
			array[1] = new InforReward();
			array[1].item = Item.Gem;
			array[1].amount = this.infor.MS_Secured.Value * num3;
			array[1].vipLevel = num2;
			if (num > 0)
			{
				array[2] = new InforReward();
				array[2].amount = num;
				array[2].item = Item.Gem;
				array[2].vipLevel = (int)ProfileManager.inAppProfile.vipProfile.level;
			}
			PopupManager.Instance.ShowCongratulation(array, true, null);
			PopupManager.Instance.SaveReward(Item.Gem, array[1].amount + num, "_Achievement", null);
			PopupManager.Instance.SaveReward(Item.Exp, array[0].amount, "_Achievement", delegate(bool cb)
			{
				this.callbackRank(cb);
			});
			int num4 = this.infor.Total - this.infor.TotalRequirement;
			UnityEngine.Debug.Log("sodu_cu: " + num4);
			AchievementManager.Instance.AddAchievement(this.infor);
			this.infor.State = EChievement.CLEAR;
			DataLoader.ReloadProfileAchievement();
			DataLoader.achievement[this.index].ResidueNumber = num4;
			EDailyQuest type = (EDailyQuest)DataLoader.achievement[this.index].Type;
			if (type == EDailyQuest.EARN_MONEY || type == EDailyQuest.ENEMY || type == EDailyQuest.BOSS_MODE)
			{
				DataLoader.achievement[this.index].achievement.Total += DataLoader.achievement[this.index].ResidueNumber;
				DataLoader.achievement[this.index].ResidueNumber = 0;
			}
			if (DataLoader.achievement[this.index].achievement.State == EChievement.DOING && DataLoader.achievement[this.index].achievement.Total >= DataLoader.achievement[this.index].achievement.TotalRequirement)
			{
				DataLoader.achievement[this.index].achievement.State = EChievement.DONE;
				
			}
			this.Show(DataLoader.achievement[this.index].achievement, this.index);
			if (this.callback != null)
			{
				this.callback(true);
			}
			MenuManager.Instance.topUI.ShowCoin();
		}

		[Header("_____________Icon___________")]
		[SerializeField]
		private Sprite[] iconSprites;

		[SerializeField]
		private Image imgIcon;

		[SerializeField]
		private Text txtIconValue;

		[SerializeField]
		[Header("_____________Description___________")]
		private Text txtName;

		[SerializeField]
		private Text description;

		[Header("_____________Reward___________")]
		[SerializeField]
		private GameObject objClaim;

		[SerializeField]
		private GameObject objCompleted;

		[SerializeField]
		private GameObject objReward;

		[SerializeField]
		private Text txtCoin;

		[SerializeField]
		private Text txtExp;

		[SerializeField]
		private Slider slide;

		[SerializeField]
		private GameObject objSlide;

		[SerializeField]
		private Text txtValueSlider;

		[SerializeField]
		private RectTransform rectransform;

		private InforAchievement infor;

		[SerializeField]
		private Text txt_Claim;

		private Action<bool> callback;

		private Action<bool> callbackRank;

		private int index;
	}
}
