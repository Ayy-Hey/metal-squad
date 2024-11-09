using System;
using System.Text;
using CrossAdPlugin;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
	public class PopupDailySale : PopupBase
	{
		public void OnShowDailySaleRandom()
		{
			this.Show();
			int num = UnityEngine.Random.Range(0, this.packDailySale.Length - 1);
			for (int i = 0; i < this.packDailySale.Length; i++)
			{
				if (i != num)
				{
					this.packDailySale[i].gameObject.SetActive(false);
				}
			}
			this.packDailySale[num].OnShowRandom(delegate
			{
				if (this.OnPurchaseSuccessed != null)
				{
					this.OnPurchaseSuccessed();
				}
				this.OnClose();
				base.gameObject.SetActive(false);
			});
			string parameterValue = "GamePlay";
			try
			{
				parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
			
		}

		public void OnShowDailySale()
		{
			string parameterValue = "GamePlay";
			try
			{
				parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
			try
			{
			
			}
			catch
			{
			}
			if (ProfileManager.dailySaleProfile.ID < 0)
			{
				return;
			}
			this.Show();
			this.packDailySale[ProfileManager.dailySaleProfile.ID].OnShow(delegate
			{
				if (this.OnPurchaseSuccessed != null)
				{
					this.OnPurchaseSuccessed();
				}
				ProfileManager.dailySaleProfile.ID = -1;
				this.OnClose();
			});
		}

		public void OnShowDoubleGem()
		{
			string parameterValue = "GamePlay";
			try
			{
				parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
			
			this.Show();
			this.ObjOverrideDailySale.SetActive(true);
		}

		public void LateUpdate()
		{
			if (this.txtCountdownDailySale != null)
			{
				this.builderTimeStarterPack.Length = 0;
				this.builderTimeStarterPack.Append("(");
				int value = 24 - DateTime.Now.Hour;
				this.builderTimeStarterPack.Append(value);
				this.builderTimeStarterPack.Append("h ");
				int value2 = 60 - DateTime.Now.Minute;
				this.builderTimeStarterPack.Append(value2);
				this.builderTimeStarterPack.Append("m ");
				int value3 = 60 - DateTime.Now.Second;
				this.builderTimeStarterPack.Append(value3);
				this.builderTimeStarterPack.Append("s");
				this.builderTimeStarterPack.Append(")");
				this.txtCountdownDailySale.text = this.builderTimeStarterPack.ToString();
			}
		}

		public void ShowInapp(int type)
		{
			if (this.ObjOverrideDailySale != null)
			{
				try
				{
					Singleton<CrossAdsManager>.Instance.HideFloatAds();
				}
				catch
				{
				}
			}
			AudioClick.Instance.OnClick();
			base.transform.localScale = Vector3.zero;
			PopupManager.Instance.ShowInapp((INAPP_TYPE)type, delegate(bool isClosed)
			{
				if (isClosed)
				{
					LeanTween.scale(base.gameObject, Vector3.one, 0.3f);
					try
					{
						MenuManager.Instance.topUI.ReloadCoin();
					}
					catch
					{
					}
					if (this.ObjOverrideDailySale != null)
					{
						this.ObjOverrideDailySale.SetActive(false);
						this.OnClose();
					}
				}
			});
		}

		[SerializeField]
		private PackInforDailySale[] packDailySale;

		[SerializeField]
		private Text txtCountdownDailySale;

		public GameObject ObjOverrideDailySale;

		private StringBuilder builderTimeStarterPack = new StringBuilder();

		public Action OnPurchaseSuccessed;
	}
}
