using System;
using System.Collections;
using ABI;
using CrossAdPlugin;
using Rank;
using UnityEngine;
using UnityEngine.UI;

namespace Profile.UI
{
	public class PopupProfile : PopupBase
	{
		private void OnDisable()
		{
			if (this.OnHide != null)
			{
				this.OnHide();
			}
		}

		public void OnShow(UserLB user, Action OnHide = null)
		{
			if (user == null)
			{
				return;
			}
			this.OnHide = OnHide;
			this.Show();
			base.StopAllCoroutines();
			this.isReadyShare = false;
			this.txtName.text = user.UserName;
			this.imgAvatar.sprite = this.imgAvatarDefault;
			this.imgAvatar.gameObject.name = "avatar";
			this.LoadAvatar(user);
			this.txtPower.text = user.GetValue(PlayfabManager.EKey.Power);
			int num = 0;
			try
			{
				num = user.entry.StatValue;
			}
			catch
			{
			}
			this.txtTotalStar.text = num.ToString();
			int num2 = 0;
			try
			{
				num2 = int.Parse(user.GetValue(PlayfabManager.EKey.LevelRank));
			}
			catch
			{
			}
			RankInfor rankInfoByLevel = RankManager.Instance.GetRankInfoByLevel(num2);
			this.imgRank.sprite = this.spriteRank[rankInfoByLevel.Level];
			this.txtNameRank.text = PopupManager.Instance.GetText((Localization0)rankInfoByLevel.IdName, null);
			RankInfor rankInfoByLevel2 = RankManager.Instance.GetRankInfoByLevel(Mathf.Clamp(num2 + 1, 0, DataLoader.rankInfor.Length - 1));
			this.imgProgressRank.fillAmount = RankManager.Instance.ExpCurrent01(user.TotalExpRank, rankInfoByLevel, rankInfoByLevel2);
			this.txtProgressRank.text = RankManager.Instance.ExpCurrent(user.TotalExpRank, rankInfoByLevel, rankInfoByLevel2);
			int exp = int.Parse(user.GetValue(PlayfabManager.EKey.Vip));
			int num3 = VipManager.Instance.LevelCurrent(exp);
			num3++;
			this.imgVip.sprite = this.spriteVip[num3];
			this.imgProgressVip.fillAmount = VipManager.Instance.ExpCurrent01(exp, num3);
			this.txtProgressVip.text = VipManager.Instance.ExpCurrent(exp, num3);
			int num4 = 0;
			try
			{
				num4 = user.GetCharacters.Length;
			}
			catch (Exception ex)
			{
			}
			for (int i = 0; i < num4; i++)
			{
				int num5 = -1;
				int num6 = 0;
				try
				{
					num5 = int.Parse(user.GetCharacters[i]);
					num6 = num5;
					if (num6 > 2)
					{
						num6 = num5 / 10;
					}
				}
				catch (Exception ex2)
				{
				}
				this.charCards[i].obj_Lock.SetActive(num5 < 0);
				if (!this.charCards[i].obj_Lock.activeSelf)
				{
					int num7 = num6 + i;
					if (num7 >= 0 && num7 <= 4)
					{
						this.charCards[i].img_Boder.color = PopupManager.Instance.color_Rank[num7];
						this.charCards[i].img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[num7];
					}
				}
				else
				{
					this.charCards[i].img_Boder.color = PopupManager.Instance.color_Rank[i];
					this.charCards[i].img_IconRank.sprite = PopupManager.Instance.sprite_IconRankItem[i];
				}
			}
			num4 = 0;
			try
			{
				num4 = user.GetWeapon1.Length;
			}
			catch (Exception ex3)
			{
			}
			for (int j = 0; j < num4; j++)
			{
				int num8 = int.Parse(user.GetWeapon1[j]);
				bool isLock = num8 < 0;
				if (num8 > 2)
				{
					num8 /= 10;
				}
				num8 = Mathf.Clamp(num8, 0, 2);
				int num9 = num8 + PopupManager.Instance.GetRankItemStart(this.ConvertIDToItemWeapon1(j));
				this.childProfileGun1[j].OnShow(PopupManager.Instance.sprite_IconRankItem_NoName[num9], PopupManager.Instance.sprite_GunMain[num8].Sprites[j], num9, isLock);
			}
			if (num4 < ProfileManager.weaponsRifle.Length)
			{
				int num10 = 2;
				bool isLock2 = num10 < 0;
				if (num10 > 2)
				{
					num10 /= 10;
				}
				num10 = Mathf.Clamp(num10, 0, 2);
				int num11 = num10 + PopupManager.Instance.GetRankItemStart(this.ConvertIDToItemWeapon1(5));
				this.childProfileGun1[5].OnShow(PopupManager.Instance.sprite_IconRankItem_NoName[num11], PopupManager.Instance.sprite_GunMain[num10].Sprites[5], num11, isLock2);
			}
			num4 = 0;
			try
			{
				num4 = user.GetWeapon2.Length;
			}
			catch (Exception ex4)
			{
			}
			for (int k = 0; k < num4; k++)
			{
				int num12 = int.Parse(user.GetWeapon2[k]);
				bool isLock3 = num12 < 0;
				if (num12 > 2)
				{
					num12 /= 10;
				}
				num12 = Mathf.Clamp(num12, 0, 2);
				int num13 = num12 + PopupManager.Instance.GetRankItemStart(this.ConvertIDToItemWeapon2(k));
				this.childProfileGun2[k].OnShow(PopupManager.Instance.sprite_IconRankItem_NoName[num13], PopupManager.Instance.sprite_GunSpecial[num12].Sprites[k], num13, isLock3);
			}
			if (num4 < ProfileManager.weaponsSpecial.Length)
			{
				int num14 = 2;
				bool isLock4 = num14 < 0;
				if (num14 > 2)
				{
					num14 /= 10;
				}
				num14 = Mathf.Clamp(num14, 0, 2);
				int num15 = num14 + PopupManager.Instance.GetRankItemStart(this.ConvertIDToItemWeapon2(5));
				this.childProfileGun2[5].OnShow(PopupManager.Instance.sprite_IconRankItem_NoName[num15], PopupManager.Instance.sprite_GunSpecial[num14].Sprites[5], num15, isLock4);
			}
			num4 = 0;
			try
			{
				num4 = user.GetWorldMap.Length;
			}
			catch (Exception ex5)
			{
			}
			for (int l = 0; l < num4; l++)
			{
				this.txtStarCampaign[l].text = user.GetWorldMap[l];
			}
		}

