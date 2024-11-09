using System;

public class MachineValue : GunValue
{
	public MachineValue()
	{
		this.gunName = PopupManager.Instance.GetText(Localization0.Machine_Gun, null);
		this.isVip = false;
	}
}
