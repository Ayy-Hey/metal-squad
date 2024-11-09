using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class FxNo_Spine : CachingMonoBehaviour
{
	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (!this.ske)
		{
			this.ske = base.GetComponent<SkeletonAnimation>();
		}
	}

	public void Init(int idAnim, Vector3 pos, Vector3 scale, Action<FxNo_Spine> hideCallback, int skin = 0, bool randomRotation = true)
	{
		this.actionHide = hideCallback;
		base.gameObject.SetActive(true);
		if (this.anims == null || this.anims.Length == 0)
		{
			SkeletonData skeletonData = this.ske.SkeletonDataAsset.GetAnimationStateData().SkeletonData;
			this.anims = skeletonData.Animations.Items;
			this.skins = skeletonData.Skins.Items;
		}
		this.transform.position = pos;
		this.transform.localScale = scale;
		this.ske.Skeleton.SetSkin(this.skins[skin]);
		this.ske.AnimationState.Complete += this.OnComplete;
		this.ske.AnimationState.SetAnimation(0, this.anims[idAnim], false);
		if (randomRotation)
		{
			this.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		}
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.audioClip, 0.2f);
		}
		catch
		{
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		this.ske.AnimationState.SetEmptyAnimations(0f);
		this.ske.state.Complete -= this.OnComplete;
		base.gameObject.SetActive(false);
		if (this.actionHide != null)
		{
			this.actionHide(this);
		}
	}

	[SerializeField]
	private SkeletonAnimation ske;

	[SerializeField]
	private AudioClip audioClip;

	private Action<FxNo_Spine> actionHide;

	private Spine.Animation[] anims;

	private Skin[] skins;
}
