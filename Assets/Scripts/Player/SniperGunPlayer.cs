using System;
using Spine;
using UnityEngine;

namespace Player
{
	public class SniperGunPlayer : RamboWeapon
	{
		public override void OnInit(WeaponProfile weaponProfile, PlayerMain inputPlayer)
		{
			base.OnInit(weaponProfile, inputPlayer);
			this.bone1 = this.player._PlayerSpine.FindBone("GunTip_Sniper-1");
			this.bone2 = this.player._PlayerSpine.FindBone("GunTip_Sniper-2");
			this.boneLaser = this.player._PlayerSpine.FindBone("laser");
			this.boneOriginGun = this.player._PlayerSpine.FindBone("GunTip_Sniper-1");
		}

		public override void OnResetBullet()
		{
			base.OnResetBullet();
			this.cacheGunProfile.OnResetBullet();
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
				this.player.syncRamboState.SendRpc_CreateSniper(vector2, this.directionGun);
			}
			GameManager.Instance.bulletManager.CreateSniper(this.player, vector2, this.directionGun, true);
			this.cacheGunProfile.TotalBullet--;
			EventDispatcher.PostEvent("BulletValueChange");
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			TrackEntry entry = this.player._PlayerSpine.GetEntry(PlayerSpine.TRACK_AIM);
			if (entry != null)
			{
				Vector2 zero = Vector2.zero;
				zero.x = this.player.transform.position.x + this.boneLaser.WorldX;
				zero.y = this.player.transform.position.y + this.boneLaser.WorldY;
				this.lineLaserSniper.gameObject.SetActive(true);
				this.lineLaserSniper.position = zero;
				Vector2 a = new Vector2(this.bone1.WorldX, this.bone1.WorldY);
				Vector2 b = new Vector2(this.bone2.WorldX, this.bone2.WorldY);
				Vector2 v = a - b;
				this.lineLaserSniper.rotation = Quaternion.FromToRotation(Vector3.right, v);
			}
			else
			{
				this.lineLaserSniper.gameObject.SetActive(false);
			}
		}

		public override void OnRelease()
		{
			base.OnRelease();
			this.lineLaserSniper.gameObject.SetActive(false);
		}

		private Bone bone1;

		private Bone bone2;

		private Bone boneLaser;

		private Vector2 directionGun;

		[SerializeField]
		private Transform lineLaserSniper;
	}
}
