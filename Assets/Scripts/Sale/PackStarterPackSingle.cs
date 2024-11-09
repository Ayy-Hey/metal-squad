using System;
using UnityEngine;

namespace Sale
{
	public class PackStarterPackSingle : MonoBehaviour
	{
		public void OnShow(int ID, Action OnPurchaseSuccess)
		{
			base.gameObject.SetActive(true);
			for (int i = 0; i < this.packs.Length; i++)
			{
				this.packs[i].gameObject.SetActive(false);
			}
			this.packs[ID].OnShow(OnPurchaseSuccess);
		}

		public void OnBack()
		{
			base.gameObject.SetActive(false);
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}

		public PackInforStarterPack[] packs;

		public Action OnClose;
	}
}
