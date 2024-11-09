using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SkyStone : CachingMonoBehaviour
{
	public void Init(float damage, Vector2 pos, Action<SkyStone> callback)
	{
		this.callback = callback;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		if (!this.isFirstInit)
		{
			this.isFirstInit = true;
			this.skeletonAnimation.state.Complete += this.OnComplete;
			this._boneAttack = this.skeletonAnimation.skeleton.FindBone(this.boneAttack);
			this._boneGround = this.skeletonAnimation.skeleton.FindBone(this.boneGround);
			this.anims = this.skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
		}
		this.skeletonAnimation.state.SetEmptyAnimation(0, 0f);
		SingletonGame<AudioController>.Instance.PlaySound(this.clip, 1f);
		this.attackBox.Active(damage, true, null);
		this.colliderGround.enabled = true;
		this.colliderGround.transform.position = this._boneGround.GetWorldPosition(this.transform);
		this.attackBox.transform.position = this._boneAttack.GetWorldPosition(this.transform);
		this.isInit = true;
	}

	public void End()
	{
		this.attackBox.Deactive();
		this.colliderGround.enabled = false;
		this.skeletonAnimation.state.SetAnimation(0, this.anims[0], false);
		SingletonGame<AudioController>.Instance.PlaySound(this.clip, 1f);
		this.isInit = false;
		if (this.callback != null)
		{
			this.callback(this);
		}
	}

	public void OnUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		this.colliderGround.transform.position = this._boneGround.GetWorldPosition(this.transform);
		if (this.attackBox.isAttack)
		{
			this.attackBox.transform.position = this._boneAttack.GetWorldPosition(this.transform);
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string name = trackEntry.Animation.Name;
		if (name != null)
		{
			if (!(name == "start"))
			{
				if (!(name == "end"))
				{
					if (name == "<empty>")
					{
						this.skeletonAnimation.state.SetAnimation(0, this.anims[2], false);
					}
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
			else
			{
				this.attackBox.Deactive();
			}
		}
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineBone("", "", true, false)]
	[SerializeField]
	private string boneAttack;

	[SerializeField]
	[SpineBone("", "", true, false)]
	private string boneGround;

	[SerializeField]
	private AttackBox attackBox;

	[SerializeField]
	private Collider2D colliderGround;

	[SerializeField]
	private AudioClip clip;

	private Action<SkyStone> callback;

	private Spine.Animation[] anims;

	private Bone _boneAttack;

	private Bone _boneGround;

	private bool isFirstInit;
}
