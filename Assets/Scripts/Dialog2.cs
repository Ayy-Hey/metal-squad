using System;
using UnityEngine;
using UnityEngine.UI;

public class Dialog2 : PopupBase
{
	public override void Show()
	{
		base.Show();
		this.groupButton1.SetActive(this.typeDialog == 0);
		this.groupButton2.SetActive(this.typeDialog != 0);
		
	}

	public void OnOk()
	{
		
		if (this.isOK != null)
		{
			this.isOK(true);
		}
		base.OnClose();
	}

	public override void OnClose()
	{
		if (this.isOK != null)
		{
			this.isOK(false);
		}
		base.OnClose();
	}

	[SerializeField]
	private GameObject groupButton1;

	[SerializeField]
	private GameObject groupButton2;

	[Header("---------------Show()---------------")]
	public Text txtTitle;

	public Text txtContent;

	public int typeDialog;

	public Action<bool> isOK;
}
