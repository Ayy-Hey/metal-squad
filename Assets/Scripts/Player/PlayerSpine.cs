using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerSpine
	{
		public PlayerSpine(PlayerMain inputPlayer, bool ignoreAnimationEvent = false)
		{
			this.player = inputPlayer;
			if (!ignoreAnimationEvent)
			{
				this.player.skeletonAnimation.state.Event += this.HandleEvent;
				this.player.skeletonAnimation.AnimationState.Start += this.HandleStart;
				this.player.skeletonAnimation.state.Complete += this.HandleComplete;
			}
		}

		public void OnInit()
		{
			this.isInit = true;
			this.SetAttachment();
		}

		public void OnUpdate(float deltaTime)
		{
			this.OnShooting(deltaTime);
		}

		public void ResetAnimTarget()
		{
			this.targetAnimation = null;
			this.previousTargetAnimation = null;
		}

		protected void HandleStart(TrackEntry entry)
		{
			if (entry == null || !this.isInit)
			{
				return;
			}
			try
			{
				if (entry.Animation == this.player.GunCurrent.GetAnimKnife().Animation && this.player._PlayerData.IDKnife == 1)
				{
					this.player.objKnife.Show(this.player, entry);
				}
				if (entry.Animation == this.player.GunCurrent.GetAnimKnife().Animation && this.player._PlayerData.IDKnife == 2)
				{
					this.player.objKnifeSword.Show(this.player, entry);
				}
				if (entry.Animation == this.player.GunCurrent.GetAnimKnife().Animation && this.player._PlayerData.IDKnife == 0)
				{
					this.player.objKnife.Show(this.player, entry);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Exception____" + ex.Message);
			}
		}

		protected void HandleEvent(TrackEntry entry, Spine.Event e)
		{
			if (entry == null || !this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
			{
				return;
			}
			string name = e.Data.Name;
			if (name != null)
			{
				if (!(name == "Shoot"))
				{
					if (!(name == "Shoot2"))
					{
						if (name == "thown")
						{
							this.player.OnCreateGrenade();
						}
					}
					else
					{
						this.player.GunCurrent.WeaponCurrent.OnShootBullet2();
					}
				}
				else
				{
					this.player.GunCurrent.WeaponCurrent.OnShootBullet();
				}
			}
		}

		protected void HandleComplete(TrackEntry entry)
		{
			if (entry == null || !this.isInit)
			{
				return;
			}
			if (entry.TrackIndex == 1 && entry.Animation == this.player.GunCurrent.WeaponCurrent.GetAnimShoot().Animation && !this.player._PlayerInput.IsShooting)
			{
				this.RemoveAim();
				this.entryShooting = null;
				this.player.skeletonAnimation.state.ClearTrack(1);
			}
		}

		public void SetAttachment()
		{
			if (GameManager.Instance.isJetpackMode)
			{
				for (int i = 0; i < 2; i++)
				{
					string slotName = this.player.SlotJetpack[i];
					string attachmentName = this.player.AttachmentJetpack[i];
					this.player.skeletonAnimation.Skeleton.SetAttachment(slotName, attachmentName);
				}
			}
			else if (this.player._PlayerData.IDCharacter == 1)
			{
				this.player.skeletonAnimation.Skeleton.SetAttachment(this.player.SlotBag, this.player.AttachmentBag);
			}
			else
			{
				for (int j = 0; j < 2; j++)
				{
					string slotName2 = this.player.SlotJetpack[j];
					string text = this.player.AttachmentJetpack[j];
					this.player.skeletonAnimation.Skeleton.SetAttachment(slotName2, null);
				}
			}
		}

		public void StartAnimKnife()
		{
			this.RemoveAim();
			this.entryShooting = this.player.skeletonAnimation.state.SetAnimation(1, this.player.GunCurrent.GetAnimKnife(), false);
		}

		public void StopAnimKnife()
		{
			TrackEntry entry = this.GetEntry(1);
			if (entry != null && (entry.Animation == this.player.GunCurrent.GetAnimKnife().Animation || entry.Animation == this.player.GunCurrent.GetAnimThrowGrenades().Animation))
			{
				this.player.skeletonAnimation.state.SetEmptyAnimation(1, 0f);
			}
		}

		public void SetParachute(Vector3 posJump)
		{
			this.targetAnimation = this.player.GunCurrent.GetAnimJump(1);
			if (this.targetAnimation != this.previousTargetAnimation)
			{
				this.player.skeletonAnimation.state.SetAnimation(0, this.targetAnimation, true);
				posJump.z = 0f;
				this.player.transform.position = posJump;
			}
			this.previousTargetAnimation = this.targetAnimation;
		}

		public bool FlipX
		{
			get
			{
				return this.player.skeletonAnimation.skeleton.FlipX;
			}
			set
			{
				this.player.skeletonAnimation.skeleton.FlipX = value;
			}
		}

		public TrackEntry GetEntry(int track)
		{
			return this.player.skeletonAnimation.state.GetCurrent(track);
		}

		public void SetAim()
		{
			TrackEntry entry = this.GetEntry(PlayerSpine.TRACK_AIM);
			if ((entry == null || entry.Animation != this.player.GunCurrent.WeaponCurrent.anim.Animation) && !this.player._PlayerInput.isPressJump && !this.player.CheckKnife())
			{
				this.player.skeletonAnimation.state.SetAnimation(PlayerSpine.TRACK_AIM, this.player.GunCurrent.WeaponCurrent.anim, false);
			}
		}

		public void RemoveAim()
		{
			TrackEntry entry = this.GetEntry(PlayerSpine.TRACK_AIM);
			if (entry != null && entry.Animation == this.player.GunCurrent.WeaponCurrent.anim.Animation)
			{
				this.player.skeletonAnimation.state.SetEmptyAnimation(PlayerSpine.TRACK_AIM, 0f);
			}
		}

		public void OnVictory()
		{
			SingletonGame<AudioController>.Instance.PlaySound_P(this.player._audioVictory, 1f);
			this.player.EMovement = BaseCharactor.EMovementBasic.Release;
			this.RemoveAim();
			this.player.skeletonAnimation.state.SetAnimation(1, this.player.GunCurrent.WeaponCurrent.Victory1, false);
			if (this.player.GunCurrent.WeaponCurrent.Victory2 != null)
			{
				this.player.skeletonAnimation.state.AddAnimation(1, this.player.GunCurrent.WeaponCurrent.Victory2, true, 0f);
			}
		}

		public void OnRun(bool isLooping = true)
		{
			if (this.player.EMovement == BaseCharactor.EMovementBasic.DIE || this.player.EMovement == BaseCharactor.EMovementBasic.FREEZE)
			{
				return;
			}
			this.player.skeletonAnimation.state.SetAnimation(0, this.player.GunCurrent.GetAnimRun(false), isLooping);
			this.player.EMovement = BaseCharactor.EMovementBasic.Right;
		}

		public void OnIdle(bool isLooping = true)
		{
			if (this.player.EMovement == BaseCharactor.EMovementBasic.DIE || this.player.EMovement == BaseCharactor.EMovementBasic.FREEZE)
			{
				return;
			}
			this.player.skeletonAnimation.state.SetAnimation(0, this.player.GunCurrent.GetAnimIdle(), isLooping);
			this.player.EMovement = BaseCharactor.EMovementBasic.Release;
		}

		public void OnDie()
		{
			this.RemoveAim();
			this.player.skeletonAnimation.state.SetAnimation(0, this.player.GunCurrent.anim_dead, false);
		}

		public Bone FindBone(string name)
		{
			return this.player.skeletonAnimation.skeleton.FindBone(name);
		}

		private void OnShooting(float deltaTime)
		{
			if (!this.player._PlayerInput.IsShooting || (this.entryShooting != null && this.entryShooting.animation != null && !this.entryShooting.IsComplete))
			{
				return;
			}
			TrackEntry entry = this.GetEntry(1);
			if (entry != null && !entry.IsComplete && entry.Animation == this.player.GunCurrent.GetAnimThrowGrenades().Animation)
			{
				return;
			}
			this.player.GunCurrent.WeaponCurrent.OnShootUpdate(deltaTime);
			float num = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.Time_Reload;
			if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
			{
				num = 0.2f;
			}
			if (Time.timeSinceLevelLoad - this.timeShooting >= num)
			{
				if (this.player._controller.isGrounded)
				{
					if (this.player.CheckKnife())
					{
						this.StartAnimKnife();
					}
					else
					{
						this.SetAim();
						this.entryShooting = this.player.skeletonAnimation.state.SetAnimation(1, this.player.GunCurrent.WeaponCurrent.GetAnimShoot(), false);
					}
				}
				else
				{
					UnityEngine.Debug.Log("dkm");
					this.player.GunCurrent.WeaponCurrent.OnShootBullet();
				}
				this.timeShooting = Time.timeSinceLevelLoad;
			}
		}

		private TrackEntry entryShooting;

		public float timeShooting;

		public AnimationReferenceAsset targetAnimation;

		public AnimationReferenceAsset previousTargetAnimation;

		private bool isInit;

		public static int TRACK_AIM = 2;

		private PlayerMain player;
	}
}
