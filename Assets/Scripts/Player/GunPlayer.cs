using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class GunPlayer : MonoBehaviour
	{
		public void OnInit(PlayerMain inputPlayer)
		{
			this.player = inputPlayer;
		}

		public void OnInitGunDefault(int id, bool useOptionLevel = false, int optionLevel = 0)
		{
			WeaponProfile profile = ProfileManager.weaponsRifle[id];
			this.ListWeapon[id].OnInit(profile, this.player);
			this.gunTipHand = this.player._PlayerSpine.FindBone("GunTip_thown");
			this.originJump = this.player._PlayerSpine.FindBone("all");
			if (useOptionLevel)
			{
				this.ListWeapon[id].cacheGunProfile.Damage = this.ListWeapon[id].cacheGunProfile.profile.GetOptionByLevel(2, optionLevel);
			}
			this.ListWeapon[id].cacheGunProfile.Damage += this.ListWeapon[id].cacheGunProfile.Damage * this.player._PlayerBooster.DamagedBooster;
			if (this.player._PlayerBooster.IsCritical)
			{
				this.ListWeapon[id].cacheGunProfile.Critical += 0.2f;
			}
			this.ListWeapon[id].cacheGunProfile.Damage -= this.ListWeapon[id].cacheGunProfile.Damage * GameManager.Instance.RatePower;
			int num = 3;
			int num2 = 2;
			if (GameMode.Instance.modePlay == GameMode.ModePlay.Campaign)
			{
				int num3 = (int)(num + (int)GameManager.Instance.Level / (int)ELevel.LEVEL_13 * (int)(ELevel)num2);
				int @int = PlayerPrefs.GetInt("save.game.campaign.lost." + (int)GameManager.Instance.Level, 0);
				float num4 = UnityEngine.Random.Range(0.1f, 0.3f);
				if (@int >= num3)
				{
					PlayerPrefs.SetInt("save.game.campaign.lost." + (int)GameManager.Instance.Level, 0);
					this.ListWeapon[id].cacheGunProfile.Damage += num4 * this.ListWeapon[id].cacheGunProfile.Damage;
				}
			}
		}

		public void OnInitGunSpecial()
		{
			this.gunTipHand = this.player._PlayerSpine.FindBone("GunTip_thown");
			this.originJump = this.player._PlayerSpine.FindBone("all");
			for (int i = 0; i < this.ListWeapon.Count; i++)
			{
				this.ListWeapon[i].OnInit(ProfileManager.weaponsSpecial[i], this.player);
			}
		}

		public RamboWeapon WeaponCurrent
		{
			get
			{
				if (this.player.isGunDefault)
				{
					return this.ListWeapon[this.player._PlayerData.IDGUN1];
				}
				return this.ListWeapon[this.player._PlayerData.IDGUN2];
			}
		}

		public Vector2 GetPosShellBullet()
		{
			return Vector2.zero;
		}

		public Vector2 GetOriginJump()
		{
			Vector2 zero = Vector2.zero;
			zero.x = this.player.transform.position.x + this.originJump.WorldX;
			zero.y = this.player.transform.position.y + this.originJump.WorldY;
			return zero;
		}

		public AnimationReferenceAsset GetAnimRun(bool isRunBack = false)
		{
			return this.WeaponCurrent.runs.anim_runs_normal[(!isRunBack) ? 0 : 1];
		}

		public AnimationReferenceAsset GetAnimFreeze()
		{
			return this.anim_jumpFreeze;
		}

		public AnimationReferenceAsset GetAnimIdle()
		{
			return (this.player.EMovement != BaseCharactor.EMovementBasic.Sit) ? this.WeaponCurrent.idles.idle : this.WeaponCurrent.idles.idle_sit;
		}

		public AnimationReferenceAsset GetAnimJump(int index)
		{
			AnimationReferenceAsset result = this.jump.anim_jumps_normal[index];
			if (GameManager.Instance.isJetpackMode)
			{
				result = this.jump.anim_jumps_jetpack;
			}
			return result;
		}

		public AnimationReferenceAsset GetAnimKnife()
		{
			int idknife = this.player._PlayerData.IDKnife;
			return (this.player.EMovement != BaseCharactor.EMovementBasic.Sit) ? this.knife[idknife].anim_knife_stand : this.knife[idknife].anim_knife_sit;
		}

		public AnimationReferenceAsset GetAnimThrowGrenades()
		{
			return (this.player.EMovement != BaseCharactor.EMovementBasic.Sit) ? this.thrownGrenade.anim_throw_grenades_normal : this.thrownGrenade.anim_throw_grenades_sit;
		}

		public Vector2 GetHandPos()
		{
			Vector2 zero = Vector2.zero;
			zero.x = this.player.transform.position.x + this.gunTipHand.WorldX;
			zero.y = this.player.transform.position.y + this.gunTipHand.WorldY;
			return zero;
		}

		public List<RamboWeapon> ListWeapon;

		public AnimationReferenceAsset anim_parachute;

		public AnimationReferenceAsset anim_jumpFreeze;

		public AnimationReferenceAsset anim_dead;

		public GunPlayer.Jump jump;

		public GunPlayer.Knife[] knife;

		public GunPlayer.ThrownGrenade thrownGrenade;

		private Bone gunTipHand;

		private Bone originJump;

		private PlayerMain player;

		[Serializable]
		public struct Knife
		{
			public AnimationReferenceAsset anim_knife_stand;

			public AnimationReferenceAsset anim_knife_sit;
		}

		[Serializable]
		public struct Jump
		{
			public AnimationReferenceAsset[] anim_jumps_normal;

			public AnimationReferenceAsset anim_jumps_jetpack;
		}

		[Serializable]
		public struct ThrownGrenade
		{
			public AnimationReferenceAsset anim_throw_grenades_normal;

			public AnimationReferenceAsset anim_throw_grenades_sit;
		}
	}
}
