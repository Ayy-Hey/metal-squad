using System;
using System.Collections;
using ABI;
using UnityEngine;
using UnityEngine.UI;

namespace Profile.UI
{
	public class UserProfile : MonoBehaviour
	{
		public void Show()
		{
		}

		private IEnumerator OnLoadUserAvatar()
		{
			yield return new WaitUntil(() => MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserPictureSprite != null);
			this.imgIcon.sprite = MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserPictureSprite;
			yield break;
		}

		private IEnumerator OnLoadUserName()
		{
			yield return new WaitUntil(() => !string.IsNullOrEmpty(MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserName));
			this.txtName.text = MonoSingleton<FBAndPlayfabMgr>.Instance.FacebookUserName;
			yield break;
		}

		private void OnDisable()
		{
			base.StopAllCoroutines();
		}

		[SerializeField]
		private Image imgIcon;

		[SerializeField]
		private Sprite[] spriteIcon;

		[SerializeField]
		private Text txtName;

		[SerializeField]
		private Text txtLevel;

		[SerializeField]
		private Text txtPower;

		[SerializeField]
		private Text txtMS;

		[SerializeField]
		private Text txtCoin;

		[SerializeField]
		private Text txtGameWon;

		[SerializeField]
		private Text txtWinPercentage;

		[SerializeField]
		private Text txtTotalStar;

		[SerializeField]
		private Text txtCompletedDailyQuest;

		[SerializeField]
		private Text txtAchievement;

		[SerializeField]
		private Text txtEnemyKilled;

		private bool isLoadingFBInfor;
	}
}
