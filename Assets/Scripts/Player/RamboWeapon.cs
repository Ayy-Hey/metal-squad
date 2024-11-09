using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class RamboWeapon : MonoBehaviour
	{
		public string GetSkin(int RankUpgrade)
		{
			string result = this.SkinGun[0];
			try
			{
				result = this.SkinGun[RankUpgrade];
			}
			catch (Exception ex)
			{
			}
			return result;
		}

		public AnimationReferenceAsset GetAnimShoot()
		{
			AnimationReferenceAsset result = this.shoots.amim_shoot[0];
			try
			{
				result = this.shoots.amim_shoot[this.cacheGunProfile.RankUpgrade];
			}
			catch (Exception ex)
			{
			}
			return result;
		}

		public Vector2 GetOriginGun()
		{
			return new Vector2
			{
				x = this.boneOriginGun.WorldX + this.player.transform.position.x,
				y = this.boneOriginGun.WorldY + this.player.transform.position.y
			};
		}

		public virtual void OnInit(WeaponProfile profile, PlayerMain _player)
		{
			this.player = _player;
			this.cacheGunProfile = new CacheGunProfile(profile, _player);
		}

		public virtual void OnUpdate(float deltaTime)
		{
		}

		public virtual void OnShootBullet()
		{
		}

		public virtual void OnShootBullet2()
		{
		}

		public virtual void OnShootUpdate(float deltaTime)
		{
		}

		public virtual void OnRelease()
		{
		}

		public virtual void OnResetBullet()
		{
		}

		protected bool IsGrounded
		{
			get
			{
				bool result = this.player._controller.isGrounded;
				TrackEntry entry = this.player._PlayerSpine.GetEntry(0);
				if (entry != null && entry.Animation != this.player.GunCurrent.GetAnimJump(1).Animation)
				{
					result = true;
				}
				if (GameManager.Instance.isJetpackMode)
				{
					result = true;
				}
				return result;
			}
		}

		[SpineSkin("", "", true, false)]
		public string[] SkinGun;

		public AnimationReferenceAsset anim;

		public RamboWeapon.Idle idles;

		public RamboWeapon.Run runs;

		public RamboWeapon.Shoot shoots;

		public Bone boneOriginGun;

		public AnimationReferenceAsset Victory1;

		public AnimationReferenceAsset Victory2;

		public CacheGunProfile cacheGunProfile;

		public PlayerMain player;

		[Serializable]
		public struct Idle
		{
			public AnimationReferenceAsset idle;

			public AnimationReferenceAsset idle_sit;
		}

		[Serializable]
		public struct Run
		{
			public AnimationReferenceAsset[] anim_runs_normal;
		}

		[Serializable]
		public struct Shoot
		{
			public AnimationReferenceAsset[] amim_shoot;
		}
	}
}
