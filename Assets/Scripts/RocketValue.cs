using System;

public class RocketValue : GunValue
{
	public RocketValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Rocket, null);
		this.isVip = true;
	}
}
