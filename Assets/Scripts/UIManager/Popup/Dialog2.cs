using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager.Popup
{
	public class Dialog2 : MonoBehaviour
	{
		public void Show(Action<bool> isOK, int typeDialog, string content, string title = "Alert")
		{
			base.gameObject.SetActive(true);
			this.typeDialog = typeDialog;
			this.isOK = isOK;
			this.txtTitle.text = title;
			this.txtContent.text = content;
			this.ShowTypeDialog();
			
		}

		private void ShowTypeDialog()
		{
			this.groupButton1.SetActive(this.typeDialog == 0);
			this.groupButton2.SetActive(this.typeDialog != 0);
		}

		public void OnOk()
		{
			
			if (this.isOK != null)
			{
				this.isOK(true);
			}
			AudioClick.Instance.OnClick();
			base.gameObject.SetActive(false);
		}

		public void OnCancel()
		{
			
			if (this.isOK != null)
			{
				this.isOK(false);
			}
			AudioClick.Instance.OnClick();
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Text txtTitle;

		[SerializeField]
		private Text txtContent;

		[SerializeField]
		private GameObject groupButton1;

		[SerializeField]
		private GameObject groupButton2;

		private int typeDialog;

		private Action<bool> isOK;
	}
}
