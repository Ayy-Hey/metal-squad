using System;
using DigitalRuby.LightningBolt;
using Spine;
using UnityEngine;

namespace Player
{
	public class ThunderGunPlayer : RamboWeapon
	{
		public override void OnInit(WeaponProfile weaponProfile, PlayerMain inputPlayer)
		{
			base.OnInit(weaponProfile, inputPlayer);
			this.bone1 = this.player._PlayerSpine.FindBone("GunTip_Thunder-1");
			this.bone2 = this.player._PlayerSpine.FindBone("GunTip_Thunder-2");
			this.boneOriginGun = this.player._PlayerSpine.FindBone("aim-constraint-target");
			GameManager.Instance.fxManager.CreateFxLighting();
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
				this.EndPosition = this.StartPosition;
				this.ReleaseGun();
				return;
			}
			TrackEntry entry = this.player._PlayerSpine.GetEntry(1);
			if (entry != null && !entry.IsComplete && (entry.Animation == this.player.GunCurrent.GetAnimThrowGrenades().Animation || entry.Animation == this.player.GunCurrent.GetAnimKnife().Animation))
			{
				this.EndPosition = this.StartPosition;
				this.ReleaseGun();
				return;
			}
			if (this.player.CheckKnife())
			{
				this.EndPosition = this.StartPosition;
				this.ReleaseGun();
				this.player._PlayerSpine.StartAnimKnife();
			}
			else
			{
				this.OnCreateBullet();
			}
		}

		public override void OnRelease()
		{
			if (this.TargetEnemy != null)
			{
				this.TargetEnemy.ListLigntingConnected.Clear();
				this.TargetEnemy.isMainLignting = false;
				this.TargetEnemy = null;
			}
			GameManager.Instance.fxManager.ReleaseEffectLighting();
			this.ReleaseGun();
		}

