using System;
using System.Collections.Generic;
using PlayerWeapon;
using Spine;
using UnityEngine;

namespace Player
{
	public class RocketGunPlayer : RamboWeapon
	{
		public override void OnInit(WeaponProfile weaponProfile, PlayerMain inputPlayer)
		{
			base.OnInit(weaponProfile, inputPlayer);
			this.bone1 = this.player._PlayerSpine.FindBone("GunTip_Rocket-1");
			this.bone2 = this.player._PlayerSpine.FindBone("GunTip_Rocket-2");
			this.bone3 = this.player._PlayerSpine.FindBone("GunTip_Rocket-3");
			this.bone4 = this.player._PlayerSpine.FindBone("GunTip_Rocket-4");
			this.boneOriginGun = this.player._PlayerSpine.FindBone("GunTip_Rocket-1");
		}

		public override void OnResetBullet()
		{
			base.OnResetBullet();
			this.cacheGunProfile.OnResetBullet();
		}

		public override void OnShootBullet()
		{
			Vector3 pos = new Vector3(this.bone4.worldX + this.player.transform.position.x, this.bone4.worldY + this.player.transform.position.y);
			this.target.Clear();
			this.GetListTarget(this.player._PlayerSpine.FlipX);
			this.InitRocket(pos, this.player._PlayerSpine.FlipX, base.IsGrounded);
			this.cacheGunProfile.TotalBullet--;
			EventDispatcher.PostEvent("BulletValueChange");
		}

		public override void OnUpdate(float deltaTime)
		{
			this.timeCheckAim += deltaTime;
			if (!this.player._PlayerInput.IsShooting && this.timeCheckAim >= 1f)
			{
				this.timeCheckAim = 0f;
				this.player._PlayerSpine.RemoveAim();
			}
		}

		private void GetListTarget(bool isFlip)
		{
			for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
			{
				BaseEnemy baseEnemy = GameManager.Instance.ListEnemy[i];
				if (!(baseEnemy == null) && baseEnemy.gameObject.activeSelf)
				{
					if (this.target.Count >= 3)
					{
						break;
					}
					if (baseEnemy.GetState() != ECharactor.DIE)
					{
						Vector2 from = new Vector2(this.player.transform.position.x + this.bone4.worldX, this.player.transform.position.y + this.bone4.worldY) - new Vector2(this.player.transform.position.x + this.bone2.worldX, this.player.transform.position.y + this.bone2.worldY);
						Vector2 to = baseEnemy.Origin() - new Vector2(this.player.transform.position.x + this.bone4.worldX, this.player.transform.position.y + this.bone4.worldY);
						float num = Vector2.Angle(from, to);
						if (CameraController.Instance.IsInView(baseEnemy.transform) && num <= 60f)
						{
							this.target.Add(baseEnemy);
						}
					}
				}
			}
		}

		private void InitRocket(Vector3 pos, bool isFlip, bool isGround)
		{
			float damage = 0f;
			bool isCritical = false;
			this.cacheGunProfile.GetTrueDamage(out damage, out isCritical);
			Vector2[] array = new Vector2[3];
			Vector3 zero = Vector3.zero;
			if (!isGround)
			{
				Vector2 directionJoystick = this.player.DirectionJoystick;
				Vector2 originJump = this.player.GunCurrent.GetOriginJump();
				array[1] = VectorUtils.Rotate(directionJoystick, 30f).normalized;
				array[0] = directionJoystick.normalized;
				array[2] = VectorUtils.Rotate(directionJoystick, -30f).normalized;
			}
			else
			{
				array[0] = new Vector2(this.player.transform.position.x + this.bone4.worldX, this.player.transform.position.y + this.bone4.worldY) - new Vector2(this.player.transform.position.x + this.bone2.worldX, this.player.transform.position.y + this.bone2.worldY);
				array[1] = new Vector2(this.player.transform.position.x + this.bone3.worldX, this.player.transform.position.y + this.bone3.worldY) - new Vector2(this.player.transform.position.x + this.bone2.worldX, this.player.transform.position.y + this.bone2.worldY);
				array[2] = new Vector2(this.player.transform.position.x + this.bone1.worldX, this.player.transform.position.y + this.bone1.worldY) - new Vector2(this.player.transform.position.x + this.bone2.worldX, this.player.transform.position.y + this.bone2.worldY);
				zero = new Vector3(this.bone4.worldX + this.player.transform.position.x, this.bone4.worldY + this.player.transform.position.y);
			}
			if (this.target.Count > 0)
			{
				for (int i = 0; i < 3; i++)
				{
					BaseBullet baseBullet = GameManager.Instance.bulletManager.CreateRocketPlayer(this.player, pos);
					float speed = this.cacheGunProfile.Speed_Bullet;
					speed = Mathf.Min(10f, this.cacheGunProfile.Speed_Bullet);
					baseBullet.OnInitRocket1(this.player, (this.target.Count - 1 < i) ? this.target[this.target.Count - 1] : this.target[i], Quaternion.FromToRotation(-Vector3.down, array[i]), speed, damage, isCritical);
				}
				return;
			}
			RaycastHit2D hit = Physics2D.Raycast(zero, array[0].normalized, 1000f, this.layerTarger);
			if (hit)
			{
				for (int j = 0; j < 3; j++)
				{
					BaseBullet baseBullet2 = GameManager.Instance.bulletManager.CreateRocketPlayer(this.player, pos);
					float speed2 = this.cacheGunProfile.Speed_Bullet;
					speed2 = Mathf.Min(10f, this.cacheGunProfile.Speed_Bullet);
					baseBullet2.OnInitRocket2(this.player, hit.point, Quaternion.FromToRotation(-Vector3.down, array[j]), speed2, damage, isCritical);
				}
			}
		}

		private Bone bone1;

		private Bone bone2;

		private Bone bone3;

		private Bone bone4;

		private Vector2 directionGun;

		private List<BaseEnemy> target = new List<BaseEnemy>();

		[SerializeField]
		private LayerMask layerTarger;

		private float timeCheckAim;
	}
}
