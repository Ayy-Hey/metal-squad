using System;
using Spine;
using UnityEngine;

namespace Player
{
	public class MglGunPlayer : RamboWeapon
	{
		public override void OnInit(WeaponProfile weaponProfile, PlayerMain inputPlayer)
		{
			base.OnInit(weaponProfile, inputPlayer);
			this.bone1 = this.player._PlayerSpine.FindBone("GunTip_MGL140-1");
			this.bone2 = this.player._PlayerSpine.FindBone("GunTip_MGL140-2");
			this.boneOriginGun = this.player._PlayerSpine.FindBone("aim-constraint-target");
		}

		public override void OnShootBullet()
		{
			Vector2 vector = new Vector2(this.bone1.WorldX, this.bone1.WorldY);
			Vector2 b = new Vector2(this.bone2.WorldX, this.bone2.WorldY);
			this.directionGun = vector - b;
			Vector2 vector2 = this.player.GunCurrent.GetOriginJump();
			if (base.IsGrounded)
			{
				vector2 = vector + (Vector2)this.player.transform.position;
			}
			else
			{
				this.directionGun = this.player.DirectionJoystick;
			}
			this.directionGun.Normalize();
			if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
			{
				this.player.syncRamboState.SendRpc_CreateMGL140(vector2, this.directionGun);
			}
			GameManager.Instance.bulletManager.CreateMGL140(this.player, vector2, this.directionGun, true);
		}

		private Bone bone1;

		private Bone bone2;

		private Vector2 directionGun;
	}
}
