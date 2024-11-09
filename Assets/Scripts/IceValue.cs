using System;

public class IceValue : GunValue
{
	public IceValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Ice_Gun, null);
		this.isVip = false;
	}
}
