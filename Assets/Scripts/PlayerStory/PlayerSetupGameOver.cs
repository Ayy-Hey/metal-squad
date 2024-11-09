using System;
using Com.LuisPedroFonseca.ProCamera2D;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace PlayerStory
{
	public class PlayerSetupGameOver : MonoBehaviour
	{
		public void OnBegin()
		{
			this.Step = 0;
			base.gameObject.SetActive(true);
			if (PlayerManagerStory.Instance.objSkip != null)
			{
				PlayerManagerStory.Instance.objSkip.SetActive(true);
			}
			float orthographicSize = CameraController.Instance.mCamera.orthographicSize;
			ProCamera2D.Instance.Zoom(Mathf.Max(0f, 4.8f - orthographicSize), 0f, EaseType.EaseInOut);
			this.dtsJumpToFly = 0.2f;
			if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
			{
				this.dtsJumpToFly = 1f;
			}
		}

		private void HandleComplete(TrackEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			this.skeletonAirPlane.gameObject.SetActive(false);
			if (this.OnEnded != null && !this.isSkiped)
			{
				this.OnEnded();
			}
			this.skeletonAirPlane.state.Complete -= this.HandleComplete;
			this.skeletonAirPlane.state.Event -= this.HandleEvent;
		}

		private void HandleEvent(TrackEntry entry, Spine.Event e)
		{
			if (entry == null)
			{
				return;
			}
			string name = e.Data.Name;
			if (name != null)
			{
				if (!(name == "bui"))
				{
					if (name == "nhay")
					{
						this.isJumped = true;
						GameManager.Instance.player._PlayerInput.OnJump(true);
					}
				}
				else
				{
					this.skeletonEffectAirPlane.gameObject.SetActive(true);
					this.skeletonEffectAirPlane.state.SetAnimation(0, "nhay", false);
				}
			}
		}

		private void Update()
		{
			switch (this.Step)
			{
			case 0:
				this.TimeRemoveOldAnim += Time.deltaTime;
				if (this.TimeRemoveOldAnim >= 1f)
				{
					this.Step = 1;
					try
					{
						GameManager.Instance.player.skeletonAnimation.state.SetEmptyAnimation(1, 0f);
					}
					catch
					{
					}
					GameManager.Instance.player.isAutoRun = true;
				}
				break;
			case 1:
				if (Mathf.Abs(this.tfTargetPlayer.position.x - GameManager.Instance.player.GetPosition().x) <= 0.5f)
				{
					GameManager.Instance.player.isAutoRun = false;
					GameManager.Instance.player._PlayerSpine.OnIdle(true);
					this.Step = 2;
					this.skeletonAirPlane.gameObject.SetActive(true);
					this.skeletonAirPlane.state.Complete += this.HandleComplete;
					this.skeletonAirPlane.state.Event += this.HandleEvent;
					this.skeletonAirPlane.state.SetAnimation(0, "animation", false);
					if (ProfileManager.settingProfile.IsSound)
					{
						this._audioAirplane.Play();
						this._audioAirplane.loop = true;
					}
				}
				else
				{
					BaseCharactor.EMovementBasic emovement = (this.tfTargetPlayer.position.x <= GameManager.Instance.player.GetPosition().x) ? BaseCharactor.EMovementBasic.Left : BaseCharactor.EMovementBasic.Right;
					GameManager.Instance.player.OnMovement(emovement);
				}
				break;
			case 2:
				if (this.isJumped && Mathf.Abs(GameManager.Instance.player.GetPosition().y - this.skeletonAirPlane.transform.position.y) <= this.dtsJumpToFly)
				{
					this.Step = 3;
					GameManager.Instance.player.gameObject.SetActive(false);
				}
				break;
			}
		}

		public void OnSkip()
		{
			this.isSkiped = true;
			if (this.OnEnded != null)
			{
				this.OnEnded();
			}
		}

		public Action OnEnded;

		private int Step;

		public Transform tfTargetPlayer;

		private float TimeRemoveOldAnim;

		public SkeletonAnimation skeletonAirPlane;

		public SkeletonAnimation skeletonEffectAirPlane;

		public AudioSource _audioAirplane;

		private bool isJumped;

		private bool isSkiped;

		private float dtsJumpToFly;
	}
}
