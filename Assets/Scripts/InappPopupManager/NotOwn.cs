using System;
using System.Collections.Generic;

namespace InappPopupManager
{
	public class NotOwn
	{
		public Character character { get; set; }

		public List<Weapon> weapons { get; set; }

		public bool OK()
		{
			bool flag = this.character == null || !ProfileManager.InforChars[this.character.ID].IsUnLocked;
			bool flag2 = this.weapons.Count == 0;
			for (int i = 0; i < this.weapons.Count; i++)
			{
				flag2 = this.weapons[i].OK();
			}
			return flag && flag2;
		}
	}
}
