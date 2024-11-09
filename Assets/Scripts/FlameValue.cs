using System;

public class FlameValue : GunValue
{
	public FlameValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Flame_Gun, null);
	}
}
