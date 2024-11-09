using System;

namespace Sale
{
	public class ProfileStarterPack
	{
		public ProfileStarterPack(int ID)
		{
			this.countTimesDisplayProfile = new IntProfileData("com.metal.squad.Sale.ProfileStarterPack.countTimesDisplayProfile" + ID, 0);
			this.isShowedProfile = new BoolProfileData("com.metal.squad.Sale.ProfileStarterPack.isShowedProfile" + ID, false);
			this.isBuyProfile = new BoolProfileData("com.metal.squad.Sale.ProfileStarterPack.isBuyProfile" + ID, false);
		}

		public int CountDisplay
		{
			get
			{
				return this.countTimesDisplayProfile.Data.Value;
			}
			set
			{
				this.countTimesDisplayProfile.setValue(value);
			}
		}

		public bool isBuy
		{
			get
			{
				return this.isBuyProfile.Data;
			}
			set
			{
				this.isBuyProfile.setValue(value);
			}
		}

		public bool isShowed
		{
			get
			{
				return this.isShowedProfile.Data;
			}
			set
			{
				this.isShowedProfile.setValue(value);
			}
		}

		private IntProfileData countTimesDisplayProfile;

		private BoolProfileData isShowedProfile;

		private BoolProfileData isBuyProfile;
	}
}
