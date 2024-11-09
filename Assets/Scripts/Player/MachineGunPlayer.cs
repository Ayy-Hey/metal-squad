using System;
using System.Collections;
using Spine;
using UnityEngine;

namespace Player
{
	public class MachineGunPlayer : RamboWeapon
	{
		public override void OnInit(WeaponProfile weaponProfile, PlayerMain inputPlayer)
		{
			base.OnInit(weaponProfile, inputPlayer);
			this.bone1 = this.player._PlayerSpine.FindBone("GunTip_Machine-1");
			this.bone2 = this.player._PlayerSpine.FindBone("GunTip_Machine-2");
			this.bone3 = this.player._PlayerSpine.FindBone("GunTip_Machine-3");
			this.boneOriginGun = this.player._PlayerSpine.FindBone("GunTip_Machine-1");
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
				this.directionGun.Normalize();
				if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
				{
					this.player.syncRamboState.SendRpc_CreateMachine(vector2, this.directionGun);
				}
				GameManager.Instance.bulletManager.CreateMachine(this.player, vector2, this.directionGun, true);
			}
			else
			{
				this.directionGun = this.player.DirectionJoystick;
				this.directionGun.Normalize();
				this.tfOrigin.rotation = Quaternion.FromToRotation(Vector3.right, this.directionGun);
				base.StartCoroutine(this.CreateBullet(this.tfOrigin1.position, this.tfOrigin2.position, this.directionGun));
			}
		}

		public override void OnShootBullet2()
		{
			base.OnShootBullet2();
			Vector2 a = new Vector2(this.bone1.WorldX, this.bone1.WorldY);
			Vector2 b = new Vector2(this.bone2.WorldX, this.bone2.WorldY);
			Vector2 v = new Vector2(this.bone3.WorldX, this.bone3.WorldY);
			Vector2 direction = a - b;
			Vector2 vector = this.player.GunCurrent.GetOriginJump();
			vector = v + (Vector2)this.player.transform.position;
			direction.Normalize();
			if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
			{
				this.player.syncRamboState.SendRpc_CreateMachine(vector, direction);
			}
			GameManager.Instance.bulletManager.CreateMachine(this.player, vector, direction, true);
		}

		private IEnumerator CreateBullet(Vector2 pos, Vector2 pos2, Vector2 dir)
		{
			if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
			{
				this.player.syncRamboState.SendRpc_CreateMachine(pos, dir);
			}
			GameManager.Instance.bulletManager.CreateMachine(this.player, pos, dir, true);
			yield return new WaitForSeconds(this.cacheGunProfile.Time_Reload / 2f);
			if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
			{
				this.player.syncRamboState.SendRpc_CreateMachine(pos2, dir);
			}
			GameManager.Instance.bulletManager.CreateMachine(this.player, pos2, dir, true);
			yield break;
		}

		private Bone bone1;

		private Bone bone2;

		private Bone bone3;

		private Vector2 directionGun;

		[SerializeField]
		private Transform tfOrigin;

		[SerializeField]
		private Transform tfOrigin1;

		[SerializeField]
		private Transform tfOrigin2;
	}
}
