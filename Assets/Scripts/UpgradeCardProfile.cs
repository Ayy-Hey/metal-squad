using System;

public class UpgradeCardProfile
{
	public UpgradeCardProfile()
	{
		this.upgradeCharacter = new IntProfileData(this._strUpgradeCharacter, 0);
		this.upgradeGun = new IntProfileData(this._strUpgradeGun, 0);
	}

	public int UpgradeCharacter
	{
		get
		{
			return this.upgradeCharacter.Data;
		}
		set
		{
			this.upgradeCharacter.setValue(value);
		}
	}

	public int UpgradeGun
	{
		get
		{
			return this.upgradeGun.Data;
		}
		set
		{
			this.upgradeGun.setValue(value);
		}
	}

	private string _strUpgradeCharacter = "com.metal.squad.upgradecard.character";

	private string _strUpgradeGun = "com.metal.squad.upgradecard.gun";

	private IntProfileData upgradeCharacter;

	private IntProfileData upgradeGun;
}
