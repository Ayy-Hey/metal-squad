using System;

public class Ct9Value : GunValue
{
	public Ct9Value()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.CT_9, null);
		this.isVip = true;
	}
}
