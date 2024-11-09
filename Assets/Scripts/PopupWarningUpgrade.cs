using System;


public class PopupWarningUpgrade : PopupBase
{
	public override void Show()
	{
		base.Show();
		this.formCurrent = MenuManager.Instance.formCurrent.idForm;
	}

	public void UpgradeChar()
	{
		
		AudioClick.Instance.OnClick();
		if (this.OnUpgrade != null)
		{
			this.OnUpgrade();
		}
		MenuManager.Instance.ChangeForm(FormUI.UICharacter, MenuManager.Instance.formCurrent.idForm);
	}

	public void UpgradeWeapon()
	{
		
		AudioClick.Instance.OnClick();
		if (this.OnUpgrade != null)
		{
			this.OnUpgrade();
		}
		MenuManager.Instance.ChangeForm(FormUI.UIWeapon, MenuManager.Instance.formCurrent.idForm);
	}

	public void BtnCancel()
	{
		try
		{
			string nameForm = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
		AudioClick.Instance.OnClick();
		if (this.OnNext != null)
		{
			this.OnNext();
		}
		base.gameObject.SetActive(false);
	}

	public Action OnNext;

	public Action OnUpgrade;

	public FormUI formCurrent;
}
