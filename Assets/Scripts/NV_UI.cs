using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class NV_UI : MonoBehaviour
{
	private void OnValidate()
	{
		if (!this.skeletonBody)
		{
			this.skeletonBody = base.transform.GetChild(0).GetComponent<SkeletonAnimation>();
		}
		if (!this.skeletonHand)
		{
			this.skeletonHand = base.transform.GetChild(1).GetComponent<SkeletonAnimation>();
		}
	}

	public void Show()
	{
		int value = ProfileManager.rifleGunCurrentId.Data.Value;
		if (!this.isInit)
		{
			this.skeletonBody.AnimationState.Complete += this.OnCompleteBody;
			this.skeletonHand.AnimationState.Complete += this.OnCompleteHand;
			this.animBodys = this.skeletonBody.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
			this.animHands = this.skeletonHand.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
			this.isInit = true;
		}
		this.ShowGun(value);
	}

	private void OnCompleteBody(TrackEntry trackEntry)
	{
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "aim-ct"))
			{
				if (!(text == "aim-grenade"))
				{
					if (!(text == "aim-ice"))
					{
						if (!(text == "aim-m16a4"))
						{
							if (!(text == "aim-mini"))
							{
								if (text == "aim-shotgun")
								{
									this.PlayAnimBody(11, true);
								}
							}
							else
							{
								this.PlayAnimBody(10, true);
							}
						}
						else
						{
							this.PlayAnimBody(9, true);
						}
					}
					else
					{
						this.PlayAnimBody(8, true);
					}
				}
				else
				{
					this.PlayAnimBody(7, true);
				}
			}
			else
			{
				this.PlayAnimBody(6, true);
			}
		}
	}

	private void OnCompleteHand(TrackEntry trackEntry)
	{
		string text = trackEntry.ToString();
		if (text != null)
		{
			if (!(text == "aim-ct"))
			{
				if (!(text == "aim-grenade"))
				{
					if (!(text == "aim-ice"))
					{
						if (!(text == "aim-m16a4"))
						{
							if (!(text == "aim-mini"))
							{
								if (text == "aim-shotgun")
								{
									this.PlayAnimHand(11, true);
								}
							}
							else
							{
								this.PlayAnimHand(10, true);
							}
						}
						else
						{
							this.PlayAnimHand(9, true);
						}
					}
					else
					{
						this.PlayAnimHand(8, true);
					}
				}
				else
				{
					this.PlayAnimHand(7, true);
				}
			}
			else
			{
				this.PlayAnimHand(6, true);
			}
		}
	}

	public void ShowGun(int gunId)
	{
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL && !this.isMenu)
		{
			return;
		}
		int rankUpped = ProfileManager.weaponsRifle[gunId].GetRankUpped();
		this.spriteRendererGun.sprite = PopupManager.Instance.sprite_GunMain[rankUpped].Sprites[gunId];
		int animsId = this.GetAnimsId(gunId);
		this.PlayAnimBody(animsId, false);
		this.PlayAnimHand(animsId, false);
	}

	private void PlayAnimBody(int id, bool loop = false)
	{
		this.skeletonBody.state.SetAnimation(0, this.animBodys[id], loop);
	}

	private void PlayAnimHand(int id, bool loop = false)
	{
		this.skeletonHand.state.SetAnimation(0, this.animHands[id], loop);
	}

	private void ConfigGunPos(int idGun, int rank)
	{
		Vector3 zero = Vector3.zero;
		if (idGun != 1)
		{
			if (idGun == 5)
			{
				zero.y = -0.2f;
			}
		}
		else if (rank != 0)
		{
			if (rank != 1)
			{
				if (rank != 2)
				{
				}
			}
			else
			{
				zero.x = 0.15f;
				zero.y = 0.2f;
			}
		}
		else
		{
			zero.x = -0.5f;
			zero.y = 0.2f;
		}
		this.spriteRendererGun.transform.localPosition = zero;
	}

	private int GetAnimsId(int gunId)
	{
		switch (gunId)
		{
		case 0:
			return 3;
		case 1:
			return 4;
		case 2:
			return 2;
		case 3:
			return 5;
		case 4:
			return 1;
		case 5:
			return 0;
		default:
			return 4;
		}
	}

	public string nameDisplay;

	public Sprite[] sprite_Skill;

	[SerializeField]
	private SkeletonAnimation skeletonBody;

	[SerializeField]
	private SkeletonAnimation skeletonHand;

	[SerializeField]
	private SpriteRenderer spriteRendererGun;

	private Spine.Animation[] animBodys;

	private Spine.Animation[] animHands;

	private bool isMenu;

	private bool isInit;

	private enum EState
	{
		Aim_Ct,
		Aim_Grenade,
		Aim_Ice,
		Aim_M4a1,
		Aim_Mini,
		Aim_ShotGun,
		Idle_Ct,
		Idle_Grenade,
		Idle_Ice,
		Idle_M4a1,
		Idle_Mini,
		Idle_ShotGun
	}
}
