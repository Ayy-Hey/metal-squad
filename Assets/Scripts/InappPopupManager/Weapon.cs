using System;

namespace InappPopupManager
{
	public class Weapon
	{
		public int Type { get; set; }

		public int ID { get; set; }

		public bool OK()
		{
			return (this.Type == 0 && !ProfileManager.weaponsRifle[this.ID].GetGunBuy()) || (this.Type == 1 && !ProfileManager.weaponsSpecial[this.ID].GetGunBuy());
		}
	}
}
