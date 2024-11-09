using System;

public class SniperValue : GunValue
{
	public SniperValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Sniper, null);
		this.isVip = false;
	}
}
