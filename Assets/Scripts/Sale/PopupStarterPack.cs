using System;


namespace Sale
{
	public class PopupStarterPack : PopupBase
	{
		public void OnShowPack(bool isInGamePlay)
		{
			if (SaleManager.Instance.isShowAll && !isInGamePlay)
			{
				this.OnShowAll();
			}
			else
			{
				this.OnShow1();
			}
		}

		private void OnShowAll()
		{
			if (this.packShowAll == null)
			{
				this.OnShow1();
				return;
			}
			this.packSingle.gameObject.SetActive(false);
			this.packShowAll.gameObject.SetActive(true);
			this.Show();
			PackStarterPackAll packStarterPackAll = this.packShowAll;
			packStarterPackAll.OnClose = (Action)Delegate.Combine(packStarterPackAll.OnClose, new Action(delegate()
			{
				this.OnClose();
			}));
			this.packShowAll.OnShow();
			string parameterValue = "GamePlay";
			try
			{
				parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
			}
			catch
			{
			}
			
		}

		private void OnShow1()
		{
			this.Show();
			int idstarterPack = SaleManager.Instance.IDStarterPack;
			SaleManager.Instance.profileStarterPacks[idstarterPack].CountDisplay++;
			this.packSingle.gameObject.SetActive(true);
			if (this.packShowAll != null)
			{
				this.packShowAll.gameObject.SetActive(false);
			}
			PackStarterPackSingle packStarterPackSingle = this.packSingle;
			packStarterPackSingle.OnClose = (Action)Delegate.Combine(packStarterPackSingle.OnClose, new Action(delegate()
			{
				this.OnClose();
			}));
			this.packSingle.OnShow(idstarterPack, delegate
			{
				this.OnClose();
				if (this.OnPurchaseSuccessed != null)
				{
					this.OnPurchaseSuccessed();
				}
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

		public PackStarterPackSingle packSingle;

		public PackStarterPackAll packShowAll;

		public Action OnPurchaseSuccessed;
	}
}