		private Item ConvertIDToItemWeapon1(int id)
		{
			switch (id)
			{
			case 0:
				return Item.M4A1_Fragment;
			case 1:
				return Item.Machine_Gun_Fragment;
			case 2:
				return Item.Ice_Gun_Fragment;
			case 3:
				return Item.Spread_Gun_Fragment;
			case 4:
				return Item.MGL_140_Fragment;
			case 5:
				return Item.Ct9_Gun_Fragment;
			default:
				return Item.M4A1_Fragment;
			}
		}

		private Item ConvertIDToItemWeapon2(int id)
		{
			switch (id)
			{
			case 0:
				return Item.Flame_Gun_Fragment;
			case 1:
				return Item.Sniper_Fragment;
			case 2:
				return Item.Laser_Gun_Fragment;
			case 3:
				return Item.Fc10_Gun_Fragment;
			case 4:
				return Item.Rocket_Fragment;
			case 5:
				return Item.Thunder_Shot_Fragment;
			default:
				return Item.Flame_Gun_Fragment;
			}
		}

		private void LoadAvatar(UserLB user)
		{
			this.imgAvatar.transform.GetChild(0).gameObject.SetActive(false);
			if (user == null)
			{
				return;
			}
			if (user.spriteAvatar != null)
			{
				this.imgAvatar.sprite = user.spriteAvatar;
				this.imgAvatar.transform.GetChild(0).gameObject.SetActive(true);
			}
			else if (user.isPlayer && !string.IsNullOrEmpty(ProfileManager.pvpProfile.AvatarUrl))
			{
				base.StartCoroutine(MonoSingleton<FBAndPlayfabMgr>.Instance.GetProfilePic(ProfileManager.pvpProfile.AvatarUrl, delegate(Texture2D tex)
				{
					try
					{
						this.imgAvatar.sprite = Sprite.Create(tex, new Rect(0f, 0f, (float)tex.width, (float)tex.height), Vector2.zero);
						this.imgAvatar.transform.GetChild(0).gameObject.SetActive(true);
					}
					catch
					{
					}
				}));
			}
		}

		public void OnShare()
		{
			
			base.StartCoroutine(this.WaitShare());
			if (!FirebaseDatabaseManager.Instance.isLoginFB)
			{
			
			}
			else
			{
				this.isReadyShare = true;
			}
		}

		private IEnumerator WaitForPlayFabLoginCoroutine()
		{
			PopupManager.Instance.ShowMiniLoading();
			yield return new WaitUntil(() => MonoSingleton<FBAndPlayfabMgr>.Instance.IsLoggedOnPlayFab);
			PopupManager.Instance.CloseMiniLoading();
			this.isReadyShare = true;
			FirebaseDatabaseManager.Instance.CheckVersionFB(delegate(int version)
			{
				DialogWarningOldData dialogWarningOldData = UnityEngine.Object.Instantiate(Resources.Load("Popup/DialogWarningOldData", typeof(DialogWarningOldData)), MenuManager.Instance.formCanvas.transform) as DialogWarningOldData;
				dialogWarningOldData.version = version;
				dialogWarningOldData.OnClosed = delegate()
				{
				};
				dialogWarningOldData.Show();
			});
			yield break;
		}

		private IEnumerator WaitShare()
		{
			yield return new WaitUntil(() => this.isReadyShare);
			Singleton<Share>.Instance.ShareScreenshotWithText("Profile");
			yield break;
		}

		public void ShowVip()
		{
			
			PopupManager.Instance.ShowVipPopup(null, false);
		}

		public static int indexSibling;

		public Sprite[] spriteRank;

		public Sprite[] spriteVip;

		public Text txtName;

		public Image imgRank;

		public Text txtNameRank;

		public Image imgProgressRank;

		public Text txtProgressRank;

		public Image imgVip;

		public Image imgProgressVip;

		public Text txtProgressVip;

		public Color color_normal;

		public Color color_lock;

		public CardChar[] charCards;

		public Sprite imgAvatarDefault;

		public Image imgAvatar;

		public Text txtPower;

		public Text txtTotalStar;

		public Text[] txtStarCampaign;

		private Action OnHide;

		public ChildProfileGun[] childProfileGun1;

		public ChildProfileGun[] childProfileGun2;

		private bool isReadyShare;
	}
}
