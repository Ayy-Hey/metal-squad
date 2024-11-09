using System;
using PVPManager;

namespace Player
{
	public class PlayerData
	{
		public PlayerData(PlayerMain inputPlayer, int inputIDCharacter, int NPC_Level = 0)
		{
			this.player = inputPlayer;
			this.IDCharacter = inputIDCharacter;
			this.ramboProfile = ProfileManager.rambos[this.IDCharacter];
			if (inputPlayer.isNPC)
			{
				this.runSpeedNormal = this.ramboProfile.GetOptionByLevel(1, NPC_Level);
				this.jumpHeight = this.ramboProfile.GetOptionByLevel(2, NPC_Level);
				this.player.HPCurrent = this.ramboProfile.GetOptionByLevel(0, NPC_Level);
			}
			else
			{
				this.runSpeedNormal = this.ramboProfile.Speed_Normal;
				this.jumpHeight = this.ramboProfile.Force_Jump;
				this.player.HPCurrent = this.ramboProfile.HP;
			}
			this.runSpeedNormal *= 0.01f;
			this.runSpeedSlow = this.runSpeedNormal * 0.4f;
			this.jumpHeight *= 0.0052f;
			if (GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode || (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode && !this.player.IsRemotePlayer))
			{
				PvP_LocalPlayer.Instance.HP = this.player.HPCurrent;
			}
			this.player.isGunDefault = true;
		}

		public void LoadProfile()
		{
			this.IDKnife = ProfileManager.meleCurrentId.Data.Value;
			this.IDGrenades = ProfileManager.grenadeCurrentId.Data.Value;
			this.IDGUN1 = ProfileManager.rifleGunCurrentId;
			if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
			{
				this.IDGUN1 = 3;
			}
			this.InitGun1(this.IDGUN1, ProfileManager.weaponsRifle[this.IDGUN1], false, 0, false, 0);
			this.player.Guns[1].OnInit(this.player);
			this.player.Guns[1].OnInitGunSpecial();
		}

		public void InitGun1(int id, WeaponProfile weaponProfile, bool useOptionLevel = false, int optionLevel = 0, bool useOptionRank = false, int rankUpgrade = 0)
		{
			this.player.Guns[0].OnInit(this.player);
			this.player.Guns[0].OnInitGunDefault(id, useOptionLevel, optionLevel);
			this.IDGUN1 = id;
			this.player.GunCurrent = this.player.Guns[0];
			if (useOptionRank)
			{
				this.player.skeletonAnimation.skeleton.SetSkin(this.player.GunCurrent.WeaponCurrent.GetSkin(rankUpgrade));
			}
			else
			{
				this.player.skeletonAnimation.skeleton.SetSkin(this.player.GunCurrent.WeaponCurrent.GetSkin(this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade));
			}
			this.player.isGunDefault = true;
			try
			{
				ControlManager.Instance.ChangeImageWeaponIcon(true);
			}
			catch
			{
			}
			EventDispatcher.PostEvent("BulletValueChange");
		}

		public void LoadOnlineProfile(int IDGUN1, int IDKnife, int IDGrenades, bool useOptionLevel = false, int Gun1_Level = 0, int rankUpgrade = 0)
		{
			this.IDKnife = IDKnife;
			this.IDGrenades = IDGrenades;
			this.IDGUN1 = IDGUN1;
			this.InitGun1(IDGUN1, ProfileManager.weaponsRifle[IDGUN1], useOptionLevel, Gun1_Level, true, rankUpgrade);
			this.player.Guns[1].OnInit(this.player);
			this.player.Guns[1].OnInitGunSpecial();
		}

		public void IfNotEnoughBullet()
		{
			if (this.player.GunCurrent.WeaponCurrent.cacheGunProfile.TotalBullet <= 0 && GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
			{
				this.player._PlayerInput.SwitchGun(!this.player.isGunDefault);
				if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
				{
					this.player.syncRamboState.SendRpc_SwitchGun(!this.player.isGunDefault, 0);
				}
			}
		}

		public float HPMax()
		{
			return this.ramboProfile.HP;
		}

		public int IDCharacter;

		public int IDGUN1;

		public int IDGUN2 = -1;

		public int IDKnife = 1;

		public int IDGrenades;

		public float gravity = -25f;

		public float runSpeedNormal = 5f;

		public float runSpeedSlow = 5f;

		public float groundDamping = 20f;

		public float jumpHeight = 3.4f;

		public RamboProfile ramboProfile;

		private PlayerMain player;
	}
}
