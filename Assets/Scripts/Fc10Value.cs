using System;

public class Fc10Value : GunValue
{
	public Fc10Value()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.FC_10, null);
		this.isVip = true;
	}
}
