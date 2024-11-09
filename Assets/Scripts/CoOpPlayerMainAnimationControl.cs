using System;
using System.Collections;
using Player;
using Spine;
using UnityEngine;

public class CoOpPlayerMainAnimationControl : MonoBehaviour
{
	private IEnumerator Start()
	{
		this.targetPlayer = base.GetComponent<PlayerMain>();
		yield return new WaitUntil(() => this.targetPlayer != null);
		yield return new WaitUntil(() => this.targetPlayer.IsInit);
		yield break;
	}

	public void SetOnlineAnimationTrack0(CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation animation)
	{
		if (this.targetPlayer == null || this.targetPlayer.skeletonAnimation.state == null)
		{
			return;
		}
		if (animation == this.GetOnlineAnimationTrack0())
		{
			return;
		}
		switch (animation)
		{
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.LEFT:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimRun(false), true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.LEFT_BACK:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimRun(true), true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.RIGHT:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimRun(false), true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.RIGHT_BACK:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimRun(true), true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SIT:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.WeaponCurrent.idles.idle_sit, true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.RELEASE:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.WeaponCurrent.idles.idle, true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.DIE:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.anim_dead, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.FREEZE:
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimFreeze(), true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.JUMP_0:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.ClearTrack(0);
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimJump(0), false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.JUMP_1:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.ClearTrack(0);
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimJump(1), true);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.JUMP_2:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.ClearTrack(0);
			this.targetPlayer.skeletonAnimation.state.SetAnimation(0, this.targetPlayer.GunCurrent.GetAnimJump(2), false);
			break;
		default:
			this.targetPlayer.skeletonAnimation.state.ClearTrack(0);
			break;
		}
	}

	public CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation GetOnlineAnimationTrack0()
	{
		if (this.targetPlayer == null || this.targetPlayer.GunCurrent == null)
		{
			return CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;
		}
		TrackEntry entry = this.targetPlayer._PlayerSpine.GetEntry(0);
		if (entry == null)
		{
			return CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;
		}
		CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;
		if (entry.Animation == this.targetPlayer.GunCurrent.GetAnimRun(false).Animation)
		{
			if (!this.targetPlayer._PlayerSpine.FlipX)
			{
				result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.RIGHT;
			}
			else
			{
				result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.LEFT;
			}
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.GetAnimRun(true).Animation)
		{
			if (!this.targetPlayer._PlayerSpine.FlipX)
			{
				result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.LEFT_BACK;
			}
			else
			{
				result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.RIGHT_BACK;
			}
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.WeaponCurrent.idles.idle_sit.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SIT;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.WeaponCurrent.idles.idle.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.RELEASE;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.anim_dead.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.DIE;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.GetAnimFreeze().Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.FREEZE;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.GetAnimJump(0).Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.JUMP_0;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.GetAnimJump(1).Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.JUMP_1;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.GetAnimJump(2).Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.JUMP_2;
		}
		return result;
	}

	public void SetOnlineAnimationTrack1(CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation animation)
	{
		if (this.targetPlayer == null || this.targetPlayer.skeletonAnimation.state == null)
		{
			return;
		}
		if (animation == this.GetOnlineAnimationTrack1())
		{
			return;
		}
		switch (animation)
		{
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SHOOT_RANK_0:
			this.SetAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.WeaponCurrent.shoots.amim_shoot[0], false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SHOOT_RANK_1:
			this.SetAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.WeaponCurrent.shoots.amim_shoot[1], false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SHOOT_RANK_2:
			this.SetAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.WeaponCurrent.shoots.amim_shoot[2], false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_0_SIT:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.knife[0].anim_knife_sit, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_0_STAND:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.knife[0].anim_knife_stand, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_1_SIT:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.knife[1].anim_knife_sit, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_1_STAND:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.knife[1].anim_knife_stand, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_2_SIT:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.knife[2].anim_knife_sit, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_2_STAND:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.knife[2].anim_knife_stand, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.THROW_GRENADE_SIT:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.thrownGrenade.anim_throw_grenades_sit, false);
			break;
		case CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.THROW_GRENADE_STAND:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.SetAnimation(1, this.targetPlayer.GunCurrent.thrownGrenade.anim_throw_grenades_normal, false);
			break;
		default:
			this.RemoveAim();
			this.targetPlayer.skeletonAnimation.state.ClearTrack(1);
			break;
		}
	}

	public CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation GetOnlineAnimationTrack1()
	{
		if (this.targetPlayer == null || this.targetPlayer.GunCurrent == null)
		{
			return CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;
		}
		TrackEntry entry = this.targetPlayer._PlayerSpine.GetEntry(1);
		if (entry == null)
		{
			return CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;
		}
		CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.NONE;
		if (entry.Animation == this.targetPlayer.GunCurrent.WeaponCurrent.shoots.amim_shoot[0].Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SHOOT_RANK_0;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.WeaponCurrent.shoots.amim_shoot[1].Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SHOOT_RANK_1;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.WeaponCurrent.shoots.amim_shoot[2].Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.SHOOT_RANK_2;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.knife[0].anim_knife_sit.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_0_SIT;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.knife[0].anim_knife_stand.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_0_STAND;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.knife[1].anim_knife_sit.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_1_SIT;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.knife[1].anim_knife_stand.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_1_STAND;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.knife[2].anim_knife_sit.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_2_SIT;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.knife[2].anim_knife_stand.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.KNIFE_2_STAND;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.thrownGrenade.anim_throw_grenades_sit.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.THROW_GRENADE_SIT;
		}
		else if (entry.Animation == this.targetPlayer.GunCurrent.thrownGrenade.anim_throw_grenades_normal.Animation)
		{
			result = CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation.THROW_GRENADE_STAND;
		}
		return result;
	}

	private void SetAim()
	{
		TrackEntry entry = this.targetPlayer._PlayerSpine.GetEntry(PlayerSpine.TRACK_AIM);
		if (entry == null || entry.Animation != this.targetPlayer.GunCurrent.WeaponCurrent.anim.Animation)
		{
			this.targetPlayer.skeletonAnimation.state.SetAnimation(PlayerSpine.TRACK_AIM, this.targetPlayer.GunCurrent.WeaponCurrent.anim, false);
		}
	}

	private void RemoveAim()
	{
		TrackEntry entry = this.targetPlayer._PlayerSpine.GetEntry(PlayerSpine.TRACK_AIM);
		if (entry != null && entry.Animation == this.targetPlayer.GunCurrent.WeaponCurrent.anim.Animation)
		{
			this.targetPlayer.skeletonAnimation.state.SetEmptyAnimation(PlayerSpine.TRACK_AIM, 0f);
		}
	}

	private PlayerMain targetPlayer;

	public CoOpPlayerMainAnimationControl.PlayerMainOnlineAnimation playerOnlineAnimation;

	public enum PlayerMainOnlineAnimation
	{
		LEFT,
		LEFT_BACK,
		RIGHT,
		RIGHT_BACK,
		SIT,
		RELEASE,
		DIE,
		FREEZE,
		JUMP_0,
		JUMP_1,
		JUMP_2,
		SHOOT_RANK_0,
		SHOOT_RANK_1,
		SHOOT_RANK_2,
		KNIFE_0_SIT,
		KNIFE_0_STAND,
		KNIFE_1_SIT,
		KNIFE_1_STAND,
		KNIFE_2_SIT,
		KNIFE_2_STAND,
		THROW_GRENADE_SIT,
		THROW_GRENADE_STAND,
		ARM,
		NONE
	}
}
