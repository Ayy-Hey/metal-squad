using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Profile.UI
{
	public class ChildProfileGun : MonoBehaviour
	{
		public void OnShow(Sprite spriteBGRank, Sprite spriteGun, int rankGunCurrent, bool isLock)
		{
			this.objLock.SetActive(isLock);
			if (isLock)
			{
				rankGunCurrent = 0;
			}
			this.imgGun.sprite = spriteGun;
			this.imgBGRank.sprite = spriteBGRank;
			int num = rankGunCurrent - 2;
			if (num >= 0)
			{
				this.skeleton.gameObject.SetActive(true);
				this.skeleton.state.SetAnimation(0, this.animRankEffect[num], true);
			}
			else
			{
				this.skeleton.gameObject.SetActive(false);
			}
		}

		public Image imgGun;

		public Image imgBGRank;

		public GameObject objLock;

		public SkeletonAnimation skeleton;

		[SpineAnimation("", "", true, false)]
		public string[] animRankEffect;
	}
}
