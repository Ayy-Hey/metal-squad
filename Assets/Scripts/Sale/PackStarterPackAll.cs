using System;
using UnityEngine;

namespace Sale
{
	public class PackStarterPackAll : MonoBehaviour
	{
		public void OnShow()
		{
			base.gameObject.SetActive(true);
			for (int i = 0; i < this.packs.Length; i++)
			{
				this.packs[i].OnShow(null);
			}
		}

		public void OnBack()
		{
			if (this.OnClose != null)
			{
				this.OnClose();
			}
			base.gameObject.SetActive(false);
		}

		public PackInforStarterPack[] packs;

		public Action OnClose;
	}
}
