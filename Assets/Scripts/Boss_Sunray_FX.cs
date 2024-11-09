using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_Sunray_FX : MonoBehaviour
{
	private void Start()
	{
		this.InitSkeletonAnimation();
		this.InitAnimations();
		this.ClearAllFlag();
	}

	private void ClearAllFlag()
	{
		this.triggerATTACK_MINI = false;
		this.triggerBIEN_MAT = false;
		this.triggerXUAT_HIEN = false;
		this.triggerATTACK_XOAY = false;
		this.doneATTACK_MINI = false;
		this.doneBIEN_MAT = false;
		this.doneXUAT_HIEN = false;
		this.doneATTACK_XOAY = false;
		this.eventBIEN_MAT = false;
		this.eventXUAT_HIEN = false;
	}

	private void Update()
	{
		if (this.triggerATTACK_MINI)
		{
			this.PlayAnim(Boss_Sunray_FX.Boss_Sunray_FX_State.ATTACK_MINI, false, 1f);
			this.triggerATTACK_MINI = false;
		}
		else if (this.triggerBIEN_MAT)
		{
			this.PlayAnim(Boss_Sunray_FX.Boss_Sunray_FX_State.BIEN_MAT, false, 3f);
			this.PlaySound(this.BIEN_MAT_Clip, 1f);
			this.triggerBIEN_MAT = false;
		}
		else if (this.triggerXUAT_HIEN)
		{
			this.PlayAnim(Boss_Sunray_FX.Boss_Sunray_FX_State.XUAT_HIEN, false, 3f);
			this.PlaySound(this.XUAT_HIEN_Clip, 1f);
			this.triggerXUAT_HIEN = false;
		}
		else if (this.triggerATTACK_XOAY)
		{
			this.PlayAnim(Boss_Sunray_FX.Boss_Sunray_FX_State.ATTACK_XOAY, false, 1f);
			this.triggerATTACK_XOAY = false;
		}
	}

	private void InitSkeletonAnimation()
	{
		this.skeletonAnimation.timeScale = 0f;
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		UnityEngine.Debug.Log("HandleEvent: " + e.ToString());
		if (entry == null || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "fx_attack_lan_bien_mat"))
			{
				if (name == "fx_attack_lan_xuat_hien")
				{
					this.eventXUAT_HIEN = true;
				}
			}
			else
			{
				this.eventBIEN_MAT = true;
			}
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		UnityEngine.Debug.Log("HandleComplete: " + entry.ToString());
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString();
		if (text != null)
		{
			if (!(text == "attack-mini"))
			{
				if (!(text == "fx_attack_lan_bien_mat"))
				{
					if (!(text == "fx_attack_lan_xuat_hien"))
					{
						if (text == "fx_attack_xoay_1")
						{
							this.doneATTACK_XOAY = true;
						}
					}
					else
					{
						this.doneXUAT_HIEN = true;
					}
				}
				else
				{
					this.doneBIEN_MAT = true;
				}
			}
			else
			{
				this.doneATTACK_MINI = true;
			}
		}
	}

	private void InitAnimations()
	{
		this.animations = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
	}

	private void PlayAnim(Boss_Sunray_FX.Boss_Sunray_FX_State state, bool loop = false, float speedAnim = 1f)
	{
		this.skeletonAnimation.timeScale = speedAnim;
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.animations[(int)state], loop);
	}

	private void PlayAnim(Boss_Sunray_FX.Boss_Sunray_FX_State state, int order)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(order, this.animations[(int)state], false);
	}

	private void PlaySound(AudioClip sound, float volume = 1f)
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(sound, volume);
		}
		catch
		{
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	private Spine.Animation[] animations;

	[SerializeField]
	[Header("__________ AUDIO")]
	private AudioClip BIEN_MAT_Clip;

	[SerializeField]
	private AudioClip XUAT_HIEN_Clip;

	public bool triggerATTACK_MINI;

	public bool triggerBIEN_MAT;

	public bool triggerXUAT_HIEN;

	public bool triggerATTACK_XOAY;

	public bool doneATTACK_MINI;

	public bool doneBIEN_MAT;

	public bool doneXUAT_HIEN;

	public bool doneATTACK_XOAY;

	public bool eventBIEN_MAT;

	public bool eventXUAT_HIEN;

	public enum Boss_Sunray_FX_State
	{
		ATTACK_MINI,
		BIEN_MAT,
		XUAT_HIEN,
		ATTACK_XOAY
	}
}
