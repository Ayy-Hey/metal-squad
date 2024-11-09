using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BossTutorial : MonoBehaviour
{
	public void Show()
	{
		base.gameObject.SetActive(true);
		LeanTween.moveY(base.gameObject, -2.11f, 5f).setOnComplete(delegate()
		{
			for (int i = 0; i < this.skeletonAnimation.Length; i++)
			{
				this.skeletonAnimation[i].state.SetAnimation(0, this.attack, false);
			}
			for (int j = 0; j < this.TiaSet.Length; j++)
			{
				this.TiaSet[j].SetActive(true);
			}
		});
		this.skeletonAnimation[0].state.Event += this.HandleEvent;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
	}

	[SerializeField]
	private SkeletonAnimation[] skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string attack;

	[SerializeField]
	private TutorialBigPopup bigPopup;

	[SerializeField]
	private GameObject[] TiaSet;

	[SerializeField]
	private Image imgFadeOut;

	[SerializeField]
	private GameObject buttonSkip;

	private bool isEndTutorial;
}
