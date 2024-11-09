using System;
using UnityEngine;

public class FormBase : MonoBehaviour
{
	public virtual void Show()
	{
		AudioClick.Instance.OnClick();
		this.OnClosed = null;
		this.OnTabClosed = null;
		this.Localization();
		if (MenuManager.Instance.formCurrent.idForm == FormUI.Menu)
		{
			MenuManager.Instance.topUI.obj_FormOther.SetActive(false);
			MenuManager.Instance.topUI.obj_FormMainMenu.SetActive(true);
			MenuManager.Instance.topUI.obj_Setting.SetActive(true);
		}
		else
		{
			MenuManager.Instance.topUI.obj_FormOther.SetActive(true);
			MenuManager.Instance.topUI.obj_FormMainMenu.SetActive(false);
			MenuManager.Instance.topUI.obj_Setting.SetActive(false);
		}
		if (MenuManager.Instance.formCurrent.keyNameForm != Localization0.Metal_Squad)
		{
			MenuManager.Instance.txt_NameForm.text = PopupManager.Instance.GetText(MenuManager.Instance.formCurrent.keyNameForm, null).ToUpper();
		}
		else
		{
			MenuManager.Instance.txt_NameForm.text = MenuManager.Instance.formCurrent.nameForm;
		}
		MenuManager.Instance.topUI.Show();
		MenuManager.Instance.topUI.ShowCoin();
		base.gameObject.SetActive(true);
	}

	public virtual void OnTabClose()
	{
		
		if (this.OnTabClosed != null)
		{
			this.OnTabClosed();
			this.OnTabClosed = null;
		}
	}

	public virtual void OnClose()
	{
		
		if (this.OnClosed != null)
		{
			this.OnClosed();
			this.OnClosed = null;
		}
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

	public FormUI idForm;

	public string nameForm;

	public Localization0 keyNameForm;

	public int tabOpen;

	public Action OnTabClosed;

	public Action OnClosed;

	public FormUI backForm;

	public GameObject[] tabObject;

	public GameObject[] Obj_Popup;

	public GameObject[] obj_ListTut;

	public TextLocalization[] listTextLocalization;
}