		private void OnCreateBullet()
		{
			Vector2 vector = new Vector2(this.bone1.WorldX, this.bone1.WorldY);
			Vector2 b = new Vector2(this.bone2.WorldX, this.bone2.WorldY);
			Vector2 startPosition = this.player.GunCurrent.GetOriginJump();
			this.directionGun = vector - b;
			if (base.IsGrounded)
			{
				startPosition = vector + (Vector2)this.player.transform.position;
			}
			else
			{
				this.directionGun = this.player.DirectionJoystick;
			}
			this.directionGun.Normalize();
			this.StartPosition = startPosition;
			if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
			{
				this.player.syncRamboState.SendRpc_ThunderGun_OnCreateBullet(this.StartPosition, this.EndPosition);
			}
			this.SetLightningLine(this.StartPosition, this.EndPosition);
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.StartPosition, this.directionGun, 3000f, this.layerMask);
			BaseEnemy baseEnemy = null;
			try
			{
				baseEnemy = raycastHit2D.collider.GetComponent<BaseEnemy>();
			}
			catch
			{
			}
			if (baseEnemy != null && baseEnemy.gameObject.activeSelf && baseEnemy.HP > 0f)
			{
				this.HitEnemy(baseEnemy);
				this.EndPosition = baseEnemy.GetTarget();
				return;
			}
			if (this.TargetEnemy != null)
			{
				GameManager.Instance.fxManager.ReleaseEffectLighting();
				this.TargetEnemy.ListLigntingConnected.Clear();
				this.TargetEnemy.isMainLignting = false;
			}
			this.TargetEnemy = null;
			if (Time.timeSinceLevelLoad - this.timeShooting >= this.cacheGunProfile.Time_Reload)
			{
				this.timeShooting = Time.timeSinceLevelLoad;
				CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
				if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
				{
					this.cacheGunProfile.TotalBullet--;
					EventDispatcher.PostEvent("BulletValueChange");
				}
			}
			try
			{
				this.EndPosition = raycastHit2D.point;
			}
			catch
			{
			}
		}

		public void SetLightningLine(Vector2 StartPosition, Vector2 EndPosition)
		{
			this.lightningScript.OnInit(this.player);
			this.lightningScript.StartEffect.gameObject.SetActive(true);
			this.lightningScript.StartPosition = StartPosition;
			this.lightningScript.StartEffect.transform.position = StartPosition;
			this.lightningScript.EndPosition = EndPosition;
			this.lightningScript.EndEffect.transform.position = EndPosition;
		}

		public void ReleaseGun()
		{
			if (!this.lightningScript.gameObject.activeSelf)
			{
				return;
			}
			if (!this.player.IsRemotePlayer && this.player.syncRamboState != null)
			{
				this.player.syncRamboState.SendRpc_ThunderGun_OnRelease();
			}
			this.lightningScript.ClearEffect();
			this.lightningScript.StartEffect.gameObject.SetActive(false);
			this.lightningScript.gameObject.SetActive(false);
		}

		private void HitEnemy(BaseEnemy enemy)
		{
			if (this.TargetEnemy != enemy)
			{
				if (this.TargetEnemy != null)
				{
					this.TargetEnemy.ListLigntingConnected.Clear();
					this.TargetEnemy.isMainLignting = false;
				}
				this.TargetEnemy = enemy;
			}
			this.TargetEnemy.isMainLignting = true;
			if (this.TargetEnemy.ListLigntingConnected.Count < 3)
			{
				for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
				{
					BaseEnemy baseEnemy = GameManager.Instance.ListEnemy[i];
					float num = Vector2.Distance(this.TargetEnemy.Origin(), baseEnemy.Origin());
					if (!baseEnemy.isMainLignting && num < 5f && baseEnemy.HP > 0f && baseEnemy.gameObject.activeSelf && baseEnemy.isInCamera && !this.TargetEnemy.ListLigntingConnected.Contains(baseEnemy))
					{
						this.TargetEnemy.ListLigntingConnected.Add(baseEnemy);
					}
					if ((num >= 5f || baseEnemy.HP <= 0f || !baseEnemy.isInCamera || !baseEnemy.gameObject.activeSelf) && this.TargetEnemy.ListLigntingConnected.Contains(baseEnemy))
					{
						this.TargetEnemy.ListLigntingConnected.Remove(baseEnemy);
					}
				}
			}
			GameManager.Instance.fxManager.ShowEffectLighting(this.player, this.TargetEnemy, this.lightningScript.EndEffect.transform.position);
			if (Time.timeSinceLevelLoad - this.timeShooting >= this.cacheGunProfile.Time_Reload)
			{
				this.timeShooting = Time.timeSinceLevelLoad;
				CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
				if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
				{
					this.cacheGunProfile.TotalBullet--;
					EventDispatcher.PostEvent("BulletValueChange");
				}
				float num2 = 0f;
				bool flag = false;
				this.cacheGunProfile.GetTrueDamage(out num2, out flag);
				if (flag)
				{
					GameManager.Instance.fxManager.CreateCritical(this.TargetEnemy.GetPosition());
				}
				this.TargetEnemy.AddHealthPoint(-num2, EWeapon.THUNDER);
				try
				{
					for (int j = 0; j < this.TargetEnemy.ListLigntingConnected.Count; j++)
					{
						if (flag)
						{
							GameManager.Instance.fxManager.CreateCritical(this.TargetEnemy.ListLigntingConnected[j].GetPosition());
						}
						this.TargetEnemy.ListLigntingConnected[j].AddHealthPoint(-(num2 / (float)this.TargetEnemy.ListLigntingConnected.Count), EWeapon.THUNDER);
					}
				}
				catch
				{
				}
			}
		}

		private Bone bone1;

		private Bone bone2;

		private Vector2 directionGun;

		[SerializeField]
		private LightningBoltScript lightningScript;

		private Vector2 EndPosition = Vector2.zero;

		private Vector2 StartPosition = Vector2.zero;

		[SerializeField]
		private LayerMask layerMask;

		private BaseEnemy TargetEnemy;

		private float timeShooting;
	}
}
