using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rank.UI
{
	public class InforItem : MonoBehaviour
	{
		public void Show(Vector2 pos, string infor)
		{
			this.isShow = !this.isShow;
			base.CancelInvoke();
			if (!this.isShow)
			{
				this.Hide();
				return;
			}
			base.gameObject.SetActive(true);
			this.rectTransform.anchoredPosition = pos;
			this.txtInfor.text = infor;
			base.Invoke("Hide", 5f);
		}

		public void Show(string infor)
		{
			base.CancelInvoke();
			base.gameObject.SetActive(true);
			this.txtInfor.text = infor;
			base.Invoke("Hide", 3f);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Text txtInfor;

		[SerializeField]
		private RectTransform rectTransform;

		private bool isShow;
	}
}
