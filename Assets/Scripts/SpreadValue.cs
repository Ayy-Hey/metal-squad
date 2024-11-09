using System;

internal class SpreadValue : GunValue
{
	public SpreadValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Spread_Gun, null);
		this.isVip = false;
	}
}
