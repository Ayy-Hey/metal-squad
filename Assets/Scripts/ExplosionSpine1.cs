using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class ExplosionSpine1 : CachingMonoBehaviour
{
	public void Show(Vector3 pos, int type)
	{
		this.transform.position = pos;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.skeletonAnimation.state.SetAnimation(0, (type != 0) ? this.anim2 : this.anim, false);
		GameManager.Instance.audioManager.PlayBoom();
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		this.skeletonAnimation.state.ClearTrack(entry.TrackIndex);
		string text = entry.ToString();
		if (text != null)
		{
			if (text == "explosion" || text == "explosion2")
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	private void OnDisable()
	{
		this.skeletonAnimation.state.Complete -= this.HandleComplete;
		GameManager.Instance.fxManager.ExplosionSpine1Pool.Store(this);
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string anim;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string anim2;
}
