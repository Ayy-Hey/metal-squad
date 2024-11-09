using System;

public class Mgl140Value : GunValue
{
	public Mgl140Value()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.MGL_140, null);
		this.isVip = true;
	}
}
