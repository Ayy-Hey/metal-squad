using System;
using Spine;
using UnityEngine;

namespace Player
{
	public class LaserGunPlayer : RamboWeapon
	{
		public override void OnInit(WeaponProfile weaponProfile, PlayerMain inputPlayer)
		{
			base.OnInit(weaponProfile, inputPlayer);
			this.bone1 = this.player._PlayerSpine.FindBone("GunTip_Laser-1");
			this.bone2 = this.player._PlayerSpine.FindBone("GunTip_Laser-2");
			this.boneOriginGun = this.player._PlayerSpine.FindBone("aim-constraint-target");
			this.ObjectFire.GetComponent<LineLaserPlayer>().player = this.player;
		}

		public override void OnResetBullet()
		{
			base.OnResetBullet();
			this.cacheGunProfile.OnResetBullet();
		}

		public override void OnUpdate(float deltaTime)
		{
			if (!this.player._PlayerInput.IsShooting || this.cacheGunProfile.TotalBullet <= 0)
			{
				this.ChangeFireActiveStatus(false);
				return;
			}
			TrackEntry entry = this.player._PlayerSpine.GetEntry(1);
			if (entry != null && !entry.IsComplete && (entry.Animation == this.player.GunCurrent.GetAnimThrowGrenades().Animation || entry.Animation == this.player.GunCurrent.GetAnimKnife().Animation))
			{
				this.ChangeFireActiveStatus(false);
				return;
			}
			if (this.player.CheckKnife())
			{
				this.ChangeFireActiveStatus(false);
				this.player._PlayerSpine.StartAnimKnife();
			}
			else
			{
				this.ChangeFireActiveStatus(true);
				Vector2 vector = new Vector2(this.bone1.WorldX, this.bone1.WorldY);
				Vector2 b = new Vector2(this.bone2.WorldX, this.bone2.WorldY);
				Vector2 vector2 = this.player.GunCurrent.GetOriginJump();
				this.directionGun = vector - b;
				bool isGrounded = base.IsGrounded;
				if (isGrounded)
				{
					vector2 = vector + (Vector2)this.player.transform.position;
				}
				else
				{
					this.directionGun = this.player.DirectionJoystick;
				}
				this.directionGun.Normalize();
				Vector3 position = this.ObjectFire.transform.position;
				position.x = vector2.x;
				position.y = vector2.y;
				if (!this.player.IsRemotePlayer && this.player.syncRamboState != null && ((double)Vector2.Distance(this.lastPos, position) > 0.01 || (double)Vector2.Distance(this.lastDirectionGun, this.directionGun) > 0.01))
				{
					this.player.syncRamboState.SendRpc_LaserGun_ChangeFirePos(position, this.directionGun);
				}
				this.ChangeFirePos(position, this.directionGun);
				this.OnCreateBullet();
				this.lastPos = position;
				this.lastDirectionGun = this.directionGun;
			}
		}

		public override void OnRelease()
		{
			this.ChangeFireActiveStatus(false);
		}

		public void ChangeFireActiveStatus(bool isActive)
		{
			if (this.ObjectFire.activeSelf != isActive)
			{
				if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
				{
					this.player.syncRamboState.SendRpc_LaserGun_ChangeFireActiveStatus(isActive);
				}
				this.ObjectFire.SetActive(isActive);
			}
		}

		public void ChangeFirePos(Vector2 pos, Vector2 directionGun)
		{
			this.ObjectFire.transform.position = pos;
			this.ObjectFire.transform.rotation = Quaternion.FromToRotation(Vector3.right, directionGun);
		}

		private void OnCreateBullet()
		{
			if (Time.timeSinceLevelLoad - this.timeShooting >= this.cacheGunProfile.Time_Reload)
			{
				this.timeShooting = Time.timeSinceLevelLoad;
				this.cacheGunProfile.TotalBullet--;
				CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
				EventDispatcher.PostEvent("BulletValueChange");
			}
		}

		private Bone bone1;

		private Bone bone2;

		private Vector2 directionGun;

		private Vector2 lastDirectionGun;

		private Vector2 lastPos;

		[SerializeField]
		public GameObject ObjectFire;

		private float timeShooting;
	}
}
