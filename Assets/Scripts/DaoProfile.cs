using System;

public class DaoProfile : MeleProfile
{
	public DaoProfile()
	{
		this.Name = "Dao";
		this.unlock = new BoolProfileData(this.STR_UnLock + this.Name, false);
		this.levelUpGradeMelee = new IntProfileData(this.STR_Level + this.Name, 0);
	}
}
