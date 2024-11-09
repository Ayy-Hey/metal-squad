using System;
using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerInput
	{
		public PlayerInput(MonoBehaviour _MonoBehaviour, PlayerMain inputPlayer)
		{
			this.player = inputPlayer;
			this._MonoBehaviour = _MonoBehaviour;
		}

		public bool IsShooting
		{
			get
			{
				return this._isShooting;
			}
			set
			{
				this.TimeClearStuckControl = Time.timeSinceLevelLoad;
				this._isShooting = value;
			}
		}

		public void OnInit()
		{
			this.isInit = true;
			EventDispatcher.PostEvent("BulletValueChange");
			ControlManager.Instance.BombValueChange();
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.isInit)
			{
				return;
			}
			if (this.player.EMovement == BaseCharactor.EMovementBasic.FREEZE || this.player.EMovement == BaseCharactor.EMovementBasic.DIE)
			{
				return;
			}
			if (!this.IsShooting && Time.timeSinceLevelLoad - this.TimeClearStuckControl >= 2f)
			{
				this.TimeClearStuckControl = Time.timeSinceLevelLoad;
				this.player._PlayerSpine.RemoveAim();
			}
			switch (ProfileManager.settingProfile.TypeControl)
			{
			case 0:
				this.JoystickMovement(ControlManager.Instance.Controls[ProfileManager.settingProfile.TypeControl].joysticks[0]);
				break;
			case 1:
				if (!ProfileManager.unlockAll)
				{
					this.JoystickShooting(ControlManager.Instance.Controls[ProfileManager.settingProfile.TypeControl].joysticks[0], deltaTime);
				}
				break;
			case 2:
				this.JoystickMovement(ControlManager.Instance.Controls[ProfileManager.settingProfile.TypeControl].joysticks[0]);
				this.JoystickShooting(ControlManager.Instance.Controls[ProfileManager.settingProfile.TypeControl].joysticks[1], deltaTime);
				break;
			default:
				this.JoystickShooting(ControlManager.Instance.Controls[ProfileManager.settingProfile.TypeControl].joysticks[0], deltaTime);
				break;
			}
		}

		private void JoystickMovement(UltimateJoystick joystick)
		{
			this.movePosition = new Vector3(joystick.GetHorizontalAxis(), joystick.GetVerticalAxis(), 0f);
			if (joystick.GetJoystickState())
			{
				this.OnMove(this.movePosition);
			}
			else
			{
				this.OnMoveEnd();
			}
		}

		private void JoystickShooting(UltimateJoystick joystick, float deltaTime)
		{
			this.shootPosition = new Vector3(joystick.GetHorizontalAxis(), joystick.GetVerticalAxis(), 0f);
			if (joystick.GetJoystickState())
			{
				if (!this.IsShooting)
				{
					this.player.DirectionJoystick = ((!this.player._PlayerSpine.FlipX) ? Vector2.left : Vector2.right);
					this.OnShootDown();
					this.IsShooting = true;
				}
				this.player.isLockDirection = (joystick.GetDistance() >= 0.9f);
				if (this.player.isLockDirection)
				{
					this.player._PlayerSpine.FlipX = (this.shootPosition.x < 0f);
					this.player.DirectionJoystick = this.shootPosition;
					Vector2 targetFromDirection = this.player.GetTargetFromDirection(this.shootPosition);
					this.player.tfBone.position = Vector2.MoveTowards(this.player.tfBone.position, targetFromDirection, deltaTime * 15f);
				}
			}
			else if (this.IsShooting)
			{
				this.OnShootUp();
				this.player.GunCurrent.WeaponCurrent.OnRelease();
				this.IsShooting = false;
			}
		}

		public void OnRemoveInput(bool onlyRemoveInput = false)
		{
			this.IsShooting = false;
			this.isPressJump = false;
			this.isPressThrowGrenade = false;
			if (!onlyRemoveInput)
			{
				try
				{
					this.player._PlayerSpine.ResetAnimTarget();
					this.player._PlayerSpine.RemoveAim();
					this.player.GunCurrent.WeaponCurrent.OnRelease();
				}
				catch
				{
				}
			}
		}

		public void SwitchGun(bool isGunDefault)
		{
			TrackEntry entry = this.player._PlayerSpine.GetEntry(0);
			if (this.player._PlayerData.IDGUN2 < 0)
			{
				return;
			}
			if (this._CoroutineWaitingReskin != null)
			{
				this._MonoBehaviour.StopCoroutine(this._CoroutineWaitingReskin);
			}
			this.player.GunCurrent.WeaponCurrent.OnRelease();
			this.player.isGunDefault = isGunDefault;
			this.player.GunCurrent = this.player.Guns[(!isGunDefault) ? 1 : 0];
			ControlManager.Instance.ChangeImageWeaponIcon(isGunDefault);
			if (entry != null && entry.Animation == this.player.GunCurrent.GetAnimJump(1).Animation)
			{
				this._CoroutineWaitingReskin = this._MonoBehaviour.StartCoroutine(this.WaitingReskin(entry));
				return;
			}
			string skin = this.player.GunCurrent.WeaponCurrent.GetSkin(this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade);
			this.player.skeletonAnimation.skeleton.SetSkin(skin);
			this.player.GunCurrent.WeaponCurrent.OnResetBullet();
			EventDispatcher.PostEvent("BulletValueChange");
			this.ReCheckObjectEffect();
		}

		private IEnumerator WaitingReskin(TrackEntry entry)
		{
			yield return new WaitUntil(() => entry == null || entry.Animation != this.player.GunCurrent.GetAnimJump(1).Animation);
			this.player.skeletonAnimation.skeleton.SetSkin(this.player.GunCurrent.WeaponCurrent.GetSkin(this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade));
			this.player.GunCurrent.WeaponCurrent.OnResetBullet();
			EventDispatcher.PostEvent("BulletValueChange");
			this.ReCheckObjectEffect();
			yield break;
		}

		private void ReCheckObjectEffect()
		{
			if (this.player.isGunDefault)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				if (i != this.player._PlayerData.IDGUN2)
				{
					this.player.Guns[1].ListWeapon[i].OnRelease();
				}
			}
		}

		public void OnShootDown()
		{
			if (this.player.EMovement == BaseCharactor.EMovementBasic.FREEZE || this.player.EMovement == BaseCharactor.EMovementBasic.DIE)
			{
				return;
			}
			try
			{
				this._MonoBehaviour.StopCoroutine(this._Coroutine);
			}
			catch
			{
			}
			this.IsShooting = true;
			TrackEntry entry = this.player._PlayerSpine.GetEntry(1);
			if (!this.player._controller.isGrounded || (entry != null && !entry.IsComplete))
			{
				return;
			}
			this.player._PlayerSpine.SetAim();
			if (this.player.CheckKnife())
			{
				try
				{
					this.player._PlayerSpine.RemoveAim();
					this.player.skeletonAnimation.state.SetEmptyAnimation(1, 0f);
				}
				catch
				{
				}
				this.player.skeletonAnimation.state.SetAnimation(1, this.player.GunCurrent.GetAnimKnife(), false);
			}
			else
			{
				this.player.skeletonAnimation.state.SetAnimation(1, this.player.GunCurrent.WeaponCurrent.GetAnimShoot(), false);
			}
			this.player._PlayerSpine.timeShooting = Time.timeSinceLevelLoad;
		}

		public void OnShootUp()
		{
			this.IsShooting = false;
			try
			{
				TrackEntry entry = this.player._PlayerSpine.GetEntry(1);
				if (entry != null && entry.Animation == this.player.GunCurrent.WeaponCurrent.GetAnimShoot().Animation)
				{
					this._Coroutine = this._MonoBehaviour.StartCoroutine(this.WaitForRemoveAim(entry));
				}
			}
			catch
			{
			}
		}

		private IEnumerator WaitForRemoveAim(TrackEntry entry)
		{
			yield return new WaitForSpineAnimationComplete(entry);
			this.player._PlayerSpine.RemoveAim();
			yield break;
		}

		public void OnJump(bool isJump)
		{
			if (this.player.EMovement == BaseCharactor.EMovementBasic.FREEZE || this.player.EMovement == BaseCharactor.EMovementBasic.DIE)
			{
				return;
			}
			if (!this.player._PlayerJetpack.isReadyFly && GameManager.Instance.isJetpackMode)
			{
				isJump = false;
				this.isPressJump = false;
			}
			if (isJump)
			{
				this.player._PlayerSpine.StopAnimKnife();
			}
			if (isJump && this.IsShooting)
			{
				this.player._PlayerSpine.RemoveAim();
			}
			this.OnShootUp();
			this.isPressJump = isJump;
		}

		public void OnThrowGrenades(bool isDown)
		{
			if (this.player.EMovement == BaseCharactor.EMovementBasic.FREEZE || this.player.EMovement == BaseCharactor.EMovementBasic.DIE || ProfileManager.grenadesProfile[this.player._PlayerData.IDGrenades].TotalBomb <= 0)
			{
				return;
			}
			this.isPressThrowGrenade = isDown;
			if (isDown)
			{
				TrackEntry entry = this.player._PlayerSpine.GetEntry(1);
				if (entry != null && !entry.IsComplete && entry.Animation == this.player.GunCurrent.GetAnimKnife().Animation)
				{
					return;
				}
				GameManager.Instance.audioManager.PlayThrownGrenade();
				if (this.player._controller.isGrounded)
				{
					this.player._PlayerSpine.RemoveAim();
					this.player.skeletonAnimation.state.SetAnimation(1, this.player.GunCurrent.GetAnimThrowGrenades(), false);
				}
				else
				{
					this.player.OnCreateGrenade();
				}
			}
		}

		public void OnMoveEnd()
		{
			if (this.player.EMovement == BaseCharactor.EMovementBasic.FREEZE || this.player.EMovement == BaseCharactor.EMovementBasic.DIE)
			{
				return;
			}
			if (this.player.EMovement != BaseCharactor.EMovementBasic.Release)
			{
				this.player.EMovement = BaseCharactor.EMovementBasic.Release;
			}
		}

		public void OnMove(Vector2 axis)
		{
			if (this.player.isLockDirection && this.IsShooting && ProfileManager.settingProfile.TypeControl == 0)
			{
				this.player.DirectionJoystick = axis;
				Vector2 targetFromDirection = this.player.GetTargetFromDirection(axis);
				this.player.tfBone.position = Vector2.MoveTowards(this.player.tfBone.position, targetFromDirection, Time.deltaTime * 20f);
			}
			float num = Mathf.Atan2(axis.x, axis.y) * 57.29578f;
			float x = axis.x;
			if (num <= 135f && num >= -135.5f)
			{
				this.player.EMovement = ((x <= 0f) ? BaseCharactor.EMovementBasic.Left : BaseCharactor.EMovementBasic.Right);
			}
			else if ((num > -180f && num < -135f) || (num > 135f && num < 180f))
			{
				this.player.EMovement = BaseCharactor.EMovementBasic.Sit;
			}
			else
			{
				this.player.EMovement = BaseCharactor.EMovementBasic.Release;
			}
		}

		private bool isInit;

		public bool isPressJump;

		public bool isPressThrowGrenade;

		private bool _isShooting;

		private MonoBehaviour _MonoBehaviour;

		private Coroutine _Coroutine;

		private Vector3 shootPosition;

		private Vector3 movePosition;

		private float TimeClearStuckControl;

		private PlayerMain player;

		private Coroutine _CoroutineWaitingReskin;
	}
}
