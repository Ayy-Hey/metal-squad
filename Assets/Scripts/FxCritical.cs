using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class FxCritical : MonoBehaviour
{
	public void TryAwake()
	{
		this.transform = base.GetComponent<Transform>();
	}

	public void OnInit()
	{
		this.skeletonAnimation.state.SetAnimation(0, "animation", false);
		this.skeletonAnimation.state.Complete += delegate(TrackEntry entry)
		{
			base.gameObject.SetActive(false);
		};
	}

	public void OnUpdate(float deltaTime)
	{
	}

	private void OnDisable()
	{
		try
		{
			GameManager.Instance.fxManager.PoolCritical.Store(this);
		}
		catch
		{
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[NonSerialized]
	public new Transform transform;
}
