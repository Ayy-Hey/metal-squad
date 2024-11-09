using System;

public class ThunderValue : GunValue
{
	public ThunderValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Thunder_Shot, null);
	}
}
