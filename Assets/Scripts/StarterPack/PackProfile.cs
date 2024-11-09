using System;
using UnityEngine;

namespace StarterPack
{
	public class PackProfile
	{
		public PackProfile(string datapath)
		{
			this.WeightProfile = new IntProfileData(datapath + ".Pack14", 500);
			this.BuyProfile = new BoolProfileData(datapath + ".BuyPack", false);
		}

		public bool IsBuy
		{
			get
			{
				return this.BuyProfile.Data;
			}
			set
			{
				this.BuyProfile.setValue(value);
			}
		}

		public int Weight
		{
			get
			{
				return Mathf.Max(0, this.WeightProfile.Data.Value);
			}
			set
			{
				this.WeightProfile.setValue(value);
			}
		}

		private IntProfileData WeightProfile;

		private BoolProfileData BuyProfile;
	}
}
