using System;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
	public virtual void Show()
	{
		string parameterValue = string.Empty;
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
		AudioClick.Instance.OnClick();
		this.Localization();
		base.gameObject.SetActive(true);
		this.isActive = true;
	}

	public virtual void OnClose()
	{
		string parameterValue = string.Empty;
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
		if (this.OnClosed != null)
		{
			this.OnClosed();
		}
		AudioClick.Instance.OnClick();
		if (!this.isSkipDisActive)
		{
			base.gameObject.SetActive(false);
		}
		this.isActive = false;
	}

	public void Localization()
	{
		for (int i = 0; i < this.listTextLocalization.Length; i++)
		{
			if (this.listTextLocalization[i].isUpcaseText)
			{
				this.listTextLocalization[i].txt_Text.text = (this.listTextLocalization[i].textStart + PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd).ToUpper();
			}
			else
			{
				this.listTextLocalization[i].txt_Text.text = this.listTextLocalization[i].textStart + PopupManager.Instance.GetText(this.listTextLocalization[i].key, null) + this.listTextLocalization[i].textEnd;
			}
		}
	}

	public PopupUI idPopup;

	[HideInInspector]
	public bool isActive;

	public Action OnClosed;

	public bool isSkipDisActive;

	public TextLocalization[] listTextLocalization;
}
