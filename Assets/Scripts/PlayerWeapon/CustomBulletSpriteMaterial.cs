using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class CustomBulletSpriteMaterial : MonoBehaviour
	{
		public bool IsVisible
		{
			get
			{
				return this.ren.isVisible;
			}
		}

		public void SetMaterial()
		{
			GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
			int num;
			if (modePlay != GameMode.ModePlay.Campaign)
			{
				if (modePlay != GameMode.ModePlay.Boss_Mode)
				{
					num = 0;
				}
				else
				{
					EBoss bossCurrent = ProfileManager.bossCurrent;
					switch (bossCurrent)
					{
					case EBoss.Spider_Toxic:
					case EBoss.War_Bunker:
					case EBoss.Super_Spider:
					case EBoss.Mechanical_Scorpion:
						break;
					default:
						if (bossCurrent != EBoss.Schneider_G25 && bossCurrent != EBoss.Ultron)
						{
							num = 0;
							goto IL_F4;
						}
						break;
					}
					num = 1;
					IL_F4:;
				}
			}
			else
			{
				switch (GameManager.Instance.Level)
				{
				case ELevel.LEVEL_13:
				case ELevel.LEVEL_14:
				case ELevel.LEVEL_15:
				case ELevel.LEVEL_16:
				case ELevel.LEVEL_17:
				case ELevel.LEVEL_18:
				case ELevel.LEVEL_19:
				case ELevel.LEVEL_20:
				case ELevel.LEVEL_21:
				case ELevel.LEVEL_22:
				case ELevel.LEVEL_23:
				case ELevel.LEVEL_24:
				case ELevel.LEVEL_25:
				case ELevel.LEVEL_26:
				case ELevel.LEVEL_27:
				case ELevel.LEVEL_28:
				case ELevel.LEVEL_29:
				case ELevel.LEVEL_30:
				case ELevel.LEVEL_31:
				case ELevel.LEVEL_32:
				case ELevel.LEVEL_33:
				case ELevel.LEVEL_34:
				case ELevel.LEVEL_35:
				case ELevel.LEVEL_36:
					num = 1;
					break;
				default:
					num = 0;
					break;
				}
			}
			if (this.currentMatId != num)
			{
				this.currentMatId = num;
				this.ren.material = this.mats[this.currentMatId];
			}
		}

		[SerializeField]
		private Renderer ren;

		[SerializeField]
		private Material[] mats;

		private int currentMatId;
	}
}
