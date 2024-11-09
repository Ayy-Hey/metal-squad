using System;

public class LaserValue : GunValue
{
	public LaserValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Laser_Gun, null);
		this.isVip = false;
	}
}
