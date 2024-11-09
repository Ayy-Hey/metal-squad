using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EffectUpgrade : MonoBehaviour
{
	public void ShowUpgrade(Transform tf, float scale = 0.5f)
	{
		base.gameObject.SetActive(true);
		this.skeleton.transform.localScale = Vector3.one * scale;
		this.skeleton.transform.position = tf.position;
		this.skeleton.AnimationState.SetAnimation(0, this.anim[1], false);
		SingletonGame<AudioController>.Instance.PlaySound(this.mAudio, 1f);
		this.skeleton.AnimationState.Complete += delegate(TrackEntry entry)
		{
			base.gameObject.SetActive(false);
		};
	}

	[SpineAnimation("", "", true, false)]
	public string[] anim;

	public SkeletonGraphic skeleton;

	public AudioClip mAudio;
}
